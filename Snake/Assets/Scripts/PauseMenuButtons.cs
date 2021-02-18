using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour {
   
    public void PauseToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void PauseToResume() {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }
}