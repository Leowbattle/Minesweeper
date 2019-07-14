using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
	public abstract class GameObject
	{
		protected Minesweeper Game;
		protected SpriteBatch SpriteBatch;

		private List<IEnumerator> Coroutines;

		public GameObject()
		{
			Game = Minesweeper.Instance;
			SpriteBatch = Game.SpriteBatch;

			Coroutines = new List<IEnumerator>();
		}

		public void StartCoroutine(IEnumerator coro)
		{
			Coroutines.Add(coro);
		}

		private void UpdateCoroutines()
		{
			Coroutines.RemoveAll((e) => e.MoveNext() == false);
		}

		public void Kill()
		{
			Minesweeper.Instance.GameObjects.Remove(this);
		}

		public virtual void Update()
		{
			UpdateCoroutines();
		}

		public virtual void Draw()
		{

		}

		// Draw text, which we don't want scaled with everything else
		public virtual void DrawUnscaled()
		{

		}
	}
}
