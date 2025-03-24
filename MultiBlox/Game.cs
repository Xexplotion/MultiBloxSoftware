using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultiBlox
{

    public class GamesJson
    {
        public Dictionary<string, Game> games { get; set; } = new Dictionary<string, Game>();
    }
    public class Game
    {
        public string? ID { get; private set; }
        [JsonPropertyName("name")]
        public string? Name { get; private set; }
        [JsonPropertyName("url")]
        public string URL { get; private set; }

        public Game(string id, string? name, string? url) {
            ID = id;
            Name = name ?? string.Empty;
            URL = url ?? string.Empty;
        }

        public async Task InitializeAsync(string id)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://games.roblox.com/v1/games/multiget-place-details?placeIds={id}");
            HttpResponseMessage res = await MainWindow.Instance.getClient(MainWindow.Instance.getInputtedCookie()).SendAsync(req);
            res.EnsureSuccessStatusCode();
            string response = await res.Content.ReadAsStringAsync();
            if (response == null) return;
            if (!Utils.IsValidJson(response)) return;
            response = Utils.RemoveFirstAndLastOccurrence(response, '[', ']');
            Game? game = JsonSerializer.Deserialize<Game>(response);
            if (game == null) return;
            if (game == null) return;
            this.Name = game.Name;
            this.URL = game.URL;
        }

    }
}
