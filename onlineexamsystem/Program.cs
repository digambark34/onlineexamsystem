using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Score { get; set; } = 0;
    public bool IsAdmin { get; set; }
    public List<int> PastScores { get; set; } = new List<int>();
}

public class Question
{
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public int CorrectOption { get; set; }
    public string Category { get; set; }
    public string Difficulty { get; set; }
    public string Explanation { get; set; }

    public void Display()
    {
        Console.WriteLine(Text);
        for (int i = 0; i < Options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Options[i]}");
        }
    }
}

public class ExamSystem
{
    private List<Question> QuestionBank = new List<Question>();

    private List<User> Users = new List<User>
    {
        new User { Username = "admin", Password = "admin123", IsAdmin = true },
        new User { Username = "student1", Password = "password1", IsAdmin = false },
        new User { Username = "student2", Password = "password2", IsAdmin = false }
    };

    private User CurrentUser;
    private int ExamTimeLimitInMinutes = 30;

    public void Run()
    {
        Console.WriteLine("Welcome to the Enhanced Online Exam System!");
        AuthenticateUser();

        if (CurrentUser.IsAdmin)
        {
            AdminMenu();
        }
        else
        {
            StudentMenu();
        }
    }

    private void AuthenticateUser()
    {
        while (true)
        {
            Console.WriteLine("\n--- Login or Register ---");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                CurrentUser = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (CurrentUser != null)
                {
                    Console.WriteLine($"\nLogin successful! Welcome, {CurrentUser.Username}.");
                    break;
                }
                else
                {
                    Console.WriteLine("\nInvalid credentials. Please try again.");
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine("\n--- Registration ---");
                Console.Write("Enter a new Username: ");
                string username = Console.ReadLine();

                if (Users.Any(u => u.Username == username))
                {
                    Console.WriteLine("Username already exists. Please choose another.");
                    continue;
                }

                Console.Write("Enter a Password: ");
                string password = Console.ReadLine();

                Users.Add(new User { Username = username, Password = password, IsAdmin = false });
                Console.WriteLine("Registration successful. You can now log in.");
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }

    private void AdminMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. View All Questions");
            Console.WriteLine("2. Add a New Question");
            Console.WriteLine("3. Approve Pending Questions");
            Console.WriteLine("4. Remove a Question");
            Console.WriteLine("5. Export Results as CSV");
            Console.WriteLine("6. Logout");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllQuestions();
                    break;

                case "2":
                    AddQuestion();
                    break;

                case "3":
                    ApproveQuestions();
                    break;

                case "4":
                    RemoveQuestion();
                    break;

                case "5":
                    ExportResultsAsCSV();
                    break;

                case "6":
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void StudentMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Student Menu ---");
            Console.WriteLine("1. Start Exam");
            Console.WriteLine("2. View Progress");
            Console.WriteLine("3. View Leaderboard");
            Console.WriteLine("4. Logout");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartExam();
                    break;

                case "2":
                    ViewProgress();
                    break;

                case "3":
                    DisplayLeaderboard();
                    break;

                case "4":
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void StartExam()
    {
        Console.WriteLine("\nChoose a category (e.g., Math, Science):");
        string category = Console.ReadLine();

        var selectedQuestions = QuestionBank
            .Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(q => Guid.NewGuid())
            .ToList();

        if (!selectedQuestions.Any())
        {
            Console.WriteLine("No questions available in the selected category.");
            return;
        }

        Console.WriteLine($"\nStarting the exam in {category} category...");
        int score = 0;

        foreach (var question in selectedQuestions)
        {
            question.Display();
            Console.Write("Your answer (1-4): ");

            var startTime = DateTime.Now;
            if (!int.TryParse(Console.ReadLine(), out int answer) || (DateTime.Now - startTime).TotalSeconds > 30)
            {
                Console.WriteLine("Time's up or invalid input! Moving to the next question.");
                continue;
            }

            if (answer == question.CorrectOption)
            {
                Console.WriteLine("Correct!");
                score++;
            }
            else
            {
                Console.WriteLine("Wrong! Correct answer: " + question.Options[question.CorrectOption - 1]);
                Console.WriteLine($"Explanation: {question.Explanation}");
            }
        }

        CurrentUser.PastScores.Add(score);
        CurrentUser.Score = score;
        Console.WriteLine($"\nExam finished. Your score: {score}/{selectedQuestions.Count}");
    }

    private void ViewProgress()
    {
        Console.WriteLine("\n--- Progress Report ---");
        if (!CurrentUser.PastScores.Any())
        {
            Console.WriteLine("No exams taken yet.");
        }
        else
        {
            Console.WriteLine("Scores from past exams: " + string.Join(", ", CurrentUser.PastScores));
            Console.WriteLine($"Average Score: {CurrentUser.PastScores.Average():F2}");
        }
    }

    private void DisplayLeaderboard()
    {
        Console.WriteLine("\n--- Leaderboard ---");
        var sortedUsers = Users.Where(u => !u.IsAdmin).OrderByDescending(u => u.Score).ToList();
        foreach (var user in sortedUsers)
        {
            Console.WriteLine($"{user.Username}: {user.Score} points");
        }
    }

    private void ExportResultsAsCSV()
    {
        Console.WriteLine("\nEnter the path to save results as CSV:");
        string filePath = Console.ReadLine();
        try
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Username, Score");
                foreach (var user in Users.Where(u => !u.IsAdmin))
                {
                    writer.WriteLine($"{user.Username}, {user.Score}");
                }
            }
            Console.WriteLine("Results exported successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error exporting results: " + ex.Message);
        }
    }

    private void ApproveQuestions()
    {
        Console.WriteLine("\nNo pending questions to approve.");
    }

    private void AddQuestion()
    {
        Console.WriteLine("\nEnter the question text:");
        string text = Console.ReadLine();

        Console.WriteLine("Enter 4 options (one per line):");
        List<string> options = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Option {i + 1}: ");
            options.Add(Console.ReadLine());
        }

        Console.WriteLine("Enter the correct option number (1-4):");
        int correctOption = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the category for this question (e.g., Math, Science):");
        string category = Console.ReadLine();

        Console.WriteLine("Enter the difficulty level (Easy, Medium, Hard):");
        string difficulty = Console.ReadLine();

        Console.WriteLine("Provide an explanation for the correct answer:");
        string explanation = Console.ReadLine();

        QuestionBank.Add(new Question { Text = text, Options = options, CorrectOption = correctOption, Category = category, Difficulty = difficulty, Explanation = explanation });
        Console.WriteLine("Question added successfully.");
    }

    private void ViewAllQuestions()
    {
        if (!QuestionBank.Any())
        {
            Console.WriteLine("No questions available in the question bank.");
            return;
        }

        Console.WriteLine("\n--- Question Bank ---");
        for (int i = 0; i < QuestionBank.Count; i++)
        {
            var question = QuestionBank[i];
            Console.WriteLine($"{i + 1}. {question.Text}");
            Console.WriteLine($"Category: {question.Category}, Difficulty: {question.Difficulty}, Correct Option: {question.CorrectOption}");
        }
    }

    private void RemoveQuestion()
    {
        ViewAllQuestions();
        Console.WriteLine("\nEnter the number of the question to remove:");
        if (int.TryParse(Console.ReadLine(), out int questionNumber) && questionNumber > 0 && questionNumber <= QuestionBank.Count)
        {
            QuestionBank.RemoveAt(questionNumber - 1);
            Console.WriteLine("Question removed successfully.");
        }
        else
        {
            Console.WriteLine("Invalid question number.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        ExamSystem system = new ExamSystem();
        system.Run();
    }
}