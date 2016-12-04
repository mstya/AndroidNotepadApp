﻿/*
 * Copyright (C) 2007 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Android.Runtime;
using System.Collections.Generic;

namespace Mono.Samples.Notepad
{
	[Activity (Label = "Edit Note", ScreenOrientation = ScreenOrientation.Sensor, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
	public class NoteEditorActivity : Activity
	{
		private Note note;
		private EditText title;
		private EditText description;
		private Spinner imp_level;
		private Button save_button;
		private Button cancel_button;
		Dictionary<string, int> levels;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.NoteEditor);

			imp_level = FindViewById<Spinner>(Resource.Id.level);
			title = FindViewById<EditText>(Resource.Id.title);
			description = FindViewById<EditText> (Resource.Id.description);
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

			var note_id = Intent.GetLongExtra ("note_id", -1L);

			if (note_id < 0)
				note = new Note ();
			else
				note = NoteRepository.GetNote (note_id);
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

		protected override void OnResume ()
		{
			base.OnResume ();

			// This is a little tricky: we may be resumed after previously being
			// paused/stopped.  We want to put the new text in the text view,
			// but leave the user where they were (retain the cursor position
			// etc).  This version of setText does that for us.
			description.SetTextKeepState (note.Description);
			title.SetTextKeepState(note.Title);
			imp_level.SetSelection(note.Level - 1);

		}

		//protected override void OnPause ()
		//{
		//	base.OnPause ();

		//	// If this was a new note and no content was added, don't save it
		//	if (IsFinishing && note.Id == -1 && description.Text.Length == 0)
		//		return;

		//	// Save the note
		//	note.Description = description.Text;
		//	NoteRepository.SaveNote (note);
		//}
	}
}
