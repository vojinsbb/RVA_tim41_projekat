using System.Windows;
using WaterQuality.InformationSystem.ViewModels;

namespace WaterQuality.InformationSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}