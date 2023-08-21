using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    public event EventHandler OnLeftMove;
    public event EventHandler OnRightMove;
    public event EventHandler OnUpMove;
    public event EventHandler OnUpLeftMove;
    public event EventHandler OnUpRightMove;
    public event EventHandler OnDownMove;
    public event EventHandler OnDownLeftMove;
    public event EventHandler OnDownRightMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void left_move_on_click() => OnLeftMove?.Invoke(this, EventArgs.Empty);
    public void right_move_on_click() => OnRightMove?.Invoke(this, EventArgs.Empty);
    public void up_move_on_click() => OnUpMove?.Invoke(this, EventArgs.Empty);
    public void down_move_on_click() => OnDownMove?.Invoke(this, EventArgs.Empty);
    public void down_left_move_on_click() => OnDownLeftMove?.Invoke(this, EventArgs.Empty);
    public void down_right_move_on_click() => OnDownRightMove?.Invoke(this, EventArgs.Empty);
    public void up_left_move_on_click() => OnUpLeftMove?.Invoke(this, EventArgs.Empty);
    public void up_right_move_on_click() => OnUpRightMove?.Invoke(this, EventArgs.Empty);
}
