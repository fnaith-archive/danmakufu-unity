using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Port.Graphic;
using Adapter.Asset;

namespace Adapter.Graphic
{
	public class UnitySpriteDrawer : SpriteDrawer
	{
        public static int SORTING_LAYER_FOREGROUND = SortingLayer.NameToID("Foreground");
        public static int SORTING_LAYER_DEPLOY = SortingLayer.NameToID("Deploy");
        public static int SORTING_LAYER_BACKGROUND = SortingLayer.NameToID("Background");
        static int PREALLOC_SPRITE_SIZE = 4000;
        public static float LayerToDepth(int layer)
        {
            return -layer / 10f;
        }
        public static int LayerToSortingLayer(int renderLayer)
        {
           /* if (renderLayer < EntityTypeExtender.toRenderLayer(EntityType.SHADOW))
            {
                return SORTING_LAYER_BACKGROUND;
            }
            else if (EntityTypeExtender.toRenderLayer(EntityType.DEPLOY) < renderLayer)
            {
                return SORTING_LAYER_FOREGROUND;
            }
            else
            {*/
                return SORTING_LAYER_DEPLOY;
            //}
        }
		private UnityAssetLoader unityAssetLoader;
        private UnityCamera camera;
        private int layer;
        private GameObject[] entityToSpriteGameObject;
        private GameObject sprites;
        private IList<GameObject> spritesPool;
        private string[] spriteNames;
        private Vector3 eulerAngles;

        private GameObject triangles;
        private GameObject[] entityToTriangleGameObject;
        private Material triangleMaterial;

        private GameObject lasers;
        private GameObject[] entityToLaserGameObject;

        private Vector3 drawLineStart;
		private Vector3 drawLineEnd;
		private UnityEngine.Color drawLineColor;

        public UnitySpriteDrawer(UnityAssetLoader unityAssetLoader, UnityCamera camera, string name)
		{
			this.unityAssetLoader = unityAssetLoader;
            this.camera = camera;
            int maxEntitySize = 1024;// PowerOfTwo.require(EcxEntityManager.MAX_ENTITY) + 1;
            layer = LayerMask.NameToLayer(name);
            entityToSpriteGameObject = new GameObject[maxEntitySize];
            PrepareSprites(name);
            spritesPool = new List<GameObject>();
            spriteNames = new string[maxEntitySize];
            for (int i = 0; i < maxEntitySize; ++i)
            {
                spriteNames[i] = i.ToString();
            }
            eulerAngles = new Vector3();
            GameObject[] preallocSprite = new GameObject[PREALLOC_SPRITE_SIZE];
            for (int i = 0; i < PREALLOC_SPRITE_SIZE; ++i)
            {
                GameObject sprite = AllocSprite();
                sprite.GetComponent<SpriteRenderer>().material.shader = unityAssetLoader.GetShader(BlendMode.ALPHA);
                preallocSprite[i] = sprite;
            }
            for (int i = 0; i < PREALLOC_SPRITE_SIZE; ++i)
            {
                FreeSprite(preallocSprite[i]);
            }

            entityToTriangleGameObject = new GameObject[maxEntitySize];
            triangleMaterial = unityAssetLoader.GetTriangleMaterial();

            entityToLaserGameObject = new GameObject[maxEntitySize];

		    drawLineStart = new Vector3();
		    drawLineEnd = new Vector3();
		    drawLineColor = new UnityEngine.Color();
        }
        private void PrepareSprites(string name)
        {
            sprites = new GameObject();
            sprites.name = name + "-sprites";
        }
        public Port.Graphic.Camera GetCamera()
		{
			return camera;
        }
        public void AddSprite(int entity, int spriteData, Position position, RenderInfo renderInfo)
		{
            GameObject gameObject = AllocSprite();
            gameObject.name = spriteNames[entity/*.getId()*/];
            entityToSpriteGameObject[entity/*.getId()*/] = gameObject;

            Vector3 pos = gameObject.transform.position;
            pos.Set((float)(position.X + renderInfo.OffsetX),
                (float)(position.Y + renderInfo.OffsetY),
                LayerToDepth(renderInfo.Layer));
            gameObject.transform.position = pos;
            eulerAngles.z = (float)renderInfo.Angle;
            gameObject.transform.eulerAngles = eulerAngles;

            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = LayerToSortingLayer(renderInfo.Layer);
            if (SORTING_LAYER_DEPLOY == spriteRenderer.sortingLayerID)
            {
                spriteRenderer.sortingOrder = (int)(-Math.Floor(pos.y - renderInfo.OffsetY));
            }

            spriteRenderer.flipX = renderInfo.FlipH;
            spriteRenderer.flipY = renderInfo.FlipV;

            spriteRenderer.sprite = unityAssetLoader.GetSprite(spriteData);
            Shader shader = unityAssetLoader.GetShader(spriteData);
            if (shader != spriteRenderer.material.shader)
            {
                spriteRenderer.material.shader = shader;
            }

            Vector3 localScale = gameObject.transform.localScale;
            localScale.Set((float)(renderInfo.Width / spriteRenderer.sprite.texture.width) * 100,
                (float)(renderInfo.Height / spriteRenderer.sprite.texture.height) * 100, 1);
            gameObject.transform.localScale = localScale;

            spriteRenderer.color = ToUnityColor(renderInfo.Color);
        }

		public void RefreshSprite(int entity, int spriteData, RenderInfo renderInfo)
		{
            GameObject gameObject = entityToSpriteGameObject[entity/*.getId()*/];
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = unityAssetLoader.GetSprite(spriteData);
            Shader shader = unityAssetLoader.GetShader(spriteData);
            if (shader != spriteRenderer.material.shader)
            {
                spriteRenderer.material.shader = shader;
            }

            Vector3 localScale = gameObject.transform.localScale;
            localScale.Set((float)(renderInfo.Width / spriteRenderer.sprite.texture.width) * 100,
                (float)(renderInfo.Height / spriteRenderer.sprite.texture.height) * 100, 1);
            gameObject.transform.localScale = localScale;
        }

		public void RefreshSpriteTransform(int entity, Position position, RenderInfo renderInfo)
        {
            GameObject gameObject = entityToSpriteGameObject[entity/*.getId()*/];
            Vector3 pos = gameObject.transform.position;
            pos.Set((float)(position.X + renderInfo.OffsetX),
                (float)(position.Y + renderInfo.OffsetY),
                LayerToDepth(renderInfo.Layer));
            gameObject.transform.position = pos;
            eulerAngles.z = (float)renderInfo.Angle;
            gameObject.transform.eulerAngles = eulerAngles;

            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = LayerToSortingLayer(renderInfo.Layer);
            if (SORTING_LAYER_DEPLOY == spriteRenderer.sortingLayerID)
            {
                spriteRenderer.sortingOrder = (int)(-Math.Floor(pos.y - renderInfo.OffsetY));
            }

            spriteRenderer.flipX = renderInfo.FlipH;
            spriteRenderer.flipY = renderInfo.FlipV;

            Vector3 localScale = gameObject.transform.localScale;
            localScale.Set((float)(renderInfo.Width / spriteRenderer.sprite.texture.width) * 100,
                (float)(renderInfo.Height / spriteRenderer.sprite.texture.height) * 100, 1);
            gameObject.transform.localScale = localScale;

            spriteRenderer.color = ToUnityColor(renderInfo.Color);
        }

		public void RemoveSprite(int entity)
        {
            GameObject gameObject = entityToSpriteGameObject[entity/*.getId()*/];
            entityToSpriteGameObject[entity/*.getId()*/] = null;
            FreeSprite(gameObject);
        }
        private GameObject AllocSprite()
        {
            GameObject gameObject;
            if (0 != spritesPool.Count)
            {
                int lastIndex = spritesPool.Count - 1;
                gameObject = spritesPool[lastIndex];
                spritesPool.RemoveAt(lastIndex);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject = new GameObject();
                gameObject.transform.SetParent(sprites.transform);
                gameObject.AddComponent<SpriteRenderer>();
                gameObject.layer = layer;
            }
            return gameObject;
        }
        private void FreeSprite(GameObject gameObject)
        {
            gameObject.SetActive(false);
            spritesPool.Add(gameObject);
        }
        public void DrawLine(Position s, Position e, Util.Color color)
		{
			drawLineStart.x = (float)s.X;
			drawLineStart.y = (float)s.Y;
			drawLineEnd.x = (float)e.X;
			drawLineEnd.y = (float)e.Y;
			Debug.DrawLine(drawLineStart, drawLineEnd, ToUnityColor(color), 0, false);
		}
        private UnityEngine.Color ToUnityColor(Util.Color src)
        {
            UnityEngine.Color dist = new UnityEngine.Color();
            dist.r = (float)src.R;
            dist.g = (float)src.G;
            dist.b = (float)src.B;
            dist.a = (float)src.A;
            return dist;
        }
    }
}
