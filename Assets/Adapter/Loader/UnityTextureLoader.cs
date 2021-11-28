using System;
using UnityEngine;

namespace Adapter.Loader
{
	public class UnityTextureLoader
	{
		public UnityTextureLoader()
		{
		}
		public virtual Texture2D Load(string path)
		{
			try
			{
				Texture2D texture = UnityLoader.LoadResource(path) as Texture2D;
				return texture;
			}
			catch (Exception e)
			{
				//Log.error(path);
				//Log.error(e.ToString());
				return null;
			}
		}
	}
}
