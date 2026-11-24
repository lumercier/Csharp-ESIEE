namespace Ecole;

public static class exoEcole
{
    public static void Lancer()
    {   //CREATION DE LA PROMO
        var promo = new Promo("ING3 Info");

        var ada = new Etudiant("Lovelace", "Ada", 4);
        ada.AjouterNote(15);
        ada.AjouterNote(17.5);

        var alan = new Etudiant("Turing", "Alan", 4);
        alan.AjouterNote(12);
        alan.AjouterNote(14);

        var grace = new Etudiant("Hopper", "Grace", 4);
        grace.AjouterNote(18);
        grace.AjouterNote(16.5);

        promo.Inscrire(ada);
        promo.Inscrire(alan);
        promo.Inscrire(grace);

        promo.AfficherReleve();

        Etudiant? major = promo.Major();
        if (major is not null)
        {
            Console.WriteLine($"\nMajor de promo : {major.NomComplet} ({major.Moyenne():F2})");
        }

        Console.WriteLine($"Moyenne générale : {promo.MoyenneGenerale():F2}");
    }
}