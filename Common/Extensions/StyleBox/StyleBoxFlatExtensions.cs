using System;
using System.Xml.Schema;
using Godot;

public static class StyleBoxFlatExtensions
{
    /// <summary>
    /// Shorthand way of overriding a single instance of a Control's StyleBoxFlat
    /// 
    /// This method retrieves the existing StyleBoxFlat for the specified subtype(s),
    /// applies the provided modification function (e.g., using <see cref="StyleBoxFlatExtensions.With"/>),
    /// and sets the modified StyleBoxFlat back on the control.
    /// 
    /// Example usage with a Button:
    /// <code>
    /// var myButton = GetNode<Button>("MyButton");
    ///
    /// Change the normal background to light gray and hover to light blue
    /// myButton.OverrideStyleBoxWith(
    ///     (StyleBoxSubtype.Button.Normal, box => box.With(bgColor: Colors.LightGray)),
    ///     (StyleBoxSubtype.Button.Hover, box => box.With(bgColor: Colors.LightBlue))
    /// );
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of the Control</typeparam>
    /// <param name="control">The control whose StyleBoxFlat will be overridden.</param>
    /// <param name="styleBoxSubtype">The subtype of the stylebox to be overridden, use StyleBoxSubtypes for this.</param>
    /// <param name="entries">
    /// Tuples containing the StyleBox subtype string (e.g., "normal", "hover") 
    /// and a modification function that takes the current StyleBoxFlat and returns a modified one.
    /// </param>
    public static T OverrideStyleBoxWith<T>(
        this T control,
        params (string styleBoxSubtype, Func<StyleBoxFlat, StyleBoxFlat> modify)[] entries
    ) where T : Control
    {
        foreach (var (type, modify) in entries)
        {
            if (control.GetThemeStylebox(type) is not StyleBoxFlat flat)
            {
                GD.PushWarning($"Control '{control.Name}' does not have a StyleBoxFlat for type '{type}'");
                continue;
            }

            var modified = modify(flat.With());
            if (modified != null)
                control.AddThemeStyleboxOverride(type, modified);
            else
                GD.PushWarning($"Modify function returned null for type '{type}' on control '{control.Name}'");
        }

        return control;
    }

    /// <summary>
    /// Immutable way of modifying an a Stylebox's properties, returns the modified StyleBox
    /// 
    /// Works similar to Dart's Freezed implementation on Objects
    /// 
    /// Example usage with a Panel:
    /// <code>
    /// var panel = GetNode<Panel>("MyPanel");
    /// var style = panel.GetThemeStylebox("panel") as StyleBoxFlat;
    /// var newStyle = style.With(bgColor: Colors.LightBlue);
    /// panel.AddThemeStyleboxOverride("panel", newStyle);
    /// </code>
    ///
    /// </summary>
    public static StyleBoxFlat With(
        this StyleBoxFlat original,
        Color? bgColor = null,
        Color? borderColor = null,
        Color? shadowColor = null,
        int? shadowSize = null,
        Vector2? shadowOffset = null,
        bool? drawCenter = null,
        int? cornerDetail = null,
        CornerRadius cornerRadius = null,
        BorderWidth borderWidth = null,
        Margin expandMargin = null,
        Margin contentMargin = null
    )
    {
        if (original == null)
        {
            GD.PushWarning("Cannot call With() on a null StyleBoxFlat");
            return null;
        }

        if (original.Duplicate() is not StyleBoxFlat copy)
        {
            GD.PushWarning("Duplicate() returned null for StyleBoxFlat");
            return null;
        }

        copy.BgColor = bgColor ?? copy.BgColor;
        copy.BorderColor = borderColor ?? copy.BorderColor;
        copy.ShadowColor = shadowColor ?? copy.ShadowColor;
        copy.ShadowSize = shadowSize ?? copy.ShadowSize;
        copy.ShadowOffset = shadowOffset ?? copy.ShadowOffset;
        copy.DrawCenter = drawCenter ?? copy.DrawCenter;
        copy.CornerDetail = cornerDetail ?? copy.CornerDetail;

        copy.CornerRadiusTopLeft = cornerRadius?.TopLeft ?? copy.CornerRadiusTopLeft;
        copy.CornerRadiusTopRight = cornerRadius?.TopRight ?? copy.CornerRadiusTopRight;
        copy.CornerRadiusBottomLeft = cornerRadius?.BottomLeft ?? copy.CornerRadiusBottomLeft;
        copy.CornerRadiusBottomRight = cornerRadius?.BottomRight ?? copy.CornerRadiusBottomRight;

        copy.BorderWidthTop = borderWidth?.Top ?? copy.BorderWidthTop;
        copy.BorderWidthRight = borderWidth?.Right ?? copy.BorderWidthRight;
        copy.BorderWidthBottom = borderWidth?.Bottom ?? copy.BorderWidthBottom;
        copy.BorderWidthLeft = borderWidth?.Left ?? copy.BorderWidthLeft;

        copy.ExpandMarginTop = expandMargin?.Top ?? copy.ExpandMarginTop;
        copy.ExpandMarginRight = expandMargin?.Right ?? copy.ExpandMarginRight;
        copy.ExpandMarginBottom = expandMargin?.Bottom ?? copy.ExpandMarginBottom;
        copy.ExpandMarginLeft = expandMargin?.Left ?? copy.ExpandMarginLeft;

        copy.ContentMarginTop = contentMargin?.Top ?? copy.ContentMarginTop;
        copy.ContentMarginRight = contentMargin?.Right ?? copy.ContentMarginRight;
        copy.ContentMarginBottom = contentMargin?.Bottom ?? copy.ContentMarginBottom;
        copy.ContentMarginLeft = contentMargin?.Left ?? copy.ContentMarginLeft;

        return copy;
    }
}

