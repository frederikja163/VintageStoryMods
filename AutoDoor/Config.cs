using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace VintageStoryMods;

public sealed class Config
{
    private Regex? _whitelistRegex;
    private Regex? _blacklistRegex;

    [JsonIgnore] public Regex WhitelistRegex => _whitelistRegex ??= new Regex(Whitelist, RegexOptions.Compiled);

    [JsonIgnore] public Regex BlacklistRegex => _blacklistRegex ??= new Regex(Blacklist, RegexOptions.Compiled);

    public string Whitelist { get; } = "([Dd]oor)|([Gg]ate)";

    public string Blacklist { get; } = "([Rr]ust)";

    public int Radius { get; } = 2;
    public bool AutoOpen { get; } = true;
    public bool AutoClose { get; } = true;
}