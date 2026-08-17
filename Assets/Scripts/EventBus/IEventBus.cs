using System;

public interface IEventBus<TEvent> : IDisposable
{
    IDisposable Subscribe(string eventName, Action<TEvent> handler);
    IDisposable Subscribe<TResult>(string eventName, Func<TEvent, TResult> handler);
    void Publish(string eventName, TEvent eventData);
    void Publish<TResult>(string eventName, TEvent eventData, Action<TResult> callback);
    void PublishAll<TResult>(string eventName, TEvent eventData, Action<TResult[]> callback);
    void Clear();
}

