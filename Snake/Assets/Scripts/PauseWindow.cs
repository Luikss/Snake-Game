﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWindow : MonoBehaviour {
  
    private static PauseWindow instance;

    private void Awake() {
        instance = this;
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public static void ShowStatic() {
        instance.Show();
    }

    public static void HideStatic() {
        instance.Hide();
    }
}