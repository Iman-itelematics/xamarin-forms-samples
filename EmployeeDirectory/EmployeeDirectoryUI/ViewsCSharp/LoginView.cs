﻿using System;
using Xamarin.Forms;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectoryUI.CSharp
{
	public class LoginView : ContentPage
	{
		private LoginViewModel loginViewModel;

		private LoginViewModel Model {
			get {
				if (loginViewModel == null)
					loginViewModel = new LoginViewModel (App.Service);

				return loginViewModel;
			}
		}

		public LoginView ()
		{
			BindingContext = Model;

			var titleImage = new Image { Source = FileImageSource.FromFile ("logo") };

			var usernameEntry = new Entry { Placeholder = "Username" };
			usernameEntry.SetBinding (Entry.TextProperty, "Username");

			var passwordEntry = new Entry { IsPassword = true, Placeholder = "Password" };
			passwordEntry.SetBinding (Entry.TextProperty, "Password");

			var loginButton = new Button { Text = "Login" };
			loginButton.Clicked += OnLoginClicked;

			var helpButton = new Button { Text = "Help" };
			helpButton.Clicked += OnHelpClicked;

			var grid = new Grid () {
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			if (Device.OS == TargetPlatform.iOS) {
				grid.Children.Add (loginButton, 0, 0);
				grid.Children.Add (helpButton, 1, 0);

				Content = new StackLayout () { 
					VerticalOptions = LayoutOptions.StartAndExpand,
					Padding = new Thickness (30),
					Children = { titleImage, usernameEntry, passwordEntry, grid }
				};

				BackgroundImage = "login_box";

			} else if (Device.OS == TargetPlatform.Android) {
				grid.Children.Add (titleImage, 0, 0);
				grid.Children.Add (helpButton, 1, 0);

				Content = new StackLayout () {
					VerticalOptions = LayoutOptions.Center,
					Padding = new Thickness (30),
					Children = { grid, usernameEntry, passwordEntry, loginButton },
				};
			}

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		}

		private void OnLoginClicked (object sender, EventArgs e)
		{
			if (loginViewModel.CanLogin) {
				loginViewModel
				.LoginAsync (System.Threading.CancellationToken.None)
				.ContinueWith (_ => {
					App.LastUseTime = System.DateTime.UtcNow;

					Navigation.PopAsync ();
				});

				Navigation.PopModalAsync ();
			} else {
				DisplayAlert ("Error", loginViewModel.ValidationErrors, "OK", null);
			}
		}

		private void OnHelpClicked (object sender, EventArgs e)
		{
			DisplayAlert ("Help", "Enter any username and password", "OK", null);
		}
	}
}

