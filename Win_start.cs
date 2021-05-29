using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.ComponentModel;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Security.Cryptography;


namespace Match_3
{
	class Win_start : Transformable, Drawable
	{
		private static Texture texture_background = new Texture("Images/fon_forstart.jpg"),
			texture_header = new Texture("Images/start_header.jpg"),
			texture_pause = new Texture("Images/pause.png"),
			texture_bpause = new Texture("Images/pause_button.png"),
			texture_brestart = new Texture("Images/restart_button.png"),
			texture_bmenu = new Texture("Images/menu_button.png"),
			texture_pramka = new Texture("Images/progress_ramka.png"),
			texture_pwall = new Texture("Images/progress_wall.png"),
			texture_lscore = new Texture("Images/label_score.png"),
			texture_selement = new Texture("Images/select_element.png"),
			texture_efire = new Texture("Images/efire.png"),
			texture_ewater = new Texture("Images/ewater.png"),
			texture_eair = new Texture("Images/eair.png"),
			texture_eearth = new Texture("Images/eearth.png"),
			texture_bokey = new Texture("Images/bokey.png"),
			texture_ptime = new Texture("Images/progress_time.png"),
			texture_chm= new Texture("Images/check_mark.png"),
			texture_nameentry = new Texture("Images/name_entry.png"),
			texture_tb = new Texture("Images/TextBox.png"),
			texture_tb2 = new Texture("Images/TextBox2.png"),
			texture_theend = new Texture("Images/theend.png"),
			texture_brestart2 = new Texture("Images/restart_button2.png"),
			texture_bmenu2 = new Texture("Images/menu_button2.png"),
			texture_brecords = new Texture("Images/records_button.png");

		private static Sprite sprite_background = new Sprite(),
			sprite_header = new Sprite(),
			sprite_pause = new Sprite(),
			sprite_bpause = new Sprite(),
			sprite_brestart = new Sprite(),
			sprite_bmenu = new Sprite(),
			sprite_pramka = new Sprite(),
			sprite_pwall = new Sprite(),
			sprite_lscore = new Sprite(),
			sprite_selement = new Sprite(),
			sprite_efire = new Sprite(),
			sprite_ewater = new Sprite(),
			sprite_eair = new Sprite(),
			sprite_eearth = new Sprite(),
			sprite_bokey = new Sprite(),
			sprite_ptime = new Sprite(),
			sprite_chm = new Sprite(),
			sprite_nameentry = new Sprite(),
			sprite_tb = new Sprite(),
			sprite_tb2 = new Sprite(),
			sprite_theend = new Sprite(),
			sprite_brestart2 = new Sprite(),
			sprite_bmenu2 = new Sprite(),
			sprite_brecords = new Sprite();


		private Field field = new Field();

		public static bool decor = true,//добавить ли текстуры
			click_pause = true, //нажата ли пауза
			pause_move = false, //запрещает нажимать паузу, пока не поднимется картинка
			el_select = false,//выбран ли элемент
			click_ok1 = false,//нажата ли кнопка
			gameover = false,//закончена ли игра
			click_textbox = false,//нажат ли текст бокс
			click_ok2 = false;//нажата ли кнопка после ввода имени

		private static int type;//тип выбранного элемента

		private static double time_pause;
		public static double time_end;

		private static Vector2f pause_on = new Vector2f(140, 125),
			pause_off = new Vector2f(140, -356);

		private static Thread thread_pause, thread_time;

		private static Font font = new Font("Font/Oswald-Regular.ttf");

		private static string textname = "";

		private static Text text_score;
		private static Text text_name = new Text(textname, font, 26);

		private static List<HighScoreTable> records = new List<HighScoreTable>(10);

		public static void win_start()
		{
			if (decor)
			{
				start_decor();

			}
			if (click_ok1)
			{
				Field.field_create();
			}
			add_text_score();
		}
		private static void add_text_score()
		{
			text_score = new Text(Field.score.ToString(), font, 26);
			text_score.FillColor = new Color(195, 195, 197); 
			text_score.Position = new Vector2f(sprite_lscore.Position.X + 120, sprite_lscore.Position.Y + 10);
		
		}
		private static void start_decor()
		{
			float ScaleX = (float)Program.size_window.X / texture_background.Size.X;
			float ScaleY = (float)Program.size_window.Y / texture_background.Size.Y;
			texture_background.Smooth = true; 
			sprite_background.Texture = new Texture(texture_background);
			sprite_background.Scale = new Vector2f(ScaleX, ScaleY);

			ScaleX = (float)Program.size_window.X / texture_header.Size.X;
			ScaleY = (float)Program.size_window.Y / texture_header.Size.Y / (float)5.4;
			texture_header.Smooth = true;
			sprite_header.Texture = new Texture(texture_header);
			sprite_header.Scale = new Vector2f(ScaleX, ScaleY);

			ScaleX = (float)Program.size_window.X / texture_pause.Size.X / (float) 1.3;
			ScaleY = (float)Program.size_window.Y / texture_pause.Size.Y / (float) 1.4;
			texture_pause.Smooth = true;
			sprite_pause.Texture = new Texture(texture_pause);
			sprite_pause.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_pause.Position = pause_on;

			ScaleX = (float)Program.size_window.X / texture_bmenu.Size.X / (float)10;
			ScaleY = (float)Program.size_window.Y / texture_bmenu.Size.Y / (float)13;
			texture_bmenu.Smooth = true;
			sprite_bmenu.Texture = new Texture(texture_bmenu);
			sprite_bmenu.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_bmenu.Position = new Vector2f(30, 0);

			ScaleX = (float)Program.size_window.X / texture_brestart.Size.X / (float)9;
			ScaleY = (float)Program.size_window.Y / texture_brestart.Size.Y / (float)13;
			texture_brestart.Smooth = true;
			sprite_brestart.Texture = new Texture(texture_brestart);
			sprite_brestart.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_brestart.Position = new Vector2f(160, 0);

			ScaleX = (float)Program.size_window.X / texture_pramka.Size.X / (float)2.9;
			ScaleY = (float)Program.size_window.Y / texture_pramka.Size.Y / (float)13;
			texture_pramka.Smooth = true;
			sprite_pramka.Texture = new Texture(texture_pramka);
			sprite_pramka.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_pramka.Position = new Vector2f(310, 0);

			ScaleX = (float)Program.size_window.X / texture_pwall.Size.X / (float)2.9;
			ScaleY = (float)Program.size_window.Y / texture_pwall.Size.Y / (float)13;
			texture_pwall.Smooth = true;
			sprite_pwall.Texture = new Texture(texture_pwall);
			sprite_pwall.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_pwall.Position = new Vector2f(311, 0);

			//ScaleX = (float)Program.size_window.X / texture_ptime.Size.X / (float)2.9;
			ScaleY = (float)Program.size_window.Y / texture_ptime.Size.Y / (float)13;
			texture_ptime.Smooth = true;
			sprite_ptime.Texture = new Texture(texture_ptime);
			sprite_ptime.Scale = new Vector2f(0, ScaleY);
			sprite_ptime.Position = new Vector2f(311, 0);

			ScaleX = (float)Program.size_window.X / texture_lscore.Size.X / (float)5;
			ScaleY = (float)Program.size_window.Y / texture_lscore.Size.Y / (float)13;
			texture_lscore.Smooth = true;
			sprite_lscore.Texture = new Texture(texture_lscore);
			sprite_lscore.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_lscore.Position = new Vector2f((float)723.36, 0);


			//ScaleX = (float)Program.size_window.X / text_score.CharacterSize / (float)1;
			//ScaleY = (float)Program.size_window.Y / text_score.CharacterSize / (float)0.3;
			//text_score.Color = new Color(195, 195, 197, 1);
			//text_score.Position = new Vector2f(sprite_lscore.Position.X + 50, sprite_lscore.Position.Y);
			//text_score.Scale = new Vector2f(ScaleX, ScaleY);

			ScaleX = (float)Program.size_window.X / texture_bpause.Size.X / (float)10;
			ScaleY = (float)Program.size_window.Y / texture_bpause.Size.Y / (float)13;
			texture_bpause.Smooth = true;
			sprite_bpause.Texture = new Texture(texture_bpause);
			sprite_bpause.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_bpause.Position = new Vector2f(1020, 0);

			ScaleX = (float)Program.size_window.X / texture_selement.Size.X / (float)3.3;
			ScaleY = (float)Program.size_window.Y / texture_selement.Size.Y / (float)1.7;
			texture_selement.Smooth = true;
			sprite_selement.Texture = new Texture(texture_selement);
			sprite_selement.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_selement.Position = new Vector2f(425, 170);

			ScaleX = (float)Program.size_window.X / texture_efire.Size.X / (float)10.8;
			ScaleY = (float)Program.size_window.Y / texture_efire.Size.Y / (float)6.5;
			texture_efire.Smooth = true;
			sprite_efire.Texture = new Texture(texture_efire);
			sprite_efire.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_efire.Position = new Vector2f(620, 376);

			ScaleX = (float)Program.size_window.X / texture_ewater.Size.X / (float)10.8;
			ScaleY = (float)Program.size_window.Y / texture_ewater.Size.Y / (float)6.5;
			texture_ewater.Smooth = true;
			sprite_ewater.Texture = new Texture(texture_ewater);
			sprite_ewater.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_ewater.Position = new Vector2f(478, 248);

			ScaleX = (float)Program.size_window.X / texture_eair.Size.X / (float)11;
			ScaleY = (float)Program.size_window.Y / texture_eair.Size.Y / (float)6.5;
			texture_eair.Smooth = true;
			sprite_eair.Texture = new Texture(texture_eair);
			sprite_eair.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_eair.Position = new Vector2f(620, 248);

			ScaleX = (float)Program.size_window.X / texture_eearth.Size.X / (float)10.2;
			ScaleY = (float)Program.size_window.Y / texture_eearth.Size.Y / (float)6.2;
			texture_eearth.Smooth = true;
			sprite_eearth.Texture = new Texture(texture_eearth);
			sprite_eearth.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_eearth.Position = new Vector2f(478, 376);

			ScaleX = (float)Program.size_window.X / texture_chm.Size.X / (float)27;
			ScaleY = (float)Program.size_window.Y / texture_chm.Size.Y / (float)17;
			texture_chm.Smooth = true;
			sprite_chm.Texture = new Texture(texture_chm);
			sprite_chm.Scale = new Vector2f(ScaleX, ScaleY);

			ScaleX = (float)Program.size_window.X / texture_bokey.Size.X / (float)10;
			ScaleY = (float)Program.size_window.Y / texture_bokey.Size.Y / (float)13;
			texture_bokey.Smooth = true;
			sprite_bokey.Texture = new Texture(texture_bokey);
			sprite_bokey.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_bokey.Position = new Vector2f(540, 498);

			ScaleX = (float)Program.size_window.X / texture_nameentry.Size.X / (float)3.3;
			ScaleY = (float)Program.size_window.Y / texture_nameentry.Size.Y / (float)3;
			texture_nameentry.Smooth = true;
			sprite_nameentry.Texture = new Texture(texture_nameentry);
			sprite_nameentry.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_nameentry.Position = new Vector2f(420, 220);

			ScaleX = (float)Program.size_window.X / texture_tb.Size.X / (float)4;
			ScaleY = (float)Program.size_window.Y / texture_tb.Size.Y / (float)15;
			texture_tb.Smooth = true;
			sprite_tb.Texture = new Texture(texture_tb);
			sprite_tb.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_tb.Position = new Vector2f(455, 303);

			ScaleX = (float)Program.size_window.X / texture_tb2.Size.X / (float)4;
			ScaleY = (float)Program.size_window.Y / texture_tb2.Size.Y / (float)15;
			texture_tb2.Smooth = true;
			sprite_tb2.Texture = new Texture(texture_tb2);
			sprite_tb2.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_tb2.Position = new Vector2f(455, 303);

			ScaleX = (float)Program.size_window.X / texture_theend.Size.X;
			ScaleY = (float)Program.size_window.Y / texture_theend.Size.Y;
			texture_theend.Smooth = true;
			sprite_theend.Texture = new Texture(texture_theend);
			sprite_theend.Scale = new Vector2f(ScaleX, ScaleY);
			//sprite_theend.Position = new Vector2f(0, 0);

			ScaleX = (float)Program.size_window.X / texture_bmenu2.Size.X / (float)10;
			ScaleY = (float)Program.size_window.Y / texture_bmenu2.Size.Y / (float)13;
			texture_bmenu2.Smooth = true;
			sprite_bmenu2.Texture = new Texture(texture_bmenu2);
			sprite_bmenu2.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_bmenu2.Position = new Vector2f(535, 240);

			ScaleX = (float)Program.size_window.X / texture_brestart2.Size.X / (float)9.3;
			ScaleY = (float)Program.size_window.Y / texture_brestart2.Size.Y / (float)13;
			texture_brestart2.Smooth = true;
			sprite_brestart2.Texture = new Texture(texture_brestart2);
			sprite_brestart2.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_brestart2.Position = new Vector2f(531, 305);

			ScaleX = (float)Program.size_window.X / texture_brecords.Size.X / (float)7;
			ScaleY = (float)Program.size_window.Y / texture_brecords.Size.Y / (float)13;
			texture_brecords.Smooth = true;
			sprite_brecords.Texture = new Texture(texture_brecords);
			sprite_brecords.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_brecords.Position = new Vector2f(511, 370);

			decor = false;
		}

		public static void select_element(MouseButtonEventArgs cursor_pos)
		{
			if (sprite_ewater.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_ewater.Position.X + sprite_bpause.Scale.X * texture_ewater.Size.X &&
					   sprite_ewater.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_ewater.Position.Y + sprite_ewater.Scale.Y * texture_ewater.Size.Y)
			{
					type = 1;
					el_select = true;
				    sprite_chm.Position = new Vector2f(563, 302);
			}
			else if (sprite_efire.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_efire.Position.X + sprite_efire.Scale.X * texture_efire.Size.X &&
					   sprite_efire.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_efire.Position.Y + sprite_efire.Scale.Y * texture_efire.Size.Y)
			{
					type = 2;
					el_select = true;
				    sprite_chm.Position = new Vector2f(702, 433);
			}
			else if (sprite_eair.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_eair.Position.X + sprite_eair.Scale.X * texture_eair.Size.X &&
					   sprite_eair.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_eair.Position.Y + sprite_eair.Scale.Y * texture_eair.Size.Y)
			{
					type = 3;
					el_select = true;
				    sprite_chm.Position = new Vector2f(702, 302);
			}
			else if (sprite_eearth.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_eearth.Position.X + sprite_eearth.Scale.X * texture_eearth.Size.X &&
					   sprite_eearth.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_eearth.Position.Y + sprite_eearth.Scale.Y * texture_eearth.Size.Y)
			{
					type = 4;
					el_select = true;
				    sprite_chm.Position = new Vector2f(563, 433);
			}
		}
		public static void okey_button(MouseButtonEventArgs cursor_pos)
		{
			if (sprite_bokey.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_bokey.Position.X + sprite_bokey.Scale.X * texture_bokey.Size.X &&
					   sprite_bokey.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_bokey.Position.Y + sprite_bokey.Scale.Y * texture_bokey.Size.Y)
			{
				Field.add_type(type);
				click_ok1 = true;
				thread_pause = new Thread(new ThreadStart(pause));
				thread_pause.Start();
				thread_time = new Thread(new ThreadStart(start_time));
				thread_time.Start();
			}
		}

		public static void start_button(MouseButtonEventArgs cursor_pos)//нажатие кнопок
		{
			if (sprite_bpause.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_bpause.Position.X + sprite_bpause.Scale.X * texture_bpause.Size.X &&
					   sprite_bpause.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_bpause.Position.Y + sprite_bpause.Scale.Y * texture_bpause.Size.Y && !pause_move && !gameover && click_ok1)
			{
				thread_pause = new Thread(new ThreadStart(pause));
				thread_pause.Start();
			}
			else if (sprite_bmenu.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_bmenu.Position.X + sprite_bmenu.Scale.X * texture_bmenu.Size.X &&
					   sprite_bmenu.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_bmenu.Position.Y + sprite_bmenu.Scale.Y * texture_bmenu.Size.Y)
			{
				Win_menu.start = false;
				Win_menu.menu = true;
				sdel();
			}
			else if (sprite_brestart.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_brestart.Position.X + sprite_brestart.Scale.X * texture_brestart.Size.X &&
					   sprite_brestart.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_brestart.Position.Y + sprite_brestart.Scale.Y * texture_brestart.Size.Y && click_ok1)
			{
				sdel();
			}
		}
		private static void start_time()
		{
			while (click_pause) { }
			float ScaleX = (float)Program.size_window.X / texture_ptime.Size.X / (float)2.9;
			int t = 60;
			float scX = ScaleX / t;
			time_end = Program.time_window + t;
			while (Program.time_window <= time_end)
			{
				sprite_ptime.Scale = new Vector2f(scX * (t - ((float)time_end - Program.time_window)), sprite_ptime.Scale.Y);
			}
			the_end();
		}
		private static void pause()
		{
			pause_move = true;
			if (sprite_pause.Position == pause_on)
			{
				
				for (int i = 0; i < 30; i++)
				{
					sprite_pause.Position = new Vector2f(sprite_pause.Position.X, sprite_pause.Position.Y - 16);
					Timer.timer(0.001);
				}

				sprite_pause.Position = pause_off;
				try
				{
					Field.thread_move.Resume();
				}
				catch { }
				try
				{
					
					thread_time.Resume();
					time_end += Program.time_window - time_pause;
				}
				catch { }
				click_pause = false;
			}
			else
			{
				pause_move = true;
				click_pause = true;
				time_pause = Program.time_window;
				try
				{
					Field.thread_move.Suspend();
				}
				catch { }
				try
				{
					thread_time.Suspend();
				}
				catch { }
				for (int i = 0; i < 30; i++)
				{
					sprite_pause.Position = new Vector2f(sprite_pause.Position.X, sprite_pause.Position.Y + 16);
					Timer.timer(0.001);
				}
				sprite_pause.Position = pause_on;
			}
			pause_move = false;
			thread_pause.Abort();
		}
		private static void the_end()//конец игры
		{
			int sc = Field.score;
			sprite_bokey.Position = new Vector2f(543, 373);
			thread_pause = new Thread(new ThreadStart(pause));
			thread_pause.Start();
			gameover = true;
		}
		public static void addrecord(MouseButtonEventArgs cursor_pos)//клик на текст бокс и на окей
		{
			if (sprite_tb.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_tb.Position.X + sprite_tb.Scale.X * texture_tb.Size.X &&
					  sprite_tb.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_tb.Position.Y + sprite_tb.Scale.Y * texture_tb.Size.Y)
			{
				click_textbox = true;
			}
			else
			{
				click_textbox = false;
			}
			if (sprite_bokey.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_bokey.Position.X + sprite_bokey.Scale.X * texture_bokey.Size.X &&
					  sprite_bokey.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_bokey.Position.Y + sprite_bokey.Scale.Y * texture_bokey.Size.Y)
			{
				click_ok2 = true;
				add_records(Field.score);
			}
		}
		public static void addtext(String simvol)//ввод имени
		{
			if (simvol != "\b")
			{
				if (textname.Length < 14)
				{
					textname += simvol;
				}
			}
			else if (textname.Length > 0)
				{
					textname = textname.Substring(0, textname.Length - 1);
				}
			text_name = new Text(textname, font, 26);
			text_name.FillColor = new Color(0, 0, 0);
			text_name.Position = new Vector2f(sprite_tb.Position.X + 10, sprite_tb.Position.Y + 10);

		}

			private static void add_records(int score)//запись в таблицу рекордов
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
				if (records.Count < 10)
				{
					records.Add(new HighScoreTable(textname, score, type));
				}
				else
				{
					if (records[9].Score <= score)
					{
						records[9].Name = textname;
						records[9].Score = score;
						records[9].Element = type;
					}
				}
				var r = records.OrderByDescending(u => u.Score);
				file.SetLength(0);
				jsonFormatter.WriteObject(file, r);
			}
		}

		public static void theend2(MouseButtonEventArgs cursor_pos)// после добавления новой записи в таблицу рекордов, клики по кнопкам
		{
			if (sprite_bmenu2.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_bmenu2.Position.X + sprite_bmenu2.Scale.X * texture_bmenu2.Size.X &&
					   sprite_bmenu2.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_bmenu2.Position.Y + sprite_bmenu2.Scale.Y * texture_bmenu2.Size.Y)
			{
				Win_menu.start = false;
				Win_menu.menu = true;
				sdel();
			}
			else if (sprite_brestart2.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_brestart2.Position.X + sprite_brestart2.Scale.X * texture_brestart2.Size.X &&
					   sprite_brestart2.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_brestart2.Position.Y + sprite_brestart2.Scale.Y * texture_brestart2.Size.Y)
			{
				sdel();
			}
			if (sprite_brecords.Position.X <= cursor_pos.X && cursor_pos.X <= sprite_brecords.Position.X + sprite_brecords.Scale.X * texture_brecords.Size.X &&
					   sprite_brecords.Position.Y <= cursor_pos.Y && cursor_pos.Y <= sprite_brecords.Position.Y + sprite_brecords.Scale.Y * texture_brecords.Size.Y)
			{
				Win_menu.create_record();
				Win_menu.record = true;
			}
		}
			private static void sdel()//очищение переменных
		{
			Field.fdel();
			el_select = false;
			sprite_ptime.Scale = new Vector2f(0, sprite_ptime.Scale.Y);
			sprite_pause.Position = pause_on;
			click_pause = true;
			click_ok1 = false;
			click_ok2 = false;
			gameover = false;
			click_textbox = false;
			sprite_bokey.Position = new Vector2f(540, 498);
			try
			{
				thread_pause.Abort();
			}
			catch { }
			try
			{
				thread_time.Abort();
			}
			catch { }

		}
			public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform  *= Transform;

			target.Draw(sprite_background, states);

			if (click_ok1) target.Draw(field, states);

			target.Draw(sprite_pause, states);

			if (!click_ok1)
			{
				target.Draw(sprite_selement, states);
				target.Draw(sprite_efire, states);
				target.Draw(sprite_ewater, states);
				target.Draw(sprite_eair, states);
				target.Draw(sprite_eearth, states);
				target.Draw(sprite_bokey, states);
				if (el_select) target.Draw(sprite_chm, states);
			}
		
			target.Draw(sprite_header, states);

			target.Draw(sprite_bmenu, states);
			target.Draw(sprite_brestart, states);
			target.Draw(sprite_pwall, states);
			target.Draw(sprite_ptime, states);
			target.Draw(sprite_pramka, states);
			target.Draw(sprite_lscore, states);
			target.Draw(text_score, states);
			target.Draw(sprite_bpause, states);

			if (gameover)
			{
				if (!click_ok2)
				{
					target.Draw(sprite_nameentry, states);
					target.Draw(sprite_tb, states);
					if (click_textbox) target.Draw(sprite_tb2, states);
					target.Draw(text_name, states);
					target.Draw(sprite_bokey, states);
				}
				else
				{
					target.Draw(sprite_theend, states);
					target.Draw(sprite_brestart2, states);
					target.Draw(sprite_bmenu2, states);
					target.Draw(sprite_brecords, states);
				}
			}
		}
	}
}
