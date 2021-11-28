using System;
using System.Collections.Generic;
using UnityEngine;
using Port.Graphic;
using Adapter.Loader;

namespace Adapter.Manager
{
	public class UnityShaderManager : UnityShaderLoader
	{
		private IDictionary<int, Shader> cache = new Dictionary<int, Shader>();

		public UnityShaderManager() : base()
		{
		}

		public override Shader Load(BlendMode blendMode)
		{
			return get(blendMode);
		}

		public Shader get(BlendMode blendMode)
		{
			try
			{
				Shader shader;
				if (!cache.TryGetValue((int)blendMode, out shader))
				{
					shader = base.Load(blendMode);
					cache.Add((int)blendMode, shader);
				}
				return shader;
			}
			catch (Exception e)
			{
				//Log.error(e.ToString());
				return null;
			}
		}
	}
}
