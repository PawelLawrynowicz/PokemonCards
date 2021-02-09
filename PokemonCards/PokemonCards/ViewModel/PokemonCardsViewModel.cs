using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PokemonCards.ViewModel
{
	public class PokemonCardsViewModel
	{
		public ObservableCollection<Pokemon> Pokemons { get; set; }

		public PokemonCardsViewModel()
		{
			Pokemons = new ObservableCollection<Pokemon>
			{
				new Pokemon
				{
					PokemonName = "Pikachu",
					PokemonDescription = "shockingly cute",
					PokemonImage = "pikachu.png",
					PokemonPrimaryColor = "#F6D880",
					PokemonSecondaryColor = "#F48C75"
				},
				new Pokemon
				{
					PokemonName = "Charmander",
					PokemonDescription = "hot, hot, hot",
					PokemonImage = "charmander.png",
					PokemonPrimaryColor = "#F3B184",
					PokemonSecondaryColor = "#FFE16B"
				},
				new Pokemon
				{
					PokemonName = "Bulbasaur",
					PokemonDescription = "half plant,\nhalf dinosaur",
					PokemonImage = "bulbasaur.png",
					PokemonPrimaryColor = "#90D0B4",
					PokemonSecondaryColor = "#D06271"
				},
				new Pokemon
				{
					PokemonName = "Squirtle",
					PokemonDescription = "tiny and cool",
					PokemonImage = "squirtle.png",
					PokemonPrimaryColor = "#83C8D9",
					PokemonSecondaryColor = "#E7D7B5"
				}
			};
		}
	}

	public class Pokemon
	{
		public string PokemonName { get; set; }
		public string PokemonDescription { get; set; }
		public string PokemonImage { get; set; }
		public string PokemonPrimaryColor { get; set; }
		public string PokemonSecondaryColor { get; set; }
	}
} 
