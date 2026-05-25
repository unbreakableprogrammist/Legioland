namespace Gra.Observer;

public interface ISubject<T> where T : IEventPayload
{
    void Attach(IObserver<T> observer);
    void Detach(IObserver<T> observer);
    void Notify(T message);
}
