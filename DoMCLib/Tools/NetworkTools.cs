using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Tools
{
    public class NetworkTools
    {
        public static NetworkInterface[] GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }
        public static IPAddress GetBroadcastAddress(IPAddress address)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                var ipProps = ni.GetIPProperties();
                foreach (var unicastAddress in ipProps.UnicastAddresses)
                {
                    if (unicastAddress.Address == address)
                    {
                        // Преобразуем IP-адрес в ulong
                        ulong ipAsLong = IpAddressToUlong(unicastAddress.Address);

                        // Создаем маску подсети в виде ulong
                        ulong subnetMask = CreateSubnetMask(unicastAddress.PrefixLength);

                        // Вычисляем широковещательный адрес
                        ulong broadcastAddress = ipAsLong | ~subnetMask;

                        // Преобразуем обратно в IP-адрес
                        return UlongToIpAddress(broadcastAddress);
                    }
                }
            }
            return IPAddress.Broadcast;
        }
        public static IPAddress GetBroadcastAddress(NetworkInterface ni)
        {
            var ipProps = ni.GetIPProperties();
            foreach (var unicastAddress in ipProps.UnicastAddresses)
            {
                if (unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4
                {
                    // Преобразуем IP-адрес в ulong
                    ulong ipAsLong = IpAddressToUlong(unicastAddress.Address);

                    // Создаем маску подсети в виде ulong
                    ulong subnetMask = CreateSubnetMask(unicastAddress.PrefixLength);

                    // Вычисляем широковещательный адрес
                    ulong broadcastAddress = ipAsLong | ~subnetMask;

                    // Преобразуем обратно в IP-адрес
                    return UlongToIpAddress(broadcastAddress);
                }
            }
            return IPAddress.Broadcast;
        }
        public static ulong IpAddressToUlong(IPAddress ipAddress)
        {
            byte[] bytes = ipAddress.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes); // Для корректного порядка байт
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static IPAddress UlongToIpAddress(ulong ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes); // Для корректного порядка байт
            }
            return new IPAddress(bytes);
        }

        public static ulong CreateSubnetMask(int prefixLength)
        {
            return ulong.MaxValue << (32 - prefixLength);
        }

    }
}
