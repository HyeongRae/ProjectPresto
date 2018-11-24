using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using Presto.SDK;
using System.Globalization;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricsWindow : Window
    {
        string pattern = "([\\.? |]\\.? )";
        LyicsManager manager = new LyicsManager();

        public LyricsWindow()
        {
            string[] lines = File.ReadAllLines(@"C:\Users\김형래\Desktop\Musics\볼빨간사춘기 - 여행.lrc");
            InitializeComponent();
            
            foreach (var line in lines)
            {
                manager.lyics.Add(new Lyics
                {
                    time = TimeSpan.ParseExact(line[0].ToString(), @"mm\:ss\.ff", CultureInfo.InvariantCulture),
                    ly = line[1].ToString()
                });
            }

            //var time = TimeSpan.ParseExact(manager.lyics[0].time, @"mm\:ss\.ff", CultureInfo.InvariantCulture);
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //lyrics.Text = PrestoSDK.PrestoService.Player.Position.ToString();
            lyrics.Text = manager.lyics[0].time.ToString();
        }

    }
}
