using VintageStoryMods.TodoList.Model.Columns;

namespace VintageStoryMods.TodoList.Model.EventArgs;

public abstract class ColumnEventArgsBase : AnyChangeEventArgsBase
{
    public ColumnBase Column { get; }
    protected ColumnEventArgsBase(ColumnBase column)
    {
        Column = column;
    }
}

public sealed class ColumnAddedEventArgs : ColumnEventArgsBase
{
    internal ColumnAddedEventArgs(ColumnBase column) : base(column)
    {
    }
}

public sealed class ColumnRemovedEventArgs : ColumnEventArgsBase
{
    internal ColumnRemovedEventArgs(ColumnBase column) : base(column)
    {
    }
}