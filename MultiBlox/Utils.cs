using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiBlox
{
    public class Utils
    {
        public static byte[] ByteStringToByteArray(string byteString)
        {
            string[] byteStrings = byteString.Split(',');
            byte[] byteArray = new byte[byteStrings.Length];

            for (int i = 0; i < byteStrings.Length; i++)
            {
                byteArray[i] = byte.Parse(byteStrings[i]);
            }

            return byteArray;
        }
        public static string ByteArrayToByteString(byte[] byteArray)
        {
            string byteString = string.Join(",", byteArray);
            return byteString;
        }
        public static void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            Debug.WriteLine(sb.ToString() + bytes.Length);

        }

        public static string RemoveFirstAndLastOccurrence(string input, char firstCharToRemove, char lastCharToRemove)
        {
            int firstIndex = input.IndexOf(firstCharToRemove);
            int lastIndex = input.LastIndexOf(lastCharToRemove);

            // Remove the first occurrence of the first character
            if (firstIndex != -1)
            {
                input = input.Remove(firstIndex, 1);
                // Adjust lastIndex if it comes after the first occurrence and was shifted by the removal
                if (lastIndex > firstIndex)
                {
                    lastIndex--;
                }
            }

            // Remove the last occurrence of the second character
            if (lastIndex != -1)
            {
                input = input.Remove(lastIndex, 1);
            }

            return input;
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static bool IsValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(input);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static bool IsDigitsOnly(string str, out string number)
        {
            number = "";
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            number = str;
            return true;
        }

        public static async Task<bool> CheckIfPlaceExists(string placeId)
        {
            string apiUrl = $"https://games.roblox.com/v1/games/multiget-place-details?placeIds={placeId}";
            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                req.Headers.Add("Referer", "https://www.roblox.com/");
                HttpResponseMessage response = await MainWindow.Instance.getClient(null).SendAsync(req);
                if (response.IsSuccessStatusCode)
                {
                    // Check if place exists
                    string responseData = await response.Content.ReadAsStringAsync();
                    // Roblox API returns an empty object {} for non-existent places
                    return responseData != "{}";
                }
                else
                {
                    // Handle non-success status codes if needed
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {

                // Handle any exceptions
                Debug.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> CheckIfUserExists(string userId, string? cookie)
        {
            string apiUrl = $"https://users.roblox.com/v1/users/{userId}";

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                req.Headers.Add("Cookie", ".ROBLOSECURITY=" + cookie);
                HttpResponseMessage response = await client.SendAsync(req);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Check if user exists
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Roblox API returns an empty object {} for non-existent users
                    return responseData != "{}";
                }
                else
                {
                    // Handle non-success status codes if needed
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        static HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
        public static async Task<string> GetRandomJobId(string PlaceId, bool ChooseLowestServer = false)
        {
            Random RNG = new Random();
            List<string> ValidServers = new List<string>();
            int StopAt = Math.Max(0, 1);// AccountManager.General.Get<int>("ShufflePageCount"), 1);
            int PageCount = 0;

            async Task GetServers(string Cursor = "")
            {
                if (PageCount >= StopAt) return;

                PageCount++;


                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://games.roblox.com/v1/games/" + PlaceId + "/servers/public?sortOrder=Asc&limit=100" + (string.IsNullOrEmpty(Cursor) ? "" : "&cursor=" + Cursor));
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();


                JsonDocument Servers = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                JsonElement root = Servers.RootElement;

                if (!root.TryGetProperty("data", out JsonElement data)) return;

                if (root.TryGetProperty("nextPageCursor", out JsonElement nextPageCursorElement) && nextPageCursorElement.ValueKind == JsonValueKind.String)
                {
                    Cursor = nextPageCursorElement.GetString() ?? string.Empty;
                }

                foreach (JsonElement a in data.EnumerateArray())
                {
                    int? playing = a.GetProperty("playing").GetInt32();
                    int? maxPlayers = a.GetProperty("maxPlayers").GetInt32();

                    if (playing.HasValue && maxPlayers.HasValue && playing.Value != maxPlayers.Value && playing.Value > 0 && maxPlayers.Value > 1)
                    {
                        string? id = a.GetProperty("id").GetString();
                        if (id == null) continue;
                        ValidServers.Add(id);

                    }
                }

                if (!string.IsNullOrEmpty(Cursor) && !ChooseLowestServer)
                {
                    await GetServers(Cursor);
                }
            }

            await GetServers();

            if (ValidServers.Count == 0) return string.Empty;


            return ValidServers[ChooseLowestServer ? 0 : RNG.Next(ValidServers.Count)];
        }
    }
}
