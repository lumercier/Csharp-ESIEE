namespace Geometrie;

public class Rectangle : Forme
{
    public double Largeur { get; }
    public double Hauteur { get; }

    public Rectangle(double largeur, double hauteur) : base("Rectangle")
    {
        if (largeur <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(largeur), "La largeur doit être strictement positive.");
        }

        if (hauteur <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hauteur), "La hauteur doit être strictement positive.");
        }

        Largeur = largeur;
        Hauteur = hauteur;
    }

    public override double Aire() => Largeur * Hauteur;

    public override double Perimetre() => 2 * (Largeur + Hauteur);
}
