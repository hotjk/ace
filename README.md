# ACE

简单的微服务基础库。
提供三种领域事件，Action/Command/Event，Action 用于前端和微服务之间通信，Command 用于 Aggregate 之间通信，Event 用于微服务之间通信。

![ace](https://github.com/hotjk/ace/blob/master/ace.jpg)

> 参考
>
> http://www.udidahan.com/2009/06/14/domain-events-salvation/
> http://martinfowler.com/articles/microservices.html

### Action/ActionBus

进程间 RPC 调用消息。
前端通过 ActionBus 发送 Action 到消息总线（RabbitMQ），同时创建一个线程专用的匿名 Queue 用于接收 ActionResponse；业务处理中心（MicroServices）负责从队列中获取 Action，并执行具体的业务操作，操作完成后，MicroServices 要在 Action 指定的匿名 Queue 里发送 ActionResponse 应答。

#### Send

发送 Action 到外部的事件总线。
MUST 为每个微服务承载的消息提供统一的基类（路由基类），RPC 专用的 Exchange 会使用 direct 路由方式把一个路由基类的所有消息路由到路由基类同名 Queue，特定微服务直接订阅特定路由基类 Queue。

#### Handle

从事件总线获取 Action，调用 Invoke 方法处理 Action，并回复应答到消息总线。
在 ApplicationService 中 Handle Action，Handle 中再发送 Command 来进行具体的业务处理。

#### Invoke

发送 Action 到特定的一个 ActionHandler 处理。
如果不使用事件总线，可以直接使用 Invoke 方法处理 Action。

### Command/CommandBus

线程内通信消息。
一个 Command 只能被一个特定的 CommandHandler 处理，MUST 在 UnitOfWork 内部调用该方法，发送的多个 Command 对象将在线程内顺序执行，且保持数据库事务一致性。

#### Send

发送 Command 对象。

### Event/EventBus

既用于进程内无应答通信，也用于进程间无应答通信。

#### Publish

发送 Event 对象，一个 Event 对象可以被零到多个 EventHandler 处理。
使用该方法需要在 UnitOfWork 内，比如 CommandHandler 内部。
调用 publish 方法后，Event 对象将被缓存在线程的 Event 队列中，等待 Flush 方法将 Event 队列中的所有 Event 发布到线程池和事件总线。

#### Flush

将 Event 队列中的所有 Event 发布到线程池和事件总线。
通常不需要显式的调用，Flush 方法会在 UnitOfWork 的 Complete 方法中自动调用。 

#### Purge

放弃 Event 队列中所有 Event。
通常不需要显式的调用，Purge 方法会在 UnitOfWork 的 Dispose 方法中自动调用。

#### Handle

从事件总线获取 Event，调用 Invoke 进行 Event 处理。
在 ApplicationService 中 Handle Event，Handle 中再发送 Command 来进行具体的业务处理。
Event 是不需要应答的 Action。
直接在 Handle 中处理业务在简单场景下也是可以的。

#### Invoke

Event Consumer 收到来自事件队列的 Event 对象后，调用 Invoke 方法在进程内分发该消息，每个 EventHandler 将在独立线程内完成对 Event 的业务处理。

## ServiceBus

读服务以 Http Service 提供时，ServiceBus 用于简化服务调用。
读服务接口类与资源的映射需要配置，外部配置服务可以参考 [dotconfig](https://github.com/hotjk/dotconfig)。
采用传统分层架构时，读模型可以代码级复用，MVC 直接引入读模型，不需要使用 ServiceBus。

## UnitOfWork

工作单元，TransactionScope 和 Event 队列的发起和提交在工作单元中处理。

## MicroService

微服务，业务处理中心，消费 Action，发布 Command/Event，返回 ActionResponse。

## Event Consumer

特定业务的 Event 消费者，特定 Event consumer 需要根据特定业务创建特定 Queue，并配置路由规则将 Queue 绑定到 Event Exchange。

## BootStrapper

通过反射将 Action/Command/Event 和相应的 Handler 绑定。
