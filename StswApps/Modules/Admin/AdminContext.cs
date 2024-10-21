using System.Collections.ObjectModel;

namespace StswApps;
public class AdminContext : StswObservableObject
{
    /// Apps
    public ObservableCollection<AppModel> Apps
    {
        get => _apps;
        set => SetProperty(ref _apps, value);
    }
    private ObservableCollection<AppModel> _apps = [];
}
