using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }

    public Sprite Icon => throw new NotImplementedException();

    public Text PlayerName { get => playerName; set => playerName = value; }
    public Text PlayerLevel { get => playerLevel; set => playerLevel = value; }

    /// <summary>
    /// A reference to all action buttons
    /// </summary>
    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private Text currentTargetName;

    [SerializeField]
    private Text playerName;

    [SerializeField]
    private Text playerLevel;

    [SerializeField]
    private GameObject tooltip;

    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private CanvasGroup spellBook;

    [SerializeField]
    private CanvasGroup menu;

    [SerializeField]
    private CharacterPanel characterPanel;

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Start()
    {
        PlayerName.text = Player.Instance.CharacterName;
        PlayerLevel.text = $"Уровень: {Player.Instance.Level}";
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) OpenClose(spellBook);
        if (Input.GetKeyDown(KeyCode.Escape)) OpenClose(menu);
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.C)) characterPanel.OpenClose();
        if (Input.GetKeyDown(KeyCode.Minus)) Player.Instance.Health.CurrentValue -= 10;
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.Health.CurrentValue, target.Health.MaxValue);
        portraitFrame.sprite = target.Portrait;
        levelText.text = string.Format("Уровень: " + target.Level.ToString());
        currentTargetName.text = target.CharacterName;
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
        if (target.Level >= Player.Instance.Level + 5) levelText.color = Color.red;
        else if (target.Level == Player.Instance.Level + 3 || target.Level == Player.Instance.Level + 4)
            levelText.color = new Color32(255, 124, 0, 255);
        else if (target.Level >= Player.Instance.Level - 2 && target.Level <= Player.Instance.Level + 2)
            levelText.color = Color.yellow;
        else if (target.Level <= Player.Instance.Level - 3 && target.Level > XPManager.CalculateGrayLevel())
            levelText.color = Color.green;
        else levelText.color = Color.grey;
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    /// <summary>
    /// Updates the targetframe
    /// </summary>
    /// <param name="currentHealth"></param>
    public void UpdateTargetFrame(float currentHealth)
    {
        healthStat.CurrentValue = currentHealth;
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvas)
    {
        canvas.alpha = canvas.alpha > 0 ? 0 : 1;
        canvas.blocksRaycasts = canvas.blocksRaycasts == true ? false : true;
    }

    /// <summary>
    /// Updates the stacksize on a clickable slot
    /// </summary>
    /// <param name="clickable"></param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.Count > 1)
        {
            clickable.StackSize.text = clickable.Count.ToString();
            clickable.StackSize.color = Color.white;
            clickable.Icon.color = Color.white;
        }
        else
        {
            clickable.StackSize.color = new Color(0, 0, 0, 0);
            clickable.Icon.color = Color.white;
        }
        if (clickable.Count == 0)
        {
            clickable.Icon.color = new Color(0, 0, 0, 0);
            clickable.StackSize.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.StackSize.color = new Color(0, 0, 0, 0);
        clickable.Icon.color = Color.white;
    }

    /// <summary>
    /// Shows the tooltip
    /// </summary>
    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
