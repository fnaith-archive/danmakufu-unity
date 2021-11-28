using System;
using System.Collections.Generic;
using UnityEngine;
using Port.Graphic;
using Adapter.Loader;

namespace Adapter.Manager
{
	public class UnitySpriteManager : UnitySpriteLoader
	{
		private IDictionary<SpriteParameter, Texture2D> cache = new Dictionary<SpriteParameter, Texture2D>(new SpriteParameterComparer());
		public UnitySpriteManager() : base()
		{
		}
		public override Texture2D Load(SpriteParameter spriteParameter)
        {
			return get(spriteParameter);
		}
		public Texture2D get(SpriteParameter spriteParameter)
        {
			try
			{
				Texture2D texture;
				if (!cache.TryGetValue(spriteParameter, out texture))
                {
                    texture = base.Load(spriteParameter);
					cache.Add(spriteParameter, texture);
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
