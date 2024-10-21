namespace StswApps;
internal static class SQL
{
    private static readonly StswDatabaseModel DbApps = new("", "", "", "");

    /// GetAppModels
    internal static IEnumerable<AppModel> GetAppModels() => DbApps.Get<AppModel>(@"
        select 1")!;
}
