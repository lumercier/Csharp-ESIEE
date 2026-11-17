namespace Ecole;

public class Etudiant
{
    private int _annee = 1;
    private readonly List<double> _notes = [];

    public string Nom { get; }
    public string Prenom { get; }
    public DateOnly Naissance { get; init; }

    public string NomComplet => $"{Prenom} {Nom.ToUpper()}";
    public int NbNotes => _notes.Count;

    public int Annee
    {
        get => _annee;
        set
        {
            if (value is < 1 or > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "L'année doit être comprise entre 1 et 5.");
            }

            _annee = value;
        }
    }

    public Etudiant(string nom, string prenom, int annee=1)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom ne peut pas être vide.", nameof(nom));
        }

        if (string.IsNullOrWhiteSpace(prenom))
        {
            throw new ArgumentException("Le prénom ne peut pas être vide.", nameof(prenom));
        }

        Nom = nom;
        Prenom = prenom;
        Annee = annee;
    }

    public void AjouterNote(double n)
    {
        if (n is < 0 or > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "La note doit être comprise entre 0 et 20.");
        }

        _notes.Add(n);
    }

    public double Moyenne()
    {
        if (_notes.Count == 0)
        {
            return 0;
        }

        double somme = 0;
        foreach (double note in _notes)
        {
            somme += note;
        }

        return somme / _notes.Count;
    }

    public override string ToString()
    {
        string niveau = Annee switch
        {
            <= 3 => $"L{Annee}",
            _ => $"M{Annee - 3}"
        };

        return $"{NomComplet} ({niveau}) : {Moyenne():F2}";
    }
}
