using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class CharacterAnim : MonoBehaviour
{

    private Animator _animator;
    private EventBus<int> _eventBusI;
    private List<IDisposable> _disposables = new List<IDisposable>();

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _animator = GetComponent<Animator>();

        _eventBusI = eventBusI;

        _disposables.Add(_eventBusI.Subscribe(EventsName.StartGame, StartMove));

        _disposables.Add(_eventBusI.Subscribe(EventsName.Win, StartSamba));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Fail, StopMove));

        _disposables.Add(_eventBusI.Subscribe(EventsName.Reset, ResetAnim));
        _disposables.Add(_eventBusI.Subscribe(EventsName.NextLevel, ResetAnim));

        _disposables.Add(_eventBusI.Subscribe(EventsName.ChangeStatus, StartSpin));
    }

    private void StartSpin(int obj)
    {
        _animator.SetTrigger("Spin");
    }

    private void ResetAnim(int obj)
    {
        _animator.SetTrigger("DefeatStop");
        _animator.SetTrigger("SambaStop");
    }

    private void StopMove(int obj)
    {
        _animator.SetTrigger("DefeatStart");
    }

    private void StartSamba(int obj)
    {
        _animator.SetTrigger("SambaStart");
    }

    private void StartMove(int obj)
    {
        _animator.SetTrigger("Move");
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }
}
