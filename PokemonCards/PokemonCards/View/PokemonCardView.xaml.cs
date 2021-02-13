using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PokemonCards.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Diagnostics;

namespace PokemonCards.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PokemonCardView : ContentView
	{
		private Pokemon _viewModel;
		private readonly float _density;
		private readonly float _cardTopMargin;
		private readonly float _cornerRadius;

		SKColor _pokeColor;
		SKPaint _pokePaint;


		private CardState _cardState = CardState.Collapsed;
		private double _cardTopPosition;

		public PokemonCardView()
		{
			InitializeComponent();

			

			//density of the screen
			_density = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
			_cardTopMargin = 300f * _density;
			_cornerRadius = 30f * _density;

			PokemonContent.Margin = new Thickness(30, _cardTopMargin/6, 0, 0);

		}
		
		
		
		//public BoxView CardBackground => PokemonBackground;

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (this.BindingContext == null) return;
			_viewModel = this.BindingContext as Pokemon;

			// because we can't bind skia drawing using the binding engine
			// we cache the paint objects when the bound character changes

			_pokeColor = Color.FromHex(_viewModel.PokemonPrimaryColor).ToSKColor();
			_pokePaint = new SKPaint() { Color = _pokeColor };

			//setup initial values
			_cardTopPosition = _cardTopMargin;

			// repaint the surface with the new colors
			CardBackground.InvalidateSurface();
		}
		public Image MainImage => PokeImage;
		public Label PokemonName => PokeName;
		private void CardBackground_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
		{

			// sometimes PaintSurface is called before the binding context is set 
			if (_viewModel == null) return;

			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();


			//draw card
			canvas.DrawRoundRect(
				rect: new SKRect(0, (float)_cardTopPosition, info.Width, info.Height),
				r: new SKSize(_cornerRadius, _cornerRadius),
				paint: _pokePaint);
		}


		private void SeeMore_Tapped(object sender, EventArgs e)
		{
			//Go to state of expanded
			GoToState(CardState.Expanded);
		}

		public void GoToState(CardState cardState)
		{
			// trigger an animation
			if (_cardState == cardState)
				return;

			MessagingCenter.Send<CardEvent>(new CardEvent(), cardState.ToString());
			AnimationTransition(cardState);
			_cardState = cardState;
		}

		private void AnimationTransition(CardState cardState)
		{
			var parentAnimation = new Animation();
			if (cardState == CardState.Expanded)
			{
				parentAnimation.Add(0, 0.1, CardAnimation(cardState));
			}
			else
			{
				parentAnimation.Add(0, 0.1, CardAnimation(cardState));
			}
			parentAnimation.Commit(this, "CardExpand", 16, 2000);
		}

		private Animation CardAnimation(CardState cardState)
		{
			//work out where the top of the card should be
			Debug.WriteLine($"CARD STATE: {cardState.ToString()}");
			var animationStart = cardState == CardState.Expanded ? _cardTopMargin : -_cornerRadius;
			var animationStop = cardState == CardState.Expanded ? -_cornerRadius : _cardTopMargin;

			var animation = new Animation(
				v =>
				{
					_cardTopPosition = v;
					CardBackground.InvalidateSurface();
				},
				animationStart,
				animationStop,
				Easing.SinInOut
				);
			return animation;
		}
	}

}