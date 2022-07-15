public class PostGamePhase : PhaseActionNode
{   
    public PostGamePhase(int id) : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        Logger.Log("Post Game Phase");

        TraverseCompleted();
    }
}
