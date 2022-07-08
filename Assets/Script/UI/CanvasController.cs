using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject endgameUI, restartButton, nextLevelButton;
    [SerializeField] private TextMeshProUGUI textStackIndicator;

    
  
    private void OnEnable()
    {
        GameManager.ActionLevelPassed += setEndgameUI;
    }
    public void ButtonRestartPressed()
    {
        GameManager.Instance.RestartLevel();
    }
    private void setEndgameUI()
    {
        endgameUI.SetActive(true);
        restartButton.SetActive(false);
    }
    public void UpdateStackIndicatorText(int stackSize)
    {
        textStackIndicator.text = stackSize.ToString();
    }

    private void OnDisable()
    {
        GameManager.ActionLevelPassed -= setEndgameUI;
    }
    public void ButtonNextLevelPressed()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
