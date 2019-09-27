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
    public sealed partial class InfoDeveloper : Page
    {
        public InfoDeveloper() => this.InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            if (e.Parameter is Developer)
            {
                //var login = e.Parameter;
                DataInitializeInfoDeveloper((Developer)e.Parameter);
            }
        }
        //public InfoDeveloper()
        //{

        //    this.InitializeComponent();
        //    DataInitialize();
        //}
        public void DataInitializeInfoDeveloper(Developer developer)
        {
            txtNameInfoDev.Text = developer.Name;
            txtCostInfoDev.Text = developer.CostPerMonth.ToString();
            txtSchoolInfoDev.Text = developer.School;
            txtSpeInfoDev.Text = developer.Spec;
            txtStatusInfoDev.Text = developer.Status.ToString();

            lstCertDev.ItemsSource = new List<Certification>
            {
                new Certification
                {
                   CertificationName = "DIIAGE 1",
                   CertificationMinLevel = 1,
                   CertificationMaxLevel = 3
                },
                new Certification
                {
                   CertificationName = "DIIAGE 2",
                   CertificationMinLevel = 2,
                   CertificationMaxLevel = 5
                },
                new Certification
                {
                   CertificationID = 3,
                   CertificationName = "DIIAGE 3",
                   CertificationMinLevel = 7,
                   CertificationMaxLevel = 8
                }
            };
            lstSkillDev.ItemsSource = new List<Skill>
            {
                new Skill
                {
                   Id = 1,
                   Niveau = 2,
                   SkillName = "Web"
                },
                new Skill
                {
                   Id = 2,
                   Niveau = 1,
                   SkillName = "C#"
                },
                new Skill
                {
                   Id = 3,
                   Niveau = 5,
                   SkillName = "UWP"
                },
            };
        }

        private void imgCloseTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
