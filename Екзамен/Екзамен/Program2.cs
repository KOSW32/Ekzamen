using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class User
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public DateTime BirthDate { get; set; }
}

class Question
{
    public string Text { get; set; } = "";
    public List<string> Answers { get; set; } = new List<string>();
    public List<int> Correct { get; set; } = new List<int>();
}

class Result
{
    public string Login { get; set; } = "";
    public int Score { get; set; }
}

class Program2
{
    static string usersFile = "users.json";
    static string resultsFile = "results.json";

    static void Main()
    {
        EnsureFiles();

        User currentUser = LoginMenu();

        while (true)
        {
            Console.WriteLine("\n1. Почати вікторину");
            Console.WriteLine("2. Мої результати");
            Console.WriteLine("3. Top-20");
            Console.WriteLine("4. Змінити пароль");
            Console.WriteLine("0. Вихід");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
                StartQuiz(currentUser);
            else if (choice == "2")
                ShowMyResults(currentUser);
            else if (choice == "3")
                ShowTop20();
            else if (choice == "4")
                ChangePassword(currentUser);
            else if (choice == "0")
                break;
        }
    }

    static void EnsureFiles()
    {
        if (!File.Exists(usersFile))
            File.WriteAllText(usersFile, "[]");

        if (!File.Exists(resultsFile))
            File.WriteAllText(resultsFile, "[]");
    }

    static User LoginMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Вхід");
            Console.WriteLine("2. Реєстрація");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine() ?? "";

                Console.Write("Пароль: ");
                string password = Console.ReadLine() ?? "";

                var users = LoadUsers();
                var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                    return user;

                Console.WriteLine("Невірні дані!");
            }
            else if (choice == "2")
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine() ?? "";

                var users = LoadUsers();

                if (users.Any(u => u.Login == login))
                {
                    Console.WriteLine("Логін вже існує!");
                    continue;
                }

                Console.Write("Пароль: ");
                string password = Console.ReadLine() ?? "";

                Console.Write("Дата народження (yyyy-mm-dd): ");
                DateTime birth;
                DateTime.TryParse(Console.ReadLine(), out birth);

                User newUser = new User
                {
                    Login = login,
                    Password = password,
                    BirthDate = birth
                };

                users.Add(newUser);
                SaveUsers(users);

                Console.WriteLine("Реєстрація успішна!");
            }
        }
    }

    static void StartQuiz(User user)
    {
        var questions = GenerateQuestions();
        int score = 0;

        foreach (var q in questions)
        {
            Console.WriteLine("\n" + q.Text);

            for (int i = 0; i < q.Answers.Count; i++)
                Console.WriteLine($"{i + 1}. {q.Answers[i]}");

            Console.Write("Відповідь (через кому): ");
            string input = Console.ReadLine() ?? "";

            var selected = new List<int>();

            foreach (var part in input.Split(','))
            {
                int number;
                if (int.TryParse(part.Trim(), out number))
                    selected.Add(number - 1);
            }

            if (selected.Count == q.Correct.Count &&
                !selected.Except(q.Correct).Any())
                score++;
        }

        Console.WriteLine($"\nВаш результат: {score}/20");

        var results = LoadResults();
        results.Add(new Result { Login = user.Login, Score = score });
        SaveResults(results);
    }

    static List<Question> GenerateQuestions()
    {
        var list = new List<Question>();

        for (int i = 1; i <= 20; i++)
        {
            list.Add(new Question
            {
                Text = $"Питання {i}: 2 + 2 = ?",
                Answers = new List<string> { "3", "4", "5" },
                Correct = new List<int> { 1 }
            });
        }

        return list;
    }

    static void ShowMyResults(User user)
    {
        var results = LoadResults()
            .Where(r => r.Login == user.Login)
            .OrderByDescending(r => r.Score)
            .ToList();

        Console.WriteLine("\nМої результати:");
        foreach (var r in results)
            Console.WriteLine($"Балів: {r.Score}");
    }

    static void ShowTop20()
    {
        var results = LoadResults()
            .OrderByDescending(r => r.Score)
            .Take(20)
            .ToList();

        Console.WriteLine("\n--- TOP 20 ---");

        int place = 1;
        foreach (var r in results)
        {
            Console.WriteLine($"{place}. {r.Login} - {r.Score}");
            place++;
        }
    }

    static void ChangePassword(User user)
    {
        Console.Write("Новий пароль: ");
        string newPass = Console.ReadLine() ?? "";

        var users = LoadUsers();
        var existing = users.First(u => u.Login == user.Login);
        existing.Password = newPass;

        SaveUsers(users);

        Console.WriteLine("Пароль змінено!");
    }

    static List<User> LoadUsers()
    {
        if (!File.Exists(usersFile))
            return new List<User>();

        string json = File.ReadAllText(usersFile);

        if (string.IsNullOrWhiteSpace(json))
            return new List<User>();

        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    static void SaveUsers(List<User> users)
    {
        File.WriteAllText(usersFile,
            JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
    }

    static List<Result> LoadResults()
    {
        if (!File.Exists(resultsFile))
            return new List<Result>();

        string json = File.ReadAllText(resultsFile);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Result>();

        return JsonSerializer.Deserialize<List<Result>>(json) ?? new List<Result>();
    }

    static void SaveResults(List<Result> results)
    {
        File.WriteAllText(resultsFile,
            JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
    }
}