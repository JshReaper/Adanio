using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : BoltSingletonPrefab<PlayerCamera>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetTarget(BoltEntity entity)
    {
        //TODO set cam to follow player
    }
}