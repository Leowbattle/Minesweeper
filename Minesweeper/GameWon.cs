using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
	public class GameWon : GameObject
	{
		public GameWon()
		{
			StartCoroutine(Fade());
			StartCoroutine(DoGameWon());
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

		private const string Text = "Game Won!";
		Vector2 textPos = new Vector2(-1000, -1000);
		private IEnumerator DoGameWon()
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

			SpriteBatch.Draw(Game.PixelTexture, new Rectangle(0, 0, Game.GameWidth, Game.GameHeight), new Color(fade, fade, fade, fade));
		}

		public override void DrawUnscaled()
		{
			base.DrawUnscaled();

			SpriteBatch.DrawString(Game.FontTitle, Text, textPos, Color.White);
		}
	}
}
