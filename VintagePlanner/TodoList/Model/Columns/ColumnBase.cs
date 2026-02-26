using System;
using System.Collections.Generic;
using VintageStoryMods.TodoList.Model.EventArgs;

namespace VintageStoryMods.TodoList.Model.Columns;

public abstract class ColumnBase(string name, object defaultValue)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    private readonly List<object> _values = new ();

    public event EventHandler<RowChangedEventArgs>? OnRowChanged;
    
    internal void ClearRows()
    {
        _values.Clear();
    }
    
    internal void AddRow()
    {
        _values.Add(defaultValue);
    }

    internal void RemoveRow(int index)
    {
        _values.RemoveAt(index);
    }

    public void SetValue(int row, object value)
    {
        object oldValue = _values[row];
        _values[row] = value;
        OnRowChanged?.Invoke(this, new RowChangedEventArgs(this, _values.Count - 1, oldValue, value));
    }

    public abstract bool Validate(object value);

    public IEnumerable<object> GetValues()
    {
        return _values;
    }

    public IEnumerable<object> GetValues(IEnumerable<int> indices)
    {
        foreach (int index in indices)
        {
            yield return _values[index];
        }
    }

    public object GetValue(int index)
    {
        return _values[index];
    }
}
