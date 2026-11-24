using Ecole;
using Geometrie;
using TP1;
using TP2;
using TP3;
using System.Globalization;


//AFFICHAGE DU MENU PRINCIPAL
//ASCII Art
Console.WriteLine("");
Console.WriteLine(@" $$$$$$\    $$\ $$\                       $$\                                             ");
Console.WriteLine(@"$$  __$$\   $$ \$$ \                      $$ |                                            ");
Console.WriteLine(@"$$ /  \__|$$$$$$$$$$\                     $$ |     $$\   $$\  $$$$$$$\ $$$$$$\   $$$$$$$\ ");
Console.WriteLine(@"$$ |      \_$$  $$   |      $$$$$$\       $$ |     $$ |  $$ |$$  _____|\____$$\ $$  _____|");
Console.WriteLine(@"$$ |      $$$$$$$$$$\       \______|      $$ |     $$ |  $$ |$$ /      $$$$$$$ |\$$$$$$\  ");
Console.WriteLine(@"$$ |  $$\ \_$$  $$  _|                    $$ |     $$ |  $$ |$$ |     $$  __$$ | \____$$\ ");
Console.WriteLine(@"\$$$$$$  |  $$ |$$ |                      $$$$$$$$\\$$$$$$  |\$$$$$$$\\$$$$$$$ |$$$$$$$  |");
Console.WriteLine(@" \______/   \__|\__|                      \________|\______/  \_______|\_______|\_______/ ");
Console.WriteLine("");
//CHOIX    
Console.WriteLine("1 - Calculateur d'âge");
Console.WriteLine("2 - Gestion des notes");
Console.WriteLine("3 - Test classe Etudiant");
Console.WriteLine("4 - Géométrie");
Console.WriteLine("5 - Lecture de notes.csv");
Console.WriteLine("6 - Lire le fichier en LINQ");
Console.WriteLine("7 - Requêtes LINQ");
Console.WriteLine("8 - TP Json en Asynchrone");
Console.WriteLine("9 - Recherche sur geo.api.gouv.fr");
Console.WriteLine("10 - Afficher CP des étudiants");

Console.Write("Votre choix : ");

string? choix = Console.ReadLine();

switch (choix)
{
//CHOIX 1
    case "1":
        CalculateurAge.Lancer();
        break;

//CHOIX 2
    case "2":
        GestionNotes.Lancer();
        break;
//CHOIX 3
    case "3":
        exoEcole.Lancer();
        break;

//CHOIX 4      
    case "4":
        exoGeometrie.Lancer();
        break;

//CHOIX 5
    case "5":
        GestionFichierNotes.Lancer();
        break;

//CHOIX 6   
    case "6":
        Linq.Lancer();
        break;

//CHOIX 7
    case "7":
        Requetes.Lancer();
        break;

//CHOIX 8
    case "8":
        await TP3.TpJson.LancerAsync();
        break;

//CHOIX 9
    case "9":
        await TP3.GeoApi.LancerAsync();
        break;

//CHOIX 10
    case "10":
        await TP3.GeoApi.CommunesEtudiantsAsync();
    break;

//DEFAUT
    default:
        Console.WriteLine("Choix invalide.");
        break;
}
