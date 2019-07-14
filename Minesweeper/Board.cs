using System;
using System.Linq;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
	public struct Tile
	{
		public bool Clicked;
		public bool Mine;
		public bool Flagged;
		public int AdjacentMines;

		public static Rectangle GetTextureRect(Tile tile)
		{
			var TileSize = Minesweeper.TileSize;

			var sourceRect = new Rectangle(new Point(), new Point(TileSize, TileSize));
			if (tile.Flagged)
			{
				sourceRect.Location = new Point(2 * Minesweeper.TileSize, 2 * Minesweeper.TileSize);
			}
			else if (!tile.Clicked)
			{
				sourceRect.Location = new Point(Minesweeper.TileSize, 2 * Minesweeper.TileSize);
			}
			else if (tile.Mine)
			{
				sourceRect.Location = new Point(3 * Minesweeper.TileSize, 2 * Minesweeper.TileSize);
			}
			else
			{
				sourceRect.Location = new Point((TileSize * tile.AdjacentMines) % (4 * TileSize), (tile.AdjacentMines / 4) * TileSize);
			}

#if DEBUG
			// If debug, show where the mines are
			if (tile.Mine)
			{
				sourceRect.Location = new Point(24, 16);
			}
#endif

			return sourceRect;
		}
	}

	public enum Difficulty
	{
		Easy,
		Medium,
		Hard
	}

	public static class DiffcultyExt
	{
		public static Point GridSize(this Difficulty d)
		{
			switch (d)
			{
				case Difficulty.Easy:
					return new Point(8, 8);
				case Difficulty.Medium:
					return new Point(16, 16);
				case Difficulty.Hard:
					return new Point(32, 32);
				default:
					throw new ArgumentException("Could not find board size");
			}
		}
	}

	public class Board : GameObject
	{
		private Tile[,] Tiles;
		private Point? HoveredTile = new Point(0, 0);

		public Difficulty Difficulty;
		public int BoardScale { get; private set; } = 1;

		public static Board Instance;

		public Board(Difficulty difficulty)
		{
			Instance = this;

			Difficulty = difficulty;
			BoardScale = Difficulty.Hard.GridSize().X / difficulty.GridSize().X;

			Tiles = new Tile[Difficulty.GridSize().X, Difficulty.GridSize().Y];
		}

		private Tile[,] GenerateTiles(Point? mousePos = null)
		{
			var random = new Random();

			var tiles = new Tile[Difficulty.GridSize().X, Difficulty.GridSize().Y];

			// Place mines
			int numMines = Difficulty.GridSize().X * Difficulty.GridSize().Y / 6;
			int[,] mineInfo = new int[Difficulty.GridSize().X, Difficulty.GridSize().Y];
			for (int i = 0; i < numMines; i++)
			{
				int x, y;
				do
				{
					x = random.Next() % Difficulty.GridSize().X;
					y = random.Next() % Difficulty.GridSize().X;
				} while (tiles[x, y].Mine || new Point(x, y) == mousePos);

				for (int x2 = -1; x2 < 2; x2++)
				{
					for (int y2 = -1; y2 < 2; y2++)
					{
						if (x2 == 0 && y2 == 0)
						{
							mineInfo[x, y] = -numMines;
						}
						else
						{
							mineInfo[x, y]++;
							tiles[x, y].Mine = mineInfo[x, y] < 0;
						}
					}
				}
			}

			return tiles;
		}

		private bool InBounds(int x, int y)
		{
			return y >= 0 && y < Tiles.GetLength(0) && x >= 0 && x < Tiles.GetLength(1);
		}

		private void ForEachTile(Tile[,] tiles, Action<int, int> action)
		{
			for (int i = 0; i < tiles.GetLength(0); i++)
			{
				for (int j = 0; j < tiles.GetLength(1); j++)
				{
					action(i, j);
				}
			}
		}

		private void ForEachNeighbour(int x, int y, Action<int, int> action)
		{
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (InBounds(j, i) && !(i == x && j == y))
					{
						action(i, j);
					}
				}
			}
		}

		private int CountAdjacentMines(int x, int y)
		{
			int count = 0;
			ForEachNeighbour(x, y, (i, j) =>
			{
				if (Tiles[i, j].Mine) count++;
			});

			return count;
		}

		// Generate the mine field after the first click, so the user never clicks on a mine in their first turn
		bool firstClick = true;

		public override void Update()
		{
			base.Update();

			if (Input.KeyJustPressed(Keys.R))
			{
				Game.CurrentScreen = new MenuScreen();
			}

			if (gameOverStarted) return;

			var hoveredTile = Input.MouseState.Position / new Point(Game.WindowScale * Minesweeper.TileSize) / new Point(BoardScale);
			if (InBounds(hoveredTile.X, hoveredTile.Y))
			{
				HoveredTile = hoveredTile;

				if (Input.RightMouseClicked && !Tiles[hoveredTile.X, hoveredTile.Y].Clicked)
				{
					Tiles[hoveredTile.X, hoveredTile.Y].Flagged ^= true;
				}

				if (Input.LeftMouseClicked && !Tiles[hoveredTile.X, hoveredTile.Y].Flagged)
				{
					if (firstClick)
					{
						firstClick = false;
						Tiles = GenerateTiles(hoveredTile);
					}

					if (Tiles[hoveredTile.X, hoveredTile.Y].Mine)
					{
						StartCoroutine(GameOver());
					}

					StartCoroutine(ShowTile(hoveredTile.X, hoveredTile.Y));
				}
			}
			else
			{
				HoveredTile = null;
			}

			if (CheckWin())
			{
				Game.Woo.Play();

				gameOverStarted = true;
				HoveredTile = null;

				AddChild(new GameWon());
			}
		}

		public bool CheckWin()
		{
			if (firstClick) return false;

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					if (Tiles[i, j].Mine)
					{
						if (!Tiles[i, j].Flagged)
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		private IEnumerator ShowTile(int x, int y, bool recur = true)
		{
			if (!Tiles[x, y].Clicked && !Tiles[x, y].Mine)
			{
				Game.Blip.Play();

				AddChild(new FadingTile(new Point(x * Minesweeper.TileSize, y * Minesweeper.TileSize), new Color(1, 1, 1, 1), new Color(0, 0, 0, 0), 0.1f));
				Tiles[x, y].Clicked = true;
				Tiles[x, y].AdjacentMines = CountAdjacentMines(x, y);

				for (int i = 0; i < 6; i++) yield return null;

				if (recur)
				{
					ForEachNeighbour(x, y, (i, j) =>
					{
						StartCoroutine(ShowTile(i, j, CountAdjacentMines(i, j) == 0));
					});
				}
			}
		}

		bool gameOverStarted = false;
		private IEnumerator GameOver()
		{
			Game.Boo.Play();

			gameOverStarted = true;
			HoveredTile = null;

			AddChild(new GameOver());

			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					if (Tiles[j, i].Mine)
					{
						AddChild(new FadingTile(new Point(j * Minesweeper.TileSize, i * Minesweeper.TileSize), new Color(2, 1, 0, 2), new Color(0, 0, 0, 0), 0.01f));

						Tiles[j, i].Clicked = true;
						yield return null;
					}
				}
				Game.Boom.Play();
			}
		}

		public override void Draw()
		{
			var TileSize = Minesweeper.TileSize;
			ForEachTile(Tiles, (x, y) =>
			{
				SpriteBatch.Draw(Game.TilesTexture, new Rectangle(x * TileSize * BoardScale, y * TileSize * BoardScale, TileSize * BoardScale, TileSize * BoardScale), Tile.GetTextureRect(Tiles[x, y]), Color.White);
			});

			if (HoveredTile is Point hoveredTile)
			{
				SpriteBatch.Draw(Game.PixelTexture, new Rectangle(hoveredTile.X * Minesweeper.TileSize * BoardScale, hoveredTile.Y * Minesweeper.TileSize * BoardScale, Minesweeper.TileSize * BoardScale, Minesweeper.TileSize * BoardScale), new Color(0.25f, 0.25f, 0.25f, 0.25f));
			}

			base.Draw();
		}
	}
}
