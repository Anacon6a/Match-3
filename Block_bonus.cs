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
	class Block_bonus : Block
	{
		private float Scale_X, Scale_Y;
		public override void this_texture_block()
		{
			switch (type_block)
			{
				case 5:
					texture_block = new Texture("images/block_spirit_jelly.png");
					break;

				case 6:
					texture_block = new Texture("images/block_flash_jellyv.png");
					break;

				case 7:
					texture_block = new Texture("images/block_flash_jellyh.png");
					break;

				case 8:
					texture_block = new Texture("images/block_lindworm_jelly.png");
					break;
			}

			texture_block.Smooth = true;

			size_block = texture_block.Size;

			Scale_X = (float)Program.size_window.X / size_block.X / (float)25;
			Scale_Y = (float)Program.size_window.Y / size_block.Y / (float)13.7;

			sprite_block = new Sprite(texture_block);
			sprite_block.Scale = new Vector2f(Scale_X, Scale_Y);
			scale_block = sprite_block.Scale;
			sprite_block.Position = coordinate;

			score_block = 1;
		}

	}
}
