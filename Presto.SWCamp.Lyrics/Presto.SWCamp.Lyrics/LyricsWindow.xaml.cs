﻿using System;
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
using System.Text.RegularExpressions;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricsWindow : Window
    {
        string[] lines = File.ReadAllLines(@"C:\Users\김형래\Desktop\Musics\TWICE - Dance The Night Away.lrc");
        string pattern = @"\[([\d]{2,}:[\d]{2}.[\d]{2})\]";
        //List<KeyValuePair<TimeSpan, string>> list = new List<KeyValuePair<TimeSpan, string>>();

        LyicsManager manager = new LyicsManager();

        public LyricsWindow()
        {
            InitializeComponent();
            


            for (int i = 3; i < lines.Length; i++)
            {
                string[] a = Regex.Split(lines[i], pattern);

                //앞선 가사의 시간이 같은 경우 통합
                // if (i != 3 && manager.lyics[i-1].time == TimeSpan.ParseExact(a[1], @"mm\:ss\.ff", CultureInfo.InvariantCulture))
                //     manager.lyics[i].ly += "\n" + a[2];
                 
                manager.lyics.Add(new Lyics
                {
                    time = TimeSpan.ParseExact(a[1], @"mm\:ss\.ff", CultureInfo.InvariantCulture),
                    ly = a[2]
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
            for (int i = 3; i < lines.Length; i++)
            {
                if (manager.lyics[i].time == TimeSpan.FromMilliseconds(PrestoSDK.PrestoService.Player.Position))
                    lyrics.Text = manager.lyics[0].time.ToString();
            }
        }

    }
}
