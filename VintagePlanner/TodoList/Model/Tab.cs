using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VintageStoryMods.TodoList.Model.Columns;
using VintageStoryMods.TodoList.Model.EventArgs;

namespace VintageStoryMods.TodoList.Model;

public record ColumnTabConfig(float Width)
{
    // TODO: Filters should be stored here.
    public required ColumnBase Column { get; init;  }
}
public sealed class Tab
{
    public static float DefaultWidth { get; set; } = 50f;
    private readonly Dictionary<Guid, ColumnTabConfig> _columns = new();

    public event EventHandler<ColumnEventArgsBase>? OnColumnChanged;
    public event EventHandler<ColumnConfigChangedEventArgs>? OnColumnConfigChanged;
    
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }

    internal Tab(string name)
    {
        Name = name;
    }
    
    public float GetColumnWidth(ColumnBase column)
    {
        return _columns[column.Id].Width;
    }

    public void SetColumnWidth(ColumnBase column, float width)
    {
        ColumnTabConfig oldValue = _columns[column.Id];
        ColumnTabConfig value = oldValue with { Width = width};
        _columns[column.Id] = value;
        OnColumnConfigChanged?.Invoke(this, new ColumnConfigChangedEventArgs(this, oldValue, value));
    }

    public void AddColumn(ColumnBase column)
    {
        ColumnTabConfig columnConf = new ColumnTabConfig(DefaultWidth)
        {
            Column = column,
        };
        
        if (!_columns.TryAdd(column.Id, columnConf)) return;
        
        OnColumnChanged?.Invoke(this, new ColumnAddedEventArgs(column));
    }

    public void AddColumns(IEnumerable<ColumnBase> columns)
    {
        foreach (ColumnBase column in columns)
        {
            AddColumn(column);
        }
    }

    public void RemoveColumn(ColumnBase column)
    {
        if (!_columns.Remove(column.Id))
            return;
        
        OnColumnChanged?.Invoke(this, new ColumnRemovedEventArgs(column));
    }

    public void RemoveColumns(IEnumerable<ColumnBase> columns)
    {
        foreach (ColumnBase column in columns)
        {
            RemoveColumn(column);
        }
    }

    public IEnumerable<ColumnBase> GetColumns()
    {
        return _columns.Values.Select(cc => cc.Column);
    }

    public bool TryGetColumn(Guid id, [NotNullWhen(true)] out ColumnBase? column)
    {
        if (_columns.TryGetValue(id, out ColumnTabConfig? columnConf))
        {
            column = columnConf.Column;
            return true;
        }

        column = null;
        return false;
    }
}