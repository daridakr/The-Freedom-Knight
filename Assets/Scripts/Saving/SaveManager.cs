using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] items;

    private Chest[] chests;

    private CharButton[] equipment;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private SavedGame[] saveSlots;

    private static SaveManager instance;

    [SerializeField]
    private CanvasGroup saveWindow;

    public static SaveManager Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<SaveManager>();
            return instance;
        }
    }


    private string action;
 
    void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();
        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);
        }
    }

    public void ShowDialoge(GameObject clickButton)
    {
        action = clickButton.name;
        switch(action)
        {
            case "Load":
                Load(clickButton.GetComponentInParent<SavedGame>());
                break;
            case "Save":
                Save(clickButton.GetComponentInParent<SavedGame>());
                break;
            case "Delete":
                Delete(clickButton.GetComponentInParent<SavedGame>());
                break;
            case "Back":
                Back();
                break;
        }
    }

    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }

    private void Back()
    {
        UIManager.Instance.OpenClose(saveWindow);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F5)) Save();
        //if (Input.GetKeyDown(KeyCode.F9)) Load();
    }

    public void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)binaryFormatter.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);
            SaveData data = new SaveData();
            data.Scene = SceneManager.GetActiveScene().name;
            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveActionButtons(data);
            SaveQuests(data);
            SaveQuestGivers(data);
            binaryFormatter.Serialize(file, data);
            file.Close();
            ShowSavedFiles(savedGame);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.PlayerData = new PlayerData(Player.Instance.Level,
            Player.Instance.XpStat.CurrentValue,Player.Instance.XpStat.MaxValue,
            Player.Instance.Health.CurrentValue,Player.Instance.Health.MaxValue,
            Player.Instance.Mana.CurrentValue,Player.Instance.Mana.MaxValue,
            Player.Instance.transform.position,Player.Instance.Money,
            Player.Instance.CharacterName);
    }

    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            data.ChestData.Add(new ChestData(chests[i].name));
            foreach (Item item in chests[i].Items)
                if (chests[i].Items.Count > 0)
                    data.ChestData[i].Items.Add(new ItemData(item.Title, item.Slot.Items.Count, item.Slot.Index));
        }
    }

    public void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryFrameScrit.Instance.Bags.Count; i++)
        {
            data.InventoryData.Bags.Add(new BagData(InventoryFrameScrit.Instance.Bags[i].SlotCount, InventoryFrameScrit.Instance.Bags[i].BagButton.BagIndex)); ;
        }
    }

    public void SaveEquipment(SaveData data)
    {
        foreach (CharButton charButton in equipment)
            if (charButton.EquippedArmor != null)
                data.EquipmentData.Add(new EquipmentData(charButton.EquippedArmor.Title, charButton.name));
    }

    public void SaveActionButtons(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].Useable != null)
            {
                ActionButtonData action;
                if (actionButtons[i].Useable is Spell)
                {
                    action = new ActionButtonData((actionButtons[i].Useable as Spell).Name, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].Useable as Item).Title, true, i);
                }
                data.ActionButtonData.Add(action);
            }   
        }
    }

    public void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryFrameScrit.Instance.GetAllItems();
        foreach (SlotScript slot in slots)
            data.InventoryData.Items.Add(new ItemData(slot.Item.Title, slot.Items.Count, slot.Index, slot.Inventory.Index));
    }

    public void SaveQuests(SaveData data)
    {
        foreach (Quest quest in Questlog.Instance.Quests)
            data.QuestData.Add(new QuestData(quest.Title, quest.Description, quest.CollectObjectives, quest.KillObjectives, quest.QuestGiver.QuestGiverId));
    }

    public void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestGiver questGiver in questGivers)
            data.QuestGiverData.Add(new QuestGiverData(questGiver.QuestGiverId, questGiver.CompletedQuests));
    }

    private void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)binaryFormatter.Deserialize(file);
            file.Close();
            LoadEquipment(data);
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadChests(data);
            LoadActionButtons(data);
            LoadQuests(data);
            LoadQuestGivers(data);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.Instance.Level = data.PlayerData.Level;
        Player.Instance.UpdateLevel();
        Player.Instance.Health.Initialize(data.PlayerData.Health, data.PlayerData.MaxHealth);
        Player.Instance.Mana.Initialize(data.PlayerData.Mana, data.PlayerData.MaxMana);
        Player.Instance.XpStat.Initialize(data.PlayerData.Xp, data.PlayerData.MaxXp);
        Player.Instance.transform.position = new Vector2(data.PlayerData.X, data.PlayerData.Y);
    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestData chestData in data.ChestData)
        {
            Chest chest = Array.Find(chests, x => x.name == chestData.Name);
            foreach (ItemData itemData in chestData.Items)
            {
                Item item = Array.Find(items, x => x.Title == itemData.Title);
                item.Slot = chest.Inventory.Slots.Find(x => x.Index == itemData.SlotIndex);
                chest.Items.Add(item);
            }
        }
    }

    public void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.InventoryData.Bags)
        {
            Bag newBag = (Bag)Instantiate(items[0]);
            newBag.Initialize(bagData.SlotCount);
            InventoryFrameScrit.Instance.AddBag(newBag, bagData.BagIndex);
        }
    }

    public void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.EquipmentData)
        {
            CharButton charButton = Array.Find(equipment, x => x.name == equipmentData.Type);
            charButton.EquipArmor(Array.Find(items, x => x.Title == equipmentData.Title) as Armory);
        }
    }

    public void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.ActionButtonData)
        {
            if (buttonData.IsItem)
                actionButtons[buttonData.Index].SetUseable(InventoryFrameScrit.Instance.GetUseable(buttonData.Action));
            else actionButtons[buttonData.Index].SetUseable(SpellBook.Instance.GetSpell(buttonData.Action));
        }
    }

    public void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.InventoryData.Items)
        {
            Item item = Array.Find(items, x => x.Title == itemData.Title);
            for (int i = 0; i < itemData.StackCount; i++)
            {
                InventoryFrameScrit.Instance.PlaceInSpecific(item, itemData.SlotIndex, itemData.InventoryIndex);
            }
        }
    }

    public void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestData questData in data.QuestData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.QuestGiverId == questData.QuestGiverId);
            Quest quest = Array.Find(questGiver.Quests, x => x.Title == questData.Title);
            quest.QuestGiver = questGiver;
            quest.KillObjectives = questData.KillObjectives;
            Questlog.Instance.AcceptQuest(quest);
        }
    }

    public void LoadQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestGiverData questGiverData in data.QuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.QuestGiverId == questGiverData.QuestGiverId);
            questGiver.CompletedQuests = questGiverData.CompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}
