using TP1;
namespace Ecole;

public class Promo
{
    private readonly List<Etudiant> _etudiants = [];

    public string Nom { get; }

    public Promo(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom de la promo ne peut pas être vide.", nameof(nom));
        }

        Nom = nom;
    }

    public void Inscrire(Etudiant e)
    {
        ArgumentNullException.ThrowIfNull(e);
        _etudiants.Add(e);
    }

    public Etudiant? Chercher(string nom)
    {
        foreach (Etudiant etudiant in _etudiants)
        {
            if (string.Equals(etudiant.Nom, nom, StringComparison.OrdinalIgnoreCase))
            {
                return etudiant;
            }
        }

        return null;
    }

    public Etudiant? Major()
    {
        if (_etudiants.Count == 0)
        {
            return null;
        }

        Etudiant major = _etudiants[0];

        foreach (Etudiant etudiant in _etudiants)
        {
            if (etudiant.Moyenne() > major.Moyenne())
            {
                major = etudiant;
            }
        }

        return major;
    }

    public double MoyenneGenerale()
    {
        if (_etudiants.Count == 0)
        {
            return 0;
        }

        double somme = 0;

        foreach (Etudiant etudiant in _etudiants)
        {
            somme += etudiant.Moyenne();
        }

        return somme / _etudiants.Count;
    }

    public void AfficherReleve()
    {
        Console.WriteLine($"=== Relevé de la promo : {Nom} ===");

        if (_etudiants.Count == 0)
        {
            Console.WriteLine("Aucun étudiant inscrit.");
            return;
        }

        foreach (Etudiant etudiant in _etudiants)
        {
            double moyenne = etudiant.Moyenne();
            string mention = GestionNotes.Mention(moyenne);
            Console.WriteLine($"{etudiant} — Mention : {mention}");
        }

        Console.WriteLine($"Moyenne générale : {MoyenneGenerale():F2}");
    }
}
