using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	[Activity(Label = "Menu", MainLauncher = true, Icon = "@drawable/icon")]
	public class MenuActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.menu);
			this.AddButtonHandles();
		}

		private void AddButtonHandles()
		{
			Button colorpickerButton = FindViewById<Button>(Resource.Id.Colorpicker);
			Button calcButton = FindViewById<Button>(Resource.Id.Calc);
			Button listButton = FindViewById<Button>(Resource.Id.List);

			colorpickerButton.Click += (object o, EventArgs e) => StartActivity(typeof(ColorpickerActivity));
			calcButton.Click += (object o, EventArgs e) => StartActivity(typeof(CalculatorActivity));
			listButton.Click += (object o, EventArgs e) => StartActivity(typeof(NotesListActivity));
		}
	}
}
