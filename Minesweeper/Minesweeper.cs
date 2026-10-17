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
#if !BLAZORGL
				GraphicsDeviceManager.PreferredBackBufferWidth = windowScale * GameWidth;
				GraphicsDeviceManager.PreferredBackBufferHeight = windowScale * GameHeight;
				GraphicsDeviceManager.ApplyChanges();
#endif
			}
		}

		public int LogicalWidth => GameWidth * WindowScale;
		public int LogicalHeight => GameHeight * WindowScale;

		public GraphicsDeviceManager GraphicsDeviceManager;
		public SpriteBatch SpriteBatch;

		public Texture2D PixelTexture;
		public Texture2D TilesTexture;
		public Texture2D UITexture;

		public Texture2D MenuBG;

#if BLAZORGL
		private RenderTarget2D GameRenderTarget;
#endif

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
#if BLAZORGL
			if (GameRenderTarget == null || GameRenderTarget.IsDisposed)
			{
				GameRenderTarget = new RenderTarget2D(GraphicsDevice, LogicalWidth, LogicalHeight);
			}

			// Render the game at its fixed logical resolution.
			GraphicsDevice.SetRenderTarget(GameRenderTarget);
			GraphicsDevice.Clear(Color.Black);

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(WindowScale));
			CurrentScreen.Draw();
			SpriteBatch.End();

			SpriteBatch.Begin();
			CurrentScreen.DrawUnscaled();
			SpriteBatch.End();

			// Letterbox the logical frame into the actual back buffer.
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);

			var pp = GraphicsDevice.PresentationParameters;
			float scale = Math.Min(
				(float)pp.BackBufferWidth / LogicalWidth,
				(float)pp.BackBufferHeight / LogicalHeight);

			int width = (int)Math.Round(LogicalWidth * scale);
			int height = (int)Math.Round(LogicalHeight * scale);
			int x = (pp.BackBufferWidth - width) / 2;
			int y = (pp.BackBufferHeight - height) / 2;

			SpriteBatch.Begin(samplerState: SamplerState.LinearClamp);
			SpriteBatch.Draw(GameRenderTarget, new Rectangle(x, y, width, height), Color.White);
			SpriteBatch.End();
#else
			GraphicsDevice.Clear(Color.Black);

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(WindowScale));
			CurrentScreen.Draw();
			SpriteBatch.End();

			SpriteBatch.Begin();
			CurrentScreen.DrawUnscaled();
			SpriteBatch.End();
#endif
			base.Draw(gameTime);
		}
	}
}
