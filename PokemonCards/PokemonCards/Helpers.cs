using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCards
{
	class Helpers
	{
		public static double LimitRange(double value, double min, double max)
		{
			if (value >= min)
			{
				return value <= max ? value : max;
			}

			return min;
		}
	}
}
