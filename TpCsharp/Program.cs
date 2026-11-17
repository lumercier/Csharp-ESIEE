using Ecole;
using Geometrie;

Console.WriteLine("1 - Calculateur d'âge");
Console.WriteLine("2 - Gestion des notes");
Console.WriteLine("3 - Test classe Etudiant");
Console.WriteLine("4 - Géométrie");
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
        
    case "4":
        List<Forme> formes =
        [
            new Cercle(3),          // Aire ≈ 28,27
            new Rectangle(4, 5),    // Aire = 20,00
            new Carre(2)            // Aire = 4,00
        ];

        Console.WriteLine("=== Avant le tri ===");
        foreach (IDessinable f in formes)
        {
            f.Dessiner(); // Appelle Decrire() de façon polymorphique
        }

        // Fonctionne automatiquement grâce à IComparable<Forme> !
        formes.Sort();

        Console.WriteLine("\n=== Après formes.Sort() (triées par aire) ===");
        foreach (Forme f in formes)
        {
            f.Dessiner();
        }
        break;

    default:
        Console.WriteLine("Choix invalide.");
        break;
}
