using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace dynabot
{// start of namespace

    internal class DatabaseHelper
    {// start of DatabaseHelper class

        // ── connection string built from local config file ───────────
        private static string connectionString = null;

        // ── get (and cache) the connection string ─────────────────────
        public static string GetConnectionString()
        {// start of method

            if (connectionString != null)
            {// start of if
                return connectionString;
            }// end of if

            // path to the local-only config file (never committed to git)
            string configPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "db_config.txt");

            string password;

            if (File.Exists(configPath))
            {// start of if - read existing password
                password = File.ReadAllText(configPath).Trim();
            }// end of if
            else
            {// start of else - first run, ask the user
                password = PromptForPassword();
                File.WriteAllText(configPath, password);
            }// end of else

            connectionString =
                "Server=127.0.0.1;Port=3306;Database=dynabot_db;Uid=root;Pwd=" + password + ";";

            return connectionString;

        }// end of method

        // ── simple WPF prompt for the MySQL root password ──────────────
        private static string PromptForPassword()
        {// start of method

            // basic input box using a small inline window
            System.Windows.Window promptWindow = new System.Windows.Window
            {
                Title = "DynaBot - Database Setup",
                Width = 380,
                Height = 160,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                ResizeMode = System.Windows.ResizeMode.NoResize
            };

            System.Windows.Controls.StackPanel panel = new System.Windows.Controls.StackPanel
            {
                Margin = new System.Windows.Thickness(15)
            };

            panel.Children.Add(new System.Windows.Controls.TextBlock
            {
                Text = "Enter your local MySQL root password:",
                Margin = new System.Windows.Thickness(0, 0, 0, 8)
            });

            System.Windows.Controls.PasswordBox passwordBox = new System.Windows.Controls.PasswordBox
            {
                Margin = new System.Windows.Thickness(0, 0, 0, 12)
            };
            panel.Children.Add(passwordBox);

            System.Windows.Controls.Button okButton = new System.Windows.Controls.Button
            {
                Content = "Save & Continue",
                Width = 130,
                Height = 30
            };
            okButton.Click += (s, e) => promptWindow.Close();
            panel.Children.Add(okButton);

            promptWindow.Content = panel;
            promptWindow.ShowDialog();

            return passwordBox.Password;

        }// end of method

        // ── create the database tables if they do not exist yet ───────
        public static void InitializeDatabase()
        {// start of method
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
                {// start of using
                    conn.Open();

                    // ── tasks table ─────────────────────────────────────
                    string createTasksTable = @"
                        CREATE TABLE IF NOT EXISTS tasks (
                            task_id INT AUTO_INCREMENT PRIMARY KEY,
                            username VARCHAR(100) NOT NULL,
                            title VARCHAR(255) NOT NULL,
                            description VARCHAR(500),
                            reminder_date VARCHAR(100),
                            is_completed BOOLEAN DEFAULT FALSE,
                            date_created DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(createTasksTable, conn))
                    {// start of using
                        cmd.ExecuteNonQuery();
                    }// end of using

                    // ── activity log table ──────────────────────────────
                    string createLogTable = @"
                        CREATE TABLE IF NOT EXISTS activity_log (
                            log_id INT AUTO_INCREMENT PRIMARY KEY,
                            username VARCHAR(100) NOT NULL,
                            action_description VARCHAR(500) NOT NULL,
                            timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(createLogTable, conn))
                    {// start of using
                        cmd.ExecuteNonQuery();
                    }// end of using

                }// end of using
            }// end of try
            catch (Exception ex)
            {// start of catch
                System.Windows.MessageBox.Show(
                    "Could not connect to the database. Please check that MySQL is running " +
                    "and your password is correct.\n\nError: " + ex.Message,
                    "DynaBot - Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }// end of catch
        }// end of method

    }// end of DatabaseHelper class

}// end of namespace
