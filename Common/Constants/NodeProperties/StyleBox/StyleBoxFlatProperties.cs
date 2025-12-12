

using System.Numerics;


public static class StyleBoxFlatProperties
{
	public const string BgColor = "bg_color";
	public const string BorderColor = "border_color";
	public const string ShadowColor = "shadow_color";
	public const string DrawCenter = "draw_center";
	public const string CornerDetail = "corner_detail";

	public static class CornerRadius
	{
		public const string TopLeft = "corner_radius_top_left";
		public const string TopRight = "corner_radius_top_right";
		public const string BottomLeft = "corner_radius_bottom_left";
		public const string BottomRight = "corner_radius_bottom_right";
	}

	public static class BorderWidth
	{
		public const string Top = "border_width_top";
		public const string Bottom = "border_width_bottom";
		public const string Left = "border_width_left";
		public const string Right = "border_width_right";
	}

	public static class ExpandMargin
	{
		public const string Top = "expand_margin_top";
		public const string Bottom = "expand_margin_bottom";
		public const string Left = "expand_margin_left";
		public const string Right = "expand_margin_right";
	}

	public static class ContentMargin
	{
		public const string Top = "content_margin_top";
		public const string Bottom = "content_margin_bottom";
		public const string Left = "content_margin_left";
		public const string Right = "content_margin_right";
	}
}

public record CornerRadius(int? TopLeft = null, int? TopRight = null, int? BottomRight = null, int? BottomLeft = null)
{
	public int? All
	{
		init
		{
			TopLeft = value;
			TopRight = value;
			BottomLeft = value;
			BottomRight = value;
		}
	}
}

public record BorderWidth(int? Top = null, int? Right = null, int? Bottom = null, int? Left = null)
{
	public int? All
	{
		init
		{
			Top = value;
			Right = value;
			Bottom = value;
			Left = value;
		}
	}
}

public record Margin(float? Top = null, float? Right = null, float? Bottom = null, float? Left = null)
{
	public float? All
	{
		init
		{
			Top = value;
			Right = value;
			Bottom = value;
			Left = value;
		}
	}
}
