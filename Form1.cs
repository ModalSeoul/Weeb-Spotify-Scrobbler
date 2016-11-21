using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IDontKnowCSharp
{
    public partial class mainForm : Form
    {
        Scrobble Scrobble = new Scrobble();
        WeebFm Weeb = new IDontKnowCSharp.WeebFm();
        public bool IsLoggedIn = false;
        public bool TaskBar = true;
        String Service;

        public mainForm()
        {
            InitializeComponent();
        }

        public void osuScrobble()
        {
            if (IsLoggedIn)
            {
                String Title = Scrobble.GetPlayerHandle(Scrobble.Player.OSU);

                if (Title != "osu!")
                {
                    Title = Title.Split(new[] { "osu!  - " }, StringSplitOptions.None)[1];
                    string Artist = Title.Split(new[] { " - " }, StringSplitOptions.None)[0];
                    string Song = Title.Split(new[] { "- " }, StringSplitOptions.None)[1];
                    Song = Song.Split(new[] { " [" }, StringSplitOptions.None)[0];

                    if (label2.Text != Song)
                    {
                        label2.Text = Song;
                        Weeb.Scrobble(Song, Artist);
                    }
                }
                else
                {
                    label2.Text = "In Menus";
                }
            }
        }

        public void WinAmpScrobble()
        {
            if (IsLoggedIn)
            {
                try
                {
                    String Title = Scrobble.GetPlayerHandle(Scrobble.Player.WINAMP);
                    try
                    {
                        Title = Title.Split(new[] { ". " }, StringSplitOptions.None)[1]; // Gets rid of track number
                    }
                    catch { }
                    string Artist = Title.Split(new[] { " - " }, StringSplitOptions.None)[0].Trim();
                    string Song = Title.Split(new[] { " - " }, StringSplitOptions.None)[1].Trim();

                    if (label2.Text != Song)
                    {
                        label2.Text = Song;
                        Weeb.Scrobble(Song, Artist);
                    }
                }
                catch
                {
                    MessageBox.Show("Coudln't parse window title!");
                }
            }
        }

        public void MusicBeeScrobble()
        {
            if (IsLoggedIn)
            {
                String Title = Scrobble.GetPlayerHandle(Scrobble.Player.MUSICBEE);

                if (Title != "MusicBee")
                {
                    try
                    {
                        String Artist = Title.Split(new[] { " - " }, StringSplitOptions.None)[0].Trim();
                        String Song = Title.Split(new[] { " - " }, StringSplitOptions.None)[1].Trim();
                        if (label2.Text != Song)
                        {
                            label2.Text = Song;
                            Weeb.Scrobble(Song, Artist);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't parse the current window title!");
                    }
                }
                else
                {
                    label2.Text = "No song playing!";
                }
            }
        }

        public void SpotifyScrobble()
        {
            if (IsLoggedIn)
            {
                String Title = Scrobble.SpotifyHandle();

                // Hacky ad fixes for personal use until I hook the process
                if (!Title.Contains("Spotify") || !Title.Contains("Marines") || !Title.Contains("Learn More"))
                {
                    String[] WTitle = Title.Split('-');

                    String Song = WTitle[1].TrimEnd(' ');
                    String Artist = WTitle[0].TrimEnd(' ');
                    if (label2.Text != Song)
                    {
                        label2.Text = Song;
                        Weeb.Scrobble(Song, Artist);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsLoggedIn)
            {
                String Login = Weeb.Login(userName.Text, passWord.Text);
                if (Login.Length > 1)
                {
                    label1.Text = "Logged In";
                    IsLoggedIn = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Service == "WinAmp")
                WinAmpScrobble();
            else if (Service == "Spotify")
                SpotifyScrobble();
            else if (Service == "osu!")
                osuScrobble();
            else if (Service == "MusicBee")
                MusicBeeScrobble();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Service = "Spotify";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Service = "osu!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Service = "WinAmp";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Service = "MusicBee";
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Login and toggle your preferred service using the buttons below.", "BETA");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }
    }
}
