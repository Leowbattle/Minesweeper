using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
	public class Minesweeper : Game
	{
		public const int TileSize = 8;
		public int GameWidth = TileSize * Difficulty.Hard.GridSize().X;
		public int GameHeight = TileSize * Difficulty.Hard.GridSize().Y;

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

		public Texture2D MenuBG;

		public SpriteFont FontRegular;
		public SpriteFont FontTitle;

		public SoundEffect Woo;
		public SoundEffect Boo;
		public SoundEffect Boom;
		public SoundEffect Blip;

		public Screen CurrentScreen;

		public static Minesweeper Instance;

		public Minesweeper()
		{
			Instance = this;

			IsMouseVisible = true;

			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			WindowScale = 4;

			//Window.IsBorderless = true;
		}

		protected override void LoadContent()
		{
			Content.RootDirectory = "Content";
			PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
			PixelTexture.SetData(new[] { Color.White });

			TilesTexture = Content.Load<Texture2D>("tiles");
			UITexture = Content.Load<Texture2D>("ui");

			MenuBG = Content.Load<Texture2D>("bg1");

			FontRegular = Content.Load<SpriteFont>("Font_Regular");
			FontTitle = Content.Load<SpriteFont>("Font_Title");

			Woo = Content.Load<SoundEffect>("woo");
			Boo = Content.Load<SoundEffect>("boo");
			Boom = Content.Load<SoundEffect>("boom");
			Blip = Content.Load<SoundEffect>("blip");

			SpriteBatch = new SpriteBatch(GraphicsDevice);

			CurrentScreen = new MenuScreen();

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			Input.Update();

			CurrentScreen.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(WindowScale));
			CurrentScreen.Draw();
			SpriteBatch.End();

			SpriteBatch.Begin();
			CurrentScreen.DrawUnscaled();
			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
