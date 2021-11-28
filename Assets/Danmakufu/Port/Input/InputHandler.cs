using System.Collections.Generic;
using Util;

namespace Port.Input
{
	public class InputHandler : InputListener
	{
		public List<InputListener> inputListeners = new List<InputListener>();
		public Position MousePosition { get; } = new Position();
		public bool LeftMousePressed { get; set; } = false;
		public bool RightMousePressed { get; set; } = false;
		public bool LeftMouseDown { get; set; } = false;
		public bool RightMouseDown { get; set; } = false;
		public bool KeyWPressed { get; set; } = false;
		public bool KeyAPressed { get; set; } = false;
		public bool KeySPressed { get; set; } = false;
		public bool KeyDPressed { get; set; } = false;
		public bool KeySpacePressed { get; set; } = false;
		public bool KeyLShiftPressed { get; set; } = false;
		public void SetMousePosition(Position position)
        {
			MousePosition.X = position.X;
			MousePosition.Y = position.Y;
		}
		public void Register(InputListener inputListener)
		{
			inputListeners.Add(inputListener);
		}
		public void Remove(InputListener inputListener)
		{
			inputListeners.Remove(inputListener);
		}
		public void Clear()
		{
			inputListeners.Clear();
		}
		public void Update()
		{
			foreach (InputListener inputListener in inputListeners)
            {
				inputListener.Update();
			}
		}
	}
}
