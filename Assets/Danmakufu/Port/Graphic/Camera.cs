namespace Port.Graphic
{
	public interface Camera
	{
		bool SetPosition(double x, double y);
		void GetPosition(out double x, out double y);
		bool SetRotation(double degree);
		double GetRotation();
		bool SetScale(double scale);
		double GetScale();
	}
}
