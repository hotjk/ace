namespace ACE
{
    public interface IEventHandler<T> where T : Event
    {
        void Handle(T handle);
    }
}
