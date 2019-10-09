using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;
using TMPro;

public class Player : Bolt.EntityBehaviour<IPlayerState>
{
    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private GameObject canvas;

    private GameObject canvasInstance;
    private float roundTimer;
    private bool _left;
    private bool _right;
    private bool _up;
    private bool _down;

    public override void Attached()
    {
        // This couples the Transform property of the State with the GameObject Transform
        state.SetTransforms(state.Transform, transform);
    }

    private void PollKeys()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _left = true;
            _right = false;
            _up = false;
            _down = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _right = true;
            _left = false;
            _up = false;
            _down = false;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            _up = true;
            _left = false;
            _right = false;
            _down = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _down = true;
            _right = false;
            _up = false;
            _left = false;
        }
    }

    private void Update()
    {
        PollKeys();
        roundTimer += BoltNetwork.FrameDeltaTime;
        if (roundTimer > 10)
        {
            roundTimer = 0;
        }
    }

    public override void SimulateController()
    {
        PollKeys();
        if (canvasInstance)
            if (canvasInstance.GetComponentInChildren<TextMeshProUGUI>())
                canvasInstance.GetComponentInChildren<TextMeshProUGUI>().text = (10 - roundTimer).ToString();
            else
            {
                Debug.LogError("couldn't find text");
            }
        if (roundTimer >= 10)
        {
            IPlayerCommandInput input = PlayerCommand.Create();
            input.Down = _down;
            input.Up = _up;
            input.Left = _left;
            input.Right = _right;
            entity.QueueInput(input);
        }
    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;
        if (resetState)
        {
            transform.position = new Vector3(cmd.Result.Position.x, cmd.Result.Position.y, 0);
            roundTimer = cmd.Result.RoundTimer;
            _left = false;
            _right = false;
            _up = false;
            _down = false;
        }
        else
        {
            if (cmd.Input.Left)
            {
                transform.position = new Vector3(-1, 0, 0);
            }
            else if (cmd.Input.Right)
            {
                transform.position = new Vector3(1, 0, 0);
            }
            else if (cmd.Input.Up)
            {
                transform.position = new Vector3(0, 1, 0);
            }
            else if (cmd.Input.Down)
            {
                transform.position = new Vector3(0, -1, 0);
            }
            cmd.Result.Position = transform.position;
            cmd.Result.RoundTimer = roundTimer;
            _left = false;
            _right = false;
            _up = false;
            _down = false;
        }
    }

    public override void ControlGained()
    {
        StartCoroutine(SetCam());
    }

    private IEnumerator SetCam()
    {
        while (playerCamera == null)
        {
            yield return null;
        }
        while (canvas == null)
        {
        }
        if (BoltNetwork.IsClient)
        {
            GameObject go = Instantiate(playerCamera);
            go.GetComponent<PlayerCamera>().SetTarget(gameObject);
            canvasInstance = Instantiate(canvas);
        }
    }
}