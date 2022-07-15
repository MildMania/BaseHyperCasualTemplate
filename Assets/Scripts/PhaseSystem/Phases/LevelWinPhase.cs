public class LevelWinPhase : PhaseActionNode
{
    public LevelWinPhase(int id) : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        Logger.Log("Level Win Phase");
    }

    public void CompletePhase()
    {
        TraverseCompleted();

        GameManager.Instance.SceneManager.LoadNextScene();
    }
}