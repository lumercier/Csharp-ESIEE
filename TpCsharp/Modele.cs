public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef)
{
    public double Valeur { get; init; } = Valeur is >= 0 and <= 20
    ? Valeur
    : throw new ArgumentOutOfRangeException(nameof(Valeur));

    public int Coef { get; init; } = ValiderCoef(Coef);

    private static int ValiderCoef(int coef)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(coef);
        return coef;
    }

};

//public record Inscrit(string Nom, string Prenom, string Promo);
public record Inscrit(string Nom, string Prenom, string Promo, string CodePostal);
public record Bulletin(string Prenom, List<double> Valeurs);
public record Commune(string Nom, string Code, int Population);
