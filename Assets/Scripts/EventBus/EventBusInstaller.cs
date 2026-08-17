using VContainer;


public static class EventBusInstaller
{
    public static void RegisterEventBus<TEvent>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
    {
        builder.Register<EventBus<TEvent>>(lifetime)
            .As<IEventBus<TEvent>>()
            .AsSelf();
    }
}
