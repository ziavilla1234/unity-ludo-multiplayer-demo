﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is independent from Reactor Console
/// </summary>

public class ConsoleActivator : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Console.DeveloperConsole.active = !Console.DeveloperConsole.active;
        }
    }
}
