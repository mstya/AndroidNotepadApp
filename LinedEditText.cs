using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Android.Runtime;

// A custom EditText that draws lines so it looks like a notepad
namespace Mono.Samples.Notepad
{
	[Register ("mono.samples.notepad.LinedEditText")]
	class LinedEditText : EditText
	{
		private Rect rect;
		private Paint paint;

		// we need this constructor for LayoutInflater
		public LinedEditText (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{
			rect = new Rect ();
			paint = new Paint ();
			paint.SetStyle (Android.Graphics.Paint.Style.Stroke);
			paint.Color = Color.LightGray;
		}

		protected override void OnDraw (Canvas canvas)
		{
			int count = LineCount;

			for (int i = 0; i < count; i++) {
				int baseline = GetLineBounds (i, rect);

				canvas.DrawLine (rect.Left, baseline + 1, rect.Right, baseline + 1, paint);
			}

			base.OnDraw (canvas);
		}
	}
}