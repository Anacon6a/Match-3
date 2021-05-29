using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Runtime.Serialization.Json;
using System.IO;



namespace Match_3
{
	class Win_menu : Transformable, Drawable  //класс 1 прорисовки объектов, 2 отвечает за положение, маштабирование..
	{
		private static Texture texture_background = new Texture("Images/fon.jpg"),
		 texture_background_start = new Texture("Images/fon_start.jpg"),
		 texture_background_settings = new Texture("Images/fon_settings.jpg"),
		 texture_background_help = new Texture("Images/fon_help.jpg"),
		 texture_background_records = new Texture("Images/fon_records.jpg"),
		 texture_records = new Texture("Images/tableofrecords.png"),
		 texture_fonrecords = new Texture("Images/theend.png"),
		 texture_back = new Texture("Images/back.png"),
		 texture_efire = new Texture("Images/efire.png"),
		 texture_ewater = new Texture("Images/ewater.png"),
		 texture_eair = new Texture("Images/eair.png"),
		 texture_eearth = new Texture("Images/eearth.png"),
		 //texture_help1 = new Texture("Images/help1.png"),
		 texture_blockb1 = new Texture("images/block_spirit_jelly.png"),
		 texture_blockb2 = new Texture("images/block_flash_jellyh.png"),
		 texture_blockb3 = new Texture("images/block_flash_jellyv.png"),
		 texture_blockb4 = new Texture("images/block_lindworm_jelly.png");

		private static Font font = new Font("Font/AmaticSC-Bold.ttf");

		private static Sprite sprite_background = new Sprite(),
			sprite_records = new Sprite(),
			sprite_fonrecords = new Sprite(),
			sprite_back = new Sprite(),
			//sprite_help1 = new Sprite(),
			sprite_blockb1 = new Sprite(),
			sprite_blockb2 = new Sprite(),
			sprite_blockb3 = new Sprite(),
			sprite_blockb4 = new Sprite();

		private static List<float> k_size = new List<float>() { 9, (float)12.5 },
								   k_x = new List<float>() { (float)4.75, (float)5.19, (float)4.95, (float)4.05 },
								   k_y = new List<float>() { (float)8.887501, (float)2.41875, (float)1.816875, (float)1.50131249 };

		public static bool decor = true,
			menu = true,
			start = false,
			record = false,
			help = false;

		static Win_start win_start;

		private static List<HighScoreTable> records = new List<HighScoreTable>(10);

		private static Text[] text_nomer = new Text[10];
		private static Sprite[] sprite_element = new Sprite[10];
		private static Text[] text_name = new Text[10];
		private static Text[] text_score = new Text[10];
		private static Text text_help, text_rules;

		public void win_menu()
		{

			if (menu)
			{
				if (decor) background();
				button_hover(k_size[0], k_x[0], k_y[0], 0);
				button_hover(k_size[1], k_x[1], k_y[1], 1);
				button_hover(k_size[1], k_x[2], k_y[2], 2);
				button_hover(k_size[1], k_x[3], k_y[3], 3);
			}

			if (start)
				Win_start.win_start();

		}
		private void background()
		{
			float ScaleX = (float)Program.size_window.X / texture_background.Size.X;
			float ScaleY = (float)Program.size_window.Y / texture_background.Size.Y;
			texture_background.Smooth = true; //сглаживание текстуры
			sprite_background.Texture = new Texture(texture_background);
			sprite_background.Scale = new Vector2f(ScaleX, ScaleY);

			ScaleX = (float)Program.size_window.X / texture_records.Size.X / (float)1.5;
			ScaleY = (float)Program.size_window.Y / texture_records.Size.Y / (float)1.1;
			texture_records.Smooth = true;
			sprite_records.Texture = new Texture(texture_records);
			sprite_records.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_records.Position = new Vector2f(200, 33);

			ScaleX = (float)Program.size_window.X / texture_fonrecords.Size.X;
			ScaleY = (float)Program.size_window.Y / texture_fonrecords.Size.Y;
			texture_fonrecords.Smooth = true;
			sprite_fonrecords.Texture = new Texture(texture_fonrecords);
			sprite_fonrecords.Scale = new Vector2f(ScaleX, ScaleY);
			//sprite_fonrecords.Position = new Vector2f(0, 0);

			ScaleX = (float)Program.size_window.X / texture_back.Size.X / (float)13;
			ScaleY = (float)Program.size_window.Y / texture_back.Size.Y / (float)10;
			texture_back.Smooth = true;
			sprite_back.Texture = new Texture(texture_back);
			sprite_back.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_back.Position = new Vector2f(100, 50);

			texture_background_start.Smooth = true;
			texture_background_settings.Smooth = true;
			texture_background_help.Smooth = true;
			texture_background_records.Smooth = true;
			texture_ewater.Smooth = true;
			texture_efire.Smooth = true;
			texture_eair.Smooth = true;
			texture_eearth.Smooth = true;

			text_help = new Text("Первоначально вам необходимо выбрать одну из 4 стихий: вода, воздух, земля, огонь.\n" +
				"Это повлияет на дальнейшую игру таким образом, что убранный блок выбранной стихии принесет вам 2 очка,\n" +
	"противоположный ему(при выборе он расположен по диагонали) 0, остальные по 1 очку. \n" +
 "Собирайте различные комбинации из блоков. Комбинации более чем из 3 блоков дадут вам бонусы,\nони бывают 4 видов:\n" +
 "         - подрывает блоки вокруг себя в радиусе 1 блока,\n" +
 "                  - убирают линии блоков по гоизонтали и по вертикали соответственно,\n" +
 "         - уничтожает все блоки одного типа\n" +
 "Бонусы так же могут взаимодействовать друг с другом.\n" +
 "Помните, ваше время ограничено! Но чем больше очков вы получаете, тем больше времени дается дополнительно.\n" +
 "Пусть удача всегда будет на вашей стороне!", font, 30);
			text_help.Position = new Vector2f(110, 150);
			text_rules = new Text("Rules of the game", font, 36);
			text_rules.Position = new Vector2f(520, 64);

			//ScaleX = (float)Program.size_window.X / texture_help1.Size.X / (float)11;
			//ScaleY = (float)Program.size_window.Y / texture_help1.Size.Y / (float)6;
			//texture_help1.Smooth = true;
			//sprite_help1.Texture = new Texture(texture_help1);
			//sprite_help1.Scale = new Vector2f(ScaleX, ScaleY);
			//sprite_help1.Position = new Vector2f(875, 300); 

			ScaleX = (float)Program.size_window.X / texture_blockb1.Size.X / (float)30;
			ScaleY = (float)Program.size_window.Y / texture_blockb1.Size.Y / (float)17.5;
			texture_blockb1.Smooth = true;
			sprite_blockb1.Texture = new Texture(texture_blockb1);
			sprite_blockb1.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_blockb1.Position = new Vector2f(110, 338);

			ScaleX = (float)Program.size_window.X / texture_blockb2.Size.X / (float)30;
			ScaleY = (float)Program.size_window.Y / texture_blockb2.Size.Y / (float)17.5;
			texture_blockb2.Smooth = true;
			sprite_blockb2.Texture = new Texture(texture_blockb2);
			sprite_blockb2.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_blockb2.Position = new Vector2f(110, 378);

			ScaleX = (float)Program.size_window.X / texture_blockb3.Size.X / (float)30;
			ScaleY = (float)Program.size_window.Y / texture_blockb3.Size.Y / (float)17.5;
			texture_blockb3.Smooth = true;
			sprite_blockb3.Texture = new Texture(texture_blockb3);
			sprite_blockb3.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_blockb3.Position = new Vector2f(155, 378);

			ScaleX = (float)Program.size_window.X / texture_blockb4.Size.X / (float)30;
			ScaleY = (float)Program.size_window.Y / texture_blockb4.Size.Y / (float)17.5;
			texture_blockb4.Smooth = true;
			sprite_blockb4.Texture = new Texture(texture_blockb4);
			sprite_blockb4.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_blockb4.Position = new Vector2f(110, 419);

			decor = false;
		}


		private static void button_hover(float k_s, float k_x, float k_y, int nom)
		{
			float size_button = (float)Program.size_window_changing.X / k_s;
			float x_button = (float)Program.size_window_changing.X / k_x;
			float y_button = (float)Program.size_window_changing.Y / k_y;


			if (x_button <= Program.cursor_position.X && Program.cursor_position.X <= x_button + size_button &&
			y_button <= Program.cursor_position.Y && Program.cursor_position.Y <= y_button + size_button && !Win_menu.record && !Win_menu.help)
			{
				switch (nom)
				{
					case 0:
						sprite_background.Texture = texture_background_start;
						break;
					case 1:
						sprite_background.Texture = texture_background_settings;
						break;
					case 2:
						sprite_background.Texture = texture_background_help;
						break;
					case 3:
						sprite_background.Texture = texture_background_records;
						break;
				}

			}
		}

		public static void button_click(MouseButtonEventArgs cursor_pos)
		{
			button_click2(k_size[0], k_x[0], k_y[0], 0, cursor_pos);
			button_click2(k_size[1], k_x[1], k_y[1], 1, cursor_pos);
			button_click2(k_size[1], k_x[2], k_y[2], 2, cursor_pos);
			button_click2(k_size[1], k_x[3], k_y[3], 3, cursor_pos);
		}

		private static void button_click2(float k_s, float k_x, float k_y, int nom, MouseButtonEventArgs cursor_pos)
		{

			float size_button = (float)Program.size_window_changing.X / k_s;
			float x_button = (float)Program.size_window_changing.X / k_x;
			float y_button = (float)Program.size_window_changing.Y / k_y;


			if (x_button <= cursor_pos.X && cursor_pos.X <= x_button + size_button &&
			y_button <= cursor_pos.Y && cursor_pos.Y <= y_button + size_button)
			{
				switch (nom)
				{
					case 0:
						win_start = new Win_start();
						start = true;
						menu = false;
						break;

					case 1:

						break;

					case 2:
						help = true;
						break;

					case 3:
						record = true;
						create_record();
						break;
				}
			}
		}
		public static void create_record()
		{
			var jsonFormatter = new DataContractJsonSerializer(typeof(List<HighScoreTable>));

			using (var file = new FileStream("records.json", FileMode.OpenOrCreate))
			{
				try
				{
					records = jsonFormatter.ReadObject(file) as List<HighScoreTable>;
				}
				catch//если элементов еще нет
				{
					records.Add(new HighScoreTable("alexan9ra", 666666, 1));
				}
				for (int i = 0; i < records.Count; i++)
				{
					text_nomer[i] = new Text(Convert.ToString(i + 1), font, 32);
					if (i != 0 )text_nomer[i].Position = new Vector2f(sprite_records.Position.X + 200, text_nomer[i - 1].Position.Y + 50);
					else text_nomer[i].Position = new Vector2f(sprite_records.Position.X + 200, sprite_records.Position.Y + 87);
					switch (records[i].Element)
					{
						case 1:
							float ScaleX = (float)Program.size_window.X / texture_ewater.Size.X / (float)28;
							float ScaleY = (float)Program.size_window.Y / texture_ewater.Size.Y / (float)17.5;
							sprite_element[i] = new Sprite();
							sprite_element[i].Texture = new Texture(texture_ewater);
							sprite_element[i].Scale = new Vector2f(ScaleX, ScaleY);
							break;
						case 2:
							ScaleX = (float)Program.size_window.X / texture_efire.Size.X / (float)26;
							ScaleY = (float)Program.size_window.Y / texture_efire.Size.Y / (float)17;
							sprite_element[i] = new Sprite();
							sprite_element[i].Texture = new Texture(texture_efire);
							sprite_element[i].Scale = new Vector2f(ScaleX, ScaleY);
							break;
						case 3:
							ScaleX = (float)Program.size_window.X / texture_eair.Size.X / (float)29;
							ScaleY = (float)Program.size_window.Y / texture_eair.Size.Y / (float)17;
							sprite_element[i] = new Sprite();
							sprite_element[i].Texture = new Texture(texture_eair);
							sprite_element[i].Scale = new Vector2f(ScaleX, ScaleY);
							break;
						case 4:
							ScaleX = (float)Program.size_window.X / texture_eearth.Size.X / (float)26.3;
							ScaleY = (float)Program.size_window.Y / texture_eearth.Size.Y / (float)15.8;
							sprite_element[i] = new Sprite();
							sprite_element[i].Texture = new Texture(texture_eearth);
							sprite_element[i].Scale = new Vector2f(ScaleX, ScaleY);
							break;
					}
					 sprite_element[i].Position = new Vector2f(text_nomer[0].Position.X + 40, text_nomer[i].Position.Y + 1);
					text_name[i] = new Text(records[i].Name, font, 31);
					text_name[i].Position = new Vector2f(sprite_element[0].Position.X + 75, text_nomer[i].Position.Y);
					text_score[i] = new Text(Convert.ToString(records[i].Score), font, 32);
					text_score[i].Position = new Vector2f(text_name[0].Position.X + 250, text_nomer[i].Position.Y);
					
				}
			}
		}

		public static void close_record(MouseButtonEventArgs cursor_pos)
		{
			if (sprite_back.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_back.Position.X + sprite_back.Scale.X * texture_back.Size.X &&
					   sprite_back.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_back.Position.Y + sprite_back.Scale.Y * texture_back.Size.Y)
			{
				if (record) record = false;
				if (help) help = false;
			}
		}
		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;

			if (menu) target.Draw(sprite_background, states);

			if (start) target.Draw(win_start, states);

			if (record)
			{
				if (menu) target.Draw(sprite_fonrecords, states);
				target.Draw(sprite_records, states);
				target.Draw(sprite_back, states);

				for (int i = 0; i < records.Count; i++)
				{
					target.Draw(text_nomer[i], states);
					target.Draw(sprite_element[i], states);
					target.Draw(text_name[i], states);
					target.Draw(text_score[i], states);
			}
				
			}
			if (help)
			{
				target.Draw(sprite_fonrecords, states); 
				//target.Draw(sprite_help1, states);
				target.Draw(sprite_blockb1, states);
				target.Draw(sprite_blockb2, states);
				target.Draw(sprite_blockb3, states);
				target.Draw(sprite_blockb4, states);
				target.Draw(text_help, states);
				target.Draw(text_rules, states);
				target.Draw(sprite_back, states);
			}
		}
	}

}
