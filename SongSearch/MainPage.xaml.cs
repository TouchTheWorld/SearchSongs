using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace SongSearch
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string search_api = "http://i.y.qq.com/s.music/fcgi-bin/search_for_qq_cp?g_tk=5381&uin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=h5&needNewCode=1&w={0}&t=0&flag=1&ie=utf-8&sem=1&aggr=0&p=1&remoteplace=txt.mqq.all&_=1460982060643";
        private List<string> songs = new List<string>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> songs = new List<string>();
            if (SearchText.Text != "")
            {
                Uri uri = new Uri(search_api.Replace("{0}", SearchText.Text));
                string result = null;
                try
                {
                    HttpClient httpclient = new HttpClient();
                    HttpResponseMessage response = await httpclient.GetAsync(uri);
                    result = await response.Content.ReadAsStringAsync();
                    result = result.Replace("callback(", "");
                    result = result.Replace(")", "");
                    Regex reg = new Regex("(songname\":\")" + "[^\"]+" + "\"");
                    Match m = reg.Match(result);
                    while (m.Success)
                    {
                        string[] s = m.Value.Split('\"');
                        songs.Add(s[2]);
                        m = m.NextMatch();
                    }
                }
                catch(Exception ex)
                {

                }
                SongsList.ItemsSource = songs;
            }
        }
    }
}
