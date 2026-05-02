namespace Gra.Observer;

// tworzymy jeden inferfejs ktory bedzie sluchal typu T ktory jest rzecza pochodan od IEventPayload 
public interface IObserver<T> where T : IEventPayload
{
    void OnNotify(T message); // metoda ktora bedzie nadpisywal subskrybtent 
}