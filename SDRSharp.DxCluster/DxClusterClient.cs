using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace SDRSharp.DxCluster
{

    internal class DxClusterClient
    {

        DxClusterClientWrapper _client;
        public List<Dictionary<string, string>> lastEntries;
        private string _mycallsign;
        public bool connected;

        public DxClusterClient(string mycallsign) 
        {
            _mycallsign = mycallsign;
            _client = new DxClusterClientWrapper();
            _client.DataReceived += _client_DataReceived;

           
            // List to store the last 100 entries
            lastEntries = new List<Dictionary<string, string>>();
        }

        public void disconnect()
        {
            _client.disconnect();
        }

        public void _client_DataReceived(object sender, string data)
        {
            Debug.WriteLine(data);
            connected = _client.connected;

            if (data.Contains("DX de"))
            {
                string[] columns = data.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (columns.Length >= 6)
                {

                    //Save to Dictionary
                    saveToDictionary(columns[3], columns[4], columns[5]);
                }
            }
 
            if (data.Contains("Please enter your call") || data.Contains("login"))
            {
                _client.writer.WriteLineAsync(_mycallsign);
            }

        }

        private void saveToDictionary(string frequency, string callsign, string mode)
        {

            // Check if same callsign and near frequency, don't save
            foreach (var spot in lastEntries)
            {
                if (spot["callsign"].Contains(callsign))
                {
                    float.TryParse(spot["callsign"], out float frequency_spot);
                    float.TryParse(frequency, out float frequency_save);
                    if (frequency_save < 10 || frequency_save > 10)
                    {
                        return; // don't save
                    }
                }
            }

            // Store the entry in a dictionary
            var entry = new Dictionary<string, string>
            {
                        { "frequency", frequency },
                        { "callsign", callsign },
                        { "mode", mode },
                        { "y_position", "" },
                        { "x_position", "" }

            };

            lastEntries.Add(entry);

            // Remove the oldest entry if more than 100 entries are stored
            if (lastEntries.Count > 1000)
            {
                lastEntries.RemoveAt(0);
            }
        }
    }

   internal class DxClusterClientWrapper
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        public StreamWriter writer;
        public bool connected;

        public event EventHandler<string> DataReceived;

        public DxClusterClientWrapper()
        {
            try
            {
                client = new TcpClient("dxc.ve7cc.net", 23);
                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.ASCII);
                writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
                Task.Run(ReceiveDataAsync);
            }
            catch(Exception ex)
            {

            }
        }

        public void disconnect()
        {
            client.Close();
            connected = false;
        }

        private async Task ReceiveDataAsync()
        {
            try
            {
                while (true)
                {
                    connected = client.Connected;
                    string data = await reader.ReadLineAsync();
                    if (data == null)
                    {
                        Debug.WriteLine("Close connection");
                        connected = false;
                        break; // Connection closed by the server
                        
                    }
                    DataReceived?.Invoke(this, data);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                connected = false;
                DataReceived?.Invoke(this, ex.Message);
            }
        }
    }
}
