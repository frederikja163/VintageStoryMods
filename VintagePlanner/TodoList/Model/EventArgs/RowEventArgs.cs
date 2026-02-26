using VintageStoryMods.TodoList.Model.Columns;

namespace VintageStoryMods.TodoList.Model.EventArgs;

public abstract class RowEventArgsBase : AnyChangeEventArgsBase
{
    public int Index { get; }
    protected RowEventArgsBase(int index)
    {
        Index = index;
    }
}

public sealed class RowAddedEventArgs : RowEventArgsBase
{
    internal RowAddedEventArgs(int index) : base(index)
    {
    }
}

public sealed class RowChangedEventArgs : RowEventArgsBase
{
    public ColumnBase Column { get; }
    public object Value { get; }
    public object OldValue { get; }
    
    internal RowChangedEventArgs(ColumnBase column, int index, object value, object oldValue) : base(index)
    {
        Column = column;
        Value = value;
        OldValue = oldValue;
    }
}

public sealed class RowRemovedEventArgs : RowEventArgsBase
{
    internal RowRemovedEventArgs(int index) : base(index)
    {
    }
}