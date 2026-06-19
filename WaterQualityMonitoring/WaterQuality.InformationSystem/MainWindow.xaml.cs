using System;
using System.Windows;
using WaterQuality.InformationSystem.Services;
using WaterQuality.InformationSystem.ViewModels;

namespace WaterQuality.InformationSystem
{
    public partial class MainWindow : Window
    {
        private readonly WcfHostService _wcfHostService;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            _wcfHostService = new WcfHostService();

            try
            {
                _wcfHostService.Start();
                Title = "Water Quality Information System - WCF service running";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "WCF service could not be started: " + ex.Message,
                    "WCF error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _wcfHostService.Stop();

            base.OnClosed(e);
        }
    }
}