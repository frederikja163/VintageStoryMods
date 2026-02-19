using System.Collections.Generic;

namespace VintageStoryMods.Todo.Columns;


internal abstract class ColumnBase(string name, object defaultValue)
{
    public string Name { get; set; } = name;
    public int RowCount { get; private set; } = 0;
    private readonly List<object> _values = new ();

    public void Clear()
    {
        RowCount = 0;
        _values.Clear();
    }
    
    public void AddRow()
    {
        RowCount += 1;
        _values.Add(defaultValue);
    }

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

    public void SetValue(int row, object value)
    {
        _values[row] = value;
    }
}
