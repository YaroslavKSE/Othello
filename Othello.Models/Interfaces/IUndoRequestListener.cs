namespace Othello.Models.Interfaces;

public interface IUndoRequestListener
{
    Task<bool> UndoKeyPressedAsync(CancellationToken ctsToken);
}