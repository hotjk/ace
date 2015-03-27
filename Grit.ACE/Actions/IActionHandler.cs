namespace ACE
{
    public interface IActionHandler<T> where T : Action
    {
        void Invoke(T action);
    }
}
