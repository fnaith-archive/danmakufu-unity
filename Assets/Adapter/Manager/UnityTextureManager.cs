using System;
using System.Collections.Generic;
using UnityEngine;
using Adapter.Loader;

namespace Adapter.Manager
{
	public class UnityTextureManager : UnityTextureLoader
	{
		private IDictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();
		public UnityTextureManager() : base()
		{
		}
		public override Texture2D Load(string path)
		{
			return Get(path);
		}
		public Texture2D Get(string path)
		{
			try
			{
				Texture2D texture;
				if (!cache.TryGetValue(path, out texture))
				{
					texture = base.Load(path);
					cache.Add(path, texture);
				}
				return texture;
			}
			catch (Exception e)
			{
				//Log.error(e.ToString());
				return null;
			}
		}
	}
}
