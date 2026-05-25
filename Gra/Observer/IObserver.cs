namespace Gra.Observer;

public interface IObserver<T> where T : IEventPayload
{
    void OnNotify(T message);
}
