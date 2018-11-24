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
        SortedList<double, string> list = new SortedList<double, string>();
        
        public LyricsWindow()
        {
            InitializeComponent();
            
            for (int i = 3; i < lines.Length; i++)
            {
                string[] a = Regex.Split(lines[i], pattern);
                
                //앞선 가사의 시간이 같은 경우 통합
                if (i != 3 && list.Keys[i - 4] == TimeSpan.ParseExact(a[1], @"mm\:ss\.ff", CultureInfo.InvariantCulture).TotalMilliseconds)
                     list.Values[i - 4] += "\n" + a[2];
                list.Add(TimeSpan.ParseExact(a[1], @"mm\:ss\.ff", CultureInfo.InvariantCulture).TotalMilliseconds, a[2]);
               
            }
            
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double CURRENT = PrestoSDK.PrestoService.Player.Position;
            for (int i = 0; i < list.Keys.Count; i++)
            {
                if (list.Keys[i] <= CURRENT && list.Keys[i + 1] > CURRENT)
                {
                    lyrics.Text = list.Values[i];
                }
            }          
        }
    }
}
