namespace ACE
{
    public interface ICommandBus
    {
        /// <summary>
        /// Send command to corresponding commandHandler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        ICommandBus Send<T>(T command) where T : ICommand;
    }
}
