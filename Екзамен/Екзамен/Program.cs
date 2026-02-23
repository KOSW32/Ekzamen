using System;

class Program
{
    static void Main()
    {
        DictionaryManager manager = new DictionaryManager();

        while (true)
        {
            Console.WriteLine("\n=== СЛОВНИКИ ===");
            Console.WriteLine("1. Створити словник");
            Console.WriteLine("2. Додати слово");
            Console.WriteLine("3. Замінити слово");
            Console.WriteLine("4. Видалити слово");
            Console.WriteLine("5. Пошук слова");
            Console.WriteLine("6. Експорт слова");
            Console.WriteLine("0. Вихід");

            string choice = Console.ReadLine() ?? "";

            if (choice == "0")
                break;

            Console.Write("Назва словника: ");
            string name = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    manager.CreateDictionary(name);
                    break;

                case "2":
                    Console.Write("Слово: ");
                    string word = Console.ReadLine() ?? "";

                    Console.Write("Переклад: ");
                    string translation = Console.ReadLine() ?? "";

                    manager.AddWord(name, word, translation);
                    break;

                case "3":
                    Console.Write("Старе слово: ");
                    string oldWord = Console.ReadLine() ?? "";

                    Console.Write("Нове слово: ");
                    string newWord = Console.ReadLine() ?? "";

                    manager.ReplaceWord(name, oldWord, newWord);
                    break;

                case "4":
                    Console.Write("Слово для видалення: ");
                    string deleteWord = Console.ReadLine() ?? "";

                    manager.DeleteWord(name, deleteWord);
                    break;

                case "5":
                    Console.Write("Слово для пошуку: ");
                    string searchWord = Console.ReadLine() ?? "";

                    manager.Search(name, searchWord);
                    break;

                case "6":
                    Console.Write("Слово для експорту: ");
                    string exportWord = Console.ReadLine() ?? "";

                    manager.ExportWord(name, exportWord);
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}