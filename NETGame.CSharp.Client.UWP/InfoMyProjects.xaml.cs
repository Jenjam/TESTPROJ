using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Popups;
using NETGame.CSharp.Entities;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace NETGame.CSharp.Client.UWP
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class InfoMyProjects : Page
    {
        public InfoMyProjects() => this.InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            if (e.Parameter is Project)
            {
                //var login = e.Parameter;
                DataInitializeInfoMyProject((Project)e.Parameter);
            }
            //if (e.Parameter is Developer)
            //{
            //    DataInitializeInfoMyProject((Developer)e.Parameter);
            //}
        }

        public void DataInitializeInfoMyProject(Project project)
        {
            progressMyProject.Value = 80;
            txtIDMyProject.Text = project.PlayerID.ToString();
            txtNameMyProject.Text = project.ProjectName;
            txtNumberRoundsTotal.Text = project.Duration.ToString();
            txtRemunerationMyProject.Text = "1.000";


        }

        private void btRturnProjectClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
