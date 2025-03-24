using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiBlox
{

    public class Github
    {
        public class GitHubRelease
        {
            public string TagName { get; set; }
            public string Name { get; set; }
            public string Body { get; set; }
            public string HtmlUrl { get; set; }
            public GitHubAsset[] Assets { get; set; }
        }

        public class GitHubAsset
        {
            public string Name { get; set; }
            public string BrowserDownloadUrl { get; set; }
        }

        public class GitHubApiHelper
        {
            private static readonly HttpClient _httpClient = new HttpClient();

            public GitHubApiHelper()
            {
                // Set the GitHub API base address
                if(_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri("https://api.github.com/");
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
                // If using a personal access token, uncomment the following line and replace "YOUR_GITHUB_TOKEN" with your token
                // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_GITHUB_TOKEN");
            }

            public async Task<GitHubRelease> GetLatestReleaseAsync(string owner, string repo)
            {
                var response = await _httpClient.GetAsync($"repos/{owner}/{repo}/releases/latest");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var release = JsonSerializer.Deserialize<GitHubRelease>(jsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                return release;
            }

            public static void UnzipFile(string zipPath)
            {
                string extractPath = Path.GetDirectoryName(zipPath);

                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destinationPath = Path.Combine(extractPath, entry.FullName);

                        // Ensure the directory exists
                        string destinationDir = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(destinationDir))
                        {
                            Directory.CreateDirectory(destinationDir);
                        }

                        // Check if the file already exists
                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath); // Delete existing file
                        }

                        entry.ExtractToFile(destinationPath, true); // Extract and overwrite
                    }
                }
            }
        }
    }
}
