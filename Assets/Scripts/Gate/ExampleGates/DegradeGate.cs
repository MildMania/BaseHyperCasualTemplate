using UnityEngine;


public class DegradeGate : GateBase
{
    protected override void OnEnterCustomActions()
    {
        Logger.Log("Degrade Gate");
    }
}