using System;
using System.Collections;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dynabot
{// start of namespace

    public partial class MainWindow : Window
    {// start of MainWindow class

        // ── ArrayLists for answers and ignored words ──────────────────
        ArrayList reply  = new ArrayList();
        ArrayList ignore = new ArrayList();

        // ── memory: store username and favourite topic ────────────────
        private string username       = string.Empty;
        private string favouriteTopic = string.Empty;
        private string lastTopic      = string.Empty;

        // ── random selector for varied responses ──────────────────────
        private Random indexer = new Random();

        // ── constructor ───────────────────────────────────────────────
        public MainWindow()
        {// start of constructor

            InitializeComponent();

            // populate reply and ignore lists via respond class
            new respond(reply, ignore);

            // load logo into both screens
            LoadLogo();

            // play voice greeting on startup
            PlayVoiceGreeting();

        }// end of constructor

        // ── load logo.png relative to the exe ────────────────────────
        private void LoadLogo()
        {// start of method
            try
            {
                string logoPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "logo.png");

                if (File.Exists(logoPath))
                {// start of if
                    BitmapImage bitmap  = new BitmapImage(new Uri(logoPath));
                    LogoImage.Source    = bitmap;
                    HeaderLogo.Source   = bitmap;
                }// end of if
            }// end of try
            catch { /* logo not found, continue without it */ }
        }// end of method

        // ── play greeting.wav relative to the exe ────────────────────
        private void PlayVoiceGreeting()
        {// start of method
            try
            {
                string wavPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(wavPath))
                {// start of if
                    SoundPlayer player = new SoundPlayer(wavPath);
                    player.Load();
                    player.Play();
                }// end of if
            }// end of try
            catch { /* audio not found, continue without it */ }
        }// end of method

        // ── enter key on the username textbox ────────────────────────
        private void username_KeyDown(object sender, KeyEventArgs e)
        {// start of method
            if (e.Key == Key.Enter)
            {// start of if
                submit_name(sender, e);
            }// end of if
        }// end of method

        // ── submit name button ────────────────────────────────────────
        private void submit_name(object sender, RoutedEventArgs e)
        {// start of method

            string filename = "user_names.txt";

            // create the file if it does not exist yet
            if (!File.Exists(filename))
            {// start of if
                File.AppendAllText(filename, "auto create\n");
            }// end of if

            string name  = user_name.Text.Trim();

            // validate: do not allow an empty name
            if (string.IsNullOrWhiteSpace(name))
            {// start of if
                MessageBox.Show(
                    "Please enter your name before continuing.",
                    "DynaBot", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }// end of if

            // check if user has visited before
            bool found = check_name(name);

            // store username in memory
            username = name;

            if (!found)
            {// start of if - new user
                File.AppendAllText(filename, name + "\n");
                MessageBox.Show("Welcome, " + name + "!",
                    "DynaBot", MessageBoxButton.OK, MessageBoxImage.Information);
            }// end of if
            else
            {// start of else - returning user
                MessageBox.Show("Welcome back, " + name + "!",
                    "DynaBot", MessageBoxButton.OK, MessageBoxImage.Information);
            }// end of else

            // switch from name screen to chat screen
            name_grid.Visibility = Visibility.Hidden;
            Chat_Grid.Visibility = Visibility.Visible;

            // update memory display in header
            UpdateMemoryIndicator();

            // opening bot message
            AddBotMessage("══════════════════════════════════════");
            AddBotMessage("Hey " + username + "! I'm DynaBot, your cybersecurity assistant. 🔒");
            AddBotMessage("Ask me about: passwords, phishing, scams, privacy, malware, vpn, wifi, 2fa and more.");
            AddBotMessage("══════════════════════════════════════");

        }// end of method

        // ── check if name exists in the text file ────────────────────
        private Boolean check_name(string name)
        {// start of method

            string   filename   = "user_names.txt";
            bool     name_found = false;
            string[] names      = File.ReadAllLines(filename);

            // loop through each saved name
            foreach (string search_name in names)
            {// start of foreach
                if (search_name.ToLower() == name.ToLower())
                {// start of if
                    name_found = true;
                }// end of if
            }// end of foreach

            return name_found;

        }// end of method

        // ── enter key on the question textbox ────────────────────────
        private void question_KeyDown(object sender, KeyEventArgs e)
        {// start of method
            if (e.Key == Key.Enter)
            {// start of if
                send(sender, e);
            }// end of if
        }// end of method

        // ── send button / main chat processor ────────────────────────
        private void send(object sender, RoutedEventArgs e)
        {// start of send method

            string questions = question.Text.Trim();

            // error handling: empty input
            if (string.IsNullOrWhiteSpace(questions))
            {// start of if
                error_method();
                return;
            }// end of if

            // show the user message in the chat
            AddUserMessage(questions);
            question.Text = string.Empty;
            question.Focus();

            string lower = questions.ToLower();

            // ── conversation flow: follow-up detection ────────────────
            if (IsFollowUp(lower))
            {// start of if
                if (!string.IsNullOrEmpty(lastTopic))
                {// start of inner if
                    AddBotMessage("Sure! Here's another tip on " + lastTopic + ":");
                    AddBotMessage(GetResponseForKeyword(lastTopic));
                }// end of inner if
                else
                {// start of else
                    AddBotMessage("Could you remind me what topic you'd like more info on?");
                }// end of else
                return;
            }// end of if

            // ── memory: detect favourite topic interest ───────────────
            if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("favourite"))
            {// start of if
                string[] topics = {
                    "password", "phishing", "scam", "privacy", "malware",
                    "vpn", "wifi", "2fa", "firewall", "fraud", "hacked"
                };

                foreach (string keyword in topics)
                {// start of foreach
                    if (lower.Contains(keyword))
                    {// start of inner if
                        favouriteTopic = keyword;
                        UpdateMemoryIndicator();
                        AddBotMessage("Great! I'll remember that you're interested in " + keyword + ", " + username + ".");
                        AddBotMessage(GetResponseForKeyword(keyword));
                        lastTopic = keyword;
                        return;
                    }// end of inner if
                }// end of foreach
            }// end of if

            // ── main word-by-word search using respond.cs logic ──────
            string[]  words         = lower.Split(' ');
            bool      found         = false;
            ArrayList per_word      = new ArrayList();
            ArrayList answers_found = new ArrayList();

            foreach (string word in words)
            {// start of foreach - iterate per word
                if (!ignore.Contains(word))
                {// start of if - word is not ignored
                    per_word.Clear();

                    foreach (string answer in reply)
                    {// start of foreach - search all answers
                        if (answer.Contains(word))
                        {// start of if - match found
                            found = true;
                            per_word.Add(answer);
                        }// end of if
                    }// end of foreach

                    // pick one random answer per matched word
                    if (found && per_word.Count > 0)
                    {// start of if
                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);

                        // track last topic for conversation flow
                        lastTopic = word;
                    }// end of if

                }// end of if
            }// end of foreach

            if (found)
            {// start of if - answers were found
                foreach (string per_answer in answers_found)
                {// start of foreach
                    // strip keyword prefix before displaying
                    string display = StripKeyword(per_answer.ToString());

                    // personalise if favourite topic is set
                    if (!string.IsNullOrEmpty(favouriteTopic) && lastTopic != favouriteTopic)
                    {// start of if
                        AddBotMessage("As someone interested in " + favouriteTopic +
                            ", here is also something useful about " + lastTopic + ":");
                    }// end of if

                    AddBotMessage(display);
                }// end of foreach
            }// end of if
            else
            {// start of else - no match
                error_method();
            }// end of else

        }// end of send method

        // ── check if input is a follow-up phrase ─────────────────────
        private bool IsFollowUp(string input)
        {// start of method
            string[] followUps = {
                "tell me more", "give me more", "another tip",
                "explain more", "more info", "go on", "continue"
            };

            foreach (string phrase in followUps)
            {// start of foreach
                if (input.Contains(phrase)) return true;
            }// end of foreach

            return false;
        }// end of method

        // ── get a response directly for a known keyword ───────────────
        private string GetResponseForKeyword(string keyword)
        {// start of method
            ArrayList matches = new ArrayList();

            foreach (string answer in reply)
            {// start of foreach
                if (answer.StartsWith(keyword))
                {// start of if
                    matches.Add(answer);
                }// end of if
            }// end of foreach

            if (matches.Count > 0)
            {// start of if
                int i = indexer.Next(0, matches.Count);
                return StripKeyword(matches[i].ToString());
            }// end of if

            return "I don't have a specific tip on that yet, but always stay cautious online!";
        }// end of method

        // ── strip the keyword prefix from an answer string ───────────
        private string StripKeyword(string answer)
        {// start of method

            // answers are stored as "keyword response text"
            // find the first space and return everything after it capitalised
            int spaceIndex = answer.IndexOf(' ');
            if (spaceIndex >= 0 && spaceIndex < answer.Length - 1)
            {// start of if
                string rest = answer.Substring(spaceIndex + 1);
                return char.ToUpper(rest[0]) + rest.Substring(1);
            }// end of if

            return answer;
        }// end of method

        // ── error / default response ──────────────────────────────────
        private void error_method()
        {// start of method

            TextBlock block      = new TextBlock();
            block.TextWrapping   = TextWrapping.Wrap;
            block.Margin         = new Thickness(0, 2, 0, 2);

            // "DynaBot :" label in cyan
            Run label = new Run("DynaBot : ")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 212, 255)),
                FontWeight = FontWeights.Bold
            };

            // error message in red
            Run msg = new Run(
                "I'm not sure I understand. Can you try rephrasing? " +
                "Try asking about passwords, phishing, scams, privacy, malware, vpn, wifi, or 2fa.")
            {
                Foreground = Brushes.Tomato
            };

            block.Inlines.Add(label);
            block.Inlines.Add(msg);

            chats.Items.Add(block);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

            StatusBar.Text = "DynaBot could not find an answer.";

        }// end of method

        // ── add a bot message to the listview ────────────────────────
        private void AddBotMessage(string message)
        {// start of method

            TextBlock block    = new TextBlock();
            block.TextWrapping = TextWrapping.Wrap;
            block.Margin       = new Thickness(0, 2, 0, 2);

            // "DynaBot :" label in cyan
            Run label = new Run("DynaBot : ")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 212, 255)),
                FontWeight = FontWeights.Bold
            };

            // message text in light gray
            Run msg = new Run(message)
            {
                Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220))
            };

            block.Inlines.Add(label);
            block.Inlines.Add(msg);

            chats.Items.Add(block);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

            StatusBar.Text = "DynaBot responded.";

        }// end of method

        // ── add a user message to the listview ───────────────────────
        private void AddUserMessage(string message)
        {// start of method

            TextBlock block       = new TextBlock();
            block.TextWrapping    = TextWrapping.Wrap;
            block.Margin          = new Thickness(0, 2, 0, 2);
            block.TextAlignment   = TextAlignment.Right;

            // username label in green
            Run label = new Run(username + " : ")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 136)),
                FontWeight = FontWeights.Bold
            };

            // message in white
            Run msg = new Run(message)
            {
                Foreground = Brushes.White
            };

            block.Inlines.Add(label);
            block.Inlines.Add(msg);

            chats.Items.Add(block);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

            StatusBar.Text = "You sent a message.";

        }// end of method

        // ── update the memory indicator in the header ─────────────────
        private void UpdateMemoryIndicator()
        {// start of method
            string mem = "";
            if (!string.IsNullOrEmpty(username))       mem += "👤 " + username;
            if (!string.IsNullOrEmpty(favouriteTopic)) mem += "  |  ⭐ " + favouriteTopic;
            MemoryIndicator.Text = mem;
        }// end of method

    }// end of MainWindow class

}// end of namespace
