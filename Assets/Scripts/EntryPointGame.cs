using ButchersGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EntryPointGame : MonoBehaviour
{
    [SerializeField]
    private Transform _characterRoot;
    [SerializeField]
    private Transform _characterChild;
    [SerializeField]
    private LevelManager _levelManager;
    [SerializeField]
    private CharacterSettings _settings;

    private CharacterMovementController _playerMovement;
    private GameManager _gameManager;
    private CharacterMoney _ñharacterMoney;

    [Inject] private IObjectResolver _container;
    [Inject] private EventBus<int> _eventBusI;

    private List<IDisposable> _disposables = new List<IDisposable>();

    private bool isStarted = false;

    private void Start()
    {
        _disposables.Add(_eventBusI.Subscribe(EventsName.SetPathFollower, SetPathFollower));

        _gameManager = new GameManager();
        _ñharacterMoney = new CharacterMoney();

        _container.Inject(_gameManager);
        _container.Inject(_ñharacterMoney);

        _disposables.Add(_eventBusI.Subscribe(EventsName.OneMove, StartGame));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Fail, SetFalseIsStarted));
        _disposables.Add(_eventBusI.Subscribe(EventsName.Win, SetFalseIsStarted));
    }


    private void SetPathFollower(int i)
    {
        _container.InjectGameObject(_levelManager.gameObject);

        _playerMovement = new CharacterMovementController(_characterRoot, _characterChild, _levelManager.GetComponentInChildren<PathFollower>(), _settings);

        _container.Inject(_playerMovement);
    }

    private void StartGame(int obj)
    {
        isStarted = true;
    }

    private void SetFalseIsStarted(int obj)
    {
        isStarted = false;
    }

    private void Update()
    {
        if (!isStarted) return;

        _playerMovement?.UpdateMovement();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();

        _playerMovement?.Dispose();
        _gameManager?.Dispose();
        _ñharacterMoney?.Dispose();   
    }
}
