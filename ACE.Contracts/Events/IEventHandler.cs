namespace ACE
{
    public interface IEventHandler<T> where T : IEvent
    {
        void Handle(T handle);
    }
}
