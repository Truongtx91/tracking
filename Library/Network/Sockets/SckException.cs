﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Sockets
{
   public class SckException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the ProxyException class.
      /// </summary>
      public SckException() : this("An error occured while talking to the proxy server.") { }
      /// <summary>
      /// Initializes a new instance of the ProxyException class.
      /// </summary>
      /// <param name="message">The message that describes the error.</param>
      public SckException(string message) : base(message) { }
      /// <summary>
      /// Initializes a new instance of the ProxyException class.
      /// </summary>
      /// <param name="socks5Error">The error number returned by a SOCKS5 server.</param>
      public SckException(int socks5Error) : this(SckException.Socks5ToString(socks5Error)) { }
      /// <summary>
      /// Converts a SOCKS5 error number to a human readable string.
      /// </summary>
      /// <param name="socks5Error">The error number returned by a SOCKS5 server.</param>
      /// <returns>A string representation of the specified SOCKS5 error number.</returns>
      public static string Socks5ToString(int socks5Error)
      {
         switch (socks5Error)
         {
            case 0:
               return "Connection succeeded.";
            case 1:
               return "General SOCKS server failure.";
            case 2:
               return "Connection not allowed by ruleset.";
            case 3:
               return "Network unreachable.";
            case 4:
               return "Host unreachable.";
            case 5:
               return "Connection refused.";
            case 6:
               return "TTL expired.";
            case 7:
               return "Command not supported.";
            case 8:
               return "Address type not supported.";
            default:
               return "Unspecified SOCKS error.";
         }
      }
   }
}
