using LiveChartsCore.Geo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WaterQuality.Contracts.Enums;
using WaterQuality.InformationSystem.Commands;
using WaterQuality.InformationSystem.Data;
using WaterQuality.InformationSystem.Helpers;
using WaterQuality.InformationSystem.Models;
using WaterQuality.InformationSystem.Repositories;
using WaterQuality.InformationSystem.Services;

namespace WaterQuality.InformationSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IWaterSourceRepository _sourceRepository;
        private readonly IWaterQualityReadingRepository _readingRepository;
        private readonly ILoggingService _loggingService;
        private readonly IDataPersistenceService _dataPersistenceService;

        private readonly UndoRedoService _undoRedoService;

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

        public ICommand SaveDataCommand { get; set; }

        public ICommand LoadDataCommand { get; set; }

        public ICommand UndoCommand { get; set; }

        public ICommand RedoCommand { get; set; }

        public MainViewModel()
        {
            _sourceRepository = new WaterSourceRepository();
            _readingRepository = new WaterQualityReadingRepository();
            _loggingService = new FileLoggingService();
            _dataPersistenceService = new JsonDataPersistenceService();
            _undoRedoService = new UndoRedoService();

            LoadInitialApplicationData();

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();

            _loggingService.Log("Application started.");

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

            SaveDataCommand = new RelayCommand(SaveData);
            LoadDataCommand = new RelayCommand(LoadData);

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);

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

            _undoRedoService.ExecuteCommand(new AddWaterSourceCommand(_sourceRepository, source));
            _loggingService.Log(
                string.Format(
                    "Added water source: Id={0}, Name={1}, Location={2}, SourceType={3}, Municipality={4}, CapacityM3={5}.",
                    source.Id,
                    source.Name,
                    source.Location,
                    source.SourceType,
                    source.Municipality,
                    source.CapacityM3.ToString(CultureInfo.InvariantCulture)));
            SaveCurrentData();
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

            WaterSource oldSource = CloneHelper.CloneWaterSource(SelectedWaterSource);

            WaterSource updatedSource = new WaterSource
            {
                Id = SelectedWaterSource.Id,
                Name = SourceName,
                Location = SourceLocation,
                SourceType = SourceType,
                Municipality = SourceMunicipality,
                CapacityM3 = ParseDoubleInput(SourceCapacityM3)
            };

            _undoRedoService.ExecuteCommand(new UpdateWaterSourceCommand(_sourceRepository, oldSource, updatedSource));
            _loggingService.Log(
                string.Format(
                    "Updated water source: Id={0}, Name={1}, Location={2}, SourceType={3}, Municipality={4}, CapacityM3={5}.",
                    updatedSource.Id,
                    updatedSource.Name,
                    updatedSource.Location,
                    updatedSource.SourceType,
                    updatedSource.Municipality,
                    updatedSource.CapacityM3.ToString(CultureInfo.InvariantCulture)));
            SaveCurrentData();
            WaterSources = _sourceRepository.GetAll();
            SourceSearchText = string.Empty;

            RefreshTables();

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

            WaterSource deletedSource = CloneHelper.CloneWaterSource(SelectedWaterSource);
            _undoRedoService.ExecuteCommand(new DeleteWaterSourceCommand(_sourceRepository, deletedSource));
            _loggingService.Log(
                string.Format(
                    "Deleted water source: Id={0}, Name={1}, Location={2}, SourceType={3}, Municipality={4}, CapacityM3={5}.",
                    deletedSource.Id,
                    deletedSource.Name,
                    deletedSource.Location,
                    deletedSource.SourceType,
                    deletedSource.Municipality,
                    deletedSource.CapacityM3.ToString(CultureInfo.InvariantCulture)));
            SaveCurrentData();
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

            _undoRedoService.ExecuteCommand(new AddWaterQualityReadingCommand(_readingRepository, reading));
            _loggingService.Log(
                string.Format(
                    "Added water quality reading: Id={0}, SourceId={1}, SampledAt={2:yyyy-MM-dd}, PHLevel={3}, TurbidityNTU={4}, ChlorineLevel={5}, State={6}.",
                    reading.Id,
                    reading.SourceId,
                    reading.SampledAt,
                    reading.PHLevel.ToString(CultureInfo.InvariantCulture),
                    reading.TurbidityNTU.ToString(CultureInfo.InvariantCulture),
                    reading.ChlorineLevel.ToString(CultureInfo.InvariantCulture),
                    reading.State));
            SaveCurrentData();
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

            WaterQualityReading oldReading = CloneHelper.CloneWaterQualityReading(SelectedWaterQualityReading);

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

            _undoRedoService.ExecuteCommand(new UpdateWaterQualityReadingCommand(_readingRepository, oldReading, updatedReading));
            _loggingService.Log(
                string.Format(
                    "Updated water quality reading: Id={0}, SourceId={1}, SampledAt={2:yyyy-MM-dd}, PHLevel={3}, TurbidityNTU={4}, ChlorineLevel={5}, State={6}.",
                    updatedReading.Id,
                    updatedReading.SourceId,
                    updatedReading.SampledAt,
                    updatedReading.PHLevel.ToString(CultureInfo.InvariantCulture),
                    updatedReading.TurbidityNTU.ToString(CultureInfo.InvariantCulture),
                    updatedReading.ChlorineLevel.ToString(CultureInfo.InvariantCulture),
                    updatedReading.State));
            SaveCurrentData();
            WaterQualityReadings = _readingRepository.GetAll();
            ReadingSearchText = string.Empty;

            RefreshTables();

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

            WaterQualityReading deletedReading = CloneHelper.CloneWaterQualityReading(SelectedWaterQualityReading);
            _undoRedoService.ExecuteCommand(new DeleteWaterQualityReadingCommand(_readingRepository, deletedReading));
            _loggingService.Log(
                string.Format(
                    "Deleted water quality reading: Id={0}, SourceId={1}, SampledAt={2:yyyy-MM-dd}, PHLevel={3}, TurbidityNTU={4}, ChlorineLevel={5}, State={6}.",
                    deletedReading.Id,
                    deletedReading.SourceId,
                    deletedReading.SampledAt,
                    deletedReading.PHLevel.ToString(CultureInfo.InvariantCulture),
                    deletedReading.TurbidityNTU.ToString(CultureInfo.InvariantCulture),
                    deletedReading.ChlorineLevel.ToString(CultureInfo.InvariantCulture),
                    deletedReading.State));
            SaveCurrentData();
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

        private void LoadInitialApplicationData()
        {
            if (_dataPersistenceService.DataFileExists())
            {
                WaterQualityDataStore loadedData = _dataPersistenceService.LoadData();

                _sourceRepository.ReplaceAll(loadedData.WaterSources);
                _readingRepository.ReplaceAll(loadedData.WaterQualityReadings);

                _loggingService.Log("Data loaded from JSON file on application startup.");

                return;
            }

            SeedDataService seedDataService = new SeedDataService(
                _sourceRepository,
                _readingRepository);

            seedDataService.LoadInitialData();

            SaveCurrentData();

            _loggingService.Log("Seed data loaded and saved to JSON file.");
        }

        private void SaveCurrentData()
        {
            WaterQualityDataStore dataStore = new WaterQualityDataStore
            {
                WaterSources = _sourceRepository.GetAll().ToList(),
                WaterQualityReadings = _readingRepository.GetAll().ToList()
            };

            _dataPersistenceService.SaveData(dataStore);
        }

        private void SaveData(object parameter)
        {
            SaveCurrentData();

            _loggingService.Log("Data manually saved to JSON file.");

            MessageBox.Show(
                "Data saved successfully.",
                "Save data",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void LoadData(object parameter)
        {
            if (!_dataPersistenceService.DataFileExists())
            {
                MessageBox.Show(
                    "Data file does not exist.",
                    "Load data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            WaterQualityDataStore loadedData = _dataPersistenceService.LoadData();

            _sourceRepository.ReplaceAll(loadedData.WaterSources);
            _readingRepository.ReplaceAll(loadedData.WaterQualityReadings);

            _undoRedoService.Clear();

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();

            SourceSearchText = string.Empty;
            ReadingSearchText = string.Empty;

            ClearWaterSourceForm(null);
            ClearWaterQualityReadingForm(null);

            _loggingService.Log("Data manually loaded from JSON file.");

            MessageBox.Show(
                "Data loaded successfully.",
                "Load data",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Undo(object parameter)
        {
            IUndoableCommand command = _undoRedoService.Undo();

            if (command == null)
            {
                MessageBox.Show(
                    "There are no actions to undo.",
                    "Undo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();

            SourceSearchText = string.Empty;
            ReadingSearchText = string.Empty;

            RefreshTables();

            ClearWaterSourceForm(null);
            ClearWaterQualityReadingForm(null);

            SaveCurrentData();

            _loggingService.Log("Undo executed: " + command.Description + ".");
        }

        private void Redo(object parameter)
        {
            IUndoableCommand command = _undoRedoService.Redo();

            if (command == null)
            {
                MessageBox.Show(
                    "There are no actions to redo.",
                    "Redo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            WaterSources = _sourceRepository.GetAll();
            WaterQualityReadings = _readingRepository.GetAll();

            SourceSearchText = string.Empty;
            ReadingSearchText = string.Empty;

            RefreshTables();

            ClearWaterSourceForm(null);
            ClearWaterQualityReadingForm(null);

            SaveCurrentData();

            _loggingService.Log("Redo executed: " + command.Description + ".");
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

        private void RefreshTables()
        {
            if (WaterSources != null)
            {
                CollectionViewSource.GetDefaultView(WaterSources).Refresh();
            }

            if (WaterQualityReadings != null)
            {
                CollectionViewSource.GetDefaultView(WaterQualityReadings).Refresh();
            }
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