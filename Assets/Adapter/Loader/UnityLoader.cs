using System.IO;
using UnityEngine;

namespace Adapter.Loader
{
	public class UnityLoader
	{
		public static Object LoadResource(string path)
		{
			return Resources.Load(GetFullPathWithoutExtension(path));
		}
		private static string GetFullPathWithoutExtension(string path)
		{
			return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
		}
	}
}
