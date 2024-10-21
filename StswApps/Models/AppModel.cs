using System.Text.Json.Serialization;
using System.Windows.Media;

namespace StswApps;
public class AppModel
{
    public int ID { get; set; }
    public AppType Type { get; set; }
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public byte[]? Icon { get; set; }
    [JsonIgnore]
    public ImageSource? IconSource => StswFn.BytesToBitmapImage(Icon);
    public bool IsCustom { get; set; }
    public IEnumerable<AppFileModel>? Files { get; set; }
}
