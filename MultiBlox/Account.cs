using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Interop;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MultiBlox
{
    public class AccountsJson
    {
        public Dictionary<string, AccountData> accounts { get; set; } = new Dictionary<string, AccountData>();
    }

    public class AccountData
    {
        [JsonPropertyName("UserId")]
        public long UserID { get; set; }
        [JsonPropertyName("Name")]
        public string? Username { get; set; }
        [JsonPropertyName("SecurityToken")]
        public string? SecurityToken { get; set; }
        [JsonIgnore]
        public string? CSRFToken { get; set; }
    }

    public class Account
    {
        public static string? SecurityToken = "";
        public string Username = "Roblox";
        public long UserID = 1;
        [JsonIgnore] public string? CSRFToken;
        [JsonIgnore] public string? Ticket;

        public Account(string Cookie, string? AccountJson = null)
        {
            SecurityToken = Cookie;
        }

        public string? getSecurityToken()
        {
            return SecurityToken;
        }

        public static async Task<Account> CreateAsync(string cookie, string? accountJson = null)
        {
            var account = new Account(cookie);
            await account.InitializeAsync(accountJson);
            return account;
        }

        // Asynchronous initialization method
        private async Task InitializeAsync(string? accountJson)
        {
            
            if (string.IsNullOrEmpty(accountJson))
            {
                var msg = MakeRequest("https://www.roblox.com/my/account/json", HttpMethod.Get);
                //msg.Headers.Add("X-CSRF-TOKEN", await GetCSRFToken());
                msg.Headers.Add("Referer", "https://www.roblox.com/my/account");
                HttpResponseMessage response = await MainWindow.Instance.getClient(MainWindow.Instance.getInputtedCookie()).SendAsync(msg);
                response.EnsureSuccessStatusCode(); // Check if the response is successful
                accountJson = await response.Content.ReadAsStringAsync();
                if(!Utils.IsValidJson(accountJson)) {
                    Error error = new Error("It looks like that token is either invalid or expired! Please enter a valid .ROBLOSECURITY token!");
                    error.Show();
                    return;
                }
                AccountData? accountData = JsonSerializer.Deserialize<AccountData>(accountJson);

                if(accountData == null) return;
                if (string.IsNullOrEmpty(accountData.Username)) return;
                Username = accountData.Username;
                UserID = accountData.UserID;
                
            } else
            {
                if (!Utils.IsValidJson(accountJson))
                {
                    Error error = new Error("It looks like that token is either invalid or expired! Please enter a valid .ROBLOSECURITY token!");
                    error.Show();
                    return;
                }
                AccountData? accountData = JsonSerializer.Deserialize<AccountData>(accountJson);
                if (accountData == null) return;
                if (string.IsNullOrEmpty(accountData.Username)) return;
                Username = accountData.Username;
                UserID = accountData.UserID;
            }
        }

        public HttpRequestMessage MakeRequest(string url, HttpMethod method)
        {
            HttpRequestMessage msg = new HttpRequestMessage(method,url);
            //msg.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return msg;
        }

        public async Task<string> GetCSRFToken()
        {
            HttpRequestMessage request = MakeRequest("https://auth.roblox.com/v1/authentication-ticket/", HttpMethod.Post);
            request.Headers.Add("Referer", "https://www.roblox.com/");
            //request.Headers.Add("Cookie", $".ROBLOSECURITY={SecurityToken}");
            HttpResponseMessage response = await MainWindow.Instance.client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                if (response.Headers.Contains("X-CSRF-Token"))
                {
                    // Retrieve the X-CSRF-Token header value
                    string? csrfToken = response.Headers.GetValues("X-CSRF-Token").FirstOrDefault();
                    if (csrfToken == null) return "";
                    this.CSRFToken = csrfToken;
                    return csrfToken;
                }
                else
                {
                    return "X-CSRF-Token header not found.";
                }
            }


            return "Err";

        }

        public async Task<string> GetAuthToken()
        {
            var authTicket = MakeRequest("https://auth.roblox.com/v1/authentication-ticket/", HttpMethod.Post);
            authTicket.Headers.Add("Referer", "https://www.roblox.com");
            authTicket.Headers.Add("X-CSRF-TOKEN", await GetCSRFToken());
            HttpResponseMessage ticketResponse = await MainWindow.Instance.getClient(SecurityToken).SendAsync(authTicket);
            ticketResponse.EnsureSuccessStatusCode();
            if (ticketResponse.Headers.Contains("rbx-authentication-ticket"))
            {
                string? authToken = ticketResponse.Headers.GetValues("rbx-authentication-ticket").FirstOrDefault();
                if (authToken == null)
                {
                    Error error = new Error("Unexpected error getting your authentication ticket. Please try again later.");
                    error.Show();
                    return "";
                }
                return authToken;
            } else
            {
                Error error = new Error("Error getting an authentication ticket: is your account cookie valid?");
                error.Show();
                return "";
            }

        }
    }
}
