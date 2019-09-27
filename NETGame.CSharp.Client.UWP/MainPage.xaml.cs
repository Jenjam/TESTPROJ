using NETGame.CSharp.Client.Services;
using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NETGame.CSharp.Client.UWP
{
    public sealed partial class MainPage : Page
    {
        ClientServices services = new ClientServices();

        public MainPage() => this.InitializeComponent();

        // Click on the play button
        private async void BtClicked(object sender, RoutedEventArgs e)
        {
            // Disabling UI elements
            txtIP.IsEnabled = false;
            txtPort.IsEnabled = false;
            txtLogin.IsEnabled = false;
            valid.IsEnabled = false;

            // Shows the progress ring
            progressRing.Visibility = Visibility.Visible;
            progressRing.IsActive = true;

            // Checking if given username is valid
            services.Username = txtLogin.Text;
            // Shows an alert if username is incorrect
            if (services.Username == "" || services.Username.Contains(" "))
            {
                var messageDialog = new MessageDialog("You must type a valid username (no space allowed)!");
                await messageDialog.ShowAsync();

                // Re-enabling UI elements
                txtIP.IsEnabled = true;
                txtPort.IsEnabled = true;
                txtLogin.IsEnabled = true;
                valid.IsEnabled = true;

                // Hiding progressRing
                progressRing.Visibility = Visibility.Collapsed;
                progressRing.IsActive = false;
            }

            else
            {
                // Checking if given IP is valid
                try
                {
                    services.IpAddr = IPAddress.Parse(txtIP.Text);

                    // Checking if given port is valid
                    try
                    {
                        services.Port = Int32.Parse(txtPort.Text);

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.clientListBox.Items.Add("Trying to connect to the game server...");
                        });
                        await Task.Run(() => ConnectToServer());
                    }

                    catch
                    {
                        var messageDialog = new MessageDialog("You must type a valid port!");
                        await messageDialog.ShowAsync();

                        // Re-enabling UI elements
                        txtIP.IsEnabled = true;
                        txtPort.IsEnabled = true;
                        txtLogin.IsEnabled = true;
                        valid.IsEnabled = true;

                        // Hiding progressRing
                        progressRing.Visibility = Visibility.Collapsed;
                        progressRing.IsActive = false;
                    }
                }

                catch
                {
                    var messageDialog = new MessageDialog("You must type a valid IP Address!");
                    await messageDialog.ShowAsync();

                    // Re-enabling UI elements
                    txtIP.IsEnabled = true;
                    txtPort.IsEnabled = true;
                    txtLogin.IsEnabled = true;
                    valid.IsEnabled = true;

                    // Hiding progressRing
                    progressRing.Visibility = Visibility.Collapsed;
                    progressRing.IsActive = false;
                }
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
                    this.clientListBox.Items.Add("Connected to the server! Waiting for the game to launch...");
                });

                await Task.Run(() => ReadMessage());
            }

            catch (Exception ex)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.clientListBox.Items.Add("Error while connecting to the server: " + ex.Message);
                });
            }
        }

        public void TestRequest()
        {
            Request firstRequest = new Request("launchGame", "client", "launchGame");
            var serializedObject = JsonConvert.SerializeObject(firstRequest);
            // Encrypting message using our own Cryptography Class
            string encryptedString = Cryptography.EnryptString(serializedObject);
            Byte[] dataLong = Encoding.UTF8.GetBytes(encryptedString);

            // Sending data through Data Stream
            services.Stream.Write(dataLong, 0, dataLong.Length);

            services.Stream.Flush();
        }

        public void TestRequest(string functionCode, string payload)
        {
            Request firstRequest = new Request(functionCode, "client", payload);
            var serializedObject = JsonConvert.SerializeObject(firstRequest);
            // Encrypting message using our own Cryptography Class
            string encryptedString = Cryptography.EnryptString(serializedObject);
            Byte[] dataLong = Encoding.UTF8.GetBytes(encryptedString);

            // Sending data through Data Stream
            services.Stream.Write(dataLong, 0, dataLong.Length);

            services.Stream.Flush();
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
                    Request receivedRequest = services.FunctionCode(Cryptography.DecryptString(response));
                    string log;

                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.clientListBox.Items.Add("Received from server: " + Cryptography.DecryptString(response));
                    });

                    if (receivedRequest.Code == "launchGame" && receivedRequest.Type == "server")
                    {
                        log = "Launching the game...";
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            status = true;
                            this.Frame.Navigate(typeof(InterfaceGame), services);
                        });
                    }

                    else
                    {
                        log = Cryptography.DecryptString(response);
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.clientListBox.Items.Add(log);
                        });
                    }

                    services.Stream.Flush();
                }

                catch (Exception ex)
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.clientListBox.Items.Add("CRITICAL ERROR: " + ex.Message);
                        // Re-enabling UI elements
                        txtIP.IsEnabled = true;
                        txtPort.IsEnabled = true;
                        txtLogin.IsEnabled = true;
                        valid.IsEnabled = true;

                        // Hiding progressRing
                        progressRing.Visibility = Visibility.Collapsed;
                        progressRing.IsActive = false;
                    });

                    status = true;
                }
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            TestRequest("createUser", txtLogin.Text);
        }
    }
}