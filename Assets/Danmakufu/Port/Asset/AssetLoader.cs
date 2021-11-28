namespace Port.Asset
{
	public interface AssetLoader
	{
		string LoadText(string path);
		int LoadSprite(string path, int left, int top, int right, int bottom, Graphic.BlendMode blendMode);
		int LoadAudio(string path);
		
	}
}
