using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    public PlayerData PlayerData { get; set; }

    public List<ChestData> ChestData { get; set; }

    public List<EquipmentData> EquipmentData { get; set; }

    public InventoryData InventoryData { get; set; }

    public List<ActionButtonData> ActionButtonData { get; set; }

    public List<QuestData> QuestData { get; set; }

    public List<QuestGiverData> QuestGiverData { get; set; }

    public DateTime DateTime { get; set; }

    public string Scene { get; set; }

    public SaveData()
    {
        InventoryData = new InventoryData();
        ChestData = new List<ChestData>();
        ActionButtonData = new List<ActionButtonData>();
        EquipmentData = new List<EquipmentData>();
        QuestData = new List<QuestData>();
        QuestGiverData = new List<QuestGiverData>();
        DateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public int Level { get; set; }

    public float Xp { get; set; }

    public float MaxXp { get; set; }

    public float Health { get; set; }

    public float MaxHealth { get; set; }

    public float Mana { get; set; }

    public float MaxMana { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public int Money { get; set; }

    public string Name { get; set; }

    //public Image Portrait { get; set; }

    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, Vector2 position, int money, string name)
    {
        Level = level;
        Xp = xp;
        MaxXp = maxXp;
        Health = health;
        MaxHealth = maxHealth;
        Mana = mana;
        MaxMana = maxMana;
        X = position.x;
        Y = position.y;
        Money = money;
        Name = name;
        //Portrait = portrait;
    }
}

[Serializable]
public class ItemData
{ 
    public string Title { get; set; }

    public int StackCount { get; set; }

    public int SlotIndex { get; set; }

    public int InventoryIndex { get; set; }

    public ItemData(string title, int stackCount = 0, int slotIndex = 0, int inventory = 0)
    {
        Title = title;
        StackCount = stackCount;
        SlotIndex = slotIndex;
        InventoryIndex = inventory;
    }
}

[Serializable]
public class ChestData
{
    public string Name { get; set; }

    public List<ItemData> Items { get; set; }

    public ChestData(string name)
    {
        Name = name;
        Items = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> Bags { get; set; }

    public List<ItemData> Items { get; set; }

    public InventoryData()
    {
        Bags = new List<BagData>();
        Items = new List<ItemData>();
    }
}

[Serializable]
public class BagData
{
    public int SlotCount { get; set; }

    public int BagIndex { get; set; }

    public BagData(int count, int index)
    {
        SlotCount = count;
        BagIndex = index;
    }
}

[Serializable]
public class EquipmentData
{
    public string Title { get; set; }
    public string Type { get; set; }

    public EquipmentData(string title, string type)
    {
        Title = title;
        Type = type;
    }
}

[Serializable]
public class ActionButtonData
{ 
    public string Action { get; set; }
    public bool IsItem { get; set; }
    public int Index { get; set; }

    public ActionButtonData(string action, bool isItem, int index)
    {
        Action = action;
        IsItem = isItem;
        Index = index;
    }
}

[Serializable]
public class QuestData
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CollectObjective[] CollectObjectives { get; set; }
    public KillObjective[] KillObjectives { get; set; }
    public int QuestGiverId { get; set; }

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjective, int questGiverId)
    {
        Title = title;
        Description = description;
        CollectObjectives = collectObjectives;
        KillObjectives = killObjective;
        QuestGiverId = questGiverId;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> CompletedQuests { get; set; }

    public int QuestGiverId { get; set; }

    public QuestGiverData(int questGiverId, List<string> copmletedQuests)
    {
        QuestGiverId = questGiverId;
        CompletedQuests = copmletedQuests;
    }
}
