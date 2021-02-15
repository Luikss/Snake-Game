using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Loading {
 
    public enum Scene {
        Main,
    }

    public static void Load(Scene scene) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
    }
}