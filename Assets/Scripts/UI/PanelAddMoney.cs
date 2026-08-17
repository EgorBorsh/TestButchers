using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class PanelAddMoney : MonoBehaviour
{

    private TMP_Text _textChildValue;
    private CanvasGroup _canvasGroup;
    private Vector2 _startPosition;

    private EventBus<int> _eventBusI;
    private List<IDisposable> _disposables = new List<IDisposable>();

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _textChildValue = GetComponentInChildren<TMP_Text>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _startPosition = GetComponent<RectTransform>().anchoredPosition;

        _eventBusI = eventBusI;

        _disposables.Add(_eventBusI.Subscribe(EventsName.AddMoney, StartAnimation));
    }

    private void StartAnimation(int value)
    {
        _textChildValue.text = value.ToString("0");

        transform.DOKill();
        _canvasGroup.DOKill();

        GetComponent<RectTransform>().anchoredPosition = _startPosition;
        _canvasGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_canvasGroup.DOFade(1f, 0f));

        float moveDistance = 400f;
        sequence.Join(GetComponent<RectTransform>().DOAnchorPosY(_startPosition.y + moveDistance, 1f));
        sequence.Join(_canvasGroup.DOFade(0f, 0.5f));

        sequence.OnComplete(() =>
        {
            GetComponent<RectTransform>().anchoredPosition = _startPosition;
            _canvasGroup.alpha = 0f;
        });

        sequence.Play();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }
}
