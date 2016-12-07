using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace Mono.Samples.Notepad
{
	[Activity(Label = "Edit Note", ScreenOrientation = ScreenOrientation.Sensor, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
	public class NoteEditorActivity : FragmentActivity// Activity
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


			//// Create our Picker Dialog
			//var p = new BetterPickers.RadialTimePickers.RadialTimePickerDialog();

			//// Set some options
			//p.SetStartTime(DateTime.Now.Hour, DateTime.Now.Minute);
			//p.SetDoneText("Finish!");

			//// Wire up the changed handler
			//p.TimeSet += (sender, e) =>
			//{
			//	//ShowToast("RadialTimePicker Set: Hour={0}, Minute={1}", e.P1, e.P2);
			//};

			//// Show the Dialog
			//p.Show(this.FragmentManager, null);


			//datepickerFragment

			var timeFragment = new TimepickerFragment(this);
			var dateFragment = new DatepickerFragment(this);

			////Fragment datepickerFragment = this.FindViewById<Fragment>(Resource.Id.datepickerFragment);
			SupportFragmentManager.BeginTransaction()
								  .Replace(Resource.Id.dateFragment, dateFragment)
								  .Commit();

			SupportFragmentManager.BeginTransaction()
								  .Replace(Resource.Id.timeFragment, timeFragment)
								  .Commit();

			//While others use the builder pattern like the Date Picker: 

			// Create our picker builder
			//var p = new BetterPickers.DatePickers.DatePickerBuilder()
			//	.SetFragmentManager(FragmentManager)
			//	.SetStyleResId(Resource.Style.BetterPickersDialogFragment_Light);

			//// Add a delegate to handle the picker change      
			//p.AddDatePickerDialogHandler((reference, year, month, day) =>
			//{
			//	ShowToast("DatePicker Set: Ref={0}, Year={1}, Month={2}, Day={3}", reference, year, month, day);
			//});

			//// Show the Dialog
			//p.Show();
		}

		void Save_Button_Click(object sender, EventArgs e)
		{
			note.Description = description.Text;
			note.Title = title.Text;
			note.Level = levels[imp_level.SelectedItem.ToString()];

			NoteRepository.SaveNote(note);
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
	}
}