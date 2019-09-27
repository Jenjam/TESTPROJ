using NETGame.CSharp.Client.Services;
using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NETGame.CSharp.Server.UWP
{
    public sealed partial class MainPage : Page
    {
        AdminServices services = new AdminServices();

        public MainPage() => this.InitializeComponent();

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            // Disabling UI elements
            txtIpAddr.IsEnabled = false;
            txtPort.IsEnabled = false;
            txtGameTitle.IsEnabled = false;
            sliderNbMoney.IsEnabled = false;
            sliderNbPlayer.IsEnabled = false;
            sliderNbRounds.IsEnabled = false;
            sliderNbDevGenerated.IsEnabled = false;
            sliderProjectGenerated.IsEnabled = false;
            btnConnect.IsEnabled = false;

            // Checking if given IP is valid
            try
            {
                services.IpAddr = IPAddress.Parse(txtIpAddr.Text);

                // Checking if given port is valid
                try
                {
                    // Checking if game title is not empty
                    if(txtGameTitle.Text != "")
                    {
                        // Getting every game parameter
                        services.CurrentGame = new Game(txtGameTitle.Text, (int)sliderNbRounds.Value, (float)sliderNbMoney.Value, (int)sliderNbPlayer.Value, (int)sliderNbDevGenerated.Value, (int)sliderProjectGenerated.Value);
                        services.Port = Int32.Parse(txtPort.Text);

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.listBoxLogs.Items.Add("Trying to connect to the game server...");
                        });
                        await Task.Run(() => ConnectToServer());
                    }

                    else
                    {
                        var messageDialog = new MessageDialog("You must type a valid game title!");
                        await messageDialog.ShowAsync();

                        // Re-enabling UI elements
                        txtIpAddr.IsEnabled = true;
                        txtPort.IsEnabled = true;
                        txtGameTitle.IsEnabled = true;
                        sliderNbMoney.IsEnabled = true;
                        sliderNbPlayer.IsEnabled = true;
                        sliderNbRounds.IsEnabled = true;
                        sliderNbDevGenerated.IsEnabled = true;
                        sliderProjectGenerated.IsEnabled = true;
                        btnConnect.IsEnabled = true;
                    }
                }

                catch
                {
                    var messageDialog = new MessageDialog("You must type a valid port!");
                    await messageDialog.ShowAsync();

                    // Re-enabling UI elements
                    txtIpAddr.IsEnabled = true;
                    txtPort.IsEnabled = true;
                    txtGameTitle.IsEnabled = true;
                    sliderNbMoney.IsEnabled = true;
                    sliderNbPlayer.IsEnabled = true;
                    sliderNbRounds.IsEnabled = true;
                    sliderNbDevGenerated.IsEnabled = true;
                    sliderProjectGenerated.IsEnabled = true;
                    btnConnect.IsEnabled = true;
                }
            }

            catch
            {
                var messageDialog = new MessageDialog("You must type a valid IP Address!");
                await messageDialog.ShowAsync();

                // Re-enabling UI elements
                txtIpAddr.IsEnabled = true;
                txtPort.IsEnabled = true;
                txtGameTitle.IsEnabled = true;
                sliderNbMoney.IsEnabled = true;
                sliderNbPlayer.IsEnabled = true;
                sliderNbRounds.IsEnabled = true;
                sliderNbDevGenerated.IsEnabled = true;
                sliderProjectGenerated.IsEnabled = true;
                btnConnect.IsEnabled = true;
            }
        }

        public async void ConnectToServer()
        {
            try
            {
                services.TcpClient = new TcpClient(services.IpAddr.ToString(), services.Port);
                services.Stream = services.TcpClient.GetStream();

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.listBoxLogs.Items.Add("Connected to the server!");
                });

                await Task.Run(() => ReadMessage());
            }

            catch (Exception ex)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.listBoxLogs.Items.Add("Error while connecting to the server: " + ex.Message);
                });
            }
        }

        private async void ReadMessage()
        {
            var status = false;
            while (!status)
            {
                try
                {
                    Int32 bytes = 0;
                    Byte[] dataLong = new Byte[4096];
                    bytes = await services.Stream.ReadAsync(dataLong, 0, dataLong.Length);
                    string response = Encoding.UTF8.GetString(dataLong, 0, bytes);

                    Request receivedRequest = JsonConvert.DeserializeObject<Request>(Cryptography.DecryptString(response));

                    // Litérallement functionCode
                    if (receivedRequest.Code == "getGame" && receivedRequest.Type == "server")
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.listBoxLogs.Items.Add("getGame request received from the server.");
                            services.CurrentGame = JsonConvert.DeserializeObject<Game>(receivedRequest.Payload);
                            txtGameTitle.Text = services.CurrentGame.GameName;
                            sliderNbMoney.Value = services.CurrentGame.GameStartMoney;
                            sliderNbPlayer.Value = services.CurrentGame.PlayerNeeded;
                            sliderNbRounds.Value = services.CurrentGame.GameTour;
                            sliderNbDevGenerated.Value = services.CurrentGame.DeveloperPerRound;
                            sliderProjectGenerated.Value = services.CurrentGame.ProjectPerRound;
                        });
                    }

                    receivedRequest = services.FunctionCode(Cryptography.DecryptString(response));

                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.listBoxLogs.Items.Add("Received from server: " + Cryptography.DecryptString(response));
                    });

                    services.Stream.Flush();
                }

                catch (Exception ex)
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.listBoxLogs.Items.Add("CRITICAL ERROR: " + ex.Message);
                    });

                    status = true;
                }
            }
        }

        private void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            Request firstRequest = new Request(txtCmd.Text, "admin", "");
            if (txtCmd.Text == "setGame")
            {
                firstRequest.Payload = JsonConvert.SerializeObject(services.CurrentGame);
            }

            var serializedObject = JsonConvert.SerializeObject(firstRequest);
            // Encrypting message using our own Cryptography Class
            string encryptedString = Cryptography.EnryptString(serializedObject);
            Byte[] dataLong = Encoding.UTF8.GetBytes(encryptedString);

            // Sending data through Data Stream
            services.Stream.Write(dataLong, 0, dataLong.Length);

            services.Stream.Flush();
        }
    }
}
