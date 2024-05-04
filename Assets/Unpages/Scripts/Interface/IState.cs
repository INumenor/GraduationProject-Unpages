using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    public StateManager stateManager { get; set; }
    void EnterState();
    void UpdateState();
    void ExitState();
}
