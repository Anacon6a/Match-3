using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace Match_3
{
	class Program
	{
		public static RenderWindow window;

		public static Time time = new Time();
		public static Clock clock = new Clock();
		public static float time_window;

		public static Vector2f cursor_position;
		public static Vector2u size_window,  size_window_changing;

		private static Image icon = new Image("Images/icon.png");
		//public static int click_counter_left = 0;

		static void Main(string[] args)
		{
			window = new RenderWindow(new VideoMode(1200, 675), "Match-3"/*, Styles.Close*/); //создание объекта окна, стиль только с возможностью нажать на кнопку закрытия, изменить на Titlebar, чтобы убрать полностью 

			window.SetVerticalSyncEnabled(true); //вертикальная синхронизация - синхронизация кадровой частоты в компьютерной игре с частотой вертикальной развёртки монитора

			window.SetIcon(512, 512, icon.Pixels);

			window.Closed += (obj, e) => { window.Close(); }; //при закрытии окна вызывается метод

			Events.is_events();

			size_window = window.Size;

			Win_menu win_menu = new Win_menu();

			while (window.IsOpen)
			{
				time_window = time.AsSeconds(); // время в микросекундах
				time = clock.ElapsedTime;
				

				window.DispatchEvents(); //пересматривает входящие сообщения окна о событиях( закрытие окна...)

				window.Clear(Color.Black); //очищаем область прорисовки окна в черный

				cursor_position = new Vector2f(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y);

				
				size_window_changing = window.Size;

				win_menu.win_menu();

				window.Draw(win_menu);

				window.Display();
			}
		}

	
	}
}

