using ButchersGames;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class CharacterMovementController : IDisposable
{
    private readonly Transform _characterRoot;
    private readonly Transform _characterChild;
    private readonly PathFollower _pathFollower;
    private readonly CharacterSettings _settings;

    private InputController _inputController;
    [Inject] private EventBus<int> _eventBusI;

    private List<IDisposable> _disposables = new List<IDisposable>();

    private float _currentRotation;
    private float _currentHorizontalOffset;
    private bool isStarted = false;

    public CharacterMovementController(Transform characterRoot, Transform characterChild, PathFollower pathFollower, CharacterSettings settings)
    {
        _characterRoot = characterRoot;
        _characterChild = characterChild;
        _pathFollower = pathFollower;
        _settings = settings;
    }

    [Inject]
    public void Construct(InputController inputController)
    {
        _inputController = inputController;
        _inputController.Enable();

        _inputController.PlayerController.Move.performed += OnMovePerformed;

        _disposables.Add(_eventBusI.Subscribe(EventsName.Reset, ResetToStart));
        _disposables.Add(_eventBusI.Subscribe(EventsName.NextLevel, ResetToStart));
    }

    public void Dispose()
    {
        _inputController.PlayerController.Move.performed -= OnMovePerformed;

        foreach (var disposable in _disposables) disposable.Dispose();

        _disposables.Clear();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!isStarted)
        {
            _eventBusI.Publish(EventsName.OneMove, 0);
            isStarted = true;
        }

        float moveValue = context.ReadValue<float>();
        UpdateHorizontalPosition(moveValue);
    }

    private void UpdateHorizontalPosition(float moveValue)
    {
        float deltaPosition = moveValue * _settings.Sensitivity * Time.deltaTime;
        float targetPosition = _currentHorizontalOffset + deltaPosition;

        float halfTrack = _settings.TrackWidth / 2f;
        if (targetPosition > halfTrack || targetPosition < -halfTrack)
            return;

        _currentHorizontalOffset = targetPosition;

        Vector3 localPos = _characterChild.localPosition;
        localPos.x = _currentHorizontalOffset;
        _characterChild.localPosition = localPos;
    }

    public void UpdateMovement()
    {
        if (_pathFollower.IsPathComplete())
            return;

        float targetRotation = _pathFollower.GetTargetRotation(_characterRoot.position);

        Quaternion currentQuat = Quaternion.AngleAxis(_currentRotation, Vector3.up);
        Quaternion targetQuat = Quaternion.AngleAxis(targetRotation, Vector3.up);

        float rotationSpeed = _settings.MaxRotationSpeed * Time.deltaTime;
        _characterRoot.rotation = Quaternion.Slerp(currentQuat, targetQuat, rotationSpeed);

        _currentRotation = _characterRoot.rotation.eulerAngles.y;
        if (_currentRotation > 180f) _currentRotation -= 360f;

        Vector3 forwardMovement = _characterRoot.forward * _settings.Speed * Time.deltaTime;
        _characterRoot.position += forwardMovement;
    }

    private void ResetToStart(int i)
    {
        _characterRoot.position = LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex].PlayerSpawnPoint;
        _characterRoot.rotation = Quaternion.identity;
        _currentRotation = 0f;

        _currentHorizontalOffset = 0f;
        _characterChild.localPosition = Vector3.zero;

        isStarted = false;

        _pathFollower.ResetPath();
    }
}