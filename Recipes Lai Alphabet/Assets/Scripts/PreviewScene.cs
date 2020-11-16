using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewScene : MonoBehaviour
{
    public GameObject loadingPanel;
    AsyncOperation loadingOperation;

    void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync("MainMenu");
        loadingOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        Invoke("ActivateLoadingPanel", 2f);
    }

    private void ActivateLoadingPanel()
    {
        if (!loadingPanel.activeInHierarchy)
            loadingPanel.SetActive(true);

        Invoke("LoadMainMenuLevel", 0.5f);
    }

    private void LoadMainMenuLevel()
    {
        loadingOperation.allowSceneActivation = true;
    }

}
