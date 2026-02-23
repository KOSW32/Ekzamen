using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class DictionaryManager
{
    private readonly string folderPath = "Data";

    public DictionaryManager()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    private string GetPath(string name)
    {
        return Path.Combine(folderPath, name + ".json");
    }

    public void CreateDictionary(string name)
    {
        string path = GetPath(name);

        if (File.Exists(path))
        {
            Console.WriteLine("Словник вже існує.");
            return;
        }

        File.WriteAllText(path, JsonSerializer.Serialize(new List<WordEntry>()));
        Console.WriteLine("Словник створено.");
    }

    private List<WordEntry> Load(string name)
    {
        string path = GetPath(name);

        if (!File.Exists(path))
            return new List<WordEntry>();

        string json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
            return new List<WordEntry>();

        var data = JsonSerializer.Deserialize<List<WordEntry>>(json);
        return data ?? new List<WordEntry>();
    }

    private void Save(string name, List<WordEntry> words)
    {
        string json = JsonSerializer.Serialize(words, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(GetPath(name), json);
    }

    public void AddWord(string name, string word, string translation)
    {
        var words = Load(name);

        var entry = words.Find(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry == null)
        {
            entry = new WordEntry { Word = word };
            words.Add(entry);
        }

        if (!entry.Translations.Contains(translation))
            entry.Translations.Add(translation);

        Save(name, words);
        Console.WriteLine("Слово додано.");
    }

    public void ReplaceWord(string name, string oldWord, string newWord)
    {
        var words = Load(name);
        var entry = words.Find(w => w.Word == oldWord);

        if (entry == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        entry.Word = newWord;
        Save(name, words);
        Console.WriteLine("Слово замінено.");
    }

    public void DeleteWord(string name, string word)
    {
        var words = Load(name);
        words.RemoveAll(w => w.Word == word);
        Save(name, words);
        Console.WriteLine("Слово видалено.");
    }

    public void Search(string name, string word)
    {
        var words = Load(name);
        var entry = words.Find(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        Console.WriteLine("Переклади:");
        foreach (var t in entry.Translations)
            Console.WriteLine("- " + t);
    }

    public void ExportWord(string name, string word)
    {
        var words = Load(name);
        var entry = words.Find(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        string exportPath = $"{word}_export.txt";
        File.WriteAllLines(exportPath, entry.Translations);
        Console.WriteLine("Експортовано у файл: " + exportPath);
    }
}