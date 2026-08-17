using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using VContainer;

public class ManagerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelTutorial;
    [SerializeField]
    private GameObject _panelWin;
    [SerializeField]
    private GameObject _panelFail;

    [Inject] private EventBus<int> _eventBusI;

    private List<IDisposable> _disposables = new List<IDisposable>();

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _eventBusI = eventBusI;

        _disposables.Add(_eventBusI.Subscribe(EventsName.StartGame, onStartGame));
        _disposables.Add(_eventBusI.Subscribe(EventsName.OpenTutorial, onOpenTutorial));
        _disposables.Add(_eventBusI.Subscribe(EventsName.OpenFail, onOpenFail));
        _disposables.Add(_eventBusI.Subscribe(EventsName.CloseFail, onCloseFail));
        _disposables.Add(_eventBusI.Subscribe(EventsName.OpenWin, onOpenWin));
        _disposables.Add(_eventBusI.Subscribe(EventsName.CloseWin, onCloseWin));
    }

    private void onCloseWin(int obj)
    {
        _panelWin.SetActive(false);
    }

    private void onOpenWin(int obj)
    {
        _panelWin.SetActive(true);
    }

    private void onCloseFail(int obj)
    {
        _panelFail.SetActive(false);
    }

    private void onOpenFail(int obj)
    {
        _panelFail.SetActive(true);
    }

    private void onStartGame(int obj)
    {
        _panelTutorial.SetActive(false);
    }

    private void onOpenTutorial(int obj)
    {
        _panelTutorial.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach(var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }
}
