public static class GestionNotes
{
    public static void Lancer()
    {        
        Dictionary<string, List<double>> dico = new()
        {
            ["Lucas"] = [14.5, 16, 15],
            ["Léna"] = [9, 11.5, 10],
            ["Sarah"] = [17, 18.5, 16.5]
        };

        // Ajout d'une note
        dico["Lucas"].Add(18);

        foreach (var (nom, notes) in dico)
        {
            double somme = 0;
            double min = notes[0];
            double max = notes[0];

            foreach (double note in notes)
            {
                somme += note;
                if (note < min)
                    min = note;
                if (note > max)
                    max = note;
            }

            double moyenne = somme / notes.Count;
            
            // AFFICHAGE
            Console.WriteLine($"{nom}");
            Console.WriteLine($"Note minimale : {min}");
            Console.WriteLine($"Note maximale : {max}");
            Console.WriteLine($"Moyenne : {moyenne:F2}");
            Console.WriteLine($"Mention : {Mention(moyenne)}");
            Console.WriteLine("");
        }

        Console.WriteLine("=== TESTS ===");
        object?[] valeursTest = [42, 7, "", "Bonjour", dico["Lucas"], null];

        foreach (object? valeur in valeursTest)
        {
            Console.WriteLine($"{valeur ?? "null"} -> {Decrire(valeur)}");
        }
    }

    public static string Mention(double moyenne) => moyenne switch
    {
        < 0 or > 20 => throw new ArgumentOutOfRangeException(nameof(moyenne), "La moyenne doit être comprise entre 0 et 20."),
        >= 16 => "Très bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Ajourné"
    };

    public static string Decrire(object? o) => o switch
    {
        null => "null",
        int n when n % 2 == 0 => "entier pair",
        int => "entier impair",
        string { Length: 0 } => "chaîne vide",
        string s => $"chaîne de {s.Length} caractères",
        List<double> liste => $"liste de {liste.Count} éléments",
        _ => "autre"
    };
}
