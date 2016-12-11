using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	[Activity(MainLauncher = true, Label = "@string/app_name", Icon = "@drawable/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
	[IntentFilter(new string[] { "android.intent.action.SEARCH" })]
	[MetaData(("android.app.searchable"), Resource = "@xml/searchable")]
	public class NotesListActivity : ListActivity
	{
		public const int MENU_ITEM_DELETE = Menu.First;
		public const int MENU_ITEM_INSERT = Resource.Id.add;
		public const int MENU_ITEM_SEARCH = Resource.Id.search;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetDefaultKeyMode(DefaultKey.Shortcut);
			ListView.SetOnCreateContextMenuListener(this);
			HandleIntent(Intent);
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

		public void PopulateList(string query = "")
		{
			new NoteAsyncHandler(this).Execute(query);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu(menu);

			MenuInflater.Inflate(Resource.Menu.options_menu, menu);

			var searchManager = (SearchManager)GetSystemService(SearchService);
			var searchView = (SearchView)menu.FindItem(Resource.Id.search).ActionView;

			searchView.SetSearchableInfo(searchManager.GetSearchableInfo(ComponentName));
			searchView.SetIconifiedByDefault(false);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case MENU_ITEM_INSERT:
					var intent = new Intent(this, typeof(NoteEditorActivity));
					intent.PutExtra("note_id", -1L);
					StartActivityForResult(intent, 0);
					return true;
				case MENU_ITEM_SEARCH:
					OnSearchRequested();
					return true;
				case 16908332:
					HandleIntent(Intent);
					return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		protected override void OnNewIntent(Intent intent)
		{
			HandleIntent(intent);
		}

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			menu.Add(0, MENU_ITEM_DELETE, 0, Resource.String.menu_delete);
		}

		public override bool OnContextItemSelected(IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			var note = (Note)ListAdapter.GetItem(info.Position);

			switch (item.ItemId)
			{
				case MENU_ITEM_DELETE:
				{
					NoteRepository.DeleteNote(note);
					PopulateList();
					return true;
				}
			}

			return false;
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var selected = (Note)ListAdapter.GetItem(position);
			var intent = new Intent(this, typeof(NoteEditorActivity));
			intent.PutExtra("note_id", selected.Id);

			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			PopulateList();
		}
	}
}