using UnityEngine.SceneManagement;

public class LoadSceneManager
{
    public static void LoadScene(string name)
    {
        LoadScene(SceneUtility.GetBuildIndexByScenePath(name));
    }

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        GameManager.Instance.UnPause();
    }
}