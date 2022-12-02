using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup save;

    [SerializeField]
    private CanvasGroup load;

    [SerializeField]
    private CanvasGroup settings;

    [SerializeField]
    private SavedGame[] saveSlots;

    [SerializeField]
    private CanvasGroup loadSaveWindow;

    private void Update()
    {
        if (Player.Instance.Health.CurrentValue <= 0)
        {
            load.alpha = 1;
            load.blocksRaycasts = true;
            foreach (SavedGame saved in saveSlots)
            {
                SaveManager.Instance.ShowSavedFiles(saved);
            }
        }
    }

    public void Save()
    {
        UIManager.Instance.OpenClose(save);
    }

    public void LoadSave()
    {
        UIManager.Instance.OpenClose(load);
        foreach (SavedGame saved in saveSlots)
        {
            SaveManager.Instance.ShowSavedFiles(saved);
        }
    }

    public void OpenSettings()
    {
        UIManager.Instance.OpenClose(settings);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        UIManager.Instance.OpenClose(loadSaveWindow);
    }
}
