using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDontKnowCSharp
{
    public partial class Form1 : Form
    {
        Scrobble Scrobble = new Scrobble();
        Spotify Spotify = new IDontKnowCSharp.Spotify();
        public bool IsLoggedIn = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsLoggedIn)
            {
                String Login = Spotify.Login(userName.Text, passWord.Text);
                if (Login.Length > 1)
                {
                    label1.Text = "Logged In";
                    IsLoggedIn = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                String Title = Scrobble.SpotifyHandle();

                if (!Title.Contains("Spotify"))
                {
                    String[] WTitle = Title.Split('-');

                    String Song = WTitle[1].TrimEnd(' ');
                    String Artist = WTitle[0].TrimEnd(' ');
                    if (label2.Text != Song)
                    {
                        label2.Text = Song;
                        Spotify.Scrobble(Song, Artist);
                    }
                } else
                {
                    MessageBox.Show("Wow man");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
