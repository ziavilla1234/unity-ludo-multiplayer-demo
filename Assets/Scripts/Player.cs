using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int PlayerId;
    public MovementControls Controls;
    public float MoveDistanceStep = .5f;
    public float MoveStepDuration = .5f;

    enum MoveDirection { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight  }
    bool _is_already_moving = false;

    Vector3 get_move_vector(MoveDirection dir) => dir switch
    {
        MoveDirection.Up => new Vector3((Vector2.up * MoveDistanceStep).x, (Vector2.up * MoveDistanceStep).y),

        MoveDirection.UpRight => get_move_vector(MoveDirection.Up) + get_move_vector(MoveDirection.Right),
        MoveDirection.UpLeft => get_move_vector(MoveDirection.Up) + get_move_vector(MoveDirection.Left),

        MoveDirection.Down => new Vector3((Vector2.down * MoveDistanceStep).x, (Vector2.down * MoveDistanceStep).y),

        MoveDirection.DownRight => get_move_vector(MoveDirection.Down) + get_move_vector(MoveDirection.Right),
        MoveDirection.DownLeft => get_move_vector(MoveDirection.Down) + get_move_vector(MoveDirection.Left),

        MoveDirection.Left => new Vector3((Vector2.left * MoveDistanceStep).x, (Vector2.left * MoveDistanceStep).y),
        MoveDirection.Right => new Vector3((Vector2.right * MoveDistanceStep).x, (Vector2.right * MoveDistanceStep).y),
        _ => throw new System.Exception("no dir")
    };
    IEnumerator MoveToDirection(MoveDirection dir)
    {
        if (_is_already_moving) yield break;

        _is_already_moving = true;

        var to_pos = transform.position + get_move_vector(dir);

        float time = 0;
        Vector3 start = transform.position;

        while(time < MoveStepDuration)
        {
            transform.position = Vector3.Lerp(start, to_pos, time / MoveStepDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = to_pos;

        _is_already_moving = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Controls.OnLeftMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.Left));
        Controls.OnRightMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.Right));
        Controls.OnUpMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.Up));
        Controls.OnDownMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.Down));

        Controls.OnUpRightMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.UpRight));
        Controls.OnUpLeftMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.UpLeft));

        Controls.OnDownRightMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.DownRight));
        Controls.OnDownLeftMove += (sender, e) => StartCoroutine(MoveToDirection(MoveDirection.DownLeft));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
