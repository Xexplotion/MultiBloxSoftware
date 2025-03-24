using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace MultiBlox
{
    public class KeyGenerator
    {
        // Generate a consistent AES key based on machine-specific data
        public static byte[] GenerateAesKey()
        {
            string uniqueIdentifier = GetMachineUniqueIdentifier(); // Replace with your chosen identifier method
            return GenerateKeyFromData(uniqueIdentifier);
        }

        // Get a unique identifier for the machine (e.g., MAC address)
        private static string GetMachineUniqueIdentifier()
        {
            // Example: Get the MAC address of the first Ethernet adapter
            string macAddress = NetworkInterface
                                .GetAllNetworkInterfaces()
                                .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                                .Select(nic => nic.GetPhysicalAddress().ToString())
                                .FirstOrDefault();

            return macAddress ?? throw new Exception("No Ethernet adapter found.");
        }

        // Generate a consistent key from input data
        private static byte[] GenerateKeyFromData(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                // AES key size is 256 bits (32 bytes), so we truncate or pad the hash as needed
                byte[] aesKey = new byte[32]; // 256 bits
                Array.Copy(hashBytes, aesKey, Math.Min(hashBytes.Length, aesKey.Length));
                return aesKey;
            }
        }
    }
}
