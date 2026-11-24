using System.Net.Http.Json;

namespace TP3;

public static class GeoApi
{
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

    public static async Task LancerAsync()
    {
        Console.Write("\nEntrez un code postal (ex: 35000, 95590, 99999) : ");
        string cp = Console.ReadLine()?.Trim() ?? "";
        
        if (string.IsNullOrEmpty(cp))
        {
            Console.WriteLine("Code postal vide.");
            return;
        }

        List<Commune> communes = await CommunesAsync(cp);

        if (communes.Count == 0)
        {
            Console.WriteLine($"{cp} -> Aucune commune trouvée.");
            return;
        }

        string detail = string.Join(", ", communes.Select(c => $"{c.Nom} ({c.Population:N0} hab.)"));
        Console.WriteLine($"{cp} -> {communes.Count} commune(s) : {detail}");



    }


    public static async Task CommunesEtudiantsAsync()
    {
        Console.WriteLine("\n=== Communes des inscrits ===");
        
        List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
            .Skip(1)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(','))
            .Select(c => new Inscrit(c[0].Trim(), c[1].Trim(), c[2].Trim(), c[3].Trim()))
            .ToList();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        try
        {
            var taches = inscrits.Select(async inscrit =>
            {
                List<Commune> communes = await CommunesAsync(inscrit.CodePostal, cts.Token);
                string ville = communes.FirstOrDefault()?.Nom ?? "commune inconnue";
                return $"{inscrit.Prenom} {inscrit.Nom} ({inscrit.Promo}) : {ville}";
            });

            string[] lignes = await Task.WhenAll(taches);

            foreach (string ligne in lignes)
            {
                Console.WriteLine($"  {ligne}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("[Timeout] Délai maximal de 3 secondes dépassé pour les requêtes API.");
        }

    }
}


