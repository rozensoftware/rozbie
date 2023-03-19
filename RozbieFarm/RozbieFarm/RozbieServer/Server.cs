using RozbieFarm.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace RozbieFarm.RozbieServer
{
    public class Server
    {
        private static readonly int SERVER_PORT = 1973;
        private static readonly string SERVER_IP = "192.168.0.22";

        private static MainWindow? _mainWindow;
        private static readonly List<RozbieTask> _tasks = new();
        private static TcpListener? _tcpListener = null;

        private static bool _running = false;

        public static List<RozbieTask> Tasks { get { return _tasks;} }

        public static async Task RunServerAsync(ObservableCollection<RozbieInfo> rozbieList, MainWindow wnd)
        {
            _mainWindow = wnd;

            try
            {
                _tcpListener = new(IPAddress.Parse(SERVER_IP), SERVER_PORT);
                _tcpListener.Start();

                _running = true;

                while (_running)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    var task = new RozbieTask(tcpClient, rozbieList, OnClientResponse);
                    _tasks.Add(task);

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        var rozbieInfo = new RozbieInfo(tcpClient.Client.RemoteEndPoint!.ToString()!, "");
                        rozbieList.Add(rozbieInfo);
                        _mainWindow.Clients.SelectedItem = rozbieInfo;
                    });                    
                }
            }
            catch(SocketException se)
            {
                MessageBox.Show($"Socket exception: {se.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void OnClientResponse(RozbieTask task)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainWindow?.SetInfo(task.GetBuffer);
                _mainWindow?.TextScroll.ScrollToEnd();
            });
        }

        public async static void StopServer()
        {
            _running = false;
            _tcpListener?.Stop();

            foreach (var task in _tasks) 
            {
                await task.DisposeAsync();
            }

            _tasks.Clear();
        }
    }
}
