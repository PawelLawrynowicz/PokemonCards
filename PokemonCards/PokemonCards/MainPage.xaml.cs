using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonCards.View;
using PokemonCards.ViewModel;
using Xamarin.Forms;
using static PokemonCards.Helpers;


namespace PokemonCards
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			this.BindingContext = new PokemonCardsViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MainCardView.UserInteracted += MainCardView_UserInteracted;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MainCardView.UserInteracted -= MainCardView_UserInteracted;
		}

		bool firstTimeRunning = true;
		private void MainCardView_UserInteracted(PanCardView.CardsView view, 
			PanCardView.EventArgs.UserInteractedEventArgs args)
		{
			
			//Get current card
			var card = MainCardView.CurrentView as PokemonCardView;
			//Animation factors
			var delayCardOpacityFactor = 0.6;
			var delayImageScaleFactor = 0.4;
			var imageHeight = card.MainImage.Height;
			var delayImageOpacityFactor = 0.3;
			//Figure out how far its swiped and add opacity based on that
			var ratioFromCenter = Math.Abs(args.Diff / this.Width);
			
			if (args.Status == PanCardView.Enums.UserInteractionStatus.Running)
			{
				

				//Change current's card opacity during swipe
				var opacity = LimitRange((1 + delayCardOpacityFactor) - ratioFromCenter,0,1);
				card.CardBackground.Opacity = opacity;

				

				//Scale current's card image down during swipe
				var scale = LimitRange((1+delayImageScaleFactor) - (ratioFromCenter*1.75),0.2,1);
				card.MainImage.Scale = scale;

				if (ratioFromCenter > 0 && scale == 1)
				{
					card.ScaleTo(0.95, 50);
				}

				//Translate current's card image as it's scaling down
				var translateY = (-imageHeight + (1-scale)*imageHeight)/2;
				card.MainImage.TranslationY = translateY;

				//Change current's card image opacity during swipe
				var imageOpacity = LimitRange((1 + delayImageOpacityFactor) - (ratioFromCenter*1.5), 0, 1);
				card.MainImage.Opacity = imageOpacity;

				//Animate 2nd card

				var nextCard = MainCardView.CurrentBackViews.First() as PokemonCardView;

					//Opacity
				nextCard.MainImage.Opacity = LimitRange((ratioFromCenter-0.3) * 3,0,1);
					//Scale
				var nextScale = LimitRange(ratioFromCenter * 1.25, 0, 1);
				//Translation
				nextCard.MainImage.Scale = nextScale;
				nextCard.MainImage.TranslationY = - (nextScale * imageHeight) / 2;
			}

			//Reset parameters of the card after swipe so it doens't look stupid
			if (args.Status == PanCardView.Enums.UserInteractionStatus.Ending)
			{
				var nextCard = MainCardView.CurrentView as PokemonCardView;
				nextCard.MainImage.FadeTo(1, 250);
				nextCard.MainImage.ScaleTo(1, 250);
				nextCard.MainImage.TranslateTo(this.X, -160, 250);
				card.ScaleTo(1, 50);
			}
			if (args.Status == PanCardView.Enums.UserInteractionStatus.Ended)
			{
				var prevCard = MainCardView.CurrentBackViews.First() as PokemonCardView;
				Debug.WriteLine($"CurrentView: {prevCard.PokemonName.Text}");
				prevCard.CardBackground.Opacity = 1;
			}

		}
	}
}
