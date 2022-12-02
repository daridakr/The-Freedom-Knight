using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private Text questDesctiption;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>(); 

    private static Questlog instance;

    public static Questlog Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Questlog>();
            return instance;
        }
    }

    public List<Quest> Quests { get => quests; set => quests = value; }

    private void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) OpenClose();
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;
            foreach (CollectObjective collectObjective in quest.CollectObjectives)
            {
                InventoryFrameScrit.Instance.itemCountChangedEvent += new ItemCountChanged(collectObjective.UpdateItemCount);
                collectObjective.UpdateItemCount();
            }
            foreach (KillObjective killObjective in quest.KillObjectives)
                GameManager.Instance.killConfirmedEvent += new KillConfirmed(killObjective.UpdateKillCount);
            Quests.Add(quest);
            GameObject gameObject = Instantiate(questPrefab, questParent);
            QuestScript questScript = gameObject.GetComponent<QuestScript>();
            quest.QuestScript = questScript;
            questScripts.Add(questScript);
            questScript.Quest = quest;
            gameObject.GetComponent<Text>().text = quest.Title;
            CheckCompletion();
        }
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest) selected.QuestScript.Deselect();
            string objectives = string.Empty;
            selected = quest;
            string title = quest.Title;
            string description = quest.Description;
            foreach (Objective objective in quest.CollectObjectives) objectives += objective.Type + ": " + objective.CurrentAmount + "/" + objective.Amount + "\n";
            foreach (Objective objective in quest.KillObjectives) objectives += objective.Type + ": " + objective.CurrentAmount + "/" + objective.Amount + "\n";
            questDesctiption.text = string.Format("<b>{0}</b>\n<size=12>{1}</size>\n÷ÂÎË\n<size=12>{2}</size>", title, description, objectives);
        }
    }

    public void CheckCompletion()
    {
        foreach (QuestScript questScript in questScripts)
        {
            questScript.Quest.QuestGiver.UpdateQuestStatus();
            questScript.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1) Close();
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AdandonQuest()
    {
        foreach (CollectObjective collectObjective in selected.CollectObjectives)
        {
            InventoryFrameScrit.Instance.itemCountChangedEvent -= new ItemCountChanged(collectObjective.UpdateItemCount);
        }
        foreach (KillObjective killObjective in selected.KillObjectives)
            GameManager.Instance.killConfirmedEvent -= new KillConfirmed(killObjective.UpdateKillCount);
        RemoveQuest(selected.QuestScript);
    }

    public void RemoveQuest(QuestScript questScript)
    {
        questScripts.Remove(questScript);
        Destroy(questScript.gameObject);
        Quests.Remove(questScript.Quest);
        questDesctiption.text = string.Empty;
        selected = null;
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        questScript.Quest.QuestGiver.UpdateQuestStatus();
        questScript = null;
    }

    public bool HasQuest(Quest quest)
    {
        return Quests.Exists(x => x.Title == quest.Title);
    }
}
