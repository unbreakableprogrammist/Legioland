namespace Gra.Observer;
public class NotificationSubject<T> : ISubject<T> where T: IEventPayload
{
    private List<IObserver<T>> _observers = new List<IObserver<T>>();

    public void Attach(IObserver<T> observer)
    {
        if(!_observers.Contains(observer))  _observers.Add(observer);
    }

    public void Detach(IObserver<T> observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(T message)
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify(message);
        }
    }

}
