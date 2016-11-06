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

        public mainForm()
        {
            InitializeComponent();
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
    }
}
