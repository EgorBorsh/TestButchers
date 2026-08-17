using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Door : MonoBehaviour
{
    [SerializeField] private Door related;
    [SerializeField] private bool isNegative;
    [SerializeField] private int value = 1;

    [Inject] private EventBus<int> _eventBusI;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<CharacterSettings>()) return;

        if (isNegative) _eventBusI.Publish(EventsName.PickedUpTheBadStuff, value);
        else _eventBusI.Publish(EventsName.PickedUpTheMoney, value);

        Destroy(gameObject);
        Destroy(related.gameObject);
    }
}
