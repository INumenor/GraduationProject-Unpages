using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }
    void EnterState();
    void UpdateState();
    void ExitState();
}
