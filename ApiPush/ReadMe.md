# ApiPush

This project reads events from a subscription in RabbitMq and pushes them to http endpoints located at Parters

### Internal workings
**ApiPushService** sets up an **AutoSubscriber** that in its turn uses an **AutoSubscriberMessageDispatcher**. This is EasyNetQ standard.
The Dispatcher is responsible for handling messages. It does that by creating a **Consumer**.

The AutoSubscriberMessageDispatcher could be skipped, EasyNetQ will then use reflection to create a Consumer by looking at the interface _IConsume<T>_. The problem with that approach is that it requires a parameterless constructor, we would need to either new up dependencies manually or use a service locator. By using the AutoSubscriberMessageDispatcher instead it becomes easier to inject and mock out dependencies when testing for example.

The Consumer fetches the subscription details from a **SubscriptionStorage** using the PartnerId from the consumed message from rabbit.
If the subscription exists the Consumer uses a **PushSender** to call an endpoint defined in the subscription for that specific Partner.

 A retry policy is used, if we get an http error a configurable amount of retries are done with a exponential back-off strategy with
 Use the app settings _RetryAttempts_ and _RetryDelayInSeconds_ configurable timing.

### Installation
Topshelf is used, the application can be run from the command line or installed as a windows servcie. See [http://topshelf.readthedocs.org/en/latest/overview/commandline.html] for installation

### Going forward
There are a few things next up in this project:
* A real subscription storage, The clients should be able to subscribe via the public Api
* Security, consider different security aspects, we could use a token in the header and hae shared keys for encrypting messages if needed
* It would be extremly useful to have an option to drop the messages on Amazon SQS instead of calling http endpoints
* The clients could be hammered to death, we could batch messages and send every x minute
* the code could be async instead, some research around how EasyNetQ handle async is probably needed, mainly thinking of batching etc,
