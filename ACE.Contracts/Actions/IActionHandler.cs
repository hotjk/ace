namespace ACE
{
    public interface IActionHandler<T> where T : IAction
    {
        void Invoke(T action);
    }
}
