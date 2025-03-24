using System.Diagnostics;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using System.Security.Policy;
using System;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultiBlox
{
    public partial class MainWindow : Form
    {
        Point lastPoint;
        private Random random;
        public HttpClient client { get; private set; }
        private Mutex? rblx;
        private Account? selectedAccount;
        private AccountsJson loadedAccounts;
        public static readonly string version = "1.1";
        private Tracker robloxTracker = new Tracker();

        public static MainWindow Instance { get; private set; }

        public string getInputtedCookie()
        {
            return RobloxTokenInput.Text;
        }

        public HttpClient getClient(string? cookie)
        {
            if(client.DefaultRequestHeaders.Contains("Cookie")) return client;
            client.DefaultRequestHeaders.Add("Cookie", $".ROBLOSECURITY={cookie}");
            return client;
        }

        public MainWindow()
        {
            InitializeComponent();
            Github.GitHubApiHelper helper = new();
            Github.GitHubRelease? latestRelease;
            random = new Random();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
            client = new HttpClient(handler);
            //string[] games = [];
            foreach(Game game in Persistance.LoadGames().games.Values)
            {
                ServerList.Items.Add(new Label() { Text = game.Name, Tag = game.ID});
            }
            ServerList.DisplayMember = "Text";
            ServerList.ForeColor = Color.White;
            Instance = this;
            latestRelease = null;
            Task.Run(async () =>
            {
                latestRelease = await helper.GetLatestReleaseAsync("Xexplotion", "MultiBlox");
                
                if (Persistance.LoadAccounts().accounts.Count > 0)
                {
                    loadedAccounts = Persistance.LoadAccounts();
                    foreach (AccountData account in loadedAccounts.accounts.Values)
                    {
                        await addAccount(account, account.SecurityToken);
                    }
                } else
                {
                    loadedAccounts = new AccountsJson() { accounts = [] };
                }
            }).GetAwaiter().GetResult();

            if(latestRelease != null) {
                float latest = float.Parse(latestRelease.Name);
                float current = float.Parse(version);
                if (latest > current)
                {
                    Update update = new Update();
                    update.Show();
                }
            }

        }


        private void close(object sender, EventArgs e)
        {
            if (rblx != null) rblx.Dispose();
            Application.Exit();
        }

        private void minimize(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        async Task<Account?> addAccount(AccountData? accountData, string? cookie = null)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                Error error = new Error("No .ROBLOSECURITY cookie provided!");
                error.Show();
                return null;
            }

            accountsTable.RowCount++;

            // Add a new RowStyle with equal size
            accountsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / accountsTable.RowCount));

            foreach (RowStyle rowStyle in accountsTable.RowStyles)
            {
                rowStyle.Height = 100f / accountsTable.RowCount;
            }

            Account account;
            if (accountData != null)
            {
                Debug.WriteLine("Importing account!");
                account = await Account.CreateAsync(cookie, JsonSerializer.Serialize(accountData));
            }
            else
            {
                Debug.WriteLine("Creating account!");
                account = await Account.CreateAsync(cookie);
            }

            // Create a new label as a sample control
            Label label = new Label
            {
                Text = "Username: " + account.Username + " UserID: " + account.UserID,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(35, 35, 35),
                BorderStyle = BorderStyle.FixedSingle,
            };

            //string? token = account.getSecurityToken();
            label.Tag = account;
            label.MouseClick += clickedAccount;

            // Add the control to the new row
            accountsTable.Controls.Add(label, 0, accountsTable.RowCount - 1);
            return account;
        }

        private async void addAccountButton(object sender, EventArgs e)
        {
            Account? account = await addAccount(null, RobloxTokenInput.Text.Replace(" ", ""));
            if (account == null)
            {
                return;
            }
            AccountData data = new AccountData { UserID = account.UserID, CSRFToken = account.CSRFToken, Username = account.Username, SecurityToken = RobloxTokenInput.Text.Replace(" ", "") };
            loadedAccounts.accounts.Add(account.Username, data);
            Persistance.SaveAccount(data);
        }


        enum Method
        {
            Get,
            Post
        }

        public HttpRequestMessage MakeRequest(string url, HttpMethod method)
        {
            HttpRequestMessage msg = new HttpRequestMessage(method, url);
            return msg;
        }

        private void clickedAccount(object? sender, MouseEventArgs e)
        {
            if (sender == null) return;
            if (sender is Label label && label.Tag != null)
            {
                Account? labelAccount = label.Tag as Account;
                if (labelAccount == null) return;
                if (loadedAccounts.accounts.ContainsKey(labelAccount.Username)) {
                    AccountData foundAccount = loadedAccounts.accounts[labelAccount.Username];
                    selectedAccount = new Account(foundAccount.SecurityToken) { UserID = foundAccount.UserID, Username = foundAccount.Username};
                }
                
                if (selectedAccount != null) client.DefaultRequestHeaders.Add("Cookie", $".ROBLOSECURITY={selectedAccount.getSecurityToken()}");
                label.BackColor = Color.FromArgb(85, 85, 85);
                foreach (Label otherAccountLabel in accountsTable.Controls)
                {
                    if (otherAccountLabel != label) otherAccountLabel.BackColor = Color.FromArgb(35, 35, 35);
                }
            }

        }
        private void drag_mouse(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void drag_mouse_move(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private async void JoinGameButton_Click(object sender, EventArgs e)
        {
            if (selectedAccount == null)
            {
                Error error = new Error("Please select an account to log into!");
                error.Show();
                return;
            }
            string placeID;
            if (string.IsNullOrEmpty(PlaceID.Text) || !Utils.IsDigitsOnly(PlaceID.Text.Replace(" ", ""), out placeID) || !await Utils.CheckIfPlaceExists(placeID))
            {
                Error error = new Error("Please enter a PlaceID to join!" + string.IsNullOrEmpty(PlaceID.Text) + !Utils.IsDigitsOnly(PlaceID.Text.Replace(" ", ""), out placeID) + placeID + !await Utils.CheckIfPlaceExists(placeID));
                error.Show();
                return;
            }


            Persistance.SaveGame(PlaceID.Text);
            HttpRequestMessage joinRequest = MakeRequest(string.Format("https://www.roblox.com/games/{0}?privateServerLinkCode={1}", PlaceID.Text, UserID.Text), HttpMethod.Get);
            joinRequest.Headers.Add("X-CSRF-TOKEN", await selectedAccount.GetCSRFToken());
            joinRequest.Headers.Add("Referer", "https://https://www.roblox.com/games/5902977746/RAIDS-Ultimate-Tower-Defense");

            HttpResponseMessage joinResponse = await client.SendAsync(joinRequest);

            joinResponse.EnsureSuccessStatusCode();

            await Task.Run(async () =>
            {
                try
                {
                    if (instanceClosing.Checked &&  robloxTracker.GetInstances() != null && robloxTracker.GetInstances().ContainsKey(selectedAccount.UserID.ToString()))
                    {
                        robloxTracker.Close(selectedAccount.UserID.ToString());
                    }
                    bool IsTeleport = false;
                    double LaunchTime = Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds * 1000);
                    Random r = new Random();

                    string BrowserTrackerID = r.Next(100000, 175000).ToString() + r.Next(100000, 900000).ToString(); // oh god this is ugly
                    //ProcessStartInfo LauncherInfo = new ProcessStartInfo();
                    string url = "";
                    if (!string.IsNullOrEmpty(UserID.Text) && joinFriend.Checked)
                    {
                        if (!await Utils.CheckIfUserExists(UserID.Text, selectedAccount.getSecurityToken()))
                        {
                            Error error = new Error("Please enter a valid(and followed) User's ID to join!");
                            error.Show();
                            return;
                        }
                        string auth = await selectedAccount.GetAuthToken();
                        if (string.IsNullOrEmpty(auth))
                        {
                            return;
                        }
                        url = $"roblox-player:1+launchmode:play+gameinfo:{auth}+launchtime:{LaunchTime}+placelauncherurl:{HttpUtility.UrlEncode($"https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestFollowUser&userId={UserID.Text}")}+browsertrackerid:{BrowserTrackerID}+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp";

                    }
                    else
                    {
                        string JobID = await Utils.GetRandomJobId(PlaceID.Text, false);
                        string auth = await selectedAccount.GetAuthToken();
                        if (string.IsNullOrEmpty(auth))
                        {
                            return;
                        }


                        url = $"roblox-player:1+launchmode:play+gameinfo:{auth}+launchtime:{LaunchTime}+placelauncherurl:{HttpUtility.UrlEncode($"https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestGame{(string.IsNullOrEmpty(JobID) ? "" : "Job")}&browserTrackerId={BrowserTrackerID}&placeId={PlaceID.Text}{(string.IsNullOrEmpty(JobID) ? "" : ("&gameId=" + JobID))}&isPlayTogetherGame=false{(IsTeleport ? "&isTeleport=true" : "")}")}+browsertrackerid:{BrowserTrackerID}+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp";


                    }
                    Process? Launcher = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    robloxTracker.Add(new Roblox(Launcher, selectedAccount.UserID.ToString()));
                    Launcher?.WaitForExit();
                }
                catch (Exception ex)
                {
                    Error error = new Error(ex.Message.ToString());
                    error.Show();
                }
            });
        }


        private async void button4_Click_1(object sender, EventArgs e)
        {
            Update update = new Update();//"Test Error error! :D");
            update.Show();
        }

        



        private void multiRoblox_CheckedChanged(object sender, EventArgs e)
        {
            if (rblx == null && ((CheckBox)sender).Checked)
            {
                // Create a new Mutex
                rblx = new Mutex(true, "ROBLOX_singletonMutex");
            }
            else
            {
                // Dispose the Mutex
                rblx?.Dispose();
                rblx = null; // Reset the mutex variable
            }
        }

        private void choose_game_dropdown(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox? comboBox = sender as System.Windows.Forms.ComboBox;
            object? selectedItem = comboBox?.SelectedItem;
            PlaceID.Text = (selectedItem as Label)?.Tag?.ToString();
        }
    }
}
