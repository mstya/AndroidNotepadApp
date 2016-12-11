using System;
namespace Mono.Samples.Notepad
{
	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			var message = intent.GetStringExtra("message");
			var title = intent.GetStringExtra("title");
			var id = intent.GetLongExtra("note_id", 0);

			// Setup an intent for SecondActivity:
			Intent secondIntent = new Intent(context, typeof(NoteEditorActivity));

			// Pass some information to SecondActivity:
			secondIntent.PutExtra("note_id", id);

			// Create a task stack builder to manage the back stack:
			TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);

			// Add all parents of SecondActivity to the stack: 
			stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(NoteEditorActivity)));

			// Push the intent that starts SecondActivity onto the stack:
			stackBuilder.AddNextIntent(secondIntent);

			const int pendingIntentId = 0;
			PendingIntent pendingIntent =
				stackBuilder.GetPendingIntent(pendingIntentId, PendingIntentFlags.OneShot);

			//Generate a notification with just short text and small icon
			var builder = new Notification.Builder(context)
							.SetContentIntent(pendingIntent)
							.SetSmallIcon(Resource.Drawable.icon)
							.SetContentTitle(title)
							.SetContentText(message)
							.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
							.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
							.SetAutoCancel(true);

			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				context.GetSystemService(Context.NotificationService) as NotificationManager;

			// Publish the notification:
			const int notificationId = 0;
			notificationManager.Notify(notificationId, notification);
		}
	}
}
