using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Gate : MonoBehaviour
{

    [Inject] private EventBus<AudioType> _eventBusAT;

    private Animator _animator;
    private Collider _collider;
    private static readonly int OpenTrigger = Animator.StringToHash("open");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<CharacterSettings>()) return;
        _animator.SetTrigger(OpenTrigger);
        _collider.enabled = false;
        _eventBusAT.Publish(EventsName.AudioPlay, AudioType.Gate);
    }
}
