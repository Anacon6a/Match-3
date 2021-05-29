using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.ComponentModel;

namespace Match_3
{
	class Field : Transformable, Drawable
	{
		private static Texture texture_field = new Texture("Images/fon_for_fields.jpg");

		private static Sprite sprite_field = new Sprite(texture_field);

		public static bool field_empty = true,//пустое ли поле;
			clblock = false,//можно ли кликать блоки
			move_block = false,//будут ли перемещены блоки
			changeonelement = false,//когда нужно сменить бонус на элемент
		    check = false;// просто убрать блоки или с повторной проверкой и премещением

		private static int click_counter_block = 0, //блоков нажато
						  type_select_element,//тип выбранного элемента
						  t = 59;
		private static double k = 0.03;

		public static int score = 0;

		private static (int, int) click_block_ij1;//нажатые блоки
		private static (int, int) click_block_ij2;

		private static Block[,] block = new Block[17, 10];

		public static Thread thread_move;
		//public static BackgroundWorker bwmove;

		public static void field_create()
		{
			if (field_empty)
			{
				field_decor();
				add_blocks();
			}
			if (!field_empty) animation_and();

		}
		public static void add_type(int t)
		{
			type_select_element = t;
		}

		private static void field_decor()
		{

			float ScaleX = (float)Program.size_window.X / texture_field.Size.X / (float)1.3;
			float ScaleY = (float)Program.size_window.Y / texture_field.Size.Y / (float)1.4;
			texture_field.Smooth = true;
			sprite_field.Scale = new Vector2f(ScaleX, ScaleY);
			sprite_field.Position = new Vector2f(140, 125);

		}
		private static void add_blocks() //заполняет поле блоками
		{

			for (int i = 0; i < 17; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					block[i, j] = new Block_element();

					block[i, j].i = i;
					block[i, j].j = j;

					block[i, j].random_block_element();

					while (i > 1 && block[i, j].type_block == block[i - 1, j].type_block && block[i, j].type_block == block[i - 2, j].type_block ||
						 j > 1 && block[i, j].type_block == block[i, j - 1].type_block && block[i, j].type_block == block[i, j - 2].type_block)
					{
						block[i, j].random_block_element();
					}

					block[i, j].this_texture_block();
					block[i, j].add_score(type_select_element);

					if (i == 16 && j == 9)
					{
						field_empty = false;
						clblock = true;
					}
				}
			}

		}

		public static void select_block(MouseButtonEventArgs cursor_position) //включает или выключает блок по клику
		{
			if (!field_empty && Win_menu.start && clblock)
			{
				for (int i = 0; i < 17; i++)
				{
					for (int j = 1; j < 10; j++)
					{
						if (block[i, j].sprite_block.Position.X <= cursor_position.X && cursor_position.X <=
							block[i, j].sprite_block.Position.X + block[i, j].sprite_block.Scale.X * block[i, j].size_block.X &&
						block[i, j].sprite_block.Position.Y <= cursor_position.Y && cursor_position.Y <=
						block[i, j].sprite_block.Position.Y + block[i, j].sprite_block.Scale.Y * block[i, j].size_block.Y)
						{
							if ((i, j) != click_block_ij1)
							{
								click_counter_block += 1;  //переключить на анимацию и с анимации на проверку			   
								if (click_counter_block == 1) click_block_ij1 = (i, j);//индексы выбранных блоков
								else click_block_ij2 = (i, j);
								return;
							}
							else
							{ //если нажат тот же блок
								click_counter_block -= 1;
								stop_animation_block();
								click_block_ij1 = (0, 0);
								return;
							}
						}
					}
				}
			}
		}

		private static void stop_animation_block()
		{
			block[click_block_ij1.Item1, click_block_ij1.Item2].impulc_stop();
			block[click_block_ij2.Item1, click_block_ij2.Item2].impulc_stop();
		}
		private static void animation_and()   // взависимости от кол-ва нажатых блоков включает анимацию, либо прооверяет расположение выбранных блоков
		{

			var bl = block[0, 0];

			if (click_counter_block == 1 && click_block_ij1.Item2 > 0 && !Win_start.click_pause)
			{
				block[click_block_ij1.Item1, click_block_ij1.Item2].impulc();
			}

			if (click_counter_block == 2 && click_block_ij2.Item2 > 0)
			{

				if (click_block_ij2.Item1 > 0 && click_block_ij2.Item1 - 1 == click_block_ij1.Item1 && click_block_ij2.Item2 == click_block_ij1.Item2 ||
					click_block_ij2.Item2 > 1 && click_block_ij2.Item2 - 1 == click_block_ij1.Item2 && click_block_ij2.Item1 == click_block_ij1.Item1 ||
					click_block_ij2.Item1 < 16 && click_block_ij2.Item1 + 1 == click_block_ij1.Item1 && click_block_ij2.Item2 == click_block_ij1.Item2 ||
					click_block_ij2.Item2 < 9 && click_block_ij2.Item2 + 1 == click_block_ij1.Item2 && click_block_ij2.Item1 == click_block_ij1.Item1) //если нажатые блоки рядом
				{
					bl = block[click_block_ij1.Item1, click_block_ij1.Item2];
					block[click_block_ij1.Item1, click_block_ij1.Item2] = block[click_block_ij2.Item1, click_block_ij2.Item2];
					block[click_block_ij2.Item1, click_block_ij2.Item2] = bl;
					clblock = false;
					//если бонус
					if (block[click_block_ij1.Item1, click_block_ij1.Item2].type_block > 4 || block[click_block_ij2.Item1, click_block_ij2.Item2].type_block > 4)
					{
						move_block = true;

						click_counter_block = 0; // останавливает анимацию импульса и проверку
						stop_animation_block();

						//bwmove = new BackgroundWorker();
						//bwmove.DoWork += (obj, ea) => swap();
						//bwmove.RunWorkerAsync();
						thread_move = new Thread(new ThreadStart(swap));
						thread_move.Start();
						return;
					}
					for (int i = 0; i < 17; i++)
					{
						for (int j = 1; j < 10; j++)
						{
							//проверяет есть ли комбинации
							if (i < 15 && block[i, j].type_block == block[i + 1, j].type_block && block[i, j].type_block == block[i + 2, j].type_block ||
						 j < 8 && block[i, j].type_block == block[i, j + 1].type_block && block[i, j].type_block == block[i, j + 2].type_block)
							{
								move_block = true;
							}
						}
					}
					click_counter_block = 0; // останавливает анимацию импульса и проверку
					stop_animation_block();

					//bwmove = new BackgroundWorker();
					//bwmove.DoWork += (obj, ea) => swap();
					//bwmove.RunWorkerAsync();
					thread_move = new Thread(new ThreadStart(swap));
					thread_move.Start();

				}
				else
				{
					click_counter_block -= 1;
					stop_animation_block();
					click_block_ij1 = click_block_ij2;
				}

			}
		}
		private static void swap() //если комбинация была, тогда перемещать иначе прокручивать и обнулять click_block_ij1 и click_block_ij2
		{
			var bl = block[0, 0];
			Vector2f move;
			move = block[click_block_ij1.Item1, click_block_ij1.Item2].coordinate - block[click_block_ij2.Item1, click_block_ij2.Item2].coordinate;

			for (int i = 0; i < 10; i++)
			{                            // перемещаем
				block[click_block_ij1.Item1, click_block_ij1.Item2].block_swap(move);
				block[click_block_ij2.Item1, click_block_ij2.Item2].block_swap(-move);
				Timer.timer(0.001);
			}


			if (!move_block)
			{
				for (int i = 0; i < 10; i++)
				{                             //обратно
					block[click_block_ij1.Item1, click_block_ij1.Item2].block_swap(-move);
					block[click_block_ij2.Item1, click_block_ij2.Item2].block_swap(move);
					Timer.timer(0.001);
				}
				bl = block[click_block_ij1.Item1, click_block_ij1.Item2];
				block[click_block_ij1.Item1, click_block_ij1.Item2] = block[click_block_ij2.Item1, click_block_ij2.Item2]; //возвращаем на места в массиве
				block[click_block_ij2.Item1, click_block_ij2.Item2] = bl;
				block[click_block_ij1.Item1, click_block_ij1.Item2].sprite_block.Position = block[click_block_ij1.Item1, click_block_ij1.Item2].coordinate;
				block[click_block_ij2.Item1, click_block_ij2.Item2].sprite_block.Position = block[click_block_ij2.Item1, click_block_ij2.Item2].coordinate;
				stop();
			}

			block[click_block_ij1.Item1, click_block_ij1.Item2].sprite_block.Position = block[click_block_ij2.Item1, click_block_ij2.Item2].coordinate;
			block[click_block_ij2.Item1, click_block_ij2.Item2].sprite_block.Position = block[click_block_ij1.Item1, click_block_ij1.Item2].coordinate;
			block[click_block_ij1.Item1, click_block_ij1.Item2].coordinate = block[click_block_ij1.Item1, click_block_ij1.Item2].sprite_block.Position;
			block[click_block_ij2.Item1, click_block_ij2.Item2].coordinate = block[click_block_ij2.Item1, click_block_ij2.Item2].sprite_block.Position;
			move_block = false;
			//если 2 бонуса
			if (block[click_block_ij1.Item1, click_block_ij1.Item2].type_block > 4 && block[click_block_ij2.Item1, click_block_ij2.Item2].type_block > 4)
			{
				two_bonus(click_block_ij1, click_block_ij2);
				for_animation_move();
				check = true;
			}
			//если один бонус
			else if (block[click_block_ij1.Item1, click_block_ij1.Item2].type_block > 4)
			{
				combination_check();
				one_bonus(click_block_ij1, block[click_block_ij2.Item1, click_block_ij2.Item2].type_block);
				for_animation_move();
				check = true;
			}
			else if (block[click_block_ij2.Item1, click_block_ij2.Item2].type_block > 4)
			{
				combination_check();
				one_bonus(click_block_ij2, block[click_block_ij1.Item1, click_block_ij1.Item2].type_block);
				for_animation_move();
				check = true;
			}
			//если обычные блоки
			else
			{
				check = true;
				block_check();
			}
			combination_check();
			stop();

		}

		private static void block_check() // проверка комбинаций
		{
			for (int i = 0; i < 17; i++)//обнуляем mark
			{
				for (int j = 0; j < 10; j++)
				{
					block[i, j].mark = 0;
				}
			}
			Timer.timer(0.3);
			for (int i = 0; i < 17; i++)
			{
				for (int j = 1; j < 10; j++)
				{
					//горизонталь 4,5
					if (i < 14 && block[i, j].type_block == block[i + 1, j].type_block && block[i, j].type_block == block[i + 2, j].type_block &&
						block[i, j].type_block == block[i + 3, j].type_block && block[i, j].mark != 4 && block[i, j].type_block < 5)
					{
						if (i < 13 && block[i, j].type_block == block[i + 4, j].type_block)
						{
							remove_row(i, j, 0, 0, 5);
						}
						else
						{
							remove_row(i, j, 0, 0, 4);
						}
						block[i + 1, j].mark = 4;
						block[i + 3, j].mark = 4;
					}
					// вертикаль 4,5
					if (j < 7 && block[i, j].type_block == block[i, j + 1].type_block && block[i, j].type_block == block[i, j + 2].type_block && //в 4 клетки и 5
						block[i, j].type_block == block[i, j + 3].type_block && block[i, j].mark != -4 && block[i, j].type_block < 5)
					{
						if (j < 6 && block[i, j].type_block == block[i, j + 4].type_block)
						{
							remove_row(i, j, 0, 0, -5);
						}
						else
						{
							remove_row(i, j, 0, 0, -4);
						}
						block[i, j + 1].mark = -4;
						block[i, j + 3].mark = -4;
					}  //3 горизонталь
					if (i < 15 && (block[i + 1, j].mark != 4 && block[i + 2, j].mark != 4) && block[i, j].type_block == block[i + 1, j].type_block &&
							block[i, j].type_block == block[i + 2, j].type_block && block[i, j].type_block < 5)
					{
						block[i, j].mark = 3;
						block[i + 1, j].mark = 3;
						block[i + 2, j].mark = 3;
						remove_row(i, j, 0, 0, 3);
					}
				}
			}
			for (int i = 0; i < 17; i++)
			{
				for (int j = 1; j < 10; j++)
				{
					//вертикаль
					if (j < 8 && (block[i, j + 1].mark != 4 && block[i, j + 2].mark != 4) && block[i, j].type_block == block[i, j + 1].type_block &&
							block[i, j].type_block == block[i, j + 2].type_block && block[i, j].type_block < 5)
					{
						if (block[i, j].mark == 3)
						{
							remove_row(i, j, i, j, 6); // если совпадает с горизонталью, то передаем индексы и индексы, где появится бонус
						}
						else if (block[i, j + 1].mark == 3)
						{
							remove_row(i, j, i, j + 1, 6);
						}
						else if (block[i, j + 2].mark == 3)
						{
							remove_row(i, j, i, j + 2, 6);
						}
						else remove_row(i, j, 0, 0, -3);
					}
				}
			}
			if (check)
			{
				for_animation_move();
				combination_check();
				stop();
			}
		}

		private static void remove_row(int i1, int j1, int i2, int j2, int comb) //удаление комбинаций
		{
			Vector2f pos;
			switch (comb)
			{
				case 5:
					for (int i = i1; i < i1 + 5; i++)
					{
						block[i, j1].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i, j1].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i, j1].score_block;
						}
					}
					i2 = i1 + 2;
					j2 = j1;
					pos = block[i2, j2].sprite_block.Position;

					block[i2, j2] = new Block_bonus();
					block[i2, j2].coordinate = pos;

					block[i2, j2].type_block = 8;
					block[i2, j2].this_texture_block();

					block[i2, j2].sprite_block.Color = Color.White;
					break;
				case 4:
					for (int i = i1; i < i1 + 4; i++)
					{
						block[i, j1].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i, j1].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i, j1].score_block;
						}
						if (click_block_ij1 == (i, j1) || click_block_ij2 == (i, j1))
						{
							i2 = i;
							j2 = j1;
						}
					}
					if (j2 == 0)
					{
						i2 = i1;
						j2 = j1;
					}

					pos = block[i2, j2].sprite_block.Position;

					block[i2, j2] = new Block_bonus();
					block[i2, j2].coordinate = pos;

					block[i2, j2].type_block = 6;
					block[i2, j2].this_texture_block();

					block[i2, j2].sprite_block.Color = Color.White;
					break;
				case -5:
					for (int j = j1; j < j1 + 5; j++)
					{
						block[i1, j].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i1, j].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i1, j].score_block;
						}
					}
					i2 = i1;
					j2 = j1 + 2;
					pos = block[i2, j2].sprite_block.Position;

					block[i2, j2] = new Block_bonus();
					block[i2, j2].coordinate = pos;

					block[i2, j2].type_block = 8;
					block[i2, j2].this_texture_block();

					block[i2, j2].sprite_block.Color = Color.White;
					break;
				case -4:
					for (int j = j1; j < j1 + 4; j++)
					{
						block[i1, j].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i1, j].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i1, j].score_block;
						}
						if (click_block_ij1 == (i1, j) || click_block_ij2 == (i1, j))
						{
							i2 = i1;
							j2 = j;
						}
					}
					if (j2 == 0)
					{
						i2 = i1;
						j2 = j1;
					}

					pos = block[i2, j2].sprite_block.Position;

					block[i2, j2] = new Block_bonus();
					block[i2, j2].coordinate = pos;

					block[i2, j2].type_block = 7;
					block[i2, j2].this_texture_block();

					block[i2, j2].sprite_block.Color = Color.White;
					break;
				case 3:
					for (int i = i1; i < i1 + 3; i++)
					{
						block[i, j1].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i, j1].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i, j1].score_block;
						}
					}
					break;
				case 6:
					for (int j = j1; j < j1 + 3; j++)
					{
						block[i1, j].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i1, j].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i1, j].score_block;
						}
					}
					pos = block[i2, j2].sprite_block.Position;

					block[i2, j2] = new Block_bonus();
					block[i2, j2].coordinate = pos;

					block[i2, j2].type_block = 5;
					block[i2, j2].this_texture_block();

					block[i2, j2].sprite_block.Color = Color.White;
					break;
				case -3:
					for (int j = j1; j < j1 + 3; j++)
					{
						block[i1, j].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i1, j].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i1, j].score_block;
						}
					}
					break;
			}

		}
		private static void two_bonus((int, int) b1, (int, int) b2)//комбинации, когда нажато два бонуса
		{
			int hb = 0;//количество найденых бонусов
			
			if (block[b1.Item1, b1.Item2].type_block == 5 && block[b2.Item1, b2.Item2].type_block == 5)
			{
				
				block[b1.Item1, b1.Item2].type_block = 0;
				block[b2.Item1, b2.Item2].type_block = 0;
				if (b1.Item1 < b2.Item1 || b1.Item2 < b2.Item2)
				{
					(int, int) b = b2;
					b2 = b1;
					b1 = b;
				}
				for (int i = b2.Item1 - 2; i <= b1.Item1 + 2; i++)
				{
					for (int j = b2.Item2 - 2; j <= b1.Item2 + 2; j++)
					{//если не выходит за пределы массива и не бонус
						if (i >= 0 && i < 17 && j > 0 && j < 10)
						{
							if (block[i, j].type_block < 5)
							{
								block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
								score += block[i, j].score_block;
								if (Win_start.time_end - Program.time_window < t)
								{
									Win_start.time_end += k * block[i, j].score_block;
								}
							}
							else
								hb++;
						}

					}
				}
				change_block(b1);
				change_block(b2);
				for_animation_move();
				combination_check();
				if (hb > 0)//чтобы лишний раз не выполнять циклы
				{//на случай, если в убираемую область попал бонус
					for (int i = b1.Item1 - 2; i <= b1.Item1 + 2 /*&& hb > 0*/; i++)
					{
						for (int j = b1.Item2 - 2; j <= b1.Item2 + 2 /*&& hb > 0*/; j++)
						{
							if (i >= 0 && i < 17 && j > 0 && j < 10 && block[i, j].type_block > 4)
							{

								one_bonus((i, j), 0);
							}

						}
					}
				}
			}

			else if (block[b1.Item1, b1.Item2].type_block == 6 && block[b2.Item1, b2.Item2].type_block == 6 ||
					  block[b1.Item1, b1.Item2].type_block == 6 && block[b2.Item1, b2.Item2].type_block == 7 ||
					  block[b1.Item1, b1.Item2].type_block == 7 && block[b2.Item1, b2.Item2].type_block == 6 ||
					  block[b1.Item1, b1.Item2].type_block == 7 && block[b2.Item1, b2.Item2].type_block == 7)
			{
				block[b1.Item1, b1.Item2].type_block = 0;
				block[b2.Item1, b2.Item2].type_block = 0;
				for (int i = 0; i < 17; i++)
				{
					if (block[i, b2.Item2].type_block < 5)
					{
						block[i, b2.Item2].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[i, b2.Item2].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[i, b2.Item2].score_block;
						}
					}
					else
						hb++;

				}
				for (int j = 1; j < 10; j++)
				{
					if (block[b2.Item1, j].type_block < 5)
					{
						block[b2.Item1, j].sprite_block.Color = new Color(0, 0, 0, 0);
						score += block[b2.Item1, j].score_block;
						if (Win_start.time_end - Program.time_window < t)
						{
							Win_start.time_end += k * block[b2.Item1, j].score_block;
						}
					}
					else
						hb++;
				}
				change_block(b1);
				change_block(b2);
				if (hb > 0)
				{
					for (int i = 0; i < 17; i++)
					{
						if (block[i, b2.Item2].type_block > 4)
						{
							one_bonus((i, b2.Item2), 0);
						}
					}
					for (int j = 1; j < 10; j++)
					{
						if (block[b2.Item1, j].type_block > 4)
						{
							one_bonus((b2.Item1, j), 0);
						}
					}
				}
			}

			else if (block[b1.Item1, b1.Item2].type_block == 8 && block[b2.Item1, b2.Item2].type_block == 8)
			{
				block[b1.Item1, b1.Item2].type_block = 0;
				block[b2.Item1, b2.Item2].type_block = 0;
				for (int i = 0; i < 17; i++)
				{
					for (int j = 1; j < 10; j++)
					{
						if (block[i, j].type_block < 5)
						{
							block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
							score += block[i, j].score_block;
							if (Win_start.time_end - Program.time_window < t)
							{
								Win_start.time_end += k * block[i, j].score_block;
							}
						}
						else hb++;
					}
				}
				change_block(b1);
				change_block(b2);
				for_animation_move();
				combination_check();
				if (hb > 0)
				{
					for (int i = 0; i < 17; i++)
					{
						for (int j = 1; j < 10; j++)
						{
							if (block[i, j].type_block > 4)
							{
								one_bonus((i, j), 0);
							}

						}
					}
				}
			}

			else if (block[b1.Item1, b1.Item2].type_block == 5 && block[b2.Item1, b2.Item2].type_block == 7 ||
				block[b1.Item1, b1.Item2].type_block == 7 && block[b2.Item1, b2.Item2].type_block == 5)
			{
				block[b1.Item1, b1.Item2].type_block = 0;
				block[b2.Item1, b2.Item2].type_block = 0;
				for (int i = 0; i < 17; i++)
				{
					for (int j = b2.Item2 - 1; j <= b2.Item2 + 1; j++)
					{
						if (j > 0 && j < 10)
						{
							if (block[i, j].type_block < 5)
							{
								block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
								score += block[i, j].score_block;
								if (Win_start.time_end - Program.time_window < t)
								{
									Win_start.time_end += k * block[i, j].score_block;
								}
							}
							else hb++;
						}
					}
				}
				change_block(b1);
				change_block(b2);
				if (hb > 0)
				{
					for (int i = 0; i < 17; i++)
					{
						for (int j = b2.Item2 - 1; j <= b2.Item2 + 1; j++)
						{
							if (j > 0 && j < 10 && block[i, j].type_block > 4)
							{
								one_bonus((i, j), 0);
							}
							//else hb++;
						}
					}
				}
			}
			else if (block[b1.Item1, b1.Item2].type_block == 5 && block[b2.Item1, b2.Item2].type_block == 6 ||
				block[b1.Item1, b1.Item2].type_block == 6 && block[b2.Item1, b2.Item2].type_block == 5)
			{
				block[b1.Item1, b1.Item2].type_block = 0;
				block[b2.Item1, b2.Item2].type_block = 0;
				for (int i = b2.Item1 - 1; i <= b2.Item1 + 1; i++)
				{
					for (int j = 1; j < 10; j++)
					{
						if (i >= 0 && i < 17)
						{
							if (block[i, j].type_block < 5)
							{
								block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
								score += block[i, j].score_block;
								if (Win_start.time_end - Program.time_window < t)
								{
									Win_start.time_end += k * block[i, j].score_block;
								}
							}
							else hb++;
						}
					}
				}
				change_block(b1);
				change_block(b2);
				if (hb > 0)
				{
					for (int i = b2.Item1 - 1; i <= b2.Item1 + 1 && hb > 0; i++)
					{
						for (int j = 1; j < 10 && hb > 0; j++)
						{
							if (i >= 0 && i < 17 && block[i, j].type_block > 4)
							{
								one_bonus((i, j), 0);
							}
							//else hb++;
						}
					}
				}
			}
			else if (block[b1.Item1, b1.Item2].type_block != 8 && block[b2.Item1, b2.Item2].type_block == 8 ||
				block[b1.Item1, b1.Item2].type_block == 8 && block[b2.Item1, b2.Item2].type_block != 8)
			{
				int n;
				if (block[b1.Item1, b1.Item2].type_block == 8)
				{
					n = block[b2.Item1, b2.Item2].type_block;
					block[b1.Item1, b1.Item2].type_block = 0;
					block[b1.Item1, b1.Item2].sprite_block.Color = new Color(0, 0, 0, 0);
					score += block[b1.Item1, b1.Item2].score_block;
					if (Win_start.time_end - Program.time_window < t)
					{
						Win_start.time_end += k * block[b1.Item1, b1.Item2].score_block;
					}
				}
				else
				{
					n = block[b1.Item1, b1.Item2].type_block;
					block[b2.Item1, b2.Item2].type_block = 0;
					block[b2.Item1, b2.Item2].sprite_block.Color = new Color(0, 0, 0, 0);
					score += block[b2.Item1, b2.Item2].score_block;
					if (Win_start.time_end - Program.time_window < t)
					{
						Win_start.time_end += k * block[b2.Item1, b2.Item2].score_block;
					}
				}
				change_block(b1);
				change_block(b2);
				for (int i = 0; i < 17; i++)
				{
					for (int j = 1; j < 10; j++)
					{
						if (block[i, j].type_block == n || block[i, j].type_block == 7 && n == 6 || block[i, j].type_block == 6 && n == 7)
						{
							one_bonus((i, j), 0);
						}
					}
				}

			}
		}
		private static void one_bonus((int, int) b, int e)//нажат один бонус
		{
			int chb = 0;
			switch (block[b.Item1, b.Item2].type_block)
			{
				case 5:
					block[b.Item1, b.Item2].type_block = 0;
					for (int i = b.Item1 - 1; i <= b.Item1 + 1; i++)
					{
						for (int j = b.Item2 - 1; j <= b.Item2 + 1; j++)
						{
							if (i >= 0 && i < 17 && j > 0 && j < 10)
							{
								if (block[i, j].type_block < 5)
								{
									block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
									score += block[i, j].score_block;
									if (Win_start.time_end - Program.time_window < t)
									{
										Win_start.time_end += k * block[i, j].score_block;
									}
								}
								else
									chb++;
							}
						}
					}
					change_block(b);
					for_animation_move();
					combination_check();
					if (chb > 0)
					{
						for_animation_move();
						for (int i = b.Item1 - 1; i <= b.Item1 + 1 /*&& chb > 0*/; i++)
						{
							for (int j = b.Item2 - 1; j <= b.Item2 + 1/* && chb > 0*/; j++)
							{
								if (i >= 0 && i < 17 && j > 0 && j < 10)
								{
									if (block[i, j].type_block > 4)
									{
										one_bonus((i, j), 0);
										//chb--;
									}
								}
							}
						}
					}

					break;
				case 6:
					block[b.Item1, b.Item2].type_block = 0;
					for (int j = 1; j < 10; j++)
					{
						if (block[b.Item1, j].type_block < 5)
						{
							block[b.Item1, j].sprite_block.Color = new Color(0, 0, 0, 0);
							score += block[b.Item1, j].score_block;
							if (Win_start.time_end - Program.time_window < t)
							{
								Win_start.time_end += k * block[b.Item1, j].score_block;
							}
						}
						else
							chb++;
					}
					change_block(b);

					if (chb > 0)
					{
						for (int j = 1; j < 10; j++)
						{
							if (block[b.Item1, j].type_block > 4)
							{
								one_bonus((b.Item1, j), 0);
							}
						}
					}
					break;
				case 7:
					block[b.Item1, b.Item2].type_block = 0;
					for (int i = 0; i < 17; i++)
					{
						if (block[i, b.Item2].type_block < 5)
						{
							block[i, b.Item2].sprite_block.Color = new Color(0, 0, 0, 0);
							score += block[i, b.Item2].score_block;
							if (Win_start.time_end - Program.time_window < t)
							{
								Win_start.time_end += k * block[i, b.Item2].score_block;
							}
						}
						else
							chb++;
					}
					change_block(b);

					if (chb > 0)
					{
						for (int i = 0; i < 17; i++)
						{
							if (block[i, b.Item2].type_block > 4)
							{
								one_bonus((i, b.Item2), 0);
							}
						}
					}
					break;
				case 8:
					block[b.Item1, b.Item2].sprite_block.Color = new Color(0, 0, 0, 0);
					score += block[b.Item1, b.Item2].score_block;
					if (Win_start.time_end - Program.time_window < t)
					{
						Win_start.time_end += k * block[b.Item1, b.Item2].score_block;
					}
					if (e == 0)
					{
						Random rn = new Random();
						e=rn.Next(1, 5);
					}
					for (int i = 0; i < 17; i++)
					{
						for (int j = 1; j < 10; j++)
						{
							if ( block[i, j].type_block == e)
							{
								block[i, j].sprite_block.Color = new Color(0, 0, 0, 0);
								score += block[i, j].score_block;
								Win_start.time_end += 0.1 * block[i, j].score_block;
							}
						}
					}
					change_block(b);
					break;
			}
		}
		private static void change_block((int, int) cord)//изменить бонус на обычный блок
		{
			Vector2f pos;
			pos = block[cord.Item1, cord.Item2].sprite_block.Position;
			block[cord.Item1, cord.Item2] = new Block_element();
			block[cord.Item1, cord.Item2].coordinate = pos;
			block[cord.Item1, cord.Item2].random_block_element();
			changeonelement = true;
			block[cord.Item1, cord.Item2].this_texture_block();
			changeonelement = false;
		}
		private static void for_animation_move()
		{
			Block bl0;
			Vector2f pos0;
			float rast;
			click_block_ij1 = (0, 0);
			for (int i = 0; i < 17; i++) //распределение блоков по новым местам
			{
				pos0 = block[i, 0].coordinate;
				rast = 0;
				for (int j = 0; j < 9; j++)
				{

					if (block[i, j + 1].sprite_block.Color == new Color(0, 0, 0, 0))
					{
						bl0 = block[i, j + 1];

						for (int jm = 0; jm <= j; jm++)
						{
							block[i, jm].coordinate = block[i, jm + 1].coordinate;
						}
						for (int jm = j; jm >= 0; jm--)
						{
							block[i, jm + 1] = block[i, jm];
						}

						block[i, 0] = bl0;
						block[i, 0].coordinate = pos0;
						block[i, 0].sprite_block.Position = new Vector2f(pos0.X, pos0.Y - rast);
						rast += 51;

					}
				}
			}
			for (int i = 0; i < 17; i++) //добавление новых блоков
			{
				for (int j = 0; j < 9; j++)
				{
					if (block[i, j].sprite_block.Color == new Color(0, 0, 0, 0))
					{
						block[i, j].random_block_element();
						while (i > 1 && block[i, j].type_block == block[i - 2, j].type_block && block[i, j].type_block == block[i - 1, j].type_block ||
						 j > 1 && block[i, j].type_block == block[i, j - 1].type_block && block[i, j].type_block == block[i, j - 2].type_block)
						{
							block[i, j].random_block_element();
						}
						block[i, j].this_texture_block();
						block[i, j].add_score(type_select_element);
						block[i, j].sprite_block.Color = Color.White;
					}

				}
			}
			for (int k = 1; k < 15; k++) //перемещение
			{
				for (int j = 9; j >= 0; j--)
				{
					for (int i = 16; i >= 0; i--)
					{
						if (block[i, j].sprite_block.Position != block[i, j].coordinate)
						{
							block[i, j].block_move(k);
						}
					}
				}
				Timer.timer(0.001);
			}
		}
		private static void combination_check()//проверка, не появились ли новые комбинации
		{
			for (int i = 0; i < 17; i++) // если появились еще комбинации
			{
				for (int j = 1; j < 10; j++)
				{
					if (i < 15 && block[i, j].type_block == block[i + 1, j].type_block && block[i, j].type_block == block[i + 2, j].type_block && block[i, j].type_block < 5 ||
					 j < 8 && block[i, j].type_block == block[i, j + 1].type_block && block[i, j].type_block == block[i, j + 2].type_block && block[i, j].type_block < 5)
					{
						block_check();
						return;
					}
				}
			}

		}
		private static void stop()//остановка потока
		{
			click_block_ij1 = (0, 0);
			clblock = true;
			check = false;
			thread_move.Abort();
			//bwmove.WorkerSupportsCancellation = true;
			//bwmove.CancelAsync();
		}
		public static void fdel()//очищение переменных
		{
			try
			{
				thread_move.Abort();
			}
			catch { }
			score = 0;
			field_empty = true;
			clblock = false;
			move_block = false;
			changeonelement = false;
		    check = false;
			click_block_ij1 = (0, 0);
		}
		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			try
			{
				target.Draw(sprite_field, states);
			}
			catch { }

			for (int i = 0; i < 17; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					try { 
						target.Draw(block[i, j].sprite_block, states); 
					}
					catch { }
				}
			}

		}
	}
}
