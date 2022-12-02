using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    [SerializeField]
    private int level;

    [SerializeField]
    private int xp;

    public QuestScript QuestScript { get; set; }

    public QuestGiver QuestGiver { get; set; }

    public string Title { get => title; set => title = value; }
    public string Description { get => description; set => description = value; }
    public CollectObjective[] CollectObjectives { get => collectObjectives; set => collectObjectives = value; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective objective in collectObjectives) if (!objective.IsComplete) return false;
            foreach (Objective objective in KillObjectives) if (!objective.IsComplete) return false;
            return true;
        }
    }

    public KillObjective[] KillObjectives { get => killObjectives; set => killObjectives = value; }
    public int Level { get => level; set => level = value; }
    public int Xp { get => xp; }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int Amount { get => amount; }
    public int CurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string Type { get => type; }

    public bool IsComplete
    {
        get { return CurrentAmount >= Amount; }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmount = InventoryFrameScrit.Instance.GetItemCount(item.Title);
            if (CurrentAmount <= Amount)
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", item.Title, CurrentAmount, Amount));
            Questlog.Instance.UpdateSelected();
            Questlog.Instance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        CurrentAmount = InventoryFrameScrit.Instance.GetItemCount(Type);
        Questlog.Instance.UpdateSelected();
        Questlog.Instance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryFrameScrit.Instance.GetItems(Type, Amount);
        foreach (Item item in items) item.Remove();
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (Type == character.Type)
        {
            CurrentAmount++;
            MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", character.Type, CurrentAmount, Amount));
            Questlog.Instance.CheckCompletion();
            Questlog.Instance.UpdateSelected();
        }
    }
}


