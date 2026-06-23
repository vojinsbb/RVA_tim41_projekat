using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WaterQuality.Contracts.DTOs;
using WaterQuality.StatisticsApp.Commands;
using WaterQuality.StatisticsApp.Services;

namespace WaterQuality.StatisticsApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WaterQualityWcfClient wcfClient;

        private WaterSourceDto selectedSource;
        private int selectedYear;
        private string statusMessage;

        public ObservableCollection<WaterSourceDto> Sources { get; set; }
        public ObservableCollection<WaterQualityReadingDto> Readings { get; set; }

        public Dictionary<string, List<WaterQualityReadingDto>> ReadingsBySourceAndYear { get; set; }

        public ICommand LoadSourcesCommand { get; set; }
        public ICommand LoadReadingsCommand { get; set; }

        public WaterSourceDto SelectedSource
        {
            get { return selectedSource; }
            set
            {
                selectedSource = value;
                OnPropertyChanged(nameof(SelectedSource));
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public MainViewModel()
        {
            wcfClient = new WaterQualityWcfClient();

            Sources = new ObservableCollection<WaterSourceDto>();
            Readings = new ObservableCollection<WaterQualityReadingDto>();
            ReadingsBySourceAndYear = new Dictionary<string, List<WaterQualityReadingDto>>();

            SelectedYear = DateTime.Now.Year;

            LoadSourcesCommand = new RelayCommand(LoadSources);
            LoadReadingsCommand = new RelayCommand(LoadReadings);
        }

        private void LoadSources()
        {
            try
            {
                Sources.Clear();

                var sources = wcfClient.GetAllSources();

                foreach (var source in sources)
                {
                    Sources.Add(source);
                }

                StatusMessage = "Izvori su uspešno učitani.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška pri učitavanju izvora.";
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadReadings()
        {
            if (SelectedSource == null)
            {
                MessageBox.Show("Izaberite izvor vode.");
                return;
            }

            try
            {
                Readings.Clear();

                var readings = wcfClient.GetReadingsBySourceAndYear(SelectedSource.Id, SelectedYear);

                foreach (var reading in readings)
                {
                    Readings.Add(reading);
                }

                string key = SelectedSource.Id + "-" + SelectedYear;

                if (ReadingsBySourceAndYear.ContainsKey(key))
                {
                    ReadingsBySourceAndYear[key] = readings;
                }
                else
                {
                    ReadingsBySourceAndYear.Add(key, readings);
                }

                StatusMessage = "Merenja su uspešno učitana.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Greška pri učitavanju merenja.";
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}