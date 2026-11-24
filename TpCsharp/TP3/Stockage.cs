using System.Text.Json;

namespace TP3;

public static class Stockage
{
    private static readonly JsonSerializerOptions Options =
        new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static void Sauver(string chemin, List<Note> notes) =>
        File.WriteAllText(chemin, JsonSerializer.Serialize(notes, Options));

    public static List<Note> Charger(string chemin) =>
        JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(chemin), Options) ?? [];

    public static async Task SauverAsync(string chemin, List<Note> notes)
    {
        await using FileStream flux = File.Create(chemin);
        await JsonSerializer.SerializeAsync(flux, notes, Options);
    }

    public static async Task<List<Note>> ChargerAsync(string chemin)
    {
        await using FileStream flux = File.OpenRead(chemin);
        return await JsonSerializer.DeserializeAsync<List<Note>>(flux, Options) ?? [];
    }
}
