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
        //Récupération la liste des notes du .JSON ou alors du .csv
        List<Note> notesPromo = await ChargerOuImporterAsync("promo.json", "notes.csv");
        Console.WriteLine($"Résultat : {notesPromo.Count} notes prêtes.");
    }

    //VALIDAER LA SYNTAXE DU .CSV
    private static Note ParserStrict(string ligne, int numero)
    {
        string[] c = ligne.Split(','); //Séparation des lignes par des virgules
        //exactement 5 colonnes
        if (c.Length != 5) 
            throw new FormatException($"Ligne {numero} : {c.Length} champ(s) au lieu de 5.");
        //Converstion de la cologne 4 en type double
        if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out double valeur))
            throw new FormatException($"Ligne {numero} : note non numérique ('{c[3]}').");
        ////Converstion de la cologne 5 en type int
        if (!int.TryParse(c[4], out int coef))
            throw new FormatException($"Ligne {numero} : coefficient non entier ('{c[4]}').");

        //Note entre 0 et 20
        return new Note(c[0].Trim(), c[1].Trim(), c[2].Trim(), valeur, coef);
    }

    //LECTURE FICHIER .CSV
    public static List<Note> ChargerNotesStrict(string chemin)
    {
        List<Note> notes = [];
        int numero = 1;
        //Lecture ligne par ligne mais sauter la ligne d'entête (1)
        foreach (string ligne in File.ReadLines(chemin).Skip(1))
        {
            numero++;
            if (string.IsNullOrWhiteSpace(ligne)) continue; //ignore lignes vides

            try
            {
                notes.Add(ParserStrict(ligne, numero));
            }
            catch (FormatException ex) //Exception de format
            {
                Console.Error.WriteLine($"[Rejetée] {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex) //Exception nombre hors limite
            {
                Console.Error.WriteLine($"[Rejetée] Ligne {numero} : {ex.ParamName} hors limites ({ex.Message})");
            }
        }

        return notes;
    }

    //Stratégie de secours JSON -> CSV
    private static async Task<List<Note>> ChargerOuImporterAsync(string cheminJson, string cheminCsv)
    {
        if (File.Exists(cheminJson))
        {
            try //si promo.json existe, on le désérialise
            {
                Console.WriteLine($"Chargement asynchrone depuis {cheminJson}...");
                return await Stockage.ChargerAsync(cheminJson);
            }
            catch (JsonException ex) //si promo.json absent, on utilise le .csv
            {
                Console.Error.WriteLine($"[Erreur JSON] {cheminJson} est corrompu ({ex.Message}).");
                Console.Error.WriteLine($"Réimportation depuis {cheminCsv}...");
            }
        }
        //Sérialise .csv vers .json edans promo.json 
        List<Note> notes = ChargerNotesStrict(cheminCsv);
        await Stockage.SauverAsync(cheminJson, notes);
        return notes;
    }   
}