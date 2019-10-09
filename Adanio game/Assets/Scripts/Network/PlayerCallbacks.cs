using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Client, "game")]
public class PlayerCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // this just instantiates our player camera,
        // the Instantiate() method is supplied by the BoltSingletonPrefab<T> class
    }
}