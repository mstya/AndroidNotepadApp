using System;
using Android.App;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	public class DatepickerFragment : Android.Support.V4.App.DialogFragment
	{
		private Activity activity;

		public DatepickerFragment(Activity activity)
		{
			this.activity = activity;
		}

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);

			Button selectDate = activity.FindViewById<Button>(Resource.Id.dateButton);
			TextView dateTextView = activity.FindViewById<TextView>(Resource.Id.dateTextView);

			selectDate.Click += (sender, e) =>
			{
				DateTime today = DateTime.Now.Date;
				var p = BetterPickers.CalendarDatePickers.CalendarDatePickerDialog.NewInstance(null, today.Year, today.Month - 1, today.Day);

				p.DateSet += (csender, ce) =>
				{
					DateTime date = new DateTime(ce.P1, ce.P2 + 1, ce.P3);
					dateTextView.Text = date.ToShortDateString();
				};
				p.Show(this.FragmentManager, "OK");
			};
		}
	}
}