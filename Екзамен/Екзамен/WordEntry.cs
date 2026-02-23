using System.Collections.Generic;

public class WordEntry
{
    public string Word { get; set; } = string.Empty;
    public List<string> Translations { get; set; } = new List<string>();
}