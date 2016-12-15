
using System.ComponentModel;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Mono.Samples.Notepad
{
[Activity(Label = "Colorpicker")]
	public class ColorpickerActivity : Activity
	{
		private SeekBar bSeek;
		private SeekBar gSeek;
		private ImageView imageView;
		private TextView result;
		private SeekBar rSeek;
		private Color currentColor;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Colorpicker);

			this.imageView = this.FindViewById<ImageView>(Resource.Id.imageView1);
			this.currentColor = Color.Black;
			this.imageView.SetBackgroundColor(this.currentColor);

			this.rSeek = this.FindViewById<SeekBar>(Resource.Id.rSeekBar);
			this.gSeek = this.FindViewById<SeekBar>(Resource.Id.gSeekBar);
			this.bSeek = this.FindViewById<SeekBar>(Resource.Id.bSeekBar);

			this.result = this.FindViewById<TextView>(Resource.Id.result);
			this.SetSeekHandlers();
		}

		private void SetSeekHandlers()
		{
			//this.rSeek.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
			//{
			//	currentColor.R = (byte)e.Progress;
			//	this.UpdateUI();
			//};

			this.rSeek.ProgressChanged += (sender, e) =>
			{
				currentColor.R = (byte)e.Progress;
				this.UpdateUI();
			};

			this.gSeek.ProgressChanged += (sender, e) =>
			{
				currentColor.G = (byte)e.Progress;
				this.UpdateUI();
			};

			this.bSeek.ProgressChanged += (sender, e) =>
			{
				currentColor.B = (byte)e.Progress;
				this.UpdateUI();
			};
		}

		private void UpdateUI()
		{
			string result = $"R: {this.currentColor.R}, G: {this.currentColor.G}, B: {this.currentColor.B}";
			this.result.Text = result;

			this.imageView = this.FindViewById<ImageView>(Resource.Id.imageView1);

			this.imageView.SetBackgroundColor(currentColor);
			this.imageView.Invalidate();
		}
	}
}