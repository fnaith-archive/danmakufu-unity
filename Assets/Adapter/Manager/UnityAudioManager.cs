using System;
using System.Collections.Generic;
using UnityEngine;
using Adapter.Loader;

namespace Adapter.Manager
{
	public class UnityAudioManager : UnityAudioLoader
	{
		private IDictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
		public UnityAudioManager() : base()
		{
		}
		public override AudioClip Load(string path)
        {
			return Get(path);
		}
		public AudioClip Get(string path)
		{
			try
			{
				AudioClip audioClip;
				if (!cache.TryGetValue(path, out audioClip))
				{
					audioClip = base.Load(path);
					cache.Add(path, audioClip);
				}
				return audioClip;
			}
			catch (Exception e)
			{
				//Log.error(e.ToString());
				return null;
			}
		}
    }
}
