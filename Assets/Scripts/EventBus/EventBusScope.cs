using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EventBusScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEventBus<int>();
        builder.RegisterEventBus<bool>();
        builder.RegisterEventBus<string>();
        builder.RegisterEventBus<AudioType>();
    }
}
