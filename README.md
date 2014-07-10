## DomainMessage

DomainMesssage 也叫 DomainEvent，用于解耦服务。

> 参考 http://www.udidahan.com/2009/06/14/domain-events-salvation/

### Action/ActionBus

进程间通信 RPC 消息

#### Send

发送 Action 到外部的事件总线（RabbitMQ）。Send 方法是一种 RPC 调用，发送之前会在 RabbitMQ 创建一个线程专用的匿名 Queue 用于接收 ActionResponse；业务处理中心（Saga）负责从队列中获取 Action，并执行具体的业务操作，操作完成后，Saga 要在 Action 指定的匿名 Queue 里发送 ActionResponse 应答；发送如果不能在指定的超时时间内收到 ActionResponse，会抛出异常。

#### Invoke

Saga 收到 Action 后，可以在使用 Invoke 方法发送 Action 到特定的一个 ActionHandler 处理。

### Command/CommandBus

线程内通信消息

#### Send

发送 Command 对象，一个 Command 只能被一个特定的 CommandHandler 处理，应该在 UnitOfWork 内部调用该方法，发送的多个 Command 对象将在线程内顺序执行，且保持数据库事务一致性。

### Event/EventBus

既用于进程内无应答通信，也用于进程间无应答通信

#### Publish

发送 Event 对象，一个 Event 对象可以被零到多个 EventHandler 处理；使用该方法需要在 UnitOfWork 内，比如 CommandHandler 内部；调用 publish 方法后，Event 对象将被缓存在线程的 Event 队列中，等待 Flush 方法将 Event 队列中的所有 Event 发布到线程池和事件总线（RabbitMQ）。

#### Flush

将 Event 队列中的所有 Event 发布到线程池和事件总线（RabbitMQ）；通常不需要显式的调用，Flush 方法会在 UnitOfWork 的 Complete 方法中自动调用。 

#### Purge

放弃 Event 队列中所有 Event，通常不需要显式的调用，Purge 方法会在 UnitOfWork 的 Dispose 方法中自动调用。

#### Handle

Event Consumer 收到来自事件队列的 Action 对象后，调用 Handle 方法在进程内分发该消息，每个 EventHandler 将在独立线程内完成对 Event 的业务处理。

## UnitOfWork

工作单元，TransactionScope 和 Event 队列的发起和提交在工作单元中处理。

## ServiceLocator

全局对象，承载 ActionBus/CommandBus/EventBus，Ninject Kernal，RabbitMQ Channel，ActionBus Exchange Name，ActionBus Queue Name，EventBus Exchange Name。

## Saga

业务处理中心，消费 Action，发布 Command/Event

## Event Consumer

特定业务的 Event 消费者，特定 Event consumer 需要根据特定业务创建特定 Queue，并配置路由规则将 Queue 绑定到 Event Exchange。