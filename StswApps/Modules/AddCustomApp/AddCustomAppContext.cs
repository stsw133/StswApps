using System.IO;

namespace StswApps;
public class AddCustomAppContext : StswObservableObject
{
    private AppType _appType;

    public StswCommand AcceptCommand { get; }
    public StswCommand CancelCommand { get; }

    public AddCustomAppContext(AppType appType)
    {
        AcceptCommand = new(Accept);
        CancelCommand = new(Cancel);

        _appType = appType;
    }

    /// Accept
    private void Accept()
    {
        if (NewApp == null)
        {
            StswMessageDialog.Show("Executable path has not been selected.", StswDialogImage.Blockade.ToString(), StswDialogButtons.OK, StswDialogImage.Blockade);
            return;
        }

        StswContentDialog.Close("MainDialog", true);
    }

    /// Cancel
    private void Cancel()
    {
        StswContentDialog.Close("MainDialog", false);
    }

    /// OnSelectedPathChanged
    private void OnSelectedPathChanged()
    {
        NewApp = new()
        {
            Files = [new() { Path = SelectedPath }],
            Icon = StswFn.ExtractAssociatedIcon(SelectedPath)?.ToBytes(),
            IsCustom = true,
            Name = Path.GetFileNameWithoutExtension(SelectedPath),
            Type = _appType
        };
    }

    /// NewApp
    public AppModel? NewApp
    {
        get => _newApp;
        set => SetProperty(ref _newApp, value);
    }
    private AppModel? _newApp;

    /// SelectedPath
    public string? SelectedPath
    {
        get => _selectedPath;
        set
        {
            SetProperty(ref _selectedPath, value);
            OnSelectedPathChanged();
        }
    }
    private string? _selectedPath;
}
