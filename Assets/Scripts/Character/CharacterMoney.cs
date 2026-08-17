using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using static UnityEngine.Rendering.DebugUI;

public class CharacterMoney : IDisposable
{
    private int _currentCountMoney = 40;
    private int _totalMoney = 0;

    private EventBus<int> _eventBusI;
    private EventBus<bool> _eventBusB;
    [Inject] private EventBus<AudioType> _eventBusAT;

    private List<IDisposable> _disposables = new List<IDisposable>();


    [Inject]
    public void Construct(EventBus<int> eventBusI, EventBus<bool> eventBusB)
    {
        _eventBusI = eventBusI;
        _eventBusB = eventBusB;

        _disposables.Add(_eventBusI.Subscribe(EventsName.PickedUpTheMoney, AddMoney));
        _disposables.Add(_eventBusI.Subscribe(EventsName.PickedUpTheBadStuff, TakeMoney));
        _disposables.Add(_eventBusB.Subscribe(EventsName.Reset, ResetMoney));
    }

    public void ResetMoney(bool success = false)
    {
        if (success)
        {
            _totalMoney += _currentCountMoney;
            _eventBusI.Publish(EventsName.TotalMoneyHasChanged, _totalMoney);
        }

        _currentCountMoney = 40;
        _eventBusI.Publish(EventsName.MoneyHasChanged, _currentCountMoney);
    }

    public void AddMoney(int value)
    {
        _currentCountMoney += value;

        _eventBusI.Publish(EventsName.AddMoney, value);
        _eventBusI.Publish(EventsName.MoneyHasChanged, _currentCountMoney);
        _eventBusAT.Publish(EventsName.AudioPlay, AudioType.AddPicUp);
    }

    public void TakeMoney(int value)
    {
        _currentCountMoney -= value;

        if (_currentCountMoney <= 0)
        {
            _currentCountMoney = 0;
            _eventBusI.Publish(EventsName.Fail, 0);
        }

        _eventBusI.Publish(EventsName.TakeMoney, value);
        _eventBusI.Publish(EventsName.MoneyHasChanged, _currentCountMoney);
        _eventBusAT.Publish(EventsName.AudioPlay, AudioType.TakePickUp);
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }
}
