namespace Geometrie;

public abstract class Forme : IDessinable, IComparable<Forme>
{
    public string Nom { get; }

    protected Forme(string nom)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nom);
        Nom = nom;
    }

    public abstract double Aire();
    public abstract double Perimetre();

    public virtual string Decrire() =>
        $"{Nom} — Aire : {Aire():F2}, Périmètre : {Perimetre():F2}";

    public int CompareTo(Forme? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Aire().CompareTo(other.Aire());
    }

    public void Dessiner() => Console.WriteLine(Decrire());
}