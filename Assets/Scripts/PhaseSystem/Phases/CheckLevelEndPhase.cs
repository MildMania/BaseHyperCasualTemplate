using ProgressionSystem;
using System;

public class CheckLevelEndPhase : PhaseConditionalNode
{
    private bool _isLevelSucceed = false;
    
    public CheckLevelEndPhase(int id, params PhaseBaseNode[] childNodeArr) : base(id, childNodeArr)
    {
    }

    protected override void CheckConditions(Action<int> callback)
    {
        Logger.Log("Check Level End Phase");

        if (ProgressionManager.Instance.ActiveProgression.GetProgressionType().Equals(EProgressionResult.Success))
        {
            callback?.Invoke(41);

            return;
        }

        callback?.Invoke(42);
    }
}
