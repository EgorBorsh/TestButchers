using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using VContainer;

public class StatusCharacter : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Image statusIcon;
    [SerializeField] private Image statusBar;

    [Header("Status Settings")]
    [SerializeField] private List<StatusItem> statuses = new List<StatusItem>();

    private EventBus<int> _eventBusI;
    private List<IDisposable> _disposables = new List<IDisposable>();

    private PlayerStatusType _currentStatus;
    private GameObject _currentSkin;

    private int _maxMoney = 400;
    private int _startMoney = 40;

    [Inject]
    public void Construct(EventBus<int> eventBusI)
    {
        _eventBusI = eventBusI;
        _disposables.Add(_eventBusI.Subscribe(EventsName.MoneyHasChanged, OnMoneyChanged));

        _disposables.Add(_eventBusI.Subscribe(EventsName.CheckStatus, GetStatus));

        _currentStatus = GetStatusByMoney(_startMoney);
        ApplyStatus(_currentStatus);
        UpdateStatusBar(_startMoney);
    }


    private PlayerStatusType GetStatus(int i)
    {
        return _currentStatus;
    }


    private void OnMoneyChanged(int money)
    {
        UpdateStatus(money);
    }

    private void UpdateStatus(int money)
    {
        PlayerStatusType newStatus = GetStatusByMoney(money);

        if (newStatus != _currentStatus)
        {
            _currentStatus = newStatus;
            ApplyStatus(_currentStatus);
        }

        UpdateStatusBar(money);
    }

    private PlayerStatusType GetStatusByMoney(int money)
    {
        if (money >= 350)
            return PlayerStatusType.Rich;
        else if (money >= 250)
            return PlayerStatusType.Affluent;
        else if (money >= 40)
            return PlayerStatusType.Poor;
        else
            return PlayerStatusType.Beggar;
    }

    private void ApplyStatus(PlayerStatusType status)
    {
        var data = GetStatusData(status);
        if (data == null) return;

        _eventBusI.Publish(EventsName.ChangeStatus, 0);

        // Выключаем текущий скин
        if (_currentSkin != null)
            _currentSkin.SetActive(false);

        // Включаем новый скин
        if (data.skin != null)
        {
            data.skin.SetActive(true);
            _currentSkin = data.skin;
        }

        if (statusText != null)
            statusText.text = data.statusName;

        if (statusIcon != null)
            statusIcon.sprite = data.icon;

    }

    private StatusItem GetStatusData(PlayerStatusType status)
    {
        foreach (var item in statuses)
        {
            if (item.statusType == status)
                return item;
        }
        return null;
    }

    private void UpdateStatusBar(int money)
    {
        if (statusBar == null) return;

        float fillAmount = Mathf.Clamp01(money / (float)_maxMoney);
        statusBar.DOFillAmount(fillAmount, 0.3f);
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
            disposable?.Dispose();

        _disposables.Clear();
    }
}

public enum PlayerStatusType
{
    Beggar,     // 0-40
    Poor,       // 40-250
    Affluent,   // 250-350
    Rich        // 350+
}

[System.Serializable]
public class StatusItem
{
    public PlayerStatusType statusType;
    public string statusName;
    public Sprite icon;
    public GameObject skin;
}