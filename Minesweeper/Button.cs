using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
	public class Button : GameObject
	{
		public Action Clicked;

		public readonly Point Size = new Point(64, 16);
		public Point Position;

		public bool Hovered { get; private set; }
		public bool Held { get; private set; }

		public string Text;

		public override void Update()
		{
			base.Update();

			if (new Rectangle(Position, Size).Contains(Input.MousePos / new Point(Game.WindowScale)))
			{
				Hovered = true;

				if (Input.LeftMouseClicked)
				{
					Held = true;
				}
				else if (Input.LastMouseState.LeftButton == ButtonState.Pressed && !Input.LeftMouseDown)
				{
					Clicked?.Invoke();
					Held = false;
				}
			}
			else
			{
				Hovered = false;
			}
		}

		public override void Draw()
		{
			base.Draw();

			var sourceRect = new Rectangle(Point.Zero, Size);
			var destRect = new Rectangle(Position, Size);

			SpriteBatch.Draw(Game.UITexture, destRect, sourceRect, Color.White);
			if (Held)
			{
				SpriteBatch.Draw(Game.PixelTexture, destRect, new Color(0, 0, 0, 0.25f));
			}
			else if (Hovered)
			{
				SpriteBatch.Draw(Game.PixelTexture, destRect, new Color(0, 0, 0, 0.125f));
			}
		}
		public override void DrawUnscaled()
		{
			base.DrawUnscaled();

			if (Text != null)
			{
				SpriteBatch.DrawString(Game.FontRegular, Text, new Vector2(Position.X * Game.WindowScale + 8, Position.Y * Game.WindowScale - 8), Color.White);
			}
		}
	}
}
