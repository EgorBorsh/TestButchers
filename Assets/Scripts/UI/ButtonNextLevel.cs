using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ButtonNextLevel : MonoBehaviour
{
    [Inject] private EventBus<int> _eventBusI;
    [Inject] private EventBus<AudioType> _eventBusAT;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            _eventBusI.Publish(EventsName.NextLevel, 0);
            _eventBusAT.Publish(EventsName.AudioPlay, AudioType.Click);
        });
    }
}
