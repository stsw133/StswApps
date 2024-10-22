using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StswApps;
public class AdminContext : StswObservableObject
{
    /// Accesses
    public ObservableCollection<AccessModel> Accesses
    {
        get => _accesses;
        set => SetProperty(ref _accesses, value);
    }
    private ObservableCollection<AccessModel> _accesses = [];

    /// AccessesCollectionView
    public ICollectionView? AccessesCollectionView
    {
        get => _accessesCollectionView;
        set => SetProperty(ref _accessesCollectionView, value);
    }
    private ICollectionView? _accessesCollectionView;

    /// Apps
    public ObservableCollection<AppModel> Apps
    {
        get => _apps;
        set => SetProperty(ref _apps, value);
    }
    private ObservableCollection<AppModel> _apps = [];

    /// AppsCollectionView
    public ICollectionView? AppsCollectionView
    {
        get => _appsCollectionView;
        set => SetProperty(ref _appsCollectionView, value);
    }
    private ICollectionView? _appsCollectionView;

    /// SelectedTab
    public int SelectedTab
    {
        get => _selectedTab;
        set => SetProperty(ref _selectedTab, value, () => AppsCollectionView?.Refresh());
    }
    private int _selectedTab;

    /// Users
    public ObservableCollection<UserModel> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }
    private ObservableCollection<UserModel> _users = [];

    /// UsersCollectionView
    public ICollectionView? UsersCollectionView
    {
        get => _usersCollectionView;
        set => SetProperty(ref _usersCollectionView, value);
    }
    private ICollectionView? _usersCollectionView;
}
