using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MultiBlox.Github;

namespace MultiBlox
{
    public partial class Update : Form
    {
        Point lastPoint = new Point();
        Github.GitHubApiHelper helper;
        Github.GitHubRelease latestRelease;
        public Update()
        {
            InitializeComponent();
            CheckAndDisableForms();
            helper = new();
            Task.Run(async () =>
            {
                latestRelease = await helper.GetLatestReleaseAsync("Xexplotion", "MultiBlox");

                label2.Text = label2.Text.Replace("{version}", $"v{MainWindow.version} -> v{latestRelease.Name}");
            }).GetAwaiter().GetResult();

            label1.TextAlign = ContentAlignment.MiddleCenter;
        }

        public void DisableAllFormsExcept<T>() where T : Form
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is T))
                {
                    form.Enabled = false;
                }
            }
        }

        public void CheckAndDisableForms()
        {
            DisableAllFormsExcept<Error>();
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

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var res = await client.GetAsync($"https://github.com/Xexplotion/MultiBlox/releases/download/{latestRelease.Name}/MultiBlox.zip");
            res.EnsureSuccessStatusCode();

            await using var fileStream = new FileStream(Path.Combine(Environment.CurrentDirectory,"MultiBlox.zip"), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            await using var httpStream = await res.Content.ReadAsStreamAsync();
            await httpStream.CopyToAsync(fileStream);
            httpStream.Dispose();
            fileStream.Dispose();

            string exePath = Path.Combine(Environment.CurrentDirectory, "UnZipper.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = Path.Combine(Environment.CurrentDirectory, "MultiBlox.zip") + " ./MultiBlox.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process? process = Process.Start(startInfo);

            Application.Exit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ignore
            foreach (Form form in Application.OpenForms)
            {
                form.Enabled = true;
            }
            this.Close();
        }
    }
}
