using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokemonCards.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PokemonCardView : ContentView
	{
		private CardState _cardState = CardState.Collapesed;
		public PokemonCardView()
		{
			InitializeComponent();
		}

		public Image MainImage => PokemonMainImage;
		public BoxView CardBackground => PokemonBackground;
		public Label PokemonName => PokemonTempName;

		private void SeeMore_Tapped(object sender, EventArgs e)
		{
			//Go to state of expanded
			GoToState(CardState.Expanded);
		}

		private void GoToState(CardState cardState)
		{
			// trigger an animation
			if (_cardState == cardState)
				return;

			MessagingCenter.Send<CardEvent>(new CardEvent(), cardState.ToString());
			//AnimationTransition();
			_cardState = cardState;
		}
	}
}