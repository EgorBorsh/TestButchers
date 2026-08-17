using ButchersGames;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class GameManager : IDisposable
{
    private EventBus<int> _eventBusI;
    [Inject] private EventBus<bool> _eventBusB;
    [Inject] private EventBus<AudioType> _eventBusAT;

    private List<IDisposable> _disposables = new List<IDisposable>();

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _eventBusI = eventBusI;

        _disposables.Add(_eventBusI.Subscribe(EventsName.OneMove, StartGame));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Fail, Fail));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Win, Win));

        _disposables.Add(_eventBusI.Subscribe(EventsName.NextLevel, NextLevel));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Reset, Reset));

        LevelManager.Default.SelectLevel(0);

        _eventBusI.Publish(EventsName.SetPathFollower, 0);
    }

    private void StartGame(int i)
    {
        _eventBusI.Publish(EventsName.StartGame, 0);
    }

    private void Fail(int i)
    {
        _eventBusAT.Publish(EventsName.AudioPlay, AudioType.Fail);
        _eventBusI.Publish(EventsName.OpenFail, 0);
    }

    private void Win(int i)
    {
        _eventBusAT.Publish(EventsName.AudioPlay, AudioType.Win);
        _eventBusI.Publish(EventsName.OpenWin, 0);
    }

    private void Reset(int obj)
    {
        _eventBusI.Publish(EventsName.CloseFail, 0);
        _eventBusI.Publish(EventsName.OpenTutorial, 0);
        _eventBusB.Publish(EventsName.Reset, false);
        LevelManager.Default.RestartLevel();

        _eventBusI.Publish(EventsName.SetPathFollower, 0);
    }

    private void NextLevel(int i)
    {
        _eventBusI.Publish(EventsName.CloseWin, 0);
        _eventBusI.Publish(EventsName.OpenTutorial, 0);
        _eventBusB.Publish(EventsName.Reset, true);

        LevelManager.Default.NextLevel();

        _eventBusI.Publish(EventsName.SetPathFollower, 0);
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }

}
