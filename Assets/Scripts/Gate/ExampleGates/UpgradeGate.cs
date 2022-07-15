using UnityEngine;


public class UpgradeGate : GateBase
{
    protected override void OnEnterCustomActions()
    {
        Logger.Log("Upgrade Gate");
    }
}