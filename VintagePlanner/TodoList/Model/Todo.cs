using System;
using System.Collections.Generic;
using VintageStoryMods.TodoList.Model.Columns;
using VintageStoryMods.TodoList.Model.EventArgs;

namespace VintageStoryMods.TodoList.Model;

public sealed class Todo
{
    private readonly Dictionary<Guid, Tab> _tabs = new();
    private readonly Dictionary<Guid, ColumnBase> _columns = new();

    public int RowCount { get; private set; } = 0;
    
    public event EventHandler<RowAddedEventArgs>? OnRowAdded;
    public event EventHandler<RowRemovedEventArgs>? OnRowRemoved;
    public event EventHandler<ColumnAddedEventArgs>? OnColumnAdded;
    public event EventHandler<ColumnRemovedEventArgs>? OnColumnRemoved;
    public event EventHandler<TabAddedEventArgs>? OnTabAdded;
    public event EventHandler<TabRemovedEventArgs>? OnTabRemoved;
    
    public void AddRow()
    {
        RowCount += 1;
        foreach (ColumnBase column in _columns.Values)
        {
            column.AddRow();
        }
        OnRowAdded?.Invoke(this, new RowAddedEventArgs(RowCount - 1));
    }

    public void RemoveRow(int index)
    {
        RowCount -= 1;
        foreach (ColumnBase column in _columns.Values)
        {
            column.RemoveRow(index);
        }
        OnRowRemoved?.Invoke(this, new RowRemovedEventArgs(index));
    }
    
    public void AddColumn(ColumnBase column)
    {
        if (!_columns.TryAdd(column.Id, column))
            return;
        for (int i = 0; i < RowCount; i++)
        {
            column.AddRow();
        }
        OnColumnAdded?.Invoke(this, new ColumnAddedEventArgs(column));
    }

    public void RemoveColumn(ColumnBase column)
    {
        if (!_columns.Remove(column.Id))
            return;
        
        foreach (Tab tab in _tabs.Values)
        {
            tab.RemoveColumn(column);
        }
        OnColumnRemoved?.Invoke(this, new ColumnRemovedEventArgs(column));
    }

    public IEnumerable<ColumnBase> GetColumns()
    {
        return _columns.Values;
    }

    public void AddTab(Tab tab)
    {
        if (!_tabs.TryAdd(tab.Id, tab))
            return;
        OnTabAdded?.Invoke(this, new TabAddedEventArgs(tab));
    }

    public void RemoveColumn(Tab tab)
    {
        if (!_tabs.Remove(tab.Id))
            return;
        
        OnTabRemoved?.Invoke(this, new TabRemovedEventArgs(tab));
    }
}