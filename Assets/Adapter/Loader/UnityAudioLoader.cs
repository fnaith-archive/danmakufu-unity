using System;
using UnityEngine;

namespace Adapter.Loader
{
	public class UnityAudioLoader
	{
		public UnityAudioLoader()
		{
		}
		public virtual AudioClip Load(string path)
		{
			try
			{
				AudioClip audioClip = UnityLoader.LoadResource(path) as AudioClip;
				return audioClip;
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
