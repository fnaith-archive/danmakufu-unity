using System;
using UnityEngine;
using Util;
using Port.Graphic;

namespace Adapter.Graphic
{
	public class UnityCamera : Port.Graphic.Camera
    {
		private const double DEFAULT_POSITION_X = 5;
		private const double DEFAULT_POSITION_Y = 5;
		private const double DEFAULT_POSITION_Z = -100;

		private const float DEFAULT_ROTATION_X = 0;
		private const float DEFAULT_ROTATION_Y = 0;
		private const float DEFAULT_ROTATION_Z = 0;

		private const double DEFAULT_SCALE = 1;

		private const TransparencySortMode DEFAULT_PROJECTION_MODE = TransparencySortMode.Orthographic;

		private const double DEFAULT_ASPECT_HEIGHT = 16;
		private const double DEFAULT_ASPECT_WIDTH = 9;

		private UnityEngine.Camera camera;
		private Position position;
		private double degree;
		private double scale;

		public UnityCamera(UnityEngine.Camera camera) : base()
        {
            this.camera = camera;
            this.camera.transparencySortMode = TransparencySortMode.Orthographic;
            position = new Position();
            SetPosition(DEFAULT_POSITION_X, DEFAULT_POSITION_Y);
            SetRotation(DEFAULT_ROTATION_Z);
            SetScale(DEFAULT_SCALE);
        }

		public bool SetPosition(double x, double y)
        {
            try
            {
                Vector3 position = camera.transform.position;
                position.Set((float)x, (float)y, (float)DEFAULT_POSITION_Z);
                camera.transform.position = position;
                this.position.X = x;
                this.position.Y = y;
                return true;
            }
            catch (Exception e)
            {
                //Log.error(e);
                return false;
            }
        }
		public void GetPosition(out double x, out double y)
		{
			x = position.X;
			y = position.Y;
		}
		public bool SetRotation(double degree)
		{
            try
            {
                Vector3 eulerAngles = camera.transform.eulerAngles;
                eulerAngles.Set(DEFAULT_ROTATION_X, DEFAULT_ROTATION_Y, (float)degree);
                camera.transform.eulerAngles = eulerAngles;
                this.degree = degree;
                return true;
            }
            catch (Exception e)
            {
                //Log.error(e);
                return false;
            }
        }
		public double GetRotation()
		{
			return degree;
		}
		public bool SetScale(double scale)
        {
            try
            {
                camera.orthographicSize = (float)(camera.orthographicSize * scale);
                this.scale = scale;
                return true;
            }
            catch (Exception e)
            {
                //Log.error(e);
                return false;
            }
        }
		public double GetScale()
		{
			return scale;
		}
		public UnityEngine.Camera GetCamera()
		{
			return camera;
		}
	}
}
