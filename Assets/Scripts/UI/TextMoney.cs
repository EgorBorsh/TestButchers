using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class TextMoney : MonoBehaviour
{
    private TMP_Text _text;

    private EventBus<int> _eventBusI;
    private List<IDisposable> _disposables = new List<IDisposable>();

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _text = GetComponent<TMP_Text>();

        _eventBusI = eventBusI;

        _disposables.Add(_eventBusI.Subscribe(EventsName.MoneyHasChanged, UpdateText));
    }

    private void UpdateText(int value)
    {
        _text.text = value.ToString("0");
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }
}
