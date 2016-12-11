using Android.App;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	public class TimepickerFragment : Android.Support.V4.App.DialogFragment
	{
		private Activity activity;

		public TimepickerFragment(Activity activity)
		{
			this.activity = activity;
		}

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);

			Button selectTime = activity.FindViewById<Button>(Resource.Id.timeButton);
			TextView timeTextView = activity.FindViewById<TextView>(Resource.Id.timeTextView);

			selectTime.Click += (sender, e) =>
			{
				var p = new BetterPickers.TimePickers.TimePickerBuilder()
				.SetFragmentManager(FragmentManager)
				.SetStyleResId(Resource.Style.BetterPickersDialogFragment_Light)
				.AddTimePickerDialogHandler((reference, hourOfDay, minute) =>
				{
					string time = $"{hourOfDay}:{minute}";
					timeTextView.Text = time;
				});
				p.Show();
			};
		}
	}
}
