using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CaroloAppMessageServer
{
    class UdpBroadcastSender
    {
        IPAddress localAddress;             //Address of the network interface to send the broadcast from (e.g. Address of wifi or ethernet adapter)
        IPAddress multicastAddress;         //Address of the multicast group to send to
        int multicastPort;                  //Port to send the multicast through
        EndPoint localMulticastEndpoint;
        Socket multicastSocket;
        MulticastOption multicastOption;
        IPEndPoint remoteEndpoint;

        /// <summary>
        /// Sets up a sender for UDP broadcasts.
        /// Sends only through the specified network interface (e.g. only wifi or ethernet)
        /// Throws System.Net.Sockets.SocketException if socket can't be bound with passed on parameters
        /// </summary>
        /// <param name="localAddress">Address of the network interface to send the broadcast from (e.g. Address of wifi or ethernet adapter)</param>
        /// <param name="multicastAddress">Address of the multicast group to send to</param>
        /// <param name="multicastPort">Port to send the multicast through</param>
        public UdpBroadcastSender(IPAddress localAddress, IPAddress multicastAddress, int multicastPort)
        {
            this.localAddress = localAddress;
            this.multicastPort = multicastPort;
            this.multicastAddress = multicastAddress;

            try
            {
                localMulticastEndpoint = (EndPoint)new IPEndPoint(localAddress, multicastPort);
                multicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                multicastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                multicastSocket.Bind(localMulticastEndpoint);
                multicastOption = new MulticastOption(multicastAddress, localAddress);
                multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
                remoteEndpoint = new IPEndPoint(multicastAddress, multicastPort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Couldn't set up socket for UDP multicasts");
                throw;
            }
            
            Debug.WriteLine("Current multicast group is: " + multicastOption.Group);
            Debug.WriteLine("Current multicast local address is: " + multicastOption.LocalAddress);
        }

        /// <summary>
        /// Send a UDP multicast package
        /// </summary>
        /// <param name="data">Contents to send</param>
        public void sendPacket(byte[] data)
        {
            multicastSocket.SendTo(data, data.Length, SocketFlags.None, remoteEndpoint);
            Console.WriteLine("Multicast Message {0} sent", Encoding.ASCII.GetString(data, 0, data.Length) );
        }
    }
}
