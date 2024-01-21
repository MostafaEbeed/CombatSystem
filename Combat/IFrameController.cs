using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IFrameController : MonoBehaviour
{
    [SerializeField] PlayerStateMachine playerStateMachine;
    [SerializeField] Health health;
    
    void Start()
    {
        playerStateMachine.InputReader.dodgeEvent += InitializeIFrames; 
    }

    private void Update()
    {
        
    }

    private void InitializeIFrames()
    {
        health.EnableIFrames();
    }
}
