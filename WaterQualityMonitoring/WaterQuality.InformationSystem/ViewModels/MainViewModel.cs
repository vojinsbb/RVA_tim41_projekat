using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WaterQuality.InformationSystem.Commands;
using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;
using WaterQuality.InformationSystem.Services;

namespace WaterQuality.InformationSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly IWaterQualityReadingRepository _readingRepository;

        private WaterSource _selectedWaterSource;

        private string _sourceName;
        private string _sourceLocation;
        private string _sourceType;
        private string _sourceMunicipality;
        private string _sourceCapacityM3;

        public ObservableCollection<WaterSource> WaterSources { get; set; }

        public ObservableCollection<WaterQualityReading> WaterQualityReadings { get; set; }

        public WaterSource SelectedWaterSource
        {
            get
            {
                return _selectedWaterSource;
            }
            set
            {
                _selectedWaterSource = value;
                OnPropertyChanged(nameof(SelectedWaterSource));

                if (_selectedWaterSource != null)
                {
                    FillSourceForm(_selectedWaterSource);
                }
            }
        }

        public string SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                _sourceName = value;
                OnPropertyChanged(nameof(SourceName));
            }
        }

        public string SourceLocation
        {
            get
            {
                return _sourceLocation;
            }
            set
            {
                _sourceLocation = value;
                OnPropertyChanged(nameof(SourceLocation));
            }
        }

        public string SourceType
        {
            get
            {
                return _sourceType;
            }
            set
            {
                _sourceType = value;
                OnPropertyChanged(nameof(SourceType));
            }
        }

        public string SourceMunicipality
        {
            get
            {
                return _sourceMunicipality;
            }
            set
            {
                _sourceMunicipality = value;
                OnPropertyChanged(nameof(SourceMunicipality));
            }
        }

        public string SourceCapacityM3
        {
            get
            {
                return _sourceCapacityM3;
            }
            set
            {
                _sourceCapacityM3 = value;
                OnPropertyChanged(nameof(SourceCapacityM3));
            }
        }

        public ICommand AddWaterSourceCommand { get; set; }

        public ICommand UpdateWaterSourceCommand { get; set; }

        public ICommand DeleteWaterSourceCommand { get; set; }

        public ICommand ClearWaterSourceFormCommand { get; set; }

        public MainViewModel()
        {
            _sourceRepository = new WaterSourceRepository();
            _readingRepository = new WaterQualityReadingRepository();

            SeedDataService seedDataService = new SeedDataService(
                _sourceRepository,
                _readingRepository);

            seedDataService.LoadInitialData();

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();

            AddWaterSourceCommand = new RelayCommand(AddWaterSource);
            UpdateWaterSourceCommand = new RelayCommand(UpdateWaterSource);
            DeleteWaterSourceCommand = new RelayCommand(DeleteWaterSource);
            ClearWaterSourceFormCommand = new RelayCommand(ClearWaterSourceForm);

            ClearWaterSourceForm(null);
        }

        private void AddWaterSource(object parameter)
        {
            if (!ValidateWaterSourceForm())
            {
                return;
            }

            WaterSource source = new WaterSource
            {
                Id = Guid.NewGuid(),
                Name = SourceName,
                Location = SourceLocation,
                SourceType = SourceType,
                Municipality = SourceMunicipality,
                CapacityM3 = double.Parse(SourceCapacityM3)
            };

            _sourceRepository.Add(source);

            ClearWaterSourceForm(null);
        }

        private void UpdateWaterSource(object parameter)
        {
            if (SelectedWaterSource == null)
            {
                MessageBox.Show(
                    "Please select a water source before updating.",
                    "Validation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!ValidateWaterSourceForm())
            {
                return;
            }

            WaterSource updatedSource = new WaterSource
            {
                Id = SelectedWaterSource.Id,
                Name = SourceName,
                Location = SourceLocation,
                SourceType = SourceType,
                Municipality = SourceMunicipality,
                CapacityM3 = double.Parse(SourceCapacityM3)
            };

            _sourceRepository.Update(updatedSource);

            CollectionViewSourceRefresh();

            ClearWaterSourceForm(null);
        }

        private void DeleteWaterSource(object parameter)
        {
            if (SelectedWaterSource == null)
            {
                MessageBox.Show(
                    "Please select a water source before deleting.",
                    "Validation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete selected water source?",
                "Delete confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            _sourceRepository.Delete(SelectedWaterSource.Id);

            ClearWaterSourceForm(null);
        }

        private void ClearWaterSourceForm(object parameter)
        {
            SelectedWaterSource = null;
            SourceName = string.Empty;
            SourceLocation = string.Empty;
            SourceType = string.Empty;
            SourceMunicipality = string.Empty;
            SourceCapacityM3 = string.Empty;
        }

        private void FillSourceForm(WaterSource source)
        {
            SourceName = source.Name;
            SourceLocation = source.Location;
            SourceType = source.SourceType;
            SourceMunicipality = source.Municipality;
            SourceCapacityM3 = source.CapacityM3.ToString();
        }

        private bool ValidateWaterSourceForm()
        {
            if (string.IsNullOrWhiteSpace(SourceName))
            {
                MessageBox.Show("Name is required.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SourceLocation))
            {
                MessageBox.Show("Location is required.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SourceType))
            {
                MessageBox.Show("Source type is required.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SourceMunicipality))
            {
                MessageBox.Show("Municipality is required.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            double capacity;

            if (!double.TryParse(SourceCapacityM3, out capacity))
            {
                MessageBox.Show("Capacity must be a valid number.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (capacity <= 0)
            {
                MessageBox.Show("Capacity must be greater than zero.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CollectionViewSourceRefresh()
        {
            System.Windows.Data.CollectionViewSource.GetDefaultView(WaterSources).Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}