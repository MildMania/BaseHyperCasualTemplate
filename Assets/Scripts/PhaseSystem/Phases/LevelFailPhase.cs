public class LevelFailPhase : PhaseActionNode
{
    public LevelFailPhase(int id) : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        Logger.Log("Level Fail Phase");
    }

    public void CompletePhase()
    {
        TraverseCompleted();

        GameManager.Instance.SceneManager.LoadCurScene();
    }
}
