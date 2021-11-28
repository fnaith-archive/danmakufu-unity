using System;
using Port.Graphic;
using UnityEngine;

namespace Adapter.Loader
{
	public class UnityShaderLoader
	{
        private Shader blend;
        private Shader add;
        private Shader mul;
        private Shader none;

		public UnityShaderLoader()
		{
			Material[] materials = GameObject.Find("Shader").GetComponent<Renderer>().materials;
			blend = materials[0].shader;
			add = materials[1].shader;
			mul = materials[2].shader;
			none = materials[3].shader;
		}
		public virtual Shader Load(BlendMode blendMode)
		{
			try
			{
				Shader shader = BlendModeToShader(blendMode);
				return shader;
			}
			catch (Exception e)
            {
                //Log.error(blendMode);
                //Log.error(e.ToString());
				return null;
			}
		}
		private Shader BlendModeToShader(BlendMode blendMode)
		{
			switch (blendMode)
			{
				case BlendMode.ALPHA:
					return blend;
				case BlendMode.ADD:
					return add;
				case BlendMode.MUL:
					return mul;
				case BlendMode.NONE:
					return none;
				default:
					return blend;
			}
		}
	}
}
