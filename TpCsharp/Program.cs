using Ecole;

Console.WriteLine("1 - Exercices précédents (âge)");
Console.WriteLine("2 - Gestion des notes");
Console.WriteLine("3 - Test classe Etudiant");
Console.Write("Votre choix : ");

string? choix = Console.ReadLine();

switch (choix)
{
    case "1":
        CalculateurAge.Lancer();
        break;
    case "2":
        GestionNotes.Lancer();
        break;
    case "3":
        var ada = new Etudiant("Lovelace", "Ada", 4)
        {
            Naissance = new DateOnly(1815, 12, 10)
        };
        ada.AjouterNote(15);
        ada.AjouterNote(16);
        Console.WriteLine(ada); // Affiche : Ada LOVELACE (M1) : 15,50
        ada.Annee = 5;          // Autorisé
        break;

    default:
        Console.WriteLine("Choix invalide.");
        break;
}
