using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace dynabot
{// start of namespace

    // ── simple data structure to represent one log entry ───────────────
    public class LogEntry
    {// start of LogEntry class
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }// end of LogEntry class

    internal class ActivityLog
    {// start of ActivityLog class

        // ── record a new action in the activity log ─────────────────────
        public static void LogAction(string username, string description)
        {// start of method
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"INSERT INTO activity_log (username, action_description)
                                      VALUES (@username, @description);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.ExecuteNonQuery();
                    }// end of using
                }// end of using
            }// end of try
            catch { /* logging should never crash the app */ }
        }// end of method

        // ── retrieve the most recent log entries for a user ─────────────
        public static List<LogEntry> GetRecentActions(string username, int count = 10)
        {// start of method

            List<LogEntry> entries = new List<LogEntry>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"SELECT action_description, timestamp FROM activity_log
                                      WHERE username = @username
                                      ORDER BY timestamp DESC
                                      LIMIT @count;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@count", count);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {// start of using
                            while (reader.Read())
                            {// start of while
                                entries.Add(new LogEntry
                                {
                                    Description = reader.GetString("action_description"),
                                    Timestamp   = reader.GetDateTime("timestamp")
                                });
                            }// end of while
                        }// end of using
                    }// end of using
                }// end of using
            }// end of try
            catch { /* return whatever was collected so far */ }

            return entries;

        }// end of method

    }// end of ActivityLog class

}// end of namespace
