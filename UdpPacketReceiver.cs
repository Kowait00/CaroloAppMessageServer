using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CaroloAppMessageServer
{
    class UdpPacketReceiver
    {
        int port;                   //Port to listen on
        IPEndPoint receiverEndpoint;
        Socket receiverSocket;
        EndPoint remoteEndpoint;    //Endpoint for remote partner we're listening to (here any)

        /// <summary>
        /// Set up a receiver for UDP packets
        /// </summary>
        /// <param name="port">Port on which to listen for UDP packets</param>
        public UdpPacketReceiver(int port)
        {
            this.port = port;
            if (port > 65535 || port < 0) throw new Exception("Invalid Port Number");
            try
            {
                //Network Endpoints to listen to all IP addresses on the specified port
                receiverEndpoint = new IPEndPoint(IPAddress.Any, port);
                receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                receiverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                receiverSocket.Bind(receiverEndpoint);
                remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);    //The endpoint from which we want to receive messages
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Couldn't set up socket for receiving UDP packets");
            }
        }

        /// <summary>
        /// Waits for next UDP packet and receives it
        /// </summary>
        /// <returns>Returns byte array of the contents of the packet</returns>
        public byte[] receivePacket()
        {
            byte[] receivedData = new byte[1024];
            int receivedByteCount = 0;

            Console.WriteLine("Waiting for messages");
            try
            {
                receivedByteCount = receiverSocket.ReceiveFrom(receivedData, ref remoteEndpoint);
            }
            catch (Exception e)
            {
                string abortReceivingMessage = "Stopped receiving UDP packets";
                Console.WriteLine(e.ToString() + "\n" + abortReceivingMessage);
                receivedData = Encoding.ASCII.GetBytes(abortReceivingMessage);
                receivedByteCount = abortReceivingMessage.Length;
            }
            Console.WriteLine("Packet received from {0}: ", remoteEndpoint.ToString());
            Console.WriteLine("Contents: {0}", Encoding.ASCII.GetString(receivedData, 0, receivedByteCount));

            Array.Resize(ref receivedData, receivedByteCount);
            return receivedData;
        }

        /// <summary>
        /// Closes the socket currently used by the UdpPacketReceiver
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                receiverSocket.Shutdown(SocketShutdown.Receive);
            }
            catch (Exception)
            {
                // Not currently receiving, that's ok
            }
            receiverSocket.Close();
        }

    }
}
