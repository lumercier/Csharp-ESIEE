namespace TP2;

public static class Requetes
{
    public static void Lancer()
    {
        var (notes, inscrits) = Linq.ChargerDonnees();

        Console.WriteLine("\n=== Filtrer, projeter, trier ===");
        
// Notes d'Algo triées par valeur décroissante
        var notesAlgoTriees = notes
            .Where(n => n.Matiere == "Algo")
            .OrderByDescending(n => n.Valeur);

        Console.WriteLine("Notes d'Algo (décroissant) :");
        foreach (var n in notesAlgoTriees)
            Console.WriteLine($"  {n.Prenom} : {n.Valeur:F2}");


// Matières triées sans doublons
        var matieres = notes
            .Select(n => n.Matiere)
            .Distinct()
            .Order();

        Console.WriteLine($"\n Matières : {string.Join(", ", matieres)}");

//Notes < 10 (anonyme puis tuple)
        var elimAnonyme = notes
            .Where(n => n.Valeur < 10)
            .Select(n => new { n.Prenom, n.Valeur });
        var elimTuple = notes
            .Where(n => n.Valeur < 10)
            .Select(n => (n.Prenom, n.Valeur));

        Console.WriteLine("\n Notes éliminatoires (< 10) :");
        foreach (var e in elimAnonyme)
            Console.WriteLine($"  [Anonyme] {e.Prenom} : {e.Valeur:F2}");
        foreach (var (prenom, valeur) in elimTuple)
            Console.WriteLine($"  [Tuple]   {prenom} : {valeur:F2}");

//Classement matière,valeur décroissante,prénom
        var classement = notes
            .OrderBy(n => n.Matiere)
            .ThenByDescending(n => n.Valeur)
            .ThenBy(n => n.Prenom);

        Console.WriteLine("\n Classement complet :");
        foreach (var n in classement)
            Console.WriteLine($"  {n.Matiere,-7} | {n.Valeur,5:F2} | {n.Prenom}");

//Top 3 et Top 6
        var parValeur = notes.OrderByDescending(n => n.Valeur);
        var top3 = parValeur.Take(3);
        var de4a6 = parValeur.Skip(3).Take(3);

        Console.WriteLine("\n Top 3 :");
        foreach (var n in top3)
            Console.WriteLine($"  {n.Prenom} ({n.Matiere}) : {n.Valeur:F2}");

        Console.WriteLine(" Top 6 :");
        foreach (var n in de4a6)
            Console.WriteLine($"  {n.Prenom} ({n.Matiere}) : {n.Valeur:F2}");



        Console.WriteLine("\n=== Agréger ===");

// Nombre de notes, notes < 10, moyenne simple
        int nbNotes = notes.Count;
        int nbEliminatoires = notes.Count(n => n.Valeur < 10);
        double moyenneSimple = notes.Average(n => n.Valeur);
        Console.WriteLine($"{nbNotes} notes, dont {nbEliminatoires} < 10 ; moyenne simple : {moyenneSimple:F2}");

// Max vs MaxBy
        double meilleureValeur = notes.Max(n => n.Valeur);
        Note? meilleureNote = notes.MaxBy(n => n.Valeur);
        Console.WriteLine($"Max : {meilleureValeur:F2} | MaxBy : {meilleureNote?.Prenom} ({meilleureNote?.Matiere}) -> {meilleureNote?.Valeur:F2}");

// Moyenne pondérée
        double moyennePonderee = notes.Sum(n => n.Valeur * n.Coef) / notes.Sum(n => n.Coef);
        Console.WriteLine($"Moyenne pondérée : {moyennePonderee:F2}");

// Prénoms distincts : Aggregate vs string.Join
        var prenomsDistincts = notes.Select(n => n.Prenom).Distinct();

        string avecAggregate = prenomsDistincts.Aggregate((acc, p) => $"{acc}, {p}");
        string avecJoin = string.Join(", ", prenomsDistincts);

        Console.WriteLine($"Aggregate : {avecAggregate}");
        Console.WriteLine($"   Join      : {avecJoin}");

        Console.WriteLine("\n=== Chercher et tester ===");

// Note de Linus en BD
        Note? linusBd = notes.FirstOrDefault(n => n.Prenom == "Linus" && n.Matiere == "BD");
        Console.WriteLine($"FirstOrDefault (Linus en BD) : {linusBd?.ToString() ?? "null"}");

        try
        {
            Note linusBdFirst = notes.First(n => n.Prenom == "Linus" && n.Matiere == "BD");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"   First (Linus en BD) -> Exception : {ex.Message}");
        }

// Single sur Ada en Algo, puis toutes les notes
        Note adaAlgo = notes.Single(n => n.Prenom == "Ada" && n.Matiere == "Algo");
        Console.WriteLine($"Single (Ada en Algo) : {adaAlgo.Valeur:F2}");

        try
        {
            Note adaToutes = notes.Single(n => n.Prenom == "Ada");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"   Single (toutes les notes d'Ada) -> Exception : {ex.Message}");
        }

// Any, All et séquence vide
        bool existeEliminatoire = notes.Any(n => n.Valeur < 10);
        bool tousAuMoins12EnBd = notes.Where(n => n.Matiere == "BD").All(n => n.Valeur >= 12);
        bool allSurVide = Enumerable.Empty<Note>().All(n => n.Valeur >= 12);

        Console.WriteLine($"{existeEliminatoire}");
        Console.WriteLine($"{tousAuMoins12EnBd}");
        Console.WriteLine($"{allSurVide}");

        Console.WriteLine("\n=== GroupBy ===");

// Regrouper les notes par matière
    var parMatiere = notes.GroupBy(n => n.Matiere);

    Console.WriteLine("Notes par matière :");
    foreach (IGrouping<string, Note> groupe in parMatiere)
    {
        Console.WriteLine($"  [{groupe.Key}]");
        foreach (Note n in groupe)
        {
            Console.WriteLine($"    - {n.Prenom} : {n.Valeur:F2} (coef {n.Coef})");
        }
    }

// Statistiques par matière et par moyenne décroissante
    var statsParMatiere = notes
        .GroupBy(n => n.Matiere)
        .Select(g => new
        {
            Matiere = g.Key,
            NbNotes = g.Count(),
            Moyenne = g.Average(n => n.Valeur),
            Min = g.Min(n => n.Valeur),
            Max = g.Max(n => n.Valeur),
            Major = g.MaxBy(n => n.Valeur)?.Prenom ?? "?"
        })
        .OrderByDescending(s => s.Moyenne);

    Console.WriteLine("\n Statistiques par matière (moyenne décroissante) :");
    foreach (var s in statsParMatiere)
    {
        Console.WriteLine($"  {s.Matiere,-7} | {s.NbNotes} notes | moy: {s.Moyenne:F2} | min: {s.Min:F2} | max: {s.Max:F2} | major: {s.Major}");
    }

// Moyenne pondéree par étudiant
    Dictionary<string, double> moyennesPonderees = notes
        .GroupBy(n => n.Prenom)
        .ToDictionary(
            g => g.Key,
            g => g.Sum(n => n.Valeur * n.Coef) / g.Sum(n => n.Coef)
        );

    Console.WriteLine("\n Moyennes pondérées par étudiant :");
    foreach (var (prenom, moy) in moyennesPonderees)
    {
        Console.WriteLine($"  {prenom,-8} : {moy:F2}");
    }

// Compter les notes par étudiant
    var nbParCountBy = notes.CountBy(n => n.Prenom);
    var nbParGroupBy = notes.GroupBy(n => n.Prenom).Select(g => (Prenom: g.Key, Nb: g.Count()));

    Console.WriteLine("\n Nombre de notes par étudiant :");
    foreach (var (prenom, nb) in nbParGroupBy)
        Console.WriteLine($"     {prenom,-8} : {nb}");


    Console.WriteLine("\n=== 3.2 Join ===");

// Jointure notes <-> inscrits sur clé composite (Nom, Prenom)
var releve = notes
    .Join(
        inscrits,
        n => (n.Nom, n.Prenom),
        i => (i.Nom, i.Prenom),
        (n, i) => new { i.Prenom, i.Promo, n.Matiere, n.Valeur }
    )
    .ToList();

    Console.WriteLine("1. Relevé complet :");
    foreach (var r in releve)
    {
        Console.WriteLine($"  {r.Prenom,-8} ({r.Promo}) | {r.Matiere,-7} : {r.Valeur:F2}");
    }

// Moyenne par promo
    var moyennesParPromo = releve
        .GroupBy(r => r.Promo)
        .Select(g => new { Promo = g.Key, Moyenne = g.Average(r => r.Valeur) });

    Console.WriteLine("\n Moyenne par promo :");
    foreach (var p in moyennesParPromo)
    {
        Console.WriteLine($"  {p.Promo} : {p.Moyenne:F2}");
    }

// Inscrits sans aucune note
    // Façon A : Where + Any
    var sansNoteWhere = inscrits
        .Where(i => !notes.Any(n => n.Nom == i.Nom && n.Prenom == i.Prenom));

    Console.WriteLine($"\n Inscrits sans note : {string.Join(", ", sansNoteWhere.Select(i => $"{i.Prenom} {i.Nom}"))}");

// Liste des bulletins
    List<Bulletin> bulletins = notes
        .GroupBy(n => n.Prenom)
        .Select(g => new Bulletin(g.Key, g.Select(n => n.Valeur).ToList()))
        .ToList();

    Console.WriteLine("Bulletins :");
    foreach (var b in bulletins)
    {
        Console.WriteLine($"  {b.Prenom,-8} : {string.Join(", ", b.Valeurs.Select(v => $"{v:F2}"))}");
    }

// Toutes les valeurs avec SelectMany
    IEnumerable<double> toutesLesValeurs = bulletins.SelectMany(b => b.Valeurs);
    Console.WriteLine($"\n Toutes les valeurs à plat ({toutesLesValeurs.Count()}) : {string.Join(", ", toutesLesValeurs)}");

// resultSelector par paires (prénom, valeur)
    var paires = bulletins.SelectMany(
        b => b.Valeurs,
        (bulletin, valeur) => (bulletin.Prenom, Valeur: valeur)
    );

    Console.WriteLine("\n Paires (prénom, valeur) :");
    foreach (var (prenom, valeur) in paires)
    {
        Console.WriteLine($"  ({prenom}, {valeur:F2})");
    }

    Console.WriteLine("\n=== L'évaluation différée ===");

    // Copie puis différé vs matérialisé
    List<Note> notesTest = [.. notes];

    var bonnes = notesTest.Where(n => n.Valeur >= 15);
    Console.WriteLine($"1. Avant ajout        : {bonnes.Count()}");

    notesTest.Add(new Note("Hamilton", "Margaret", "Algo", 19, 3));
    Console.WriteLine($"   Après ajout 19     : {bonnes.Count()}");

    var figees = notesTest.Where(n => n.Valeur >= 15).ToList();
    notesTest.Add(new Note("Hamilton", "Margaret", "BD", 20, 2));
    Console.WriteLine($"   Après ajout 20     : {bonnes.Count()} / {figees.Count}");



    Console.WriteLine("\n=== Classe Promo (LINQ) ===");
    //Instanciation promo
    var promo = new Promo("ING3", notes);
    //Moyenne et major
    Console.WriteLine($"Promo {promo.Nom} | Moyenne : {promo.MoyenneGenerale():F2} | Major : {promo.Major() ?? "aucun"}");
    //étudiant sans note
    Console.WriteLine($"Moyenne d'un absent : {promo.Moyenne("Margaret"):F2}");
    //Relevé complet de la promo
    Console.WriteLine("\nRelevé :");
    foreach (var ligne in promo.Releve())
        Console.WriteLine($"  {ligne}");
    //Compte des admis par matière
    Console.WriteLine("\nAdmis par matière :");
    foreach (var (matiere, nb) in promo.NbAdmisParMatiere())
        Console.WriteLine($"  {matiere,-7} : {nb}");
    }
}