using System.Globalization;

namespace TP2;

public static class Linq
{
    public static void Lancer()
    {
        var (notes, inscrits) = ChargerDonnees();

        Console.WriteLine($"{notes.Count} notes valides et {inscrits.Count} inscrits.\n");

        List<Note> notesAlgo = Filtrer(notes, EstEnAlgo);
        List<Note> notesEnDessousMoyenne = Filtrer(notes, n => n.Valeur < 10);

        Func<Note, bool> critereAda = n => n.Prenom == "Ada" && n.Valeur >= 15;
        List<Note> bonnesNotesAda = Filtrer(notes, critereAda);

        Console.WriteLine($"Notes en Algo : {notesAlgo.Count}");
        Console.WriteLine($"Notes inférieures à 10 : {notesEnDessousMoyenne.Count}");
        Console.WriteLine($"Notes d'Ada >= 15 : {bonnesNotesAda.Count}");


        //Points pondérés avec coefficient
        Func<Note, double> pointspond = n => n.Valeur*n.Coef;
        Action<Note> afficher = n => Console.WriteLine($"{n.Nom} - {n.Matiere} - {n.Valeur}");
        Console.WriteLine("Nom - Matière - Note");
        foreach (var n in notes)
        {
            afficher(n);
            Console.WriteLine($" Points pondérés avec coeff. : {pointspond(n)}");
        }

        //Filtrer les notes au dessus de 14
        int seuil = 14;
        List<Note> notesAuDessus = Filtrer(notes, AuDessusDe(seuil));
        Console.WriteLine($"Notes au-dessus de {seuil} : ({notesAuDessus.Count})");
        Console.WriteLine("Détails des notes");
        notesAuDessus.ForEach(afficher);

        //Copie de la liste et tri
        List<Note> copie = [.. notes];


        //par matière et valeur décroissante 
        copie.Sort((a, b) =>
        {
            int cmpMatiere = string.Compare(a.Matiere, b.Matiere, StringComparison.Ordinal);
            if (cmpMatiere != 0) return cmpMatiere;
            return b.Valeur.CompareTo(a.Valeur);
        });
        //Supprimer notes Réseau
        copie.RemoveAll(n => n.Matiere == "Reseau");
        //Vérification
        Console.WriteLine("\nCopie triée (sans Reseau) :");
        copie.ForEach(afficher);
    }

    private static List<Note> Filtrer(List<Note> source, Func<Note, bool> critere)
    {
        List<Note> resultat = [];

        foreach (Note note in source)
        {
            if (critere(note))
            {
                resultat.Add(note);
            }
        }

        return resultat;
    }


//METHODES

    private static Note? Parser(string ligne)
    {
        var c = ligne.Split(',');
        if (c.Length != 5) return null;
        if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var v)) return null;
        if (v < 0 || v > 20) return null;
        if (!int.TryParse(c[4], out var coef)) return null;
        return new Note(c[0], c[1], c[2], v, coef);
    }

    private static bool EstEnAlgo(Note n) => n.Matiere == "Algo";
    
    static Func<Note, bool> AuDessusDe(double seuil)
    {
        return n => n.Valeur > seuil;
    }

    public static (List<Note> Notes, List<Inscrit> Inscrits) ChargerDonnees()
    {
        List<Note> notes = File.ReadLines("notes.csv")
            .Skip(1)
            .Select((ligne, i) =>
            {
                Note? note = Parser(ligne);
                if (note is null)
                    Console.Error.WriteLine($"Ligne {i + 2} invalide : {ligne}");
                return note;
            })
            .OfType<Note>()
            .ToList();

        List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
            .Skip(1)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(','))
            .Select(c => new Inscrit(c[0].Trim(), c[1].Trim(), c[2].Trim(), c[3].Trim()))
            .ToList();


        return (notes, inscrits);
    }

}