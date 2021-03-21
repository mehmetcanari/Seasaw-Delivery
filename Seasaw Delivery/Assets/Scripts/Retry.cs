using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    int sceneNumber;
    private void Start()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void NextLevel()
    {
        if (sceneNumber == 0)
        {
            SceneManager.LoadScene(sceneNumber + 1);
        }
        if (sceneNumber != 0)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}






