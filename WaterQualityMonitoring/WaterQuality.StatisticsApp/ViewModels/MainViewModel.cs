using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WaterQuality.Contracts.DTOs;
using WaterQuality.StatisticsApp.Commands;
using WaterQuality.StatisticsApp.Services;
using WaterQuality.StatisticsApp.Strategies;
using Microsoft.Win32;
using WaterQuality.StatisticsApp.Helpers;

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

        private IStatisticsStrategy selectedStrategy;
        private string statisticsResult;

        public ObservableCollection<IStatisticsStrategy> Strategies { get; set; }
        public ICommand ExportCsvCommand { get; set; }

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

        public IStatisticsStrategy SelectedStrategy
        {
            get { return selectedStrategy; }
            set
            {
                selectedStrategy = value;
                OnPropertyChanged(nameof(SelectedStrategy));
            }
        }

        public string StatisticsResult
        {
            get { return statisticsResult; }
            set
            {
                statisticsResult = value;
                OnPropertyChanged(nameof(StatisticsResult));
            }
        }

        public ICommand CalculateStatisticsCommand { get; set; }

        public MainViewModel()
        {
            wcfClient = new WaterQualityWcfClient();

            Sources = new ObservableCollection<WaterSourceDto>();
            Readings = new ObservableCollection<WaterQualityReadingDto>();
            ReadingsBySourceAndYear = new Dictionary<string, List<WaterQualityReadingDto>>();

            SelectedYear = DateTime.Now.Year;

            LoadSourcesCommand = new RelayCommand(LoadSources);
            LoadReadingsCommand = new RelayCommand(LoadReadings);

            Strategies = new ObservableCollection<IStatisticsStrategy>
            {
                new MedianPHStrategy(),
                new MaxTurbidityStrategy(),
                new UnsafeContaminatedCountStrategy()
            };

            SelectedStrategy = Strategies[0];

            CalculateStatisticsCommand = new RelayCommand(CalculateStatistics);

            ExportCsvCommand = new RelayCommand(ExportCsv);
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

        private void CalculateStatistics()
        {
            if (SelectedStrategy == null)
            {
                MessageBox.Show("Izaberite statističku metodu.");
                return;
            }

            if (Readings == null || Readings.Count == 0)
            {
                MessageBox.Show("Prvo učitajte merenja.");
                return;
            }

            StatisticsResult = SelectedStrategy.Calculate(Readings.ToList());
            StatusMessage = "Statistička obrada je završena.";
        }

        private void ExportCsv()
        {
            if (Readings == null || Readings.Count == 0)
            {
                MessageBox.Show("Prvo učitajte merenja.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "statistics_results.csv"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result != true)
            {
                return;
            }

            CsvExporter.ExportReadings(
                saveFileDialog.FileName,
                Readings.ToList(),
                StatisticsResult);

            StatusMessage = "CSV fajl je uspešno sačuvan.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            
    }
}