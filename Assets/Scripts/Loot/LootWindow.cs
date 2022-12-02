using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;

    [SerializeField]
    private LootButton[] lootButtons;

    private CanvasGroup canvasGroup;

    private List<List<Item>> pages = new List<List<Item>>();

    private List<Item> droppedLoot = new List<Item>();

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private Text lootOwner;

    [SerializeField]
    private GameObject nextButton, previosButton;

    [SerializeField]
    private Item[] items;

    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }
    }

    public static LootWindow Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<LootWindow>();
            return instance;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void CreatePages(List<Item> items, Enemy owner)
    {
        if (!IsOpen)
        {
            lootOwner.text = owner.CharacterName;
            List<Item> page = new List<Item>();
            droppedLoot = items;
            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);
                if (page.Count == 4 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Item>();
                }
            }
            AddLoot();
            Open();
        }    
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;
            previosButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].Icon.sprite = pages[pageIndex][i].Icon;
                    lootButtons[i].Loot = pages[pageIndex][i];
                    lootButtons[i].gameObject.SetActive(true);
                    string title = string.Format("<color={0}>{1}</color>", QualityColor.Colors[pages[pageIndex][i].Quality], pages[pageIndex][i].Title);
                    lootButtons[i].Title.text = title;
                }
            }
        }
    }

    public void ClearButtons()
    {
        foreach (LootButton button in lootButtons) button.gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviosPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        pages[pageIndex].Remove(loot);
        droppedLoot.Remove(loot);
        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);
            if (pageIndex == pages.Count && pageIndex > 0) pageIndex--;
            AddLoot();
        }

    }

    public void Close()
    {
        pages.Clear();
        canvasGroup.alpha = 0;
        ClearButtons(); 
        //canvasGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
