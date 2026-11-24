using TP2;
using System.Globalization;
using System.Text.Json;
using System.Diagnostics;
using System.Net.Http.Json;

namespace TP3;

public static class TpJson
{
    public static async Task LancerAsync()
    {
        Console.WriteLine("=== 3.1 Chargement et sauvegarde asynchrones ===");
        List<Note> notesPromo = await ChargerOuImporterAsync("promo.json", "notes.csv");
        Console.WriteLine($"Résultat : {notesPromo.Count} notes prêtes.");
        Console.WriteLine("\n=== Mesures de Task.WhenAll ===");

        var sw = Stopwatch.StartNew();
        string b1 = OperationLenteAsync("Opération 1").Result;
        string b2 = OperationLenteAsync("Opération 2").Result;
        string b3 = OperationLenteAsync("Opération 3").Result;

        sw.Stop();
        Console.WriteLine($"16. Avec .Result : {sw.ElapsedMilliseconds} ms ({b1}, {b2}, {b3})");
    }

    private static Note ParserStrict(string ligne, int numero)
{
    string[] c = ligne.Split(',');

    if (c.Length != 5)
        throw new FormatException($"Ligne {numero} : {c.Length} champ(s) au lieu de 5.");

    if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out double valeur))
        throw new FormatException($"Ligne {numero} : note non numérique ('{c[3]}').");

    if (!int.TryParse(c[4], out int coef))
        throw new FormatException($"Ligne {numero} : coefficient non entier ('{c[4]}').");

    // Si valeur hors [0..20] ou coef <= 0, le record Note lève ArgumentOutOfRangeException
    return new Note(c[0].Trim(), c[1].Trim(), c[2].Trim(), valeur, coef);
}

public static List<Note> ChargerNotesStrict(string chemin)
{
    List<Note> notes = [];
    int numero = 1;

    foreach (string ligne in File.ReadLines(chemin).Skip(1))
    {
        numero++;
        if (string.IsNullOrWhiteSpace(ligne)) continue;

        try
        {
            notes.Add(ParserStrict(ligne, numero));
        }
        catch (FormatException ex)
        {
            Console.Error.WriteLine($"[Rejetée] {ex.Message}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.Error.WriteLine($"[Rejetée] Ligne {numero} : {ex.ParamName} hors limites ({ex.Message})");
        }
    }

    return notes;
}

private static async Task<List<Note>> ChargerOuImporterAsync(string cheminJson, string cheminCsv)
{
    if (File.Exists(cheminJson))
    {
        try
        {
            Console.WriteLine($"Chargement asynchrone depuis {cheminJson}...");
            return await Stockage.ChargerAsync(cheminJson);
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"[Erreur JSON] {cheminJson} est corrompu ({ex.Message}).");
            Console.Error.WriteLine($"Réimportation depuis {cheminCsv}...");
        }
    }
    else
    {
        Console.WriteLine($"{cheminJson} introuvable. Importation depuis {cheminCsv}...");
    }

    List<Note> notes = ChargerNotesStrict(cheminCsv);
    await Stockage.SauverAsync(cheminJson, notes);
    Console.WriteLine($"{cheminJson} créé avec {notes.Count} notes.");
    return notes;
}
    private static async Task<string> OperationLenteAsync(string nom)
        {
            await Task.Delay(1000);
            return $"{nom} terminée";
        }
}