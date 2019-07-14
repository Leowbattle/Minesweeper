using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
	public static class Input
	{
		public static MouseState MouseState;
		public static MouseState LastMouseState;

		public static Point MousePos => MouseState.Position;

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
		}
	}
}
