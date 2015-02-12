using System;
namespace ACE
{
    public interface IServiceLocator
    {
        IActionBus ActionBus { get; }
        ICommandBus CommandBus { get; }
        IEventBus EventBus { get; }
    }
}
