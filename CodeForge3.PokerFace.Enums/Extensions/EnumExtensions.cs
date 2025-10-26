using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CodeForge3.PokerFace.Enums.Extensions;

/// <summary>
/// Provides extension methods for the Enumerations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the display name of an enum value defined with a <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name defined by <see cref="DisplayAttribute"/> if present; otherwise, the enum name.</returns>
    public static string GetDisplayName(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString())!;
        DisplayAttribute? attribute = field.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? value.ToString();
    }
}
