using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WaterQuality.Contracts.Enums;
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

        private WaterQualityReading _selectedWaterQualityReading;

        private WaterSource _selectedReadingSource;
        private DateTime _readingSampledAt;
        private string _readingPHLevel;
        private string _readingTurbidityNTU;
        private string _readingChlorineLevel;
        private WaterState _selectedReadingState;

        private string _sourceSearchText;
        private string _readingSearchText;

        private ObservableCollection<WaterSource> _waterSources;
        private ObservableCollection<WaterQualityReading> _waterQualityReadings;

        public ObservableCollection<WaterSource> WaterSources
        {
            get
            {
                return _waterSources;
            }
            set
            {
                _waterSources = value;
                OnPropertyChanged(nameof(WaterSources));
            }
        }

        public ObservableCollection<WaterQualityReading> WaterQualityReadings
        {
            get
            {
                return _waterQualityReadings;
            }
            set
            {
                _waterQualityReadings = value;
                OnPropertyChanged(nameof(WaterQualityReadings));
            }
        }

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

        public WaterQualityReading SelectedWaterQualityReading
        {
            get
            {
                return _selectedWaterQualityReading;
            }
            set
            {
                _selectedWaterQualityReading = value;
                OnPropertyChanged(nameof(SelectedWaterQualityReading));

                if (_selectedWaterQualityReading != null)
                {
                    FillReadingForm(_selectedWaterQualityReading);
                }
            }
        }

        public WaterSource SelectedReadingSource
        {
            get
            {
                return _selectedReadingSource;
            }
            set
            {
                _selectedReadingSource = value;
                OnPropertyChanged(nameof(SelectedReadingSource));
            }
        }

        public DateTime ReadingSampledAt
        {
            get
            {
                return _readingSampledAt;
            }
            set
            {
                _readingSampledAt = value;
                OnPropertyChanged(nameof(ReadingSampledAt));
            }
        }

        public string ReadingPHLevel
        {
            get
            {
                return _readingPHLevel;
            }
            set
            {
                _readingPHLevel = value;
                OnPropertyChanged(nameof(ReadingPHLevel));
            }
        }

        public string ReadingTurbidityNTU
        {
            get
            {
                return _readingTurbidityNTU;
            }
            set
            {
                _readingTurbidityNTU = value;
                OnPropertyChanged(nameof(ReadingTurbidityNTU));
            }
        }

        public string ReadingChlorineLevel
        {
            get
            {
                return _readingChlorineLevel;
            }
            set
            {
                _readingChlorineLevel = value;
                OnPropertyChanged(nameof(ReadingChlorineLevel));
            }
        }

        public WaterState SelectedReadingState
        {
            get
            {
                return _selectedReadingState;
            }
            set
            {
                _selectedReadingState = value;
                OnPropertyChanged(nameof(SelectedReadingState));
            }
        }

        public List<WaterState> WaterStates
        {
            get
            {
                return Enum.GetValues(typeof(WaterState)).Cast<WaterState>().ToList();
            }
        }

        public string SourceSearchText
        {
            get
            {
                return _sourceSearchText;
            }
            set
            {
                _sourceSearchText = value;
                OnPropertyChanged(nameof(SourceSearchText));
            }
        }

        public string ReadingSearchText
        {
            get
            {
                return _readingSearchText;
            }
            set
            {
                _readingSearchText = value;
                OnPropertyChanged(nameof(ReadingSearchText));
            }
        }

        public ICommand AddWaterSourceCommand { get; set; }

        public ICommand UpdateWaterSourceCommand { get; set; }

        public ICommand DeleteWaterSourceCommand { get; set; }

        public ICommand ClearWaterSourceFormCommand { get; set; }

        public ICommand AddWaterQualityReadingCommand { get; set; }

        public ICommand UpdateWaterQualityReadingCommand { get; set; }

        public ICommand DeleteWaterQualityReadingCommand { get; set; }

        public ICommand ClearWaterQualityReadingFormCommand { get; set; }

        public ICommand SearchWaterSourcesCommand { get; set; }

        public ICommand ResetWaterSourcesSearchCommand { get; set; }

        public ICommand SearchWaterQualityReadingsCommand { get; set; }

        public ICommand ResetWaterQualityReadingsSearchCommand { get; set; }

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

            AddWaterQualityReadingCommand = new RelayCommand(AddWaterQualityReading);
            UpdateWaterQualityReadingCommand = new RelayCommand(UpdateWaterQualityReading);
            DeleteWaterQualityReadingCommand = new RelayCommand(DeleteWaterQualityReading);
            ClearWaterQualityReadingFormCommand = new RelayCommand(ClearWaterQualityReadingForm);

            SearchWaterSourcesCommand = new RelayCommand(SearchWaterSources);
            ResetWaterSourcesSearchCommand = new RelayCommand(ResetWaterSourcesSearch);

            SearchWaterQualityReadingsCommand = new RelayCommand(SearchWaterQualityReadings);
            ResetWaterQualityReadingsSearchCommand = new RelayCommand(ResetWaterQualityReadingsSearch);

            ClearWaterSourceForm(null);
            ClearWaterQualityReadingForm(null);
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
                CapacityM3 = ParseDoubleInput(SourceCapacityM3)
            };

            _sourceRepository.Add(source);
            WaterSources = _sourceRepository.GetAll();
            SourceSearchText = string.Empty;

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
                CapacityM3 = ParseDoubleInput(SourceCapacityM3)
            };

            _sourceRepository.Update(updatedSource);
            WaterSources = _sourceRepository.GetAll();
            SourceSearchText = string.Empty;

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
            WaterSources = _sourceRepository.GetAll();
            SourceSearchText = string.Empty;

            ClearWaterSourceForm(null);
        }

        private void AddWaterQualityReading(object parameter)
        {
            if (!ValidateWaterQualityReadingForm())
            {
                return;
            }

            WaterQualityReading reading = new WaterQualityReading
            {
                Id = Guid.NewGuid(),
                SourceId = SelectedReadingSource.Id,
                SampledAt = ReadingSampledAt,
                PHLevel = ParseDoubleInput(ReadingPHLevel),
                TurbidityNTU = ParseDoubleInput(ReadingTurbidityNTU),
                ChlorineLevel = ParseDoubleInput(ReadingChlorineLevel),
                State = SelectedReadingState
            };

            _readingRepository.Add(reading);
            WaterQualityReadings = _readingRepository.GetAll();
            ReadingSearchText = string.Empty;

            ClearWaterQualityReadingForm(null);
        }

        private void UpdateWaterQualityReading(object parameter)
        {
            if (SelectedWaterQualityReading == null)
            {
                MessageBox.Show(
                    "Please select a water quality reading before updating.",
                    "Validation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!ValidateWaterQualityReadingForm())
            {
                return;
            }

            WaterQualityReading updatedReading = new WaterQualityReading
            {
                Id = SelectedWaterQualityReading.Id,
                SourceId = SelectedReadingSource.Id,
                SampledAt = ReadingSampledAt,
                PHLevel = ParseDoubleInput(ReadingPHLevel),
                TurbidityNTU = ParseDoubleInput(ReadingTurbidityNTU),
                ChlorineLevel = ParseDoubleInput(ReadingChlorineLevel),
                State = SelectedReadingState
            };

            _readingRepository.Update(updatedReading);
            WaterQualityReadings = _readingRepository.GetAll();
            ReadingSearchText = string.Empty;

            System.Windows.Data.CollectionViewSource.GetDefaultView(WaterQualityReadings).Refresh();

            ClearWaterQualityReadingForm(null);
        }

        private void DeleteWaterQualityReading(object parameter)
        {
            if (SelectedWaterQualityReading == null)
            {
                MessageBox.Show(
                    "Please select a water quality reading before deleting.",
                    "Validation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete selected water quality reading?",
                "Delete confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            _readingRepository.Delete(SelectedWaterQualityReading.Id);
            WaterQualityReadings = _readingRepository.GetAll();
            ReadingSearchText = string.Empty;

            ClearWaterQualityReadingForm(null);
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

        private void SearchWaterSources(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SourceSearchText))
            {
                WaterSources = _sourceRepository.GetAll();
                return;
            }

            string searchText = SourceSearchText.Trim().ToLower();

            WaterSources = new ObservableCollection<WaterSource>(
                _sourceRepository.GetAll().Where(source =>
                    source.Id.ToString().ToLower().Contains(searchText) ||
                    source.Name.ToLower().Contains(searchText) ||
                    source.Location.ToLower().Contains(searchText) ||
                    source.SourceType.ToLower().Contains(searchText) ||
                    source.Municipality.ToLower().Contains(searchText) ||
                    source.CapacityM3.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText)
                )
            );
        }

        private void ResetWaterSourcesSearch(object parameter)
        {
            SourceSearchText = string.Empty;
            WaterSources = _sourceRepository.GetAll();
        }

        private void ClearWaterQualityReadingForm(object parameter)
        {
            SelectedWaterQualityReading = null;
            SelectedReadingSource = null;
            ReadingSampledAt = DateTime.Now;
            ReadingPHLevel = string.Empty;
            ReadingTurbidityNTU = string.Empty;
            ReadingChlorineLevel = string.Empty;
            SelectedReadingState = WaterState.Safe;
        }

        private void SearchWaterQualityReadings(object parameter)
        {
            if (string.IsNullOrWhiteSpace(ReadingSearchText))
            {
                WaterQualityReadings = _readingRepository.GetAll();
                return;
            }

            string searchText = ReadingSearchText.Trim().ToLower();

            WaterQualityReadings = new ObservableCollection<WaterQualityReading>(
                _readingRepository.GetAll().Where(reading =>
                    reading.Id.ToString().ToLower().Contains(searchText) ||
                    reading.SourceId.ToString().ToLower().Contains(searchText) ||
                    reading.SampledAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToLower().Contains(searchText) ||
                    reading.SampledAt.Year.ToString().Contains(searchText) ||
                    reading.PHLevel.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText) ||
                    reading.TurbidityNTU.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText) ||
                    reading.ChlorineLevel.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText) ||
                    reading.State.ToString().ToLower().Contains(searchText)
                )
            );
        }

        private void ResetWaterQualityReadingsSearch(object parameter)
        {
            ReadingSearchText = string.Empty;
            WaterQualityReadings = _readingRepository.GetAll();
        }

        private void FillReadingForm(WaterQualityReading reading)
        {
            SelectedReadingSource = WaterSources.FirstOrDefault(source => source.Id == reading.SourceId);
            ReadingSampledAt = reading.SampledAt;
            ReadingPHLevel = reading.PHLevel.ToString(CultureInfo.InvariantCulture);
            ReadingTurbidityNTU = reading.TurbidityNTU.ToString(CultureInfo.InvariantCulture);
            ReadingChlorineLevel = reading.ChlorineLevel.ToString(CultureInfo.InvariantCulture);
            SelectedReadingState = reading.State;
        }

        private void FillSourceForm(WaterSource source)
        {
            SourceName = source.Name;
            SourceLocation = source.Location;
            SourceType = source.SourceType;
            SourceMunicipality = source.Municipality;
            SourceCapacityM3 = source.CapacityM3.ToString(CultureInfo.InvariantCulture);
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

            if (!TryParseDoubleInput(SourceCapacityM3, out capacity))
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

        private bool ValidateWaterQualityReadingForm()
        {
            if (SelectedReadingSource == null)
            {
                MessageBox.Show("Water source is required.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            double phLevel;

            if (!TryParseDoubleInput(ReadingPHLevel, out phLevel))
            {
                MessageBox.Show("pH level must be a valid number.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (phLevel < 0 || phLevel > 14)
            {
                MessageBox.Show("pH level must be between 0 and 14.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            double turbidity;

            if (!TryParseDoubleInput(ReadingTurbidityNTU, out turbidity))
            {
                MessageBox.Show("Turbidity must be a valid number.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (turbidity < 0)
            {
                MessageBox.Show("Turbidity cannot be negative.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            double chlorineLevel;

            if (!TryParseDoubleInput(ReadingChlorineLevel, out chlorineLevel))
            {
                MessageBox.Show("Chlorine level must be a valid number.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (chlorineLevel < 0)
            {
                MessageBox.Show("Chlorine level cannot be negative.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CollectionViewSourceRefresh()
        {
            System.Windows.Data.CollectionViewSource.GetDefaultView(WaterSources).Refresh();
        }

        private bool TryParseDoubleInput(string input, out double value)
        {
            value = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string normalizedInput = input.Trim().Replace(',', '.');

            return double.TryParse(
                normalizedInput,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out value);
        }

        private double ParseDoubleInput(string input)
        {
            string normalizedInput = input.Trim().Replace(',', '.');

            return double.Parse(
                normalizedInput,
                NumberStyles.Float,
                CultureInfo.InvariantCulture);
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