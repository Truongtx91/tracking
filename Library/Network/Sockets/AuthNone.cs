﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network.Sockets
{
   public class AuthNone : AuthMethod
   {
      /// <summary>
      /// Initializes an AuthNone instance.
      /// </summary>
      /// <param name="server">The socket connection with the proxy server.</param>
      public AuthNone(Socket server) : base(server) { }
      /// <summary>
      /// Authenticates the user.
      /// </summary>
      public override void Authenticate()
      {
         return; // Do Nothing
      }
      /// <summary>
      /// Authenticates the user asynchronously.
      /// </summary>
      /// <param name="callback">The method to call when the authentication is complete.</param>
      /// <remarks>This method immediately calls the callback method.</remarks>
      public override void BeginAuthenticate(HandShakeComplete callback)
      {
         callback(null);
      }
   }
}
