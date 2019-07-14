using System;

namespace Minesweeper
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var game = new Minesweeper())
			{
				game.Run();
			}
		}
	}
}
