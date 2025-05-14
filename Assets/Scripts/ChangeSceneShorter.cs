using UnityEngine;

public class ChangeSceneShorter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeScenetoShorter()
    {
        SceneLoader.Instance.LoadSceneAsync("TrashSortingMinigame");
    }
}
