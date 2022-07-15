using UnityEngine;

public class PreGamePhase : PhaseActionNode
{
    public PreGamePhase(int id) : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        Logger.Log("Pre Game Phase");
        TraverseCompleted();
    }
}