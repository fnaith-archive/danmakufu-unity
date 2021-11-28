using Util;

namespace Port.Graphic
{
	public interface SpriteDrawer
	{
		
		Camera GetCamera();
		
		void AddSprite(int entity, int spriteData, Position position, RenderInfo renderInfo);
		
		void RefreshSprite(int entity, int spriteData, RenderInfo renderInfo);
		
		void RefreshSpriteTransform(int entity, Position position, RenderInfo renderInfo);
		
		void RemoveSprite(int entity);
		
		void DrawLine(Position start, Position end, Color color);
	}
}
