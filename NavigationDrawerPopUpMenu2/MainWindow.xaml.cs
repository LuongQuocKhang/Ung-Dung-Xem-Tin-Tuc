using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NavigationDrawerPopUpMenu2
{

    public partial class MainWindow : Window
    {
        ObservableCollection<object> listcongnghe, listtuyendung;
        HttpClient httpclient;
        HttpClientHandler handler;
        CookieContainer cookie;
        public MainWindow()
        {
            InitializeComponent();

            listcongnghe = new ObservableCollection<object>();


            listtuyendung = new ObservableCollection<object>();

            cookie = new CookieContainer();
            handler = new HttpClientHandler()
            {
                CookieContainer = cookie,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                AllowAutoRedirect = true,
                UseDefaultCredentials = false
            };
            httpclient = new HttpClient(handler);
            web.WebView.NewWindow += (sender, e) =>
            {
                Process.Start(e.TargetUrl);
            };

        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemTech":

                    listcongnghe.Clear();
                    new Task(() =>
                    {
                        Crawl_Tech_Data("https://techtalk.vn/tech");
                    }).Start();
                    new Task(() =>
                    {
                        Crawl_Tech_Data("https://topdev.vn/blog/category/lap-trinh/");
                    }).Start();
                    new Task(() =>
                    {
                        Crawl_Tech_Data("https://topdev.vn/blog/category/cong-nghe/");
                    }).Start();
                    break;
                case "ItemJob":
                    listtuyendung.Clear();
                    new Task(() =>
                    {
                        Crawl_Job_Data("https://topdev.vn/it-jobs/");
                    }).Start();

                    break;
                case "ItemExit":
                    Application.Current.Shutdown();
                    break;
                default:
                    break;
            }
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void Crawl_Job_Data(string url)
        {
            string html = Crawl.Instance.CrawlDataFromUrl(url, httpclient);
            var Ne = Regex.Matches(html, @"<div class=""clearfix box-top-level""(.*?)</div>(.*?)</div>(.*?)</div>(.*?)</div>(.*?)</div>(.*?)</div>", RegexOptions.Singleline);

            foreach (var item in Ne)
            {
                Job jobs = new Job();
                var LinkAndImage = Regex.Match(item.ToString(), @"<div class=""logo"">(.*?)</div>", RegexOptions.Singleline);
                // lấy image
                string image = Regex.Match(LinkAndImage.ToString(), @"src=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"src=""", "").Replace(@"""", "");
                jobs.Image = image;
                // lấy link,title
                var Links = Regex.Match(item.ToString(), @"<div class=""job-item-info relative"">(.*?)</div>", RegexOptions.Singleline);

                string link = Regex.Match(Links.ToString(), @"href=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"href=""", "").Replace(@"""", "");
                string temp = Regex.Match(Links.ToString(), @"<a (.*?)</a>", RegexOptions.Singleline).Value;
                string title = temp.Substring(temp.IndexOf('>') + 1).Replace("</a>", "").Trim();
                jobs.Link = link;
                jobs.Title = title;
                //lấy company
                string company = Regex.Match(Links.ToString(), @"<div class=""company"">(.*?)</div>", RegexOptions.Singleline).Value.Replace(@"<div class=""company"">", "").Replace(@"""", "").Replace("</div>", "");
                jobs.Content = company;
                //lấy location
                string temp_locate = Regex.Match(item.ToString(), @"<div class=""extra-info location text-clip"">(.*?)</div>", RegexOptions.Singleline).Value;
                string locations = Regex.Match(temp_locate.ToString(), @"<span>(.*?)</span>", RegexOptions.Singleline).Value.Replace(@"<span>", "").Replace(@"""", "").Replace("</span>", "");

                jobs.Location = locations;
                //lấy salary
                string info_salary = Regex.Match(item.ToString(), @"<div class=""extra-info salary"">(.*?)</div>", RegexOptions.Singleline).Value;
                string temp_salary = Regex.Match(info_salary.ToString(), @" <a (.*?)</a>", RegexOptions.Singleline).Value.Replace(@"<a ", "").Replace(@"""", "").Replace("</a>", "");
                string salary = temp_salary.Substring(temp_salary.IndexOf('>') + 1);
                jobs.Money = salary;
                this.Dispatcher.Invoke(() => { ListViewNews.Items.Add(jobs); });
                ListViewNews.AddHandler(ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(job_MouseDoubleClick));
            }

        }
        public void job_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string link = (ListViewNews.SelectedItem as Job).Link;
            web.WebView.Url = null;
            web.WebView.LoadUrl(link);
        }

        public void Crawl_Tech_Data(string url)
        {
            string html = Crawl.Instance.CrawlDataFromUrl(url, httpclient);
            var News = Regex.Matches(html, @"<div class=""(td_module_10|td_module_mx2|td_module_mx8) td_module_wrap td-animation-stack""(.*?)</div>(.*?)</div>(.*?)</div>", RegexOptions.Singleline);


            foreach (var item in News)
            {
                New congnghe = new New();
                var LinkAndImage = Regex.Match(item.ToString(), @"<div class=""td-module-thumb"">(.*?)</div>", RegexOptions.Singleline);

                // lấy link
                string link = Regex.Match(LinkAndImage.ToString(), @"href=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"href=""", "").Replace(@"""", "");
                congnghe.Link = link;
                // lấy hình ảnh
                string image = Regex.Match(LinkAndImage.ToString(), @"src=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"src=""", "").Replace(@"""", "");
                congnghe.Image = image;
                // lấy title
                string title = Regex.Match(LinkAndImage.ToString(), @"title=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"title=""", "").Replace(@"""", "");
                congnghe.Title = title;
                // lấy time
                string Temp = Regex.Match(item.ToString(), @"<time class=""entry-date updated td-module-date""(.*?)>(.*?)</time>", RegexOptions.Singleline).Value;
                string time = Temp.Substring(Temp.IndexOf('>') + 1).Replace("</time>", "");
                congnghe.Time = time;
                // lấy content
                string Temp_content = Regex.Match(item.ToString(), @"<div class=""td-excerpt"">(.*?)\s</div>", RegexOptions.Singleline).Value;
                string content = Temp_content.Substring(Temp_content.IndexOf('>') + 1).Replace("</div>", "").Trim();
                congnghe.Content = content;
                this.Dispatcher.Invoke(() => { ListViewNews.Items.Add(congnghe); });

            }
            ListViewNews.AddHandler(ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(item_MouseDoubleClick));
        }
        public void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string link = (ListViewNews.SelectedItem as New).Link;
            web.WebView.Url = null;
            web.WebView.LoadUrl(link);
        }
    }
}