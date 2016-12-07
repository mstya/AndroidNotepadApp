using System;
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
			//selectDate.Click += (sender, e) =>
			//{
			//	DateTime today = DateTime.Now.Date;
			//	var p = BetterPickers.CalendarDatePickers.CalendarDatePickerDialog.NewInstance(null, today.Year, today.Month - 1, today.Day);

			//	p.DateSet += (csender, ce) =>
			//	{
			//		DateTime date = new DateTime(ce.P1, ce.P2 + 1, ce.P3);
			//		dateTextView.Text = date.ToShortDateString();
			//	};
			//	p.Show(this.FragmentManager, "OK");
			//};
		}
	}
}
