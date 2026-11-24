using System.Globalization;

namespace Ecole;

public static class GestionFichierNotes
{
    public static Note LireLigne(string ligne, int numero)
    {
        string[] champs = ligne.Split(',');

        if (champs.Length != 5)
        {
            throw new NoteInvalideException(numero, $"Nombre de champs invalide ({champs.Length} au lieu de 5).");
        }

        string nom = champs[0].Trim();
        string prenom = champs[1].Trim();
        string matiere = champs[2].Trim();

        if (!double.TryParse(champs[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double valeur))
        {
            throw new NoteInvalideException(numero, $"La note '{champs[3]}' n'est pas un nombre valide.");
        }

        if (valeur is < 0 or > 20)
        {
            throw new NoteInvalideException(numero, $"La note {valeur} est hors de l'intervalle [0, 20].");
        }

        if (!int.TryParse(champs[4], out int coef) || coef <= 0)
        {
            throw new NoteInvalideException(numero, $"Le coefficient '{champs[4]}' est invalide.");
        }

        return new Note(nom, prenom, matiere, valeur, coef);
    }

    public static void Lancer(string cheminFichier = "notes.csv")
    {
        Console.WriteLine("Test du record Note");
        var n1 = new Note("Lovelace", "Ada", "Algo", 15, 3);
        var n2 = new Note("Lovelace", "Ada", "Algo", 15, 3);

        Console.WriteLine($"n1 == n2 : {n1 == n2}");
        Console.WriteLine($"n1 : {n1}");

        var n3 = n1 with { Valeur = 18 };
        Console.WriteLine($"n3 (avec with) : {n3}\n");

        Console.WriteLine("Lecture de notes.csv");
        var promo = new Promo("M1 Info");

        try
        {
            using var reader = new StreamReader(cheminFichier);

            string? entete = reader.ReadLine();
            if (entete is null)
            {
                Console.Error.WriteLine("Le fichier est vide.");
                return;
            }

            int numeroLigne = 1;
            string? ligne;

            while ((ligne = reader.ReadLine()) is not null)
            {
                numeroLigne++;

                if (string.IsNullOrWhiteSpace(ligne))
                {
                    continue;
                }

                try
                {
                    Note note = LireLigne(ligne, numeroLigne);

                
                    Etudiant? etudiant = promo.Chercher(note.Nom);
                    if (etudiant is null)
                    {
                        etudiant = new Etudiant(note.Nom, note.Prenom, 4);
                        promo.Inscrire(etudiant);
                    }

                    etudiant.AjouterNote(note.Valeur);
                }
                catch (NoteInvalideException ex)
                {
                    Console.Error.WriteLine($"[Erreur ligne {ex.Ligne}] {ex.Message}");
                }
            }

            Console.WriteLine();
            promo.AfficherReleve();
        }
        catch (FileNotFoundException)
        {
            Console.Error.WriteLine($"Erreur : le fichier '{cheminFichier}' est introuvable.");
        }
    }
}