using Terraria;

namespace LivingNPCs
{
	public class Time
	{
		public int Hours, Minutes;

		public Time(int hours = 0, int minutes = 0)
		{
			Hours = hours;
			Minutes = minutes;
		}

		public static Time Now()
		{
			double time = Main.time / 86400.0 * 24.0;
			if (Main.dayTime)
			{
				time += 4.5;
			}
			else
			{
				time -= 4.5;
				if (time < 0.0) time += 24.0;
			}

			int hours = (int) time;
			int minutes = (int) ((time - hours) * 60);
			return new Time(hours, minutes);
		}

		public static Time operator +(Time a)
		{
			return a;
		}

		public static Time operator -(Time a)
		{
			return a;
		}

		public static Time operator +(Time a, Time b)
		{
			int minutes = a.Minutes + b.Minutes;
			int hours = a.Hours + b.Hours;
			if (minutes >= 60)
			{
				minutes -= 60;
				hours++;
			}

			if (hours >= 24)
				hours -= 24;

			return new Time(hours, minutes);
		}

		public static Time operator -(Time a, Time b)
		{
			int minutes = a.Minutes - b.Minutes;
			int hours = a.Hours - b.Hours;
			if (minutes < 0)
			{
				minutes += 60;
				hours--;
			}

			if (hours < 0)
				hours += 24;

			return new Time(hours, minutes);
		}
	}
}