using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private AudioClip collectCoins;
    [SerializeField] private AudioClip loseCoins;
    [SerializeField] private AudioClip gate;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip fail;
    [SerializeField] private AudioClip win;

    private AudioSource _source;
    private EventBus<AudioType> _eventBusAT;

    private List<IDisposable> _disposables = new List<IDisposable>();


    [Inject]
    public void Construct(EventBus<AudioType> eventBusAT)
    {
        _source = GetComponent<AudioSource>();
        _eventBusAT = eventBusAT;

        _disposables.Add(_eventBusAT.Subscribe(EventsName.AudioPlay, PlayAudio));
    }

    private void PlayAudio(AudioType type)
    {
        switch (type)
        {
            case AudioType.Gate:
                _source.PlayOneShot(gate);
                break;
            case AudioType.AddPicUp:
                _source.PlayOneShot(collectCoins);
                break;
            case AudioType.TakePickUp:
                _source.PlayOneShot(loseCoins);
                break;
            case AudioType.Click:
                _source.PlayOneShot(click);
                break;
            case AudioType.Fail:
                _source.PlayOneShot(fail);
                break;
            case AudioType.Win:
                _source.PlayOneShot(win);
                break;
        }
    }
}
