using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToMainMenu());
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(1.8f);
        ScenesManager scenesManager = GameObject.FindObjectOfType<ScenesManager>();
        scenesManager.LoadLevelByName("MainMenu");
    }

}
