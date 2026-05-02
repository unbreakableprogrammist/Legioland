namespace Gra.Observer;
// to jest ogolny schemat na kanal na yt, kazdy moze stworzyc wlasny, zalezy co wlozy do T
public class NotificationSubject<T> : ISubject<T> where T: IEventPayload
{
    private List<IObserver<T>> _observers = new List<IObserver<T>>(); // lista subskrymentow 

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