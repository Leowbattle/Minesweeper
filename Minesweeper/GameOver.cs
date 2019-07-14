using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
	public class GameOver : GameObject
	{
		public GameOver()
		{
			StartCoroutine(Fade());
			StartCoroutine(DoGameOver());
		}

		float fade = 0;
		private IEnumerator Fade()
		{
			while (fade < 0.5)
			{
				fade += 0.02f;
				yield return null;
			}
		}

		private const string Text = "Game Over!";
		Vector2 textPos;
		private IEnumerator DoGameOver()
		{
			textPos = new Vector2((Game.Window.ClientBounds.Width - Game.FontTitle.MeasureString(Text).X) / 2, -150);
			while (textPos.Y < 80)
			{
				textPos.Y += 4;
				yield return null;
			}
		}

		public override void Draw()
		{
			base.Draw();

			SpriteBatch.Draw(Game.PixelTexture, new Rectangle(0, 0, Minesweeper.GameWidth, Minesweeper.GameHeight), new Color(0, 0, 0, fade));
		}

		public override void DrawUnscaled()
		{
			base.DrawUnscaled();

			SpriteBatch.DrawString(Game.FontTitle, Text, textPos, Color.White);
		}
	}
}
