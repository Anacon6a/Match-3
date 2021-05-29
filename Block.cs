using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Match_3
{
	abstract class Block
	{
		private static Random random = new Random();
		private int _x0_block = 154, _y0_block = 73,  _type_block, _score_block, _i, _j, _mark;
		private bool normal = true;
		private float move = 0, time;

		private Vector2u _size_block;
		private Vector2f _coordinate, _scale_block, scale_position;

		private Texture _texture_block;
		private Sprite _sprite_block;
		
		public Texture texture_block
		{
			get { return _texture_block; }
			set { _texture_block = value; }
		}
		public Sprite sprite_block
		{
			get { return _sprite_block; }
			set { _sprite_block = value; }
		}
		public Vector2f scale_block
		{
			get { return _scale_block; }
			set { _scale_block = value; }
		}
		public Vector2f coordinate
		{
			get { return _coordinate; }
			set { _coordinate = value; }
		}
		public Vector2u size_block
		{
			get { return _size_block; }
			set { _size_block = value; }
		}
		public int x0_block
		{
			get { return _x0_block; }
			set { _x0_block = value; }
		}
		public int y0_block
		{
			get { return _y0_block; }
			set { _y0_block = value; }
		}

		public int type_block
		{
			get { return _type_block; }
			set { _type_block = value; }
		}
		public int i
		{
			get { return _i; }
			set { _i = value; }
		}
		public int j
		{
			get { return _j; }
			set { _j = value; }
		}
		public int mark
		{
			get { return _mark; }
			set	{ _mark = value; }
		}
		public int score_block
		{
			get { return _score_block; }
			set { _score_block = value; }
		}
		public abstract void this_texture_block();
		public void random_block_element()
		{
			type_block = random.Next(1, 5);
		}
		public void add_score(int ts)
		{
			switch (ts)
			{
				case 1:
					switch (type_block)
					{
						case 1:
							score_block = 2;
							break;
						case 2:
							score_block = 0;
							break;
						case 3:
							score_block = 1;
							break;
						case 4:
							score_block = 1;
							break;
					}
					break;
				case 2:
					switch (type_block)
					{
						case 1:
							score_block = 0;
							break;
						case 2:
							score_block = 2;
							break;
						case 3:
							score_block = 1;
							break;
						case 4:
							score_block = 1;
							break;
					}
					break;
				case 3:
					switch (type_block)
					{
						case 1:
							score_block = 1;
							break;
						case 2:
							score_block = 1;
							break;
						case 3:
							score_block = 2;
							break;
						case 4:
							score_block = 0;
							break;
					}
					break;
				case 4:
					switch (type_block)
					{
						case 1:
							score_block = 1;
							break;
						case 2:
							score_block = 1;
							break;
						case 3:
							score_block = 0;
							break;
						case 4:
							score_block = 2;
							break;
					}
					break;
			}




		}
		public void impulc()
		{

			if (normal)
			{
				if (Program.time_window > time)
				{
					time = Program.time_window + 0.04f;
					scale_position = new Vector2f(sprite_block.Position.X, sprite_block.Position.Y);

					sprite_block.Position = new Vector2f(scale_position.X - 1f,
						scale_position.Y - 1f);

					scale_position = new Vector2f(sprite_block.Scale.X, sprite_block.Scale.Y);

					sprite_block.Scale = new Vector2f(scale_position.X + 1 / 64f,
						scale_position.Y + 1 / 64f);
				}

				if (size_block.X * sprite_block.Scale.X >= size_block.X * scale_block.X * 1.16f) normal = false;
			}

			if (!normal)
			{
				
				if (Program.time_window > time)
				{
					time = Program.time_window + 0.04f;

					scale_position = new Vector2f(sprite_block.Position.X, sprite_block.Position.Y);

					sprite_block.Position = new Vector2f(scale_position.X + 1f,
						scale_position.Y + 1f);

					scale_position = new Vector2f(sprite_block.Scale.X, sprite_block.Scale.Y);

					sprite_block.Scale = new Vector2f(scale_position.X - 1 / 64f,
						scale_position.Y - 1 / 64f);
				}

				if (size_block.X * sprite_block.Scale.X <= size_block.X * scale_block.X * 0.937f) normal = true;
			}
		}
		public void impulc_stop()
		{

			sprite_block.Scale = scale_block;

			sprite_block.Position = coordinate;

		}
		public void block_swap(Vector2f move)
		{
			if (move.X != 0) sprite_block.Position = new Vector2f(sprite_block.Position.X - move.X / 10, sprite_block.Position.Y);
			if (move.Y != 0) sprite_block.Position = new Vector2f(sprite_block.Position.X, sprite_block.Position.Y - move.Y / 10);
		}
		public void block_move(int k)
		{

			move = coordinate.Y - sprite_block.Position.Y;
			sprite_block.Position = new Vector2f(sprite_block.Position.X, sprite_block.Position.Y + move / 15 * k);
			if (k == 15)
			sprite_block.Position = coordinate;
		}

	}
}
