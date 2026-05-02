namespace Gra.Observer;

// to bedzie nasz kanał na YT, czyli on nadaje wiadomosci
public interface ISubject<T> where T : IEventPayload
{
    void Attach(IObserver<T> observer); // dolacz nowego sybskrybenta
    void Detach(IObserver<T> observer);
    void Notify(T message); // powiadom subskrbentow 
}