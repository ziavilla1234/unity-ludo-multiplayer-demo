using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int PlayerId;
    
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

    IEnumerator move_to_direction_co(MoveDirection dir)
    {
        if (_is_already_moving) yield break;

        _is_already_moving = true;

        var to_pos = transform.position + get_move_vector(dir);

        float time = 0;
        Vector3 start = transform.position;

        while (time < MoveStepDuration)
        {
            var new_pos = Vector3.Lerp(start, to_pos, time / MoveStepDuration);
            update_pos_ServerRpc(new_pos);
            time += Time.deltaTime;
            yield return null;
        }
        update_pos_ServerRpc(to_pos);

        yield return new WaitForSeconds(.3f);

        _is_already_moving = false;
    }

    [ServerRpc]
    void update_pos_ServerRpc(Vector3 pos)
    {
        transform.position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;

        GameManager.Instance.Controls.OnLeftMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.Left));
        GameManager.Instance.Controls.OnRightMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.Right));
        GameManager.Instance.Controls.OnUpMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.Up));
        GameManager.Instance.Controls.OnDownMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.Down));

        GameManager.Instance.Controls.OnUpRightMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.UpRight));
        GameManager.Instance.Controls.OnUpLeftMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.UpLeft));

        GameManager.Instance.Controls.OnDownRightMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.DownRight));
        GameManager.Instance.Controls.OnDownLeftMove += (sender, e) => StartCoroutine(move_to_direction_co(MoveDirection.DownLeft));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
