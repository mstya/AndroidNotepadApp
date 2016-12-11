using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Content;
using Java.Util;

namespace Mono.Samples.Notepad
{
	[Activity(Label = "Edit Note", ScreenOrientation = ScreenOrientation.Sensor, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
	public class NoteEditorActivity : FragmentActivity
	{
		private Note note;
		private EditText title;
		private EditText description;
		private Spinner imp_level;
		private Button save_button;
		private Button cancel_button;
		Dictionary<string, int> levels;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.NoteEditor);

			imp_level = FindViewById<Spinner>(Resource.Id.level);
			title = FindViewById<EditText>(Resource.Id.title);
			description = FindViewById<EditText>(Resource.Id.description);
			var adapter = ArrayAdapter.CreateFromResource(
				this, Resource.Array.imp_levels, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			imp_level.Adapter = adapter;
			save_button = FindViewById<Button>(Resource.Id.save);
			cancel_button = FindViewById<Button>(Resource.Id.cancel);
			save_button.Click += Save_Button_Click;
			cancel_button.Click += Cancel_Button_Click;
			levels = new Dictionary<string, int>()
			{
				["Level 1"] = 1,
				["Level 2"] = 2,
				["Level 3"] = 3
			};

			// Get the message from the intent:
			string message = Intent.Extras.GetString("message", "");

			var note_id = Intent.GetLongExtra("note_id", -1L);

			if (note_id < 0)
				note = new Note();
			else
				note = NoteRepository.GetNote(note_id);
			
			var timeFragment = new TimepickerFragment(this);
			var dateFragment = new DatepickerFragment(this);

			SupportFragmentManager.BeginTransaction()
								  .Replace(Resource.Id.dateFragment, dateFragment)
								  .Commit();

			SupportFragmentManager.BeginTransaction()
								  .Replace(Resource.Id.timeFragment, timeFragment)
								  .Commit();
		}

		void Save_Button_Click(object sender, EventArgs e)
		{
			note.Description = description.Text;
			note.Title = title.Text;
			note.Level = levels[imp_level.SelectedItem.ToString()];

			TextView dateTextView = this.FindViewById<TextView>(Resource.Id.dateTextView);
			TextView timeTextView = this.FindViewById<TextView>(Resource.Id.timeTextView);

			var date = DateTime.Parse(dateTextView.Text);
			var timeArray = timeTextView.Text.Split(':');
			var selectedDate = new DateTime(date.Year, date.Month, date.Day, int.Parse(timeArray[0]), int.Parse(timeArray[1]), 0);

			note.ModifiedTime = selectedDate;

			NoteRepository.SaveNote(note);
			this.SetNotification(selectedDate, note.Id, note.Title);
			Finish();
		}

		void Cancel_Button_Click(object sender, EventArgs e)
		{
			Finish();
		}

		protected override void OnResume()
		{
			base.OnResume();

			description.SetTextKeepState(note.Description);
			title.SetTextKeepState(note.Title);
			imp_level.SetSelection(note.Level - 1);
		}

		private void SetNotification(DateTime date, long id, string title)
		{                   
			Intent alarmIntent = new Intent(this, typeof(AlarmReceiver));
			alarmIntent.PutExtra("message", title);
			alarmIntent.PutExtra("title", "Та дааааам!");
			alarmIntent.PutExtra("note_id", id);

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)this.GetSystemService(Context.AlarmService);

			Calendar calendar = Calendar.Instance;
			calendar.Set(CalendarField.Year, date.Year);
			calendar.Set(CalendarField.Month, date.Month - 1);
			calendar.Set(CalendarField.DayOfMonth, date.Day);
			calendar.Set(CalendarField.HourOfDay, date.Hour);
			calendar.Set(CalendarField.Minute, date.Minute);
			calendar.Set(CalendarField.Second, date.Second);

			alarmManager.Set(AlarmType.Rtc, calendar.TimeInMillis, pendingIntent);
		}
	}
}