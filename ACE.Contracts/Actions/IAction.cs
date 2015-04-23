namespace ACE
{
    /// <summary>
    /// Action is a message direct send to MicroServices via RabbitMQ.
    /// MicroServices will process this message and reply an ActionResponse to orignal sender.
    /// Action SHOULD NOT host big size entities since it will be serilized(json) then send to MQ and saved in event log.
    /// </summary>
    public interface IAction
    {
    }
}
