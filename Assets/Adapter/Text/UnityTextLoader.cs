using System;
using UnityEngine;
using Port.Text;
using Adapter.Loader;

namespace Adapter.Text
{
	public class UnityTextLoader : TextLoader
    {
		public UnityTextLoader() : base()
		{
		}
		public string Load(string path)
		{
			try
			{
				TextAsset textAsset = UnityLoader.LoadResource(path) as TextAsset;
				string text = textAsset.text;
				return text;
			}
			catch (Exception e)
			{
				//Log.error(e.ToString());
				return null;
			}
		}
	}
}
