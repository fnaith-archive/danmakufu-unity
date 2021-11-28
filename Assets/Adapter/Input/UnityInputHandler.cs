using UnityEngine;
using UnityEngine.EventSystems;
using Port.Input;
using Util;

namespace Adapter.Input
{
	public class UnityInputHandler
	{
        private Position position = new Position();
        public UnityInputHandler()
        {
        }
        public void update(InputHandler inputHandler)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            position.X = mousePosition.x;
            position.Y = mousePosition.y;
            inputHandler.SetMousePosition(position);

            bool isMouseOnScene = IsMouseOnScene();
            bool leftMousePressed = (isMouseOnScene && UnityEngine.Input.GetMouseButton(0));
            bool rightMousePressed = (isMouseOnScene && UnityEngine.Input.GetMouseButton(1));
            bool leftMouseDown = (isMouseOnScene && UnityEngine.Input.GetMouseButtonDown(0));
            bool rightMouseDown = (isMouseOnScene && UnityEngine.Input.GetMouseButtonDown(1));
            inputHandler.LeftMousePressed = leftMousePressed;
            inputHandler.RightMousePressed = rightMousePressed;
            inputHandler.LeftMouseDown = leftMouseDown;
            inputHandler.RightMouseDown = rightMouseDown;

            bool keyWPressed = UnityEngine.Input.GetKey(KeyCode.W);
            bool keyAPressed = UnityEngine.Input.GetKey(KeyCode.A);
            bool keySPressed = UnityEngine.Input.GetKey(KeyCode.S);
            bool keyDPressed = UnityEngine.Input.GetKey(KeyCode.D);
            bool keySpacePressed = UnityEngine.Input.GetKey(KeyCode.Space);
            bool keyLShiftPressed = UnityEngine.Input.GetKey(KeyCode.LeftShift);
            inputHandler.KeyWPressed = keyWPressed;
            inputHandler.KeyAPressed = keyAPressed;
            inputHandler.KeySPressed = keySPressed;
            inputHandler.KeyDPressed = keyDPressed;
            inputHandler.KeySpacePressed = keySpacePressed;
            inputHandler.KeyLShiftPressed = keyLShiftPressed;

            inputHandler.Update();
        }

		private bool IsMouseOnScene()
		{
			return !EventSystem.current.IsPointerOverGameObject();
		}
	}
}
