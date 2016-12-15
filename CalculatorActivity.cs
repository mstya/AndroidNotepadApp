
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mono.Samples.Notepad
{
[Activity(Label = "Super Calculator")]
	public class CalculatorActivity : Activity
	{
		private Dictionary<string, Func<string, string, double>> operations = new Dictionary<string, Func<string, string, double>>();
		private string left;
		private string rigth;
		private string operation;
		private TextView resultView;

		protected override void OnStop()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString("left", left);
			editor.PutString("rigth", rigth);
			editor.PutString("operation", operation);
			editor.PutString("result", resultView.Text);
			editor.Apply();

			base.OnStop();
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Calculator);

			this.resultView = this.FindViewById<TextView>(Resource.Id.resultView);

			List<Button> digitButtons = new List<Button>();
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butOne));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butTwo));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butThree));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butFour));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butFive));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butSix));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butSeven));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butEight));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.butNine));
			digitButtons.Add(this.FindViewById<Button>(Resource.Id.zeroBut));
			digitButtons.ForEach(x => x.Click += this.OnDigitClick);

			List<Button> operationButtons = new List<Button>();
			operationButtons.Add(this.FindViewById<Button>(Resource.Id.multiplyBut));
			operationButtons.Add(this.FindViewById<Button>(Resource.Id.devideBut));
			operationButtons.Add(this.FindViewById<Button>(Resource.Id.minusBut));
			operationButtons.Add(this.FindViewById<Button>(Resource.Id.plusBut));
			operationButtons.ForEach(x => x.Click += this.OnOperationClick);

			Button equals = this.FindViewById<Button>(Resource.Id.equalsBut);
			equals.Click += OnResult;

			operations.Add("+", (l, r) => double.Parse(l) + double.Parse(r));
			operations.Add("-", (l, r) => double.Parse(l) - double.Parse(r));
			operations.Add("*", (l, r) => double.Parse(l) * double.Parse(r));
			operations.Add("÷", (l, r) => double.Parse(l) / double.Parse(r));

			Button clearBub = this.FindViewById<Button>(Resource.Id.clearBut);
			clearBub.Click += (sender, e) =>
			{
				resultView.Text = left = rigth = operation = string.Empty;
			};

			this.GetDataFromStorage();
		}

		private void GetDataFromStorage()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
			this.left = prefs.GetString("left", string.Empty);
			this.rigth = prefs.GetString("rigth", string.Empty);
			this.operation = prefs.GetString("operation", string.Empty);
			this.resultView.Text = prefs.GetString("result", string.Empty);
		}

		private void OnResult(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.left) || string.IsNullOrEmpty(this.rigth))
			{
				return;
			}

			this.resultView.Text = this.operations[this.operation](this.left, this.rigth).ToString();
			this.left = this.resultView.Text;
			this.rigth = string.Empty;
			this.operation = string.Empty;
		}

		private void OnOperationClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(left) && string.IsNullOrEmpty(this.operation))
			{
				Button but = sender as Button;
				this.operation = but.Text;
				resultView.Text += but.Text;
			}
		}

		private void OnDigitClick(object sender, EventArgs e)
		{
			Button but = sender as Button;
			int val = int.Parse(but.Text);
			if (string.IsNullOrEmpty(operation))
			{
				left += but.Text;
			}
			else
			{
				rigth += but.Text;
			}

			resultView.Text += val;
		}
	}
}
