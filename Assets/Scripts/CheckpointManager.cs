using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public Sprite firstPlace;
    public Sprite secondPlace;
    public Image display;

    public GameObject winBar;
    public GameObject lossBar;
    public GameObject playAgainButton;
    public GameObject exitButton;

    public CameraController mainCamera;

    private void Awake()
    {
        winBar.gameObject.SetActive(false);
        lossBar.gameObject.SetActive(false);
    }

    public void PlayerWinning()
    {
        display.sprite = firstPlace;
    }

    public void AIWinning()
    {
        display.sprite = secondPlace;
    }

    public void PlayerWins()
    {
        winBar.SetActive(true);
        EndGame();
    }

    public void AIWins()
    {
        lossBar.SetActive(true);
        EndGame();
    }

    void EndGame()
    {
        mainCamera.raceFinished = true;
        playAgainButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
