using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
	public class Minesweeper : Game
	{
		public const int TileSize = 8;
		public const int GridWidth = 32;
		public const int GridHeight = 32;
		public const int GameWidth = TileSize * GridWidth;
		public const int GameHeight = TileSize * GridHeight;

		private int windowScale;
		public int WindowScale
		{
			get => windowScale;

			set
			{
				windowScale = value;
				GraphicsDeviceManager.PreferredBackBufferWidth = windowScale * GameWidth;
				GraphicsDeviceManager.PreferredBackBufferHeight = windowScale * GameHeight;
				GraphicsDeviceManager.ApplyChanges();
			}
		}

		public GraphicsDeviceManager GraphicsDeviceManager;
		public SpriteBatch SpriteBatch;

		public Texture2D PixelTexture;
		public Texture2D TilesTexture;
		public Texture2D UITexture;

		public SpriteFont FontRegular;
		public SpriteFont FontTitle;

		public List<GameObject> GameObjects;

		public static Minesweeper Instance;

		public Minesweeper()
		{
			Instance = this;

			IsMouseVisible = true;

			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			WindowScale = 4;
		}

		protected override void LoadContent()
		{
			Content.RootDirectory = "Content";
			PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
			PixelTexture.SetData(new[] { Color.White });

			TilesTexture = Content.Load<Texture2D>("tiles");
			UITexture = Content.Load<Texture2D>("ui");

			FontRegular = Content.Load<SpriteFont>("Font_Regular");
			FontTitle = Content.Load<SpriteFont>("Font_Title");

			SpriteBatch = new SpriteBatch(GraphicsDevice);

			GameObjects = new List<GameObject>();
			GameObjects.Add(new Board());

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			Input.Update();

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Update();
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(WindowScale));
			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Draw();
			}
			SpriteBatch.End();

			SpriteBatch.Begin();
			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].DrawUnscaled();
			}
			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
