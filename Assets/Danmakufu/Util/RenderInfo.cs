namespace Util
{
	public sealed class RenderInfo
	{
		public double OffsetX { get; set; } = 0;
		public double OffsetY { get; set; } = 0;
		public double Angle { get; set; } = 0;
		public Color Color { get; set; } = new Color();
		public double Width { get; set; } = 0;
		public double Height { get; set; } = 0;
		public bool FlipH { get; set; } = false;
		public bool FlipV { get; set; } = false;
		public int Layer { get; set; } = 0;
	}
}
