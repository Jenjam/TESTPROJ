using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using NETGame.CSharp.Entities;
using System.Threading;
using Windows.ApplicationModel.Background;
using System.Threading.Tasks;
using NETGame.CSharp.Client.Services;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace NETGame.CSharp.Client.UWP
{
    public sealed partial class InterfaceGame : Page
    {
        ClientServices services;

        public InterfaceGame() => this.InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            if (e.Parameter is ClientServices)
            {
                var temp = e.Parameter;
                services = (ClientServices)temp;

                txtDateActual.Text = DateTime.Now.ToString("dd-MM-20y");
                txtGameName.Text = services.CurrentGame.GameName;
                txtLogin.Text = services.Company.CompanyName;
                txtMoney.Text = services.Company.MoneyOwned.ToString() + " €";
                txtNbTours.Text = services.CurrentGame.ActualRound.ToString() + " of " + services.CurrentGame.GameTour.ToString();
                txtPrev.Text = "Unknown €";
                lstDeveloperOwned.ItemsSource = services.CurrentGame.DeveloperList;

                DataInitialize();
            }
        }
        public void Actualize(IBackgroundTaskInstance taskInstance)
        {
            do { txtTimeActual.Text = DateTime.Now.ToString("hh:mm:ss tt"); } while (true);
        }

        public async void NewtaskTime()
        {
            while (true)
            {
                Thread.Sleep(200);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { txtTimeActual.Text = DateTime.Now.ToString("HH:mm:ss tt"); });
            }
        }

        public async void DataInitialize()
        {
            await Task.Run(() => NewtaskTime());
            await Task.Run(() => ReadMessage());
        }

        private async void BtPlayClicked(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Login : " + txtLogin.Text);
            await messageDialog.ShowAsync();
        }

        private void DeveloperSelect(object sender, SelectionChangedEventArgs e)
        {
            if(lstDeveloperOwned.SelectedIndex != -1)
            {
                this.Frame.Navigate(typeof(InfoDeveloper), lstDeveloperOwned.SelectedItem);
            }
        }

        //private void FrameTapped(object sender, TappedRoutedEventArgs e) => frameDeveloper.Visibility = Visibility.Collapsed;

        private async void BtDeconexionClicked(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Fermeture de l'application en cours...");
            await messageDialog.ShowAsync();
            App.Current.Exit();
        }

        private void myProjectTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InfoMyProjects), lstProjectOwned.SelectedItem);
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

                    if (receivedRequest.Code == "refreshAvailableDev" && receivedRequest.Type == "client")
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            lstDeveloperOwned.ItemsSource = new List<Developer>();
                            services.CurrentGame.DeveloperList = JsonConvert.DeserializeObject<List<Developer>>(receivedRequest.Payload);
                            lstDeveloperOwned.ItemsSource = services.CurrentGame.DeveloperList;
                        });
                    }

                    if (receivedRequest.Code == "launchGame" && receivedRequest.Type == "server")
                    {

                    }

                    services.Stream.Flush();
                }

                catch (Exception ex)
                {
                    status = true;
                }
            }
        }
    }
}
