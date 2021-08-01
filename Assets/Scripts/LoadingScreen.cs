using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen>
{
    [SerializeField] private RectTransform loadingBar;
    [SerializeField] private RectTransform loadingBarFill;
    [SerializeField] private Text loadingText;
    [SerializeField] private GameObject loadingScreen;

    public void UpdateProgress(float procentage, string text = "")
    {
        loadingBarFill.sizeDelta = new Vector2(loadingBar.rect.width * procentage, loadingBar.rect.height);
        if (!loadingText) return;
        if (text.Trim() == "") loadingText.text = "Loading " + (procentage * 100f).ToString("F0") + "%";
        else loadingText.text = text.Trim() + " " + (procentage * 100f).ToString("F0") + "%";
    }

    public void ToggleLoadingScreen(bool status)
    {
        loadingScreen.SetActive(status);
    }
}
