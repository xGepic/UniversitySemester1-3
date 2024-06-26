﻿namespace Tour_Planner_UI;
internal class MainWindowViewModel : INotifyPropertyChanged, ISubject
{
    public MainWindowViewModel()
    {
        ListViewModel = new();
        DetailsViewModel = new();
        LogsViewModel = new();

        Observers = new List<IObserver>();
        this.Attach(ListViewModel);
        this.Attach(DetailsViewModel);
        this.Attach(LogsViewModel);

        ListViewModel.Attach(DetailsViewModel);
        DetailsViewModel.Attach(ListViewModel);
        DetailsViewModel.Attach(LogsViewModel);

        SearchButtonCommand = new Command(ExecuteSearchButton, CanExecuteSearchButton);
        DarkLightButtonCommand = new Command(ExecuteDarkLightButton, CanExecuteDarkLightButton);

        Background = new SolidColorBrush(Colors.White);
        Foreground = new SolidColorBrush(Colors.Black);
    }
    private readonly List<IObserver> Observers;
    private bool Notifing;
    public TourListViewModel? ListViewModel { get; set; }
    public TourDetailsViewModel? DetailsViewModel { get; set; }
    public TourLogsViewModel? LogsViewModel { get; set; }
    public ICommand SearchButtonCommand { get; set; }
    public ICommand DarkLightButtonCommand { get; set; }
    private System.Windows.Media.Brush? BackgroundColor;
    public System.Windows.Media.Brush? Background
    {
        get{ return BackgroundColor;}
        set{ BackgroundColor = value; OnPropertyChanged(); Notify(); }
    }
    private System.Windows.Media.Brush? ForegroundColor;
    public System.Windows.Media.Brush? Foreground
    {
        get { return ForegroundColor; }
        set { ForegroundColor = value; OnPropertyChanged(); Notify(); }
    }
    private string? SearchInput;
    public string? Search
    {
        get { return SearchInput; }
        set { SearchInput = value?.ToLower(); OnPropertyChanged(); }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    private bool CanExecuteSearchButton(object? parameter)
    {
        return true;
    }
    private void ExecuteSearchButton(object? parameter)
    {
        if (ListViewModel is not null && ListViewModel.Tours is not null)
        {
            if (parameter is not null && Search is not null)
            {
                if ((bool)parameter)
                {
                    ListViewModel.IsFiltered = true;
                    ListViewModel.Tours = ListViewModel.Tours.Where(e =>
                        e.TourName!.ToLower().Contains(Search) ||
                        e.TourDescription!.ToLower().Contains(Search) ||
                        e.StartingPoint!.ToLower().Contains(Search) ||
                        e.Destination!.ToLower().Contains(Search) ||
                        e.TransportType.ToString().ToLower().Contains(Search) ||
                        e.TourDistance.ToString().ToLower().Contains(Search) ||
                        e.EstimatedTimeInMin.ToString().ToLower().Contains(Search) ||
                        e.TourType.ToString().ToLower().Contains(Search) ||
                        e.Popularity.ToString().ToLower().Contains(Search) ||
                        e.ChildFriendliness.ToString().ToLower().Contains(Search) ||
                        e.TourLogs!.Any(f =>
                            f.TourDateAndTime.ToString().ToLower().Contains(Search) ||
                            f.TourComment!.ToLower().Contains(Search) ||
                            f.TourDifficulty.ToString().ToLower().Contains(Search) ||
                            f.TourTimeInMin.ToString().ToLower().Contains(Search) ||
                            f.TourRating.ToString().ToLower().Contains(Search)
                    )).ToArray();
                }
                else
                {
                    ListViewModel.IsFiltered = false;
                    Tour? temp = ListViewModel.Selected;
                    ListViewModel.Tours = TourRepository.GetAllTours();
                    ListViewModel.Selected = temp;
                }
            }
        }
    }
    private bool CanExecuteDarkLightButton(object? parameter)
    {
        return true;
    }
    private void ExecuteDarkLightButton(object? parameter)
    {
        if (parameter is not null)
        {
            if ((bool)parameter)
            {
                Background = new SolidColorBrush(Colors.DarkSlateGray);
                Foreground = new SolidColorBrush(Colors.OrangeRed);
            }
            else
            {
                Background = new SolidColorBrush(Colors.White);
                Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
    public void Attach(IObserver observer)
    {
        Observers.Add(observer);
    }
    public void Notify()
    {
        if (Notifing)
        {
            return;
        }
        Notifing = true;
        try
        {
            Observers.ForEach(func =>
            {
                func.Update(this);
            });
        }
        finally
        {
            Notifing = false;
        }
    }
}

