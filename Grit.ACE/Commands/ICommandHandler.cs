namespace ACE
{
    public interface ICommandHandler<T> where T : Command
    {
        void Execute(T command);
    }
}
