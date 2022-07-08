using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionLevelPassed;
    private static int currLevel = 0;
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadNextLevel()
    {
        currLevel+=1;
        Debug.Log(currLevel);
        SceneManager.LoadScene(currLevel);
    }
}

