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
	class Block_element : Block
	{
		private static int yj1;
		private float Scale_X, Scale_Y;
	
		public override void this_texture_block()
		{
			Vector2f pos, scale;

			switch (type_block)
			{
				case 1:
					texture_block = new Texture("images/block_water.png");
					break;

				case 2:
					texture_block = new Texture("images/block_fire.png");
					break;

				case 3:
					texture_block = new Texture("images/block_air.png");
					break;

				case 4:
					texture_block = new Texture("images/block_earth.png");
					break;
			}

			texture_block.Smooth = true;

			if (Field.field_empty || Field.changeonelement)
			{
				size_block = texture_block.Size;

				Scale_X = (float)Program.size_window.X / size_block.X / (float)25;
				Scale_Y = (float)Program.size_window.Y / size_block.Y / (float)13.7;

				sprite_block = new Sprite(texture_block);
				sprite_block.Scale = new Vector2f(Scale_X, Scale_Y);
				scale_block = sprite_block.Scale;

				if (Field.field_empty) //при первом заполнении поля
				{
					if (j > 0) yj1 = 15; else yj1 = 0;

					sprite_block.Position = new Vector2f(x0_block + i * 53, y0_block + j * 51 + yj1);
					coordinate = sprite_block.Position;
				}
				else 
				{
					sprite_block.Color = new Color(0, 0, 0, 0);
					sprite_block.Position = coordinate;
				}

			}
			else
			{
				pos = sprite_block.Position;
				scale = sprite_block.Scale;
				sprite_block = new Sprite(texture_block);
				sprite_block.Position = pos;
				sprite_block.Scale = scale;
			}

		}

	

	}
}
			

	

