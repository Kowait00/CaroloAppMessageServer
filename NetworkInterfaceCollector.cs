using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CaroloAppMessageServer
{
    class NetworkInterfaceCollector
    {
        public NetworkInterfaceCollector()
        {

        }
        /// <summary>
        /// Collects a list with the names and addresses of all present network interfaces of the PC
        /// (Different network interfaces might be e.g. Ethernet, Wifi, etc.)
        /// </summary>
        public static List<KeyValuePair<String, IPAddress>> FindNetworkInterfaces()
        {
            List<KeyValuePair<String, IPAddress>> networkInterfaces = new List<KeyValuePair<String, IPAddress>>();

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    Debug.WriteLine(ni.Name);
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            networkInterfaces.Add(new KeyValuePair<string, IPAddress>(ni.Name,ip.Address));
                            Debug.WriteLine(ip.Address.ToString());
                        }
                    }
                }
            }
            return networkInterfaces;
        }

    }
}
