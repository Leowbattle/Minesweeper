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

		public GameObject Parent;
		public List<GameObject> Children;
		private List<IEnumerator> Coroutines;

		public GameObject()
		{
			Game = Minesweeper.Instance;
			SpriteBatch = Game.SpriteBatch;

			Children = new List<GameObject>();
			Coroutines = new List<IEnumerator>();
		}

		public void AddChild(GameObject gameObject)
		{
			Children.Add(gameObject);
			gameObject.Parent = this;
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
			Parent.Children.Remove(this);
		}

		public virtual void Update()
		{
			UpdateCoroutines();

			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Update();
			}
		}

		public virtual void Draw()
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Draw();
			}
		}

		// Draw text, which we don't want scaled with everything else
		public virtual void DrawUnscaled()
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].DrawUnscaled();
			}
		}
	}
}
