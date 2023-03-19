using RozbieFarm.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RozbieFarm.RozbieServer
{
    public class RozbieTask : IAsyncDisposable
    {
        private static readonly int MAX_BUFFER = 1024;

        private readonly TcpClient _tcpClient;
        private readonly Task _task;
        private readonly StringBuilder _stringBuffer = new();
        private readonly ObservableCollection<RozbieInfo> _rozbieList;
        private NetworkStream? _stream = null;
        private readonly Action<RozbieTask>? _clientCallback;

        private readonly string _ip;
        private bool _running;
        private readonly byte[] dataReceived = new byte[MAX_BUFFER];

        public string GetBuffer => _stringBuffer.ToString();
        public string IP => _ip;
        public bool IsRunning => _running;

        public RozbieTask(TcpClient tcpClient, ObservableCollection<RozbieInfo> rozbieList, Action<RozbieTask> clientCallback)
        {
            _rozbieList = rozbieList;
            _running = true;
            _tcpClient = tcpClient;
            _ip = tcpClient.Client.RemoteEndPoint!.ToString()!;
            _clientCallback = clientCallback;
            _task = Task.Run(async () => await ReadClientAsync());
        }

        private async Task ReadClientAsync()
        {
            try
            {
                _stream = _tcpClient.GetStream();

                while (_running)
                {
                    int bytesRead = await _stream.ReadAsync(dataReceived);
                    if(bytesRead > 0)
                    {
                        string line = Encoding.UTF8.GetString(dataReceived, 0, bytesRead);

                        if (!string.IsNullOrEmpty(line))
                        {
                            _stringBuffer.AppendLine(line);
                            _clientCallback?.Invoke(this);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                //TODO: Add logging
            }
            finally
            {
                await DisposeAsync();
            }
        }

        public async Task SendClientAsync(string txt)
        {
            if(_stream == null)
            {
                MessageBox.Show($"Stream is not created", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                byte[] dataToSend = Encoding.UTF8.GetBytes(txt);
                await _stream.WriteAsync(dataToSend);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Sending data to client failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var item in _rozbieList)
            {
                if (item.IP!.Equals(_ip))
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _rozbieList.Remove(item);
                    });

                    break;
                }
            }

            _running = false;
            _ = (_stream?.DisposeAsync());
            _tcpClient.Close();
            _task.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
