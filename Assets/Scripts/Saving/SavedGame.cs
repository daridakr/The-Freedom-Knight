using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;
    [SerializeField]
    private Image health;
    [SerializeField]
    private Image mana;
    [SerializeField]
    private Image xp;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private GameObject visuals;
    [SerializeField]
    private int index;

    public int Index { get => index; }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData data)
    {
        visuals.SetActive(true);
        dateTime.text = "Дата: " + data.DateTime.ToString("dd/MM/yyy") + " - Время: " + data.DateTime.ToString("H:mm");
        health.fillAmount = data.PlayerData.Health / data.PlayerData.MaxHealth;
        mana.fillAmount = data.PlayerData.Mana / data.PlayerData.MaxMana;
        xp.fillAmount = data.PlayerData.Xp / data.PlayerData.MaxXp;
        levelText.text = data.PlayerData.Level.ToString();
        //portrait.sprite = data.PlayerData.Portrait.sprite;
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
