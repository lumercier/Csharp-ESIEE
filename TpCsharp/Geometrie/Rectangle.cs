namespace Geometrie;

public class Rectangle : Forme
{
    public double Largeur { get; }
    public double Hauteur { get; }

    public Rectangle(double largeur, double hauteur) : base("Rectangle")
    {   //GESTION DES ERREURS
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
    //CALCUL DE L'AIRE
    public override double Aire() => Largeur * Hauteur;
    //CALCUL DU PERIMETRE
    public override double Perimetre() => 2 * (Largeur + Hauteur);
}
