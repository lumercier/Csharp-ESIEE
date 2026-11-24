namespace Ecole;

public class NoteInvalideException : Exception
{
    public int Ligne { get; }

    public NoteInvalideException(int ligne, string message) : base(message)
    {
        Ligne = ligne;
    }

    public NoteInvalideException(int ligne, string message, Exception innerException)
        : base(message, innerException)
    {
        Ligne = ligne;
    }
}