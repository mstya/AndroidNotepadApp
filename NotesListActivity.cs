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
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	// Displays a list of notes.
	[Activity (MainLauncher = true, Label = "@string/app_name", Icon = "@drawable/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
	[IntentFilter(new string[] { "android.intent.action.SEARCH" })]
	[MetaData(("android.app.searchable"), Resource = "@xml/searchable")]
	public class NotesListActivity : ListActivity
	{
		// Menu item ids
		public const int MENU_ITEM_DELETE = Menu.First;
		public const int MENU_ITEM_INSERT = Resource.Id.add;
		public const int MENU_ITEM_SEARCH = Resource.Id.search;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetDefaultKeyMode (DefaultKey.Shortcut);

			// Inform the list we provide context menus for items
			ListView.SetOnCreateContextMenuListener (this);

			HandleIntent(Intent);

			//PopulateList ();

		}

		void HandleIntent(Intent intent)
		{
			if (Intent.ActionSearch.Equals(intent.Action))
			{
				string query = intent.GetStringExtra(SearchManager.Query);
				PopulateList(query);
			}
			else
			{
				PopulateList();
			}
		}

		public void PopulateList (string query = "")
		{
			var notes = string.IsNullOrEmpty(query) ? NoteRepository.GetAllNotes () : NoteRepository.GetAllWhere(query);
			var adapter = new NoteAdapter (this, this, Resource.Layout.NoteListRow, notes.ToArray ());
			ListAdapter = adapter;
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater.Inflate(Resource.Menu.options_menu, menu);

			var searchManager = (SearchManager)GetSystemService(Context.SearchService);
			var searchView = (SearchView)menu.FindItem(Resource.Id.search).ActionView;

			searchView.SetSearchableInfo(searchManager.GetSearchableInfo(ComponentName));
			searchView.SetIconifiedByDefault(false);
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
				case MENU_ITEM_INSERT:
					// Launch activity to insert a new item
					var intent = new Intent (this, typeof (NoteEditorActivity));
					intent.PutExtra ("note_id", -1L);

					StartActivityForResult (intent, 0);
					return true;
				case MENU_ITEM_SEARCH:
					OnSearchRequested();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		protected override void OnNewIntent(Android.Content.Intent intent)
		{
			// Because this activity has set launchMode="singleTop", the system calls this method
			// to deliver the intent if this actvity is currently the foreground activity when
			// invoked again (when the user executes a search from this activity, we don't create
			// a new instance of this activity, so the system delivers the search intent here)
			HandleIntent(intent);
		}

		public override void OnCreateContextMenu (IContextMenu menu, View view, IContextMenuContextMenuInfo menuInfo)
		{
			var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
			var note = (Note)ListAdapter.GetItem (info.Position);

			// Add a menu item to delete the note
			menu.Add (0, MENU_ITEM_DELETE, 0, Resource.String.menu_delete);
		}

		public override bool OnContextItemSelected (IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			var note = (Note)ListAdapter.GetItem (info.Position);

			switch (item.ItemId) {
				case MENU_ITEM_DELETE: {
						// Delete the note that the context menu is for
						NoteRepository.DeleteNote (note);
						PopulateList ();
						return true;
				}
			}

			return false;
		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var selected = (Note)ListAdapter.GetItem (position);

			// Launch activity to view/edit the currently selected item
			var intent = new Intent (this, typeof (NoteEditorActivity));
			intent.PutExtra ("note_id", selected.Id);

			StartActivityForResult (intent, 0);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			// The only thing we care about is refreshing the list
			// in case it changed
			PopulateList ();
		}
	}
}
