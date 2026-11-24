using System.Text.Json;

namespace TP3;

public static class Stockage
{   //Configuration du sérialiseur
    private static readonly JsonSerializerOptions Options =
        new(JsonSerializerDefaults.Web) { WriteIndented = true };
    //Ecriture (asynchrone)
    public static async Task SauverAsync(string chemin, List<Note> notes)
    {
        await using FileStream flux = File.Create(chemin); //Création du ficher
        await JsonSerializer.SerializeAsync(flux, notes, Options); //Sérialisation
    }
    //Lecture (asynchrone)
    public static async Task<List<Note>> ChargerAsync(string chemin)
    {
        await using FileStream flux = File.OpenRead(chemin); //Lecture du fichier
        return await JsonSerializer.DeserializeAsync<List<Note>>(flux, Options) ?? []; //Sérialisation
    }
}
