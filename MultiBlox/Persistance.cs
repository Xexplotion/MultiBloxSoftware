using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace MultiBlox
{
    public static class Persistance
    {
        private const string AccountsFilePath = "./accounts.json";
        private const string GamesFilePath = "./games.json";

        public async static void SaveGame(string id)
        {
            GamesJson games = LoadGames();
            Game game = new(id, null, null);
            await game.InitializeAsync(id);
            if(game.Name == null) return;
            if(games.games.ContainsKey(game.Name) && games.games[game.Name].Name != game.Name) games.games[game.Name] = game;
            else if (! games.games.ContainsKey(game.Name)) games.games.Add(game.Name, game);

            File.WriteAllText(GamesFilePath, JsonSerializer.Serialize(games));
        }

        public static GamesJson LoadGames() {
            GamesJson games = new GamesJson() { games = [] };
            if (!File.Exists(GamesFilePath)) return games;
            string json = File.ReadAllText(GamesFilePath);
            if(string.IsNullOrEmpty(json) || !Utils.IsValidJson(json)) return games;
            GamesJson? gamesJson = JsonSerializer.Deserialize<GamesJson>(json);
            if(gamesJson == null) return games;
            return gamesJson;
        }

        public static void SaveAccount(AccountData account)
        {
            AccountsJson accountJson = LoadAccounts();
            if (accountJson != null)
            {
                if (string.IsNullOrEmpty(account.Username))
                {
                    Error error = new Error("Unable to save account with no username!");
                    error.Show();
                    return;
                }
                accountJson.accounts.Add(account.Username, account);
                SaveAccounts(accountJson);
            }
        }

        public static void SaveAccounts(AccountsJson accountsJson)
        {
            AccountsJson json = new AccountsJson() { accounts = [] };
            foreach (AccountData account in accountsJson.accounts.Values)
            {
                if (json.accounts.ContainsKey(account.Username)) continue;
                // TODO: Implement encryption
                json.accounts.Add(account.Username, account);
                //json.accounts.Add(account.Username, new AccountData() { UserID = account.UserID, Username = account.Username, CSRFToken = account.CSRFToken, SecurityToken = Utils.ByteArrayToByteString(Utils.EncryptStringToBytes_Aes(account.SecurityToken, KeyGenerator.GenerateAesKey(), new byte[16])) });
            }
            //serialize into json
            string endjson = JsonSerializer.Serialize(json);
            File.WriteAllText(AccountsFilePath, endjson);
        }

        public static AccountsJson LoadAccounts()
        {
            AccountsJson accounts = new AccountsJson() { accounts = [] };
            if (!File.Exists(AccountsFilePath)) return accounts;
            string json = File.ReadAllText(AccountsFilePath);
            if (string.IsNullOrEmpty(json) || !Utils.IsValidJson(json)) return accounts;
            AccountsJson? accountsJson = JsonSerializer.Deserialize<AccountsJson>(json);
            if (accountsJson == null) return accounts;
            AccountsJson final = new AccountsJson() { accounts = [] };
            foreach (AccountData account in accountsJson.accounts.Values)
            {
                final.accounts.Add(account.Username, account);
                //final.accounts.Add(account.Username, new AccountData() { Username = account.Username, CSRFToken = account.CSRFToken, UserID = account.UserID, SecurityToken = Utils.DecryptStringFromBytes_Aes(Utils.ByteStringToByteArray(account.SecurityToken), KeyGenerator.GenerateAesKey(), new byte[16])});
            }
            return final;
        }
    }
}
