using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
	public static class Input
	{
		public static MouseState MouseState;
		public static MouseState LastMouseState;

		// Mouse position in the game's logical coordinate space (see Minesweeper.Draw).
		// The browser canvas is stretched to the window, so translate from window pixels
		// into the letterboxed game frame.
		public static Point MousePos { get; private set; }

		public static bool LeftMouseDown => MouseState.LeftButton == ButtonState.Pressed;
		public static bool LeftMouseClicked => MouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton != ButtonState.Pressed;

		public static bool RightMouseDown => MouseState.RightButton == ButtonState.Pressed;
		public static bool RightMouseClicked => MouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton != ButtonState.Pressed;

		public static KeyboardState KeyboardState;
		public static KeyboardState LastKeyboardState;

		public static bool KeyJustPressed(Keys key) => KeyboardState.IsKeyDown(key) && !LastKeyboardState.IsKeyDown(key);

		public static void Update()
		{
			LastMouseState = MouseState;
			MouseState = Mouse.GetState();

			LastKeyboardState = KeyboardState;
			KeyboardState = Keyboard.GetState();

			UpdateMousePosition();
		}

		private static void UpdateMousePosition()
		{
#if BLAZORGL
			var game = Minesweeper.Instance;
			var device = game?.GraphicsDevice;

			if (device == null)
			{
				MousePos = MouseState.Position;
				return;
			}

			int logicalWidth = game.LogicalWidth;
			int logicalHeight = game.LogicalHeight;

			var pp = device.PresentationParameters;
			float scale = Math.Min(
				(float)pp.BackBufferWidth / logicalWidth,
				(float)pp.BackBufferHeight / logicalHeight);

			if (scale <= 0)
			{
				MousePos = MouseState.Position;
				return;
			}

			float offsetX = (pp.BackBufferWidth - logicalWidth * scale) * 0.5f;
			float offsetY = (pp.BackBufferHeight - logicalHeight * scale) * 0.5f;

			var position = MouseState.Position;
			MousePos = new Point(
				(int)((position.X - offsetX) / scale),
				(int)((position.Y - offsetY) / scale));
#else
			MousePos = MouseState.Position;
#endif
		}
	}
}
