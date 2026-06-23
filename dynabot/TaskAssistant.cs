using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace dynabot
{// start of namespace

    // ── simple data structure to represent one task ────────────────────
    public class CyberTask
    {// start of CyberTask class
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }// end of CyberTask class

    internal class TaskAssistant
    {// start of TaskAssistant class

        // ── add a new task to the database ──────────────────────────────
        public static int AddTask(string username, string title, string description, string reminderDate)
        {// start of method
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"INSERT INTO tasks (username, title, description, reminder_date)
                                      VALUES (@username, @title, @description, @reminder);
                                      SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description ?? "");
                        cmd.Parameters.AddWithValue("@reminder", reminderDate ?? "Not set");

                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }// end of using
                }// end of using
            }// end of try
            catch
            {// start of catch
                return -1;
            }// end of catch
        }// end of method

        // ── get all tasks for a given user ──────────────────────────────
        public static List<CyberTask> GetTasks(string username)
        {// start of method

            List<CyberTask> tasks = new List<CyberTask>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"SELECT task_id, title, description, reminder_date, is_completed
                                      FROM tasks WHERE username = @username
                                      ORDER BY date_created DESC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {// start of using
                            while (reader.Read())
                            {// start of while
                                tasks.Add(new CyberTask
                                {
                                    Id           = reader.GetInt32("task_id"),
                                    Title        = reader.GetString("title"),
                                    Description  = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                                    ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? "Not set" : reader.GetString("reminder_date"),
                                    IsCompleted  = reader.GetBoolean("is_completed")
                                });
                            }// end of while
                        }// end of using
                    }// end of using
                }// end of using
            }// end of try
            catch { /* return whatever was collected so far */ }

            return tasks;

        }// end of method

        // ── mark a task as completed ─────────────────────────────────────
        public static bool CompleteTask(string username, int taskId)
        {// start of method
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"UPDATE tasks SET is_completed = TRUE
                                      WHERE task_id = @id AND username = @username;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.Parameters.AddWithValue("@username", username);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }// end of using
                }// end of using
            }// end of try
            catch
            {// start of catch
                return false;
            }// end of catch
        }// end of method

        // ── delete a task ─────────────────────────────────────────────
        public static bool DeleteTask(string username, int taskId)
        {// start of method
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {// start of using
                    conn.Open();

                    string query = @"DELETE FROM tasks WHERE task_id = @id AND username = @username;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {// start of using
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.Parameters.AddWithValue("@username", username);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }// end of using
                }// end of using
            }// end of try
            catch
            {// start of catch
                return false;
            }// end of catch
        }// end of method

    }// end of TaskAssistant class

}// end of namespace
