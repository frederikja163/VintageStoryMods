using System;
using System.Collections;
using System.Collections.Generic;

namespace VintageStoryMods;

internal abstract class ColumnBase(string name)
{
    public string Name { get; set; } = name;
}

internal abstract class IntegerColumn(string name) : ColumnBase(name)
{
    public long MinValue { get; set; } = long.MinValue;
    public long MaxValue { get; set; } = long.MaxValue;
}

internal abstract class NumberColumn(string name) : ColumnBase(name)
{
    public double MinValue { get; set; } = double.MinValue;
    public double MaxValue { get; set; } = double.MaxValue;
}

internal abstract class TextColumn(string name) : ColumnBase(name)
{
    public HashSet<string> ValidEntries { get; set; } = new();
}