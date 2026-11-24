namespace TP1;

public static class CalculateurAge
{
    public static void Lancer()
    {   //Saisie de l'utilisateur
        Console.Write("Entrez votre prénom : ");
        string prenom = Console.ReadLine()!;
        //Boucle while pour gèrer l'erreur de saisie invalide
        while (true)
        {
            Console.Write("Entrez votre année de naissance : ");
            string? saisie = Console.ReadLine();

            if (int.TryParse(saisie, out int anneeNaissance))
            {   //On récupère l'année actuelle pour effectuer le calcul
                int age = DateTime.Now.Year - anneeNaissance;
                //Calcul de l'age
                if (age >= 0 && age <= 120) //Si année de naissance valide
                {
                    Console.WriteLine($"{prenom}, vous avez {age} ans.");
                    break;
                }
            }
            //Message d'erreur si année de naissance invalide
            Console.WriteLine("Veuillez saisir une année cohérente (âge entre 0 et 120 ans).");
        }
    }
}

