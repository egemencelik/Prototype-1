using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TileManager tileManager;


    public void UpdateSlider()
    {
        slider.value = tileManager.CompletionPercentage;
        if (Math.Abs(tileManager.CompletionPercentage - 100) < .1f)
        {
            StartCoroutine(LoadIndexAfterSeconds(1, 1));
        }
    }

    public static IEnumerator LoadIndexAfterSeconds(int index, int sec)
    {
        yield return new WaitForSeconds(sec);
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + index)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
        }
    }
}