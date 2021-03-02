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
		private double _pokeImageTranslationY = 50;
		private double _movementFactor = 100;

		public MainPage()
		{
			InitializeComponent();
			this.BindingContext = new PokemonCardsViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MainCardView.UserInteracted += MainCardView_UserInteracted;
			MessagingCenter.Subscribe<CardEvent>(this, CardState.Expanded.ToString(), CardExpand);
		}

		private void CardExpand(CardEvent obj)
		{
			//turn off swiping
			MainCardView.IsUserInteractionEnabled = false;

			//animate the title
			AnimateTitle(CardState.Expanded);
		}

		private void ArrowBack_Tapped (object sender, EventArgs e)
		{

			//animate the title
			AnimateTitle(CardState.Collapsed);

			//give collapsed status
			((PokemonCardView)MainCardView.CurrentView).GoToState(CardState.Collapsed);

			// turn on swiping
			MainCardView.IsUserInteractionEnabled = true;
		}

		private void AnimateTitle(CardState cardState)
		{

			var translation = cardState == CardState.Expanded ? -PokemonHeader.Height - PokemonHeader.Margin.Top : 0;
			var opacity = cardState == CardState.Expanded ? 0 : 1;

			var animation = new Animation();
			//Whole title animation takes 8 frames
			if (cardState == CardState.Expanded)
			{
				animation.Add(0, 0.13, new Animation(v => PokemonHeader.TranslationY = v, PokemonHeader.TranslationY, translation));
				animation.Add(0, 0.13, new Animation(v => PokemonHeader.Opacity = v, PokemonHeader.Opacity, opacity));
			}
			else
			{
				animation.Add(0.87, 1, new Animation(v => PokemonHeader.TranslationY = v, PokemonHeader.TranslationY, translation));
				animation.Add(0.87, 1, new Animation(v => PokemonHeader.Opacity = v, PokemonHeader.Opacity, opacity));
			}
			animation.Commit(this, "titleAnimation", 16, 1000);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MainCardView.UserInteracted -= MainCardView_UserInteracted;
			MessagingCenter.Unsubscribe<CardEvent>(this, CardState.Expanded.ToString());
		}

		private void MainCardView_UserInteracted(
			PanCardView.CardsView view, 
			PanCardView.EventArgs.UserInteractedEventArgs args)
		{
			var card = MainCardView.CurrentView as PokemonCardView;
			if (args.Status == PanCardView.Enums.UserInteractionStatus.Running)
			{
				
				var ratioFromCenter = Math.Abs(args.Diff / this.Width);

				animateFirstCard(card, ratioFromCenter);


				var nextCard = MainCardView.CurrentBackViews.First() as PokemonCardView;

				animateSecondCard(nextCard, ratioFromCenter);

			}

			//Reset parameters of the card after swipe so it doens't look stupid
			if (args.Status == PanCardView.Enums.UserInteractionStatus.Ending)
			{
				card.MainImage.FadeTo(1, 250);
				card.MainImage.ScaleTo(1, 250);
				card.MainImage.TranslateTo(this.X, 0, 250);
				card.ScaleTo(1, 50);
			}


			if (args.Status == PanCardView.Enums.UserInteractionStatus.Ended)
			{
				if (MainCardView.CurrentBackViews.Count() == 0)
					return;
				var prevCard = MainCardView.CurrentBackViews.First() as PokemonCardView;
				prevCard.MainImage.TranslationY = 0;
			}
		}

		
		private void animateFirstCard(PokemonCardView card ,double ratioFromCenter)
		{
			var delayCardOpacityFactor = 0.6;
			var delayImageScaleFactor = 0.4;
			var delayImageOpacityFactor = 0.3;

			MainCardView.CurrentView.Opacity = LimitRange((1 + delayCardOpacityFactor) - ratioFromCenter, 0, 1);

			var scale = LimitRange((1 + delayImageScaleFactor) - (ratioFromCenter * 1.75), 0.2, 1);

			card.MainImage.Scale = scale;
			Debug.WriteLine($"Y1: {((1 - scale) * card.MainImage.Height)/2}");
			card.MainImage.TranslationY =  ((1 - scale) * card.MainImage.Height)/2;
			

			card.MainImage.Opacity = LimitRange((1 + delayImageOpacityFactor) - (ratioFromCenter * 1.5), 0, 1);
			
			if (ratioFromCenter > 0 && scale == 1)
			{
				card.ScaleTo(0.95, 50);
			}
			
		}

		private void animateSecondCard(PokemonCardView card, double ratioFromCenter)
		{
			card.MainImage.Opacity = LimitRange((ratioFromCenter - 0.3) * 3, 0, 1);

			var scale = LimitRange(ratioFromCenter * 1.25, 0, 1);

			card.MainImage.Scale = scale;

			
			Debug.WriteLine($"Y2: {card.MainImage.Height}");
			card.MainImage.TranslationY = (card.MainImage.Height-scale*card.MainImage.Height)/2;
		}
	}
}
