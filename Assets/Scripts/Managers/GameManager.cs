using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(DelayedStartRoutine());
    }

    private IEnumerator DelayedStartRoutine()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync("MainMenu_1");
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadSceneAsync("Level_" + index);
    }
}
