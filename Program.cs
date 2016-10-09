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
 * like, at fucking all. I really don't
 * like the idea of JIT compiling, or .net
 * but I mean yeah I wrote this lol
 * no fucking clue if this is terrible or not
 * I mean it works and performs fine but
 * like, the code is probably awful?
 * I've literally only touched this language
 * twice before.
 */


namespace IDontKnowCSharp
{
    class Scrobble
    {
        public enum Player
        {
            SPOTIFY,
            MUSICBEE
        }

        public String Post(String Url, String PostData, String Token = null)
        {
            WebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (Token != null)
                request.Headers.Add("Authorization", string.Format("Token {0}", Token));

            var Data = Encoding.ASCII.GetBytes(PostData);

            using (var Stream = request.GetRequestStream())
            {
                Stream.Write(Data, 0, Data.Length);
            }

            var Response = (HttpWebResponse)request.GetResponse();
            var ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            return ResponseString;
        }

        /// <summary>
        /// Uses previously opened handle of Spotify.exe to return its' window title
        /// </summary>
        /// <returns>Current title of Spotify window</returns>
        public String SpotifyHandle()
        {
            Process Spotify = Process.GetProcessesByName("Spotify")[0];
            return Spotify.MainWindowTitle;
        }

        public String MusicBeeHandle()
        {
            Process MusicBee = Process.GetProcessesByName("MusicBee")[0];
            return MusicBee.MainWindowTitle;
        }

        public String GetPlayerHandle(Player player)
        {
            switch (player)
            {
                case Player.SPOTIFY:
                    return SpotifyHandle();
                case Player.MUSICBEE:
                    return MusicBeeHandle();
            }
            return null; // temp
        }
    }

    public class WeebFm
    {
        Scrobble Weeb = new Scrobble();
        public String Token = String.Empty;
        public String ApiAuth = "http://localhost:8000/api/api-token-auth/";
        public String ScrobbleEndPoint = "http://localhost:8000/api/scrobbles/";

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
        /// <summary>
        /// This shit right here keep ya weebfm scrobbles supa fr3$h for Spot1fy on winbl0wz
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}