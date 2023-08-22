using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Tools.NetStatsMonitor;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MovementControls Controls;
    public RuntimeNetStatsMonitor NetStatsMonitor;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("SignedIn...");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
