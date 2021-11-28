using System.Collections.Generic;

namespace Port.Graphic
{
	public class SpriteParameter
	{
		public string Path { get; set; } = "";
		public int Left { get; set; } = 0;
		public int Top { get; set; } = 0;
		public int Right { get; set; } = 0;
		public int Bottom { get; set; } = 0;
		public int Width { get => Right - Left; }
		public int Height { get => Top - Bottom; }
		public SpriteParameter(string path, int left, int top, int right, int bottom)
        {
			Path = path;
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}
	public class SpriteParameterComparer : EqualityComparer<SpriteParameter>
	{
		public override int GetHashCode(SpriteParameter spriteParameter)
		{
			return spriteParameter.Left ^ spriteParameter.Top;
		}

		public override bool Equals(SpriteParameter a, SpriteParameter b)
		{
			return ((a.Path == b.Path) && (a.Left == b.Left) && (a.Top == b.Top)
					&& (a.Right == b.Right) && (a.Bottom == b.Bottom));
		}
	}
}
