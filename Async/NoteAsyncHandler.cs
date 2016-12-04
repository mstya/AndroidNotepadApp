using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;

namespace Mono.Samples.Notepad
{
	public class NoteAsyncHandler : AsyncTask
	{
		private readonly NotesListActivity activity;
		private ProgressDialog progressDialog;

		public NoteAsyncHandler(NotesListActivity activity)
		{
			this.activity = activity;
		}

		protected override void OnPreExecute()
		{
			base.OnPreExecute();
			this.progressDialog = ProgressDialog.Show(activity, "Загрузка", "Пожалуйста подождите...");
		}

		protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] data)
		{
			var query = data[0].ToString();
			IEnumerable<Note> result;

			result = string.IsNullOrEmpty(query) ? NoteRepository.GetAllNotes() : NoteRepository.GetAllWhere(query);
			return FromArray(result.ToArray());
		}

		protected override void OnPostExecute(Java.Lang.Object result)
		{
			var rows = (Java.Lang.Object[])result;
			var adapter = new NoteAdapter(this.activity, this.activity, Resource.Layout.NoteListRow, rows);
			this.activity.ListAdapter = adapter;

			this.progressDialog.Hide();
		}
	}
}