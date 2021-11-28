using System;
using System.Collections.Generic;
using UnityEngine;
using Port.Asset;
using Port.Graphic;
using Adapter.Text;
using Adapter.Manager;

namespace Adapter.Asset
{
	public class UnityAssetLoader : AssetLoader
    {
        private static Vector2 SPRITE_PIVOT = new Vector2(0.5f, 0.5f);
        private static Vector2 TILE_PIVOT = new Vector2(1f, 1f);

        private UnityTextLoader unityTextLoader;
        private UnitySpriteManager unitySpriteManager;
        private UnityShaderManager unityShaderManager;
		private UnityAudioManager unityAudioManager;

        private IDictionary<SpriteParameter, int>[] spriteDataCache;
        private IList<Sprite> spriteDataToSprite;
        private IList<Shader> spriteDataToShader;
        private IList<Sprite> spriteDataToTileSprite;

        private IDictionary<AudioClip, int> audioClipToAudioData;
        private IList<AudioClip> audioDataToAudioClip;

        private Material triangleMaterial;

        public UnityAssetLoader() : base()
		{
            unityTextLoader = new UnityTextLoader();
            unitySpriteManager = new UnitySpriteManager();
            unitySpriteManager.SetUnityTextureLoader(new UnityTextureManager());
            unityShaderManager = new UnityShaderManager();
            unityAudioManager = new UnityAudioManager();

            SpriteParameterComparer spriteParameterComparer = new SpriteParameterComparer();
            spriteDataCache = new IDictionary<SpriteParameter, int>[Enum.GetNames(typeof(BlendMode)).Length];
            for (int i = 0; i < spriteDataCache.Length; ++i)
            {
                spriteDataCache[i] = new Dictionary<SpriteParameter, int>(spriteParameterComparer);
            }
            spriteDataToSprite = new List<UnityEngine.Sprite>();
            spriteDataToShader = new List<Shader>();
            spriteDataToTileSprite = new List<UnityEngine.Sprite>();
            audioClipToAudioData = new Dictionary<AudioClip, int>();
            audioDataToAudioClip = new List<AudioClip>();

            triangleMaterial = new Material(unityShaderManager.Load(BlendMode.ALPHA));
            triangleMaterial.mainTexture = unitySpriteManager.CreateSingleColor(64, 64, Color.white);
        }

		public string LoadText(string path)
		{
			return unityTextLoader.Load(path);
        }

        public int LoadSprite(string path, int left, int top, int right, int bottom, BlendMode blendMode)
        {
            SpriteParameter spriteParameter = new SpriteParameter(path, left, top, right, bottom);
            IDictionary<SpriteParameter, int> paramToSpriteData = spriteDataCache[(int)blendMode];
            int spriteData;
            if (!paramToSpriteData.TryGetValue(spriteParameter, out spriteData))
            {
                Texture2D texture = unitySpriteManager.Load(spriteParameter);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), SPRITE_PIVOT);
                Shader shader = unityShaderManager.Load(blendMode);
                spriteData = spriteDataToSprite.Count;
                spriteDataToSprite.Add(sprite);
                spriteDataToShader.Add(shader);
                spriteDataToTileSprite.Add(null);
            }
            return spriteData;
        }

        public int LoadAudio(string path)
        {
            AudioClip audioClip = unityAudioManager.Load(path);
            int audioData;
            if (!audioClipToAudioData.TryGetValue(audioClip, out audioData))
            {
                audioData = audioDataToAudioClip.Count;
                audioDataToAudioClip.Add(audioClip);
                audioClipToAudioData[audioClip] = audioData;
            }
            return audioData;
        }

        public Material GetTriangleMaterial()
        {
            return triangleMaterial;
        }

        public Sprite GetSprite(int spriteData)
        {
            return spriteDataToSprite[spriteData];
        }

        public Shader GetShader(int spriteData)
        {
            return spriteDataToShader[spriteData];
        }

        public Shader GetShader(BlendMode blendMode)
        {
            return unityShaderManager.Load(blendMode);
        }

        public Sprite GetTileSprite(int spriteData)
        {
            Sprite tileSprite = spriteDataToTileSprite[spriteData];
            if (null == tileSprite)
            {
                Texture2D texture = spriteDataToSprite[spriteData].texture;
                tileSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), TILE_PIVOT, texture.height);
                spriteDataToTileSprite[spriteData] = tileSprite;
            }
            return tileSprite;
        }

        public AudioClip GetAudio(int audioData)
		{
			AudioClip audio = audioDataToAudioClip[audioData];
			return audio;
		}
    }
}
