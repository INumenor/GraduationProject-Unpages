using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoliderState
{
    public SoliderStateManager soliderStateManager { get; set; }
    void EnterState();
    void UpdateState();
    void ExitState();
}
