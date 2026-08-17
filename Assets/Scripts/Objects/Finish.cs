using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private PlayerStatusType _statusFinish;

    [Inject] private EventBus<int> _eventBusI;

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

        _eventBusI.Publish<PlayerStatusType>(EventsName.CheckStatus, 0, _ =>
        {
            if(_ != _statusFinish)
            {
                _animator.SetTrigger(OpenTrigger);
                _collider.enabled = false;

                if(_ == PlayerStatusType.Rich && _statusFinish == PlayerStatusType.Rich)
                    _eventBusI.Publish(EventsName.Win, 0);
            }
            else
            {
                _eventBusI.Publish(EventsName.Win, 0);
            }
        });
    }
}
