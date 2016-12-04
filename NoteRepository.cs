using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

namespace Mono.Samples.Notepad
{
	class NoteRepository
	{
		private static string db_file = "notes.db3";

		private static SqliteConnection GetConnection ()
		{
			var dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), db_file);
			bool exists = File.Exists (dbPath);

			if (!exists)
				SqliteConnection.CreateFile (dbPath);

			var conn = new SqliteConnection ("Data Source=" + dbPath);

			if (!exists)
				CreateDatabase (conn);

			return conn;
		}

		private static void CreateDatabase (SqliteConnection connection)
		{
			var sql = "CREATE TABLE ITEMS (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title ntext, Description ntext, Level INTEGER, Modified datetime);";

			connection.Open ();

			using (var cmd = connection.CreateCommand ()) {
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery ();
			}

			sql = "INSERT INTO ITEMS (Title, Description, Level, Modified) VALUES (@Title, @Title, @Level, @Modified);";

			using (var cmd = connection.CreateCommand ()) {
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue ("@Title", "Sample Note");
				cmd.Parameters.AddWithValue ("@Description", "Sample Note");
				cmd.Parameters.AddWithValue ("@Level", 2);
				cmd.Parameters.AddWithValue ("@Modified", DateTime.Now);

				cmd.ExecuteNonQuery ();
			}

			connection.Close ();
		}

		public static IEnumerable<Note> GetAllNotes ()
		{
			var sql = "SELECT * FROM ITEMS;";

			using (var conn = GetConnection ()) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;

					using (var reader = cmd.ExecuteReader ()) {
						while (reader.Read ())
							yield return new Note (reader.GetInt32 (0), reader.GetString (1), reader.GetString(2), reader.GetInt32(3), reader.GetDateTime (4)); 
					}
				}
			}
		}

		internal static IEnumerable<Note> GetAllWhere(string query)
		{
 			var sql = $"SELECT * FROM ITEMS WHERE TITLE LIKE '%{query}%';";

			using (var conn = GetConnection())
			{
				conn.Open();

				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = sql;

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							yield return new Note(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDateTime(4));
					}
				}
			}
		}

		public static Note GetNote (long id)
		{
			var sql = $"SELECT * FROM ITEMS WHERE Id = {id};";

			using (var conn = GetConnection ()) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;

					using (var reader = cmd.ExecuteReader ()) {
						if (reader.Read ())
							return new Note (reader.GetInt32 (0), reader.GetString (1), reader.GetString(2), reader.GetInt32(3), reader.GetDateTime (4)); 
						else
							return null;
					}
				}
			}
		}

		public static void DeleteNote (Note note)
		{
			var sql = string.Format ("DELETE FROM ITEMS WHERE Id = {0};", note.Id);

			using (var conn = GetConnection ()) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
			}
		}


		public static void SaveNote (Note note)
		{
			using (var conn = GetConnection ()) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {

					if (note.Id < 0) {
						cmd.CommandText = "INSERT INTO ITEMS (Title, Description, Level, Modified) VALUES (@Title, @Description, @Level, @Modified); SELECT last_insert_rowid();";
						cmd.Parameters.AddWithValue ("@Title", note.Title);
						cmd.Parameters.AddWithValue ("@Description", note.Description);
						cmd.Parameters.AddWithValue("@Level", note.Level);
						cmd.Parameters.AddWithValue ("@Modified", DateTime.Now);

						note.Id = (long)cmd.ExecuteScalar ();
					} else {
						cmd.CommandText = "UPDATE ITEMS SET Description = @Description, Title = @Title, Level = @Level, Modified = @Modified WHERE Id = @Id";
						cmd.Parameters.AddWithValue ("@Id", note.Id);
						cmd.Parameters.AddWithValue ("@Title", note.Title);
						cmd.Parameters.AddWithValue ("@Level", note.Level);
						cmd.Parameters.AddWithValue ("@Description", note.Description);
						cmd.Parameters.AddWithValue ("@Modified", DateTime.Now);
					
						cmd.ExecuteNonQuery ();
					}
				}
			}
		}
	}
}