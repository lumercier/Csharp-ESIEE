public static class CalculateurAge
{
    public static void Lancer()
    {
        Console.Write("Entrez votre prénom : ");
        string prenom = Console.ReadLine()!;

        while (true)
        {
            Console.Write("Entrez votre année de naissance : ");
            string? saisie = Console.ReadLine();

            if (int.TryParse(saisie, out int anneeNaissance))
            {
                int age = DateTime.Now.Year - anneeNaissance;

                if (age >= 0 && age <= 120)
                {
                    Console.WriteLine($"{prenom}, vous avez {age} ans.");
                    break;
                }
            }

            Console.WriteLine("Saisie invalide. Veuillez entrer une année cohérente (âge entre 0 et 120 ans).");
        }
    }
}
