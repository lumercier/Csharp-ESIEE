using Microsoft.EntityFrameworkCore;

using (var db = new PromoContext())
{
    if (!db.Etudiants.Any())
    {
        var promo = Donnees.Promo();
        db.Etudiants.AddRange(promo);
        Console.WriteLine($"Avant : {db.Entry(promo[0]).State}");
        int n = db.SaveChanges();
        Console.WriteLine($"{n} lignes écrites");
        Console.WriteLine($"Après : {db.Entry(promo[0]).State}, Id = {promo[0].Id}");
    }

//étape 3
//QUESTION 9
    Console.WriteLine("\n\n\nLes étudiants de M2, triés par nom.");
    var m2 = db.Etudiants
        .Where(e => e.Promo == "M2")
        .OrderBy(e => e.Nom);
    Console.WriteLine("SQL :\n" + m2.ToQueryString());

    Console.WriteLine("\nRésultat :");
    foreach (var e in m2)
        Console.WriteLine($"    {e.Nom} {e.Prenom}");


//QUESTION 10
    Console.WriteLine("\n\n\nPour chaque étudiant, son nom et son nombre de notes,");
    var nbNotes = db.Etudiants
        .Select(e => new { e.Nom, Nb = e.Notes.Count });
    Console.WriteLine("SQL :\n" + nbNotes.ToQueryString());
    Console.WriteLine("\nRésultat :");
    foreach (var item in nbNotes)
        Console.WriteLine($"    {item.Nom} : {item.Nb}");


//QUESTION 11
    Console.WriteLine("\n\n\nLa moyenne de chaque matière");
    var moyennesMatieres = db.Notes
        .GroupBy(n => n.Matiere)
        .Select(g => new { Matiere = g.Key, Moyenne = g.Average(n => n.Valeur) });
    Console.WriteLine("SQL :\n" + moyennesMatieres.ToQueryString());

    Console.WriteLine("\nRésultat :");
    foreach (var m in moyennesMatieres)
        Console.WriteLine($"    {m.Matiere} : {m.Moyenne:F2}");


//QUESTION 12
    Console.WriteLine("\n\n\nParcours db.Etudiants.ToList() et affiche e.Notes.Count pour chaque étudiant.");
    Console.WriteLine("Recommence avec .Include(e => e.Notes). Compare les deux résultats, et le SQL des deux requêtes.");

    var reqSansInclude = db.Etudiants;
    Console.WriteLine("\nSQL (sans Include) :\n" + reqSansInclude.ToQueryString());
    var sansInclude = db.Etudiants.ToList();
    Console.WriteLine("\nRésultat sans Include");
    Console.WriteLine("    " + string.Join(", ", sansInclude.Select(e => $"{e.Nom} ({e.Notes.Count})")));

    var reqAvecInclude = db.Etudiants.Include(e => e.Notes);
    Console.WriteLine("\nSQL (avec Include) :\n" + reqAvecInclude.ToQueryString());
    var avecInclude = reqAvecInclude.ToList();
    Console.WriteLine("\nRésultat avec Include");
    Console.WriteLine("    " + string.Join(", ", avecInclude.Select(e => $"{e.Nom} ({e.Notes.Count})")));


//QUESTION 13
    Console.WriteLine("\n\n\nLa moyenne pondérée de chaque étudiant qui a au moins une note");
    var classement = db.Etudiants
        .Where(e => e.Notes.Any())
        .Select(e => new
        {
            e.Nom,
            Moyenne = e.Notes.Sum(n => n.Valeur * n.Coef) / e.Notes.Sum(n => n.Coef)
        })
        .OrderByDescending(x => x.Moyenne)
        .ToList();

    Console.WriteLine("Résultat :");
    foreach (var c in classement)
        Console.WriteLine($"    {c.Nom} : {c.Moyenne:F2}");


//QUESTION 14
    Console.WriteLine("\n\n\nAlan Turing passe en M2");
    var alan = db.Etudiants.FirstOrDefault(e => e.Nom == "Turing" && e.Prenom == "Alan");
    if (alan != null)
    {
        alan.Promo = "M2";
        Console.WriteLine($"État avant SaveChanges : {db.Entry(alan).State}");
        db.SaveChanges();
    }


//QUESTION 15
    Console.WriteLine("\n\n\nSuppression de Linus Torvalds");
    var linus = db.Etudiants.FirstOrDefault(e => e.Nom == "Torvalds" && e.Prenom == "Linus");
    if (linus != null)
    {
        db.Etudiants.Remove(linus);
        db.SaveChanges();
    }
    Console.WriteLine($"Notes restantes en base : {db.Notes.Count()}");


//QUESTION 16
    Console.WriteLine("\n\n\nCoef Réseau passe à 2");
    int nb = db.Notes.Where(n => n.Matiere == "Reseau")
        .ExecuteUpdate(s => s.SetProperty(n => n.Coef, 2));
    Console.WriteLine($"Lignes modifiées : {nb}");

}