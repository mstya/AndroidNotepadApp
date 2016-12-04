using System;

namespace Mono.Samples.Notepad
{
	class Note : Java.Lang.Object
	{
		public long Id { get; set; }
		public string Title
		{
			get;
			set;
		}
		public string Description { get; set; }
		public DateTime ModifiedTime { get; set; }
		public int Level { get; internal set; }

		public Note ()
		{
			Id = -1L;
			Description = string.Empty;
			Title = string.Empty;
		}

		public Note (long id, string title, string body, int level, DateTime modified)
		{
			Id = id;
			Title = title;
			Description = body;
			Level = level;
			ModifiedTime = modified;
		}

		public override string ToString ()
		{
			return ModifiedTime.ToString ();
		}
	}
}