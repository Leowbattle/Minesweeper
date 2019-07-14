using System;
using Microsoft.Xna.Framework;

namespace Minesweeper
{
	public class MenuScreen : Screen
	{
		string title = "Minesweeper";
		private Vector2 titlePos;

		public MenuScreen()
		{
			titlePos = new Vector2((Game.Window.ClientBounds.Width - Game.FontTitle.MeasureString(title).X) / 2, 80);

			var easy = new Button();
			easy.Text = "Easy";
			easy.Position = new Point((int)titlePos.X / Game.WindowScale, (int)titlePos.Y - Game.FontTitle.LineSpacing / Game.WindowScale / 2);
			easy.Clicked += () => Game.CurrentScreen = new PlayScreen(Difficulty.Easy);
			AddChild(easy);

			var medium = new Button();
			medium.Text = "Medium";
			medium.Position = new Point((int)titlePos.X / Game.WindowScale, (int)titlePos.Y - Game.FontTitle.LineSpacing / Game.WindowScale / 2 + 20);
			medium.Clicked += () => Game.CurrentScreen = new PlayScreen(Difficulty.Medium);
			AddChild(medium);

			var hard = new Button();
			hard.Text = "Hard";
			hard.Position = new Point((int)titlePos.X / Game.WindowScale, (int)titlePos.Y - Game.FontTitle.LineSpacing / Game.WindowScale / 2 + 40);
			hard.Clicked += () => Game.CurrentScreen = new PlayScreen(Difficulty.Hard);
			AddChild(hard);
		}

		public override void Draw()
		{
			SpriteBatch.Draw(Game.MenuBG, Vector2.Zero);
			SpriteBatch.Draw(Game.PixelTexture, new Rectangle(0, 0, Game.GameWidth, Game.GameHeight), new Color(0, 0, 0, 0.5f));

			base.Draw();
		}

		public override void DrawUnscaled()
		{
			base.DrawUnscaled();

			SpriteBatch.DrawString(Game.FontTitle, title, titlePos, Color.White);
		}
	}
}
