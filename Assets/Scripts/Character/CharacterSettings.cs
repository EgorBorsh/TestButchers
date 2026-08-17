using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSettings : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _maxRotationSpeed = 45f;

    [Header("Управление")]
    [SerializeField]
    private float _sensitivity = 5f;
    [SerializeField]
    private float _trackWidth = 3f;


    public float Speed => _speed;
    public float MaxRotationSpeed => _maxRotationSpeed;
    public float Sensitivity => _sensitivity;
    public float TrackWidth => _trackWidth;
}
