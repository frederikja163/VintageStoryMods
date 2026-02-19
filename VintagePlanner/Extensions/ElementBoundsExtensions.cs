using Vintagestory.API.Client;

namespace VintageStoryMods.Todo;

internal static class ElementBoundsExtensions
{
    public static ElementBounds WithPercentualWidth(this ElementBounds element, float width)
    {
        element.percentWidth = width;
        return element;
    }
    public static ElementBounds WithPercentualHeight(this ElementBounds element, float height)
    {
        element.percentHeight = height;
        return element;
    }
}