namespace TP1;

public static class GestionNotes
{
    public static void Lancer()
    {   //initialisation du dictionnaire
        Dictionary<string, List<double>> dico = new()
        {
            ["Lucas"] = [14.5, 16, 15],
            ["Léna"] = [9, 11.5, 10],
            ["Sarah"] = [17, 18.5, 16.5]
        };

        // Ajout d'une note
        dico["Lucas"].Add(18);
        //Parcours le dictionnaire
        foreach (var (nom, notes) in dico)
        {
            double somme = 0;
            double min = notes[0];
            double max = notes[0];
        //Calculs du nombre de notes
            foreach (double note in notes)
            {
                somme += note; //Somme des notes
                if (note < min)
                    min = note;
                if (note > max)
                    max = note;
            }
        //Calculs de la moyenne
            double moyenne = somme / notes.Count;
            
            // AFFICHAGE
            Console.WriteLine($"{nom}");
            Console.WriteLine($"Note minimale : {min}");
            Console.WriteLine($"Note maximale : {max}");
            Console.WriteLine($"Moyenne : {moyenne:F2}");
            Console.WriteLine($"Mention : {Mention(moyenne)}");
            Console.WriteLine("");
        }
    }
    //retourne une mention par rapport à la moyenne
    public static string Mention(double moyenne) => moyenne switch
    {
        < 0 or > 20 => throw new ArgumentOutOfRangeException(nameof(moyenne), "La moyenne doit être comprise entre 0 et 20."),
        >= 16 => "Tres bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Non admis"
    };
}