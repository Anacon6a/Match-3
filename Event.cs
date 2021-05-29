using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Runtime.Serialization.Json;

namespace Match_3
{
	
	class Events
	{
		delegate void AddRecord(object sender, EventArgs e);
		public static void is_events()
		{
			Program.window.MouseButtonReleased += Window_MouseButtonReleased;
			Program.window.TextEntered += TextInput;

		}

		private static void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
			if (Win_menu.menu)
			{
				if (!Win_menu.record && !Win_menu.help)
				{
					Win_menu.button_click(e);
				}
				else
				{
					Win_menu.close_record(e);

				}
			}
			else if (Win_menu.start)
			{
				if (!Win_start.click_ok1)
				{
					if (!Win_start.decor) Win_start.select_element(e);
					if (Win_start.el_select) Win_start.okey_button(e);
				}
				else if (!Win_start.gameover)
				{
					if (!Field.field_empty && Field.clblock && !Win_start.click_pause) Field.select_block(e);
				}
				else if (!Win_start.click_ok2)
				{
					Win_start.addrecord(e);
				}
				else if (!Win_menu.record)
				{
					Win_start.theend2(e);
				}
				else
				{
					Win_menu.close_record(e);
				}
				if (!Win_start.click_ok2)  Win_start.start_button(e);
			}
		}

		private static void TextInput(object sender, TextEventArgs e)
		{
			if (Win_start.click_textbox)
			{
				Win_start.addtext(e.Unicode);
			}
		}

	}
}
