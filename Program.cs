using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * I seriously don't know this language
 * like, at fucking all. 
 */


namespace IDontKnowCSharp
{
    class Scrobble
    {
        public enum Player
        {
            SPOTIFY,
            MUSICBEE,
            OSU,
            WINAMP,
            FOOBAR_COLUMNSUI
        }



        public String Post(String Url, String PostData, String Token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";


            if (Token != null)
                request.Headers.Add("Authorization", string.Format("Token {0}", Token));

            var Data = Encoding.ASCII.GetBytes(PostData);

            using (var Stream = request.GetRequestStream())
                Stream.Write(Data, 0, Data.Length);

            try {
                var Response = (HttpWebResponse)request.GetResponse();
                var ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                return ResponseString;
            }
            catch {
                MessageBox.Show("Login failed");
                return null;
            }
}
        public String DynamicHandle(String Name)
        {
            try
            {
                Process proc = Process.GetProcessesByName(Name)[0];
                return proc.MainWindowTitle;
            } catch
            {
                return string.Format("{0} isn't open!", Name);
            }
        }

        /// <summary>
        /// Uses previously opened handle of Spotify.exe to return its' window title
        /// </summary>
        /// <returns>Current title of Spotify window</returns>
        public String SpotifyHandle()
        {
            return DynamicHandle("Spotify");
        }

        public String MusicBeeHandle()
        {
            return DynamicHandle("MusicBee");
        }

        public String OsuHandle()
        {
            return DynamicHandle("osu!");
        }

        public String WinAmpHandle()
        {
            return DynamicHandle("winamp");
        }

        public String FoobarHandle()
        {
            return DynamicHandle("foobar");
        }

        public String GetPlayerHandle(Player player)
        {
            switch (player)
            {
                case Player.SPOTIFY:
                    return SpotifyHandle();
                case Player.MUSICBEE:
                    return MusicBeeHandle();
                case Player.OSU:
                    return OsuHandle();
                case Player.WINAMP:
                    return WinAmpHandle();
            }
            return null;
        }
    }

    public class WeebFm
    {
        Scrobble Weeb = new Scrobble();
        public String Token = String.Empty;
        public String ApiAuth = "https://wilt.fm/api/api-token-auth/";
        public String ScrobbleEndPoint = "https://wilt.fm/api/scrobbles/";

        public String Login(string Username, string Password)
        {
            String LoginDetails = string.Format("username={0}&password={1}", Username, Password);
            Token = Weeb.Post(ApiAuth, LoginDetails).Split(':')[1].Split('"')[1].Split('"')[0]; // Fucking lol
            return Token;
        }

        public void Scrobble(String song, String artist)
        {
            Weeb.Post(
                ScrobbleEndPoint,
                string.Format("song={0}&artist={1}", song, artist),
                Token);
        }
    }

    public class MusicBee
    {
        Scrobble Scrobble = new Scrobble();
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
        }
    }
}