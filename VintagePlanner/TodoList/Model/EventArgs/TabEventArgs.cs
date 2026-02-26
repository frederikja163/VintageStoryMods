namespace VintageStoryMods.TodoList.Model.EventArgs;


public abstract class TabEventArgsBase : AnyChangeEventArgsBase
{
    public Tab Tab { get; }
    protected TabEventArgsBase(Tab tab)
    {
        Tab = tab;
    }
}

public sealed class ColumnConfigChangedEventArgs : TabEventArgsBase
{
    public ColumnTabConfig OldConfig;
    public ColumnTabConfig Config;
    
    internal ColumnConfigChangedEventArgs(Tab tab, ColumnTabConfig oldConfig, ColumnTabConfig config): base(tab)
    {
        OldConfig = oldConfig;
        Config = config;
    }
}

public sealed class TabAddedEventArgs : TabEventArgsBase
{
    internal TabAddedEventArgs(Tab tab) : base(tab)
    {
    }
}

public sealed class TabRemovedEventArgs : TabEventArgsBase
{
    internal TabRemovedEventArgs(Tab tab) : base(tab)
    {
    }
}