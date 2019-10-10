using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetTarget(GameObject go)
    {
        player = go;

        offset = new Vector3(0, 0, -10);
    }

    private void LateUpdate()
    {
        if (player)
            transform.position = player.transform.position + offset;
    }
}