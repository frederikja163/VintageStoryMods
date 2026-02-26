using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace VintageStoryMods;

public sealed class Config
{
    public string WhitelistOpen { get; set; } = "([Dd]oor)|([Gg]ate)";

    public string BlacklistOpen { get; set; } = "([Rr]ust)";
    
    public string WhitelistClose { get; set; } = "([Dd]oor)|([Gg]ate)";

    public string BlacklistClose { get; set; } = "([Rr]ust)";

    public int Radius { get; set; } = 2;
    public bool AutoOpen { get; set; } = true;
    public bool AutoClose { get; set; } = true;
    public bool LeaveOpen { get; set; } = true;
}