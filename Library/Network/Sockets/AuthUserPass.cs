﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network.Sockets
{
   public class AuthUserPass : AuthMethod
   {
      /// <summary>
      /// Initializes a new AuthUserPass instance.
      /// </summary>
      /// <param name="server">The socket connection with the proxy server.</param>
      /// <param name="user">The username to use.</param>
      /// <param name="pass">The password to use.</param>
      /// <exception cref="ArgumentNullException"><c>user</c> -or- <c>pass</c> is null.</exception>
      public AuthUserPass(Socket server, string user, string pass) : base(server)
      {
         Username = user;
         Password = pass;
      }
      /// <summary>
      /// Creates an array of bytes that has to be sent if the user wants to authenticate with the username/password authentication scheme.
      /// </summary>
      /// <returns>An array of bytes that has to be sent if the user wants to authenticate with the username/password authentication scheme.</returns>
      private byte[] GetAuthenticationBytes()
      {
         byte[] buffer = new byte[3 + Username.Length + Password.Length];
         buffer[0] = 1;
         buffer[1] = (byte)Username.Length;
         Array.Copy(Encoding.ASCII.GetBytes(Username), 0, buffer, 2, Username.Length);
         buffer[Username.Length + 2] = (byte)Password.Length;
         Array.Copy(Encoding.ASCII.GetBytes(Password), 0, buffer, Username.Length + 3, Password.Length);
         return buffer;
      }
      /// <summary>
      /// Starts the authentication process.
      /// </summary>
      public override void Authenticate()
      {
         Server.Send(GetAuthenticationBytes());
         byte[] buffer = new byte[2];
         int received = 0;
         while (received != 2)
         {
            received += Server.Receive(buffer, received, 2 - received, SocketFlags.None);
         }
         if (buffer[1] != 0)
         {
            Server.Close();
            throw new SckException("Username/password combination rejected.");
         }
         return;
      }
      /// <summary>
      /// Starts the asynchronous authentication process.
      /// </summary>
      /// <param name="callback">The method to call when the authentication is complete.</param>
      public override void BeginAuthenticate(HandShakeComplete callback)
      {
         CallBack = callback;
         Server.BeginSend(GetAuthenticationBytes(), 0, 3 + Username.Length + Password.Length, SocketFlags.None, new AsyncCallback(this.OnSent), Server);
         return;
      }
      /// <summary>
      /// Called when the authentication bytes have been sent.
      /// </summary>
      /// <param name="ar">Stores state information for this asynchronous operation as well as any user-defined data.</param>
      private void OnSent(IAsyncResult ar)
      {
         try
         {
            Server.EndSend(ar);
            Buffer = new byte[2];
            Server.BeginReceive(Buffer, 0, 2, SocketFlags.None, new AsyncCallback(this.OnReceive), Server);
         }
         catch (Exception e)
         {
            CallBack(e);
         }
      }
      /// <summary>
      /// Called when the socket received an authentication reply.
      /// </summary>
      /// <param name="ar">Stores state information for this asynchronous operation as well as any user-defined data.</param>
      private void OnReceive(IAsyncResult ar)
      {
         try
         {
            Received += Server.EndReceive(ar);
            if (Received == Buffer.Length)
               if (Buffer[1] == 0)
                  CallBack(null);
               else
                  throw new SckException("Username/password combination not accepted.");
            else
               Server.BeginReceive(Buffer, Received, Buffer.Length - Received, SocketFlags.None, new AsyncCallback(this.OnReceive), Server);
         }
         catch (Exception e)
         {
            CallBack(e);
         }
      }
      /// <summary>
      /// Gets or sets the username to use when authenticating with the proxy server.
      /// </summary>
      /// <value>The username to use when authenticating with the proxy server.</value>
      /// <exception cref="ArgumentNullException">The specified value is null.</exception>
      private string Username
      {
         get
         {
            return m_Username;
         }
         set
         {
            if (value == null)
               throw new ArgumentNullException();
            m_Username = value;
         }
      }
      /// <summary>
      /// Gets or sets the password to use when authenticating with the proxy server.
      /// </summary>
      /// <value>The password to use when authenticating with the proxy server.</value>
      /// <exception cref="ArgumentNullException">The specified value is null.</exception>
      private string Password
      {
         get
         {
            return m_Password;
         }
         set
         {
            if (value == null)
               throw new ArgumentNullException();
            m_Password = value;
         }
      }
      // private variables
      /// <summary>Holds the value of the Username property.</summary>
      private string m_Username;
      /// <summary>Holds the value of the Password property.</summary>
      private string m_Password;
   }
}
