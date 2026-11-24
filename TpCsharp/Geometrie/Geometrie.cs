namespace Geometrie;

public static class exoGeometrie
{
    public static void Lancer()
    {
        // Poolymorphisme
        List<Forme> formes = [new Cercle(1), new Rectangle(2, 5), new Carre(3), new Cercle(2.5)];

        formes.Sort();

        foreach (var f in formes)
        {
            f.Dessiner();
        }

        double aireTotale = 0;
        Forme maxPerimetre = formes[0];

        foreach (Forme f in formes)
        {
            aireTotale += f.Aire();

            if (f.Perimetre() > maxPerimetre.Perimetre())
            {
                maxPerimetre = f;
            }
        }

        Console.WriteLine($"\nAire totale : {aireTotale:F2}");
        Console.WriteLine($"Plus grand périmètre : {maxPerimetre.Nom} ({maxPerimetre.Perimetre():F2})");

        Cercle c = new Cercle(1);
        Forme formeRef = c;
        Console.WriteLine($"\nVia Cercle : {c.Decrire()}");
        Console.WriteLine($"Via Forme  : {formeRef.Decrire()}");
    }
}