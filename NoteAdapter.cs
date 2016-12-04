using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;

namespace Mono.Samples.Notepad
{
	class NoteAdapter : ArrayAdapter
	{
		private Activity activity;

		private Dictionary<string, int> imageResDic;

		public NoteAdapter(Activity activity, Context context, int textViewResourceId, object[] objects)
			: base(context, textViewResourceId, objects)
		{
			this.activity = activity;
			this.imageResDic = new Dictionary<string, int>()
			{
				["1"] = Resource.Drawable.ic_looks_one_white_24dp,
				["2"] = Resource.Drawable.ic_looks_two_white_24dp,
				["3"] = Resource.Drawable.ic_looks_3_white_24dp
			};
		}

		public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var item = (Note)this.GetItem(position);
			var view = (convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.NoteListRow, parent, false)) as LinearLayout;

			view.FindViewById<TextView>(Resource.Id.body).Text = Left(item.Title.Replace("\n", " "), 25);
			view.FindViewById<TextView>(Resource.Id.modified).Text = item.ModifiedTime.ToString();
			view.FindViewById<ImageView>(Resource.Id.level).SetImageResource(imageResDic[item.Level.ToString()]);

			return view;
		}

		private string Left(string text, int length)
		{
			if (text.Length <= length)
				return text;

			return text.Substring(0, length);
		}
	}
}