using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace BF2statisticsLauncher
{
    /// <summary>
    /// Battlefield2.available.gamespy.com SERVER.
    /// </summary>
    class Bf2Available
    {
        /// <summary>
        /// The return message to send back to the client
        /// </summary>
        private byte[] reply = new byte[7] { 0xfe, 0xfd, 0x09, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Our Socket
        /// </summary>
        private Socket UdpSock;

        /// <summary>
        /// Buffer for the received messages
        /// </summary>
        private byte[] buffer = new byte[18];

        public Bf2Available()
        {
            // Creates an IPEndPoint to reference the IP Address and port number of the sender.  
            EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Bind to local address and begin accepting connections
            UdpSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UdpSock.Bind(new IPEndPoint(IPAddress.Loopback, 27900));
            UdpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref RemoteIpEndPoint, DoReceiveFrom, UdpSock);
        }

        private void DoReceiveFrom(IAsyncResult iar)
        {
            try
            {
                // Get the received message.
                EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                UdpSock.EndReceiveFrom(iar, ref clientEP);
                UdpSock.SendTo(reply, clientEP);
            }
            catch
            {

            }
            finally
            {
                //Start listening for a new message.
                buffer = new byte[18];
                EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
                UdpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, UdpSock);
            }
        }
    }
}
