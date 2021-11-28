using System;
using UnityEngine;
using Port.Graphic;

namespace Adapter.Loader
{
	public class UnitySpriteLoader
	{
		private UnityTextureLoader unityTextureLoader;

		public UnitySpriteLoader()
		{
			SetUnityTextureLoader(null);
		}
		public void SetUnityTextureLoader(UnityTextureLoader unityTextureLoader)
		{
			this.unityTextureLoader = unityTextureLoader;
		}
		public UnityTextureLoader GetUnityTextureLoader()
		{
			return unityTextureLoader;
		}
		public virtual Texture2D Load(SpriteParameter spriteParameter)
		{
			try
            {
                Texture2D texture = unityTextureLoader.Load(spriteParameter.Path);
                int width = spriteParameter.Width;
                int height = spriteParameter.Height;
                Color[] colors = texture.GetPixels(spriteParameter.Left, spriteParameter.Bottom, width, height);
                return Create(width, height, colors);
			}
			catch (Exception e)
			{
				//Log.error(String.Format("{0}-{1}-{2}-{3}-{4}", spriteParameter.getPath(),
                //    spriteParameter.getLeft(), spriteParameter.getBottom(), spriteParameter.getWidth(), spriteParameter.getHeight()));
				//Log.error(e.ToString());
				return null;
			}
		}

        public Texture2D Create(int width, int height, Color[] colors)
        {
            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(colors, 0);
            texture.Apply();
            Init(texture);
            return texture;
        }
        public Texture2D CreateSingleColor(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            Init(texture);
            return texture;
        }
        private void Init(Texture2D texture)
        {
            texture.filterMode = FilterMode.Point;
        }
    }
}
