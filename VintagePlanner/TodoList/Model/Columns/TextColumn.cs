namespace VintageStoryMods.TodoList.Model.Columns;

internal sealed class TextColumn(string name) : ColumnBase(name, string.Empty)
{
    public override bool Validate(object value)
    {
        return value is string;
    }
}