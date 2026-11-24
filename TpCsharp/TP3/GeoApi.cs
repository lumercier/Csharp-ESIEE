using System.Net.Http.Json;

namespace TP3;

public static class GeoApi
{
    //ADRESSE DE L'API 
    private static readonly HttpClient Http = new()
    {
        BaseAddress = new Uri("https://geo.api.gouv.fr/")
    };

    public static async Task<List<Commune>> CommunesAsync(string codePostal, CancellationToken ct = default)
    {
        try //API Joinable 
        {
            return await Http.GetFromJsonAsync<List<Commune>>(
                $"communes?codePostal={Uri.EscapeDataString(codePostal)}") ?? [];
        }
        catch //API Injoinable 
        {
            Console.Error.WriteLine($"Impossible de joindre l'API pour {codePostal}");
            return [];
        }
    }

    //RECHERCHE COMMUNE AVEC CODE POSTAL
    public static async Task LancerAsync()
    {
        //Entrée de l'utilisateur
        Console.Write("\nEntrez un code postal (5 chiffres) : ");
        string cp = Console.ReadLine()?.Trim() ?? "";
        //Si code postal vide
        if (string.IsNullOrEmpty(cp))
        {
            Console.WriteLine("Code postal vide.");
            return;
        }

        List<Commune> communes = await CommunesAsync(cp); //Appel asynchrone API
        //Si pas de commune
        if (communes.Count == 0)
        {
            Console.WriteLine($"{cp} -> Aucune commune trouvée pour ce code postal.");
            return;
        }
        //Assemblage du texte avec LINQ
        string detail = string.Join(", ", communes.Select(c => $"{c.Nom} ({c.Population:N0} hab.)"));
        //Affichage
        Console.WriteLine($"{cp} -> {communes.Count} commune(s) : {detail}");



    }

    //AFFICHER CP DES ETUDIANTS INSCRITS
    public static async Task CommunesEtudiantsAsync()
    {
        Console.WriteLine("\n=== Communes des inscrits ===");
        //Lecture du .csv
        List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
            .Skip(1)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(','))
            .Select(c => new Inscrit(c[0].Trim(), c[1].Trim(), c[2].Trim(), c[3].Trim()))
            .ToList();

        //Utilisation d'un token qui annule au bout 3 secondes sans réponse
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        try
        {   //Création de toutes les tâches pour les différentes communes des élèves
            var taches = inscrits.Select(async inscrit =>
            {
                List<Commune> communes = await CommunesAsync(inscrit.CodePostal, cts.Token);
                string ville = communes.FirstOrDefault()?.Nom ?? "commune inconnue";
                return $"{inscrit.Prenom} {inscrit.Nom} ({inscrit.Promo}) : {ville}";
            });
            //Lancement simultané de toutes les tâches et attente qu'elles soient terminées
            string[] lignes = await Task.WhenAll(taches);
            //Affichage des communes
            foreach (string ligne in lignes)
            {
                Console.WriteLine($"  {ligne}");
            }
        }
        //Activation du token si pas de réponse au bout de 3 secondes
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Délai maximal dépassé pour les requêtes API.");
        }

    }
}


