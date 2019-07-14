using System;
namespace Minesweeper
{
	public class PlayScreen : Screen
	{
		public PlayScreen(Difficulty difficulty)
		{
			AddChild(new Board(difficulty));
		}
	}
}
