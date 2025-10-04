using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneLoader.Instance.LoadScene("Tiltle");
    }

    public void LoadInGameScene()
    {
        SceneLoader.Instance.LoadScene("Ingame");
    }

    public void LoadGameOverScene()
    {
        SceneLoader.Instance.LoadScene("GameOver");
    }

    public void LoadClearScene()
    {
        SceneLoader.Instance.LoadScene("GameClear");
    }
}
