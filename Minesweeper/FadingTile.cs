using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Minesweeper
{
	public class FadingTile : GameObject
	{
		public FadingTile(Point position, Color c1, Color c2, float step)
		{
			Position = position;
			C1 = c1;
			C2 = c2;
			Step = step;

			StartCoroutine(Fade());
		}

		Point Position;
		Color C1, C2;
		float Step;
		float Amount;

		private IEnumerator Fade()
		{
			while (Amount < 1)
			{
				Amount += Step;
				yield return null;
			}

			Kill();
		}

		public override void Draw()
		{
			base.Draw();

			var colour = new Color(MathHelper.Lerp(C1.R, C2.R, Amount), MathHelper.Lerp(C1.G, C2.G, Amount), MathHelper.Lerp(C1.B, C2.B, Amount), MathHelper.Lerp(C1.A, C2.A, Amount));
			SpriteBatch.Draw(Game.TilesTexture, new Rectangle(Position * new Point(Board.Instance.BoardScale), new Point(Minesweeper.TileSize * Board.Instance.BoardScale)), new Rectangle(Minesweeper.TileSize, Minesweeper.TileSize * 2, Minesweeper.TileSize, Minesweeper.TileSize), colour);
		}
	}
}
