using UnityEngine;
using DG.Tweening;

public class Hand : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float _moveDistance = 50f;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _delayBetweenMoves = 0.2f;
    [SerializeField] private Ease _moveEase = Ease.InOutQuad;

    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private Sequence _sequence;
    private bool _isMoving;

    private void Start()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        _startPosition = _rectTransform.anchoredPosition;
        StartMoving();
    }

    public void StartMoving()
    {
        if (_isMoving) return;
        _isMoving = true;

        StopMoving();

        _sequence = DOTween.Sequence();

        _sequence
            .Append(MoveRight())
            .AppendInterval(_delayBetweenMoves)
            .Append(MoveLeft())
            .AppendInterval(_delayBetweenMoves)
            .SetLoops(-1);

        _sequence.Play();
    }
    public void StopMoving()
    {
        _isMoving = false;
        _sequence?.Kill();
        _sequence = null;
    }

    private Tween MoveRight()
    {
        return _rectTransform.DOAnchorPosX(_startPosition.x + _moveDistance, _moveDuration)
            .SetEase(_moveEase);
    }

    private Tween MoveLeft()
    {
        return _rectTransform.DOAnchorPosX(_startPosition.x, _moveDuration)
            .SetEase(_moveEase);
    }

    private void OnDestroy()
    {
        StopMoving();
    }

    private void OnDisable()
    {
        StopMoving();
    }
}