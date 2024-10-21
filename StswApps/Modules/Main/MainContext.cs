using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Data;

namespace StswApps;
public class MainContext : StswObservableObject
{
    public StswAsyncCommand<AppModel?> OpenAppCommand { get; }
    public StswAsyncCommand<AppModel?> RemoveAppCommand { get; }
    public StswAsyncCommand RefreshCommand { get; }
    public StswAsyncCommand AddCustomAppCommand { get; }
    public StswAsyncCommand HistoryCommand { get; }
    public StswAsyncCommand ConfigCommand { get; }

    public MainContext()
    {
        OpenAppCommand = new(OpenApp);
        RemoveAppCommand = new(RemoveApp);
        RefreshCommand = new(Refresh);
        AddCustomAppCommand = new(AddCustomApp);
        HistoryCommand = new(History, () => false);
        ConfigCommand = new(Config, () => false);

        Task.Run(Refresh);
    }

    /// Command: OpenApp
    private async Task OpenApp(AppModel? app)
    {
        await Task.Run(() =>
        {
            if (app != null && app.Files?.FirstOrDefault() is AppFileModel fileModel)
                StswFn.OpenFile(fileModel.Path!);
        });
    }

    /// Command: RemoveApp
    private async Task RemoveApp(AppModel? app)
    {
        if (app != null && app.IsCustom)
        {
            await Task.Run(() =>
            {
                Apps.Remove(app);
                App.Current.Dispatcher.Invoke(() => AppsCollectionView?.Refresh());
                File.WriteAllText(CustomAppsPath, JsonSerializer.Serialize(Apps.Where(x => x.IsCustom)));
            });
        }
    }

    /// Command: Refresh
    private async Task Refresh()
    {
        await Task.Run(() =>
        {
            Apps.Clear();

            if (!File.Exists(CustomAppsPath))
                File.WriteAllText(CustomAppsPath, JsonSerializer.Serialize(Apps.Where(x => x.IsCustom)));

            foreach (var item in JsonSerializer.Deserialize<IEnumerable<AppModel>>(File.ReadAllText(CustomAppsPath))!)
                Apps.Add(item);

            App.Current.Dispatcher.Invoke(() =>
            {
                AppsCollectionView = CollectionViewSource.GetDefaultView(Apps);
                AppsCollectionView.Filter = FilterApps;

                if (AppsCollectionView is ICollectionViewLiveShaping liveShaping)
                {
                    liveShaping.IsLiveFiltering = true;
                    liveShaping.IsLiveSorting = true;
                    liveShaping.IsLiveGrouping = true;
                }
            });
        });
    }

    /// Command: AddCustomApp
    private async Task AddCustomApp()
    {
        var newContext = new AddCustomAppContext((AppType)SelectedTab);
        if (((bool?)await StswContentDialog.Show(newContext, "MainDialog")) == true)
        {
            Apps.Add(newContext.NewApp!);
            App.Current.Dispatcher.Invoke(() => AppsCollectionView?.Refresh());
            File.WriteAllText(CustomAppsPath, JsonSerializer.Serialize(Apps.Where(x => x.IsCustom)));
        }
    }

    /// Command: History
    private async Task History()
    {
        await StswContentDialog.Show(new HistoryContext(), "MainDialog");
    }

    /// Command: Config
    private async Task Config()
    {
        await StswContentDialog.Show(new ConfigContext(), "MainDialog");
    }

    /// FilterApps
    private bool FilterApps(object item)
    {
        if (item is AppModel app)
            return (int)app.Type == SelectedTab;
        
        return false;
    }

    /// Apps
    public IList<AppModel> Apps
    {
        get => _apps;
        set => SetProperty(ref _apps, value);
    }
    private IList<AppModel> _apps = [];
    
    /// AppsCollectionView
    public ICollectionView? AppsCollectionView
    {
        get => _appsCollectionView;
        set => SetProperty(ref _appsCollectionView, value);
    }
    private ICollectionView? _appsCollectionView;

    /// CustomAppsPath
    private readonly string CustomAppsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "customApps.json");

    /// SelectedTab
    public int SelectedTab
    {
        get => _selectedTab;
        set => SetProperty(ref _selectedTab, value, () => AppsCollectionView?.Refresh());
    }
    private int _selectedTab;

    /// User
    public UserModel User { get; set; } = new()
    {
        Name = Environment.UserName
    };
}
