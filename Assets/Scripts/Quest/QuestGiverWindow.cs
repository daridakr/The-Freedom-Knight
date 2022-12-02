using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;

    public static QuestGiverWindow Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<QuestGiverWindow>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject backButton, acceptButton, completeButton, questDescription;

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questArea;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;
        foreach (GameObject gameObject1 in quests) Destroy(gameObject1);
        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);
        foreach (Quest quest in questGiver.Quests)
        {
            if (quest != null)
            {
                GameObject gameObject = Instantiate(questPrefab, questArea);
                gameObject.GetComponent<Text>().text = "[" + quest.Level + "] " + quest.Title + "<color=#ffbb04> <size=12>!</size></color>";

                gameObject.GetComponent<QGQuestScript>().Quest = quest;
                quests.Add(gameObject);
                if (Questlog.Instance.HasQuest(quest) && quest.IsComplete) gameObject.GetComponent<Text>().text = quest.Title + "<color=#ffbb04> <size=12>?</size></color>";
                else if (Questlog.Instance.HasQuest(quest))
                {
                    Color color = gameObject.GetComponent<Text>().color;
                    color.a = 0.5f;
                    gameObject.GetComponent<Text>().color = color;
                    gameObject.GetComponent<Text>().text = quest.Title + "<color=#c0c0c0ff> <size=12>?</size></color>";
                }
            }
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests((npc as QuestGiver));
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        selectedQuest = quest;
        if (Questlog.Instance.HasQuest(quest) && quest.IsComplete)
        {
            acceptButton.SetActive(false);
            completeButton.SetActive(true);
        }
        else if (!Questlog.Instance.HasQuest(quest)) acceptButton.SetActive(true);
        backButton.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);
        string title = quest.Title;
        string description = quest.Description;
        string objectives = string.Empty;
        foreach (Objective objective in quest.CollectObjectives) objectives += objective.Type + ": " + objective.CurrentAmount + "/" + objective.Amount + "\n";
        questDescription.GetComponent<Text>().text = string.Format("<b>{0}</b>\n<size=12>{1}</size>\n", title, description);
    }

    public void Back()
    {
        backButton.SetActive(false);
        acceptButton.SetActive(false);
        ShowQuests(questGiver);
        completeButton.SetActive(false);
    }

    public void Accept()
    {
        Questlog.Instance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeButton.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.Quests.Length; i++)
            {
                if (selectedQuest == questGiver.Quests[i])
                {
                    questGiver.CompletedQuests.Add(selectedQuest.Title);
                    questGiver.Quests[i] = null;
                    selectedQuest.QuestGiver.UpdateQuestStatus();
                }
            }
            foreach (CollectObjective collectObjective in selectedQuest.CollectObjectives)
            {
                InventoryFrameScrit.Instance.itemCountChangedEvent -= new ItemCountChanged(collectObjective.UpdateItemCount);
                collectObjective.Complete();
            }
            foreach (KillObjective killObjective in selectedQuest.KillObjectives)
                GameManager.Instance.killConfirmedEvent -= new KillConfirmed(killObjective.UpdateKillCount);
            Player.Instance.GainXp(XPManager.CalculateXP(selectedQuest));
            Questlog.Instance.RemoveQuest(selectedQuest.QuestScript);
            Back();
        }
    }
}
