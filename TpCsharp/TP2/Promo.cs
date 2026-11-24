namespace TP2;

public class Promo(string nom, IEnumerable<Note> notes)
{
    private readonly List<Note> _notes = notes.ToList();

    public string Nom { get; } = nom;

    // Prénoms distincts
    public IEnumerable<string> Etudiants() =>
        _notes.Select(n => n.Prenom).Distinct();

    // Moyenne pondérée, 0 si aucune note
    public double Moyenne(string prenom) =>
        _notes.Where(n => n.Prenom == prenom).ToList() is { Count: > 0 } ns
            ? ns.Sum(n => n.Valeur * n.Coef) / ns.Sum(n => n.Coef)
            : 0;

    // Moyenne des moyennes des étudiants (0 si promo vide)
    public double MoyenneGenerale() =>
        Etudiants().Select(Moyenne).DefaultIfEmpty(0).Average();

    // Prénom du major, null si promo vide
    public string? Major() =>
        Classement().Select(c => c.Prenom).FirstOrDefault();

    // (prénom, moyenne) par moyenne décroissante
    public IEnumerable<(string Prenom, double Moyenne)> Classement() =>
        Etudiants()
            .Select(p => (Prenom: p, Moyenne: Moyenne(p)))
            .OrderByDescending(c => c.Moyenne);

    // Matière -> nombre de notes >= 10
    public IReadOnlyDictionary<string, int> NbAdmisParMatiere() =>
        _notes
            .GroupBy(n => n.Matiere)
            .ToDictionary(g => g.Key, g => g.Count(n => n.Valeur >= 10));

    // Ligne de relevé par étudiant dans l'ordre du classement
    public IEnumerable<string> Releve() =>
        Classement().Select((c, i) => $"{i + 1}. {c.Prenom} {c.Moyenne:F2} {Mention(c.Moyenne)}");

    // Réutilise ta méthode de la séance 1 (ou appelle Ecole.Etudiant.Mention si elle est public static)
    private static string Mention(double moyenne) => moyenne switch
    {
        >= 16 => "Très bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Ajourné"
    };
}
