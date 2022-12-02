using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSilver, exclemation;

    [SerializeField]
    private Sprite mini_question, mini_questionSilver, mini_exclemation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int questGiverId;

    private List<string> completedQuests = new List<string>();

    [SerializeField]
    private SpriteRenderer minimapRenderer;

    public Quest[] Quests { get => quests; }
    public int QuestGiverId { get => questGiverId; }
    public List<string> CompletedQuests
    {
        get { return completedQuests; }
        set
        {
            completedQuests = value;
            foreach (string title in completedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].Title == title) quests[i] = null;
                }
            }
        }
    }

    private void Start()
    {
        foreach (Quest quest in quests) quest.QuestGiver = this;
    }

    public void UpdateQuestStatus()
    {
        int count = 0;
        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    minimapRenderer.sprite = mini_question;
                    break;
                }
                else if (!Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclemation;
                    minimapRenderer.sprite = mini_exclemation;
                    break;
                }
                else if (!quest.IsComplete && Questlog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    minimapRenderer.sprite = mini_questionSilver;
                }
            }
            else
            {
                count++;
                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    minimapRenderer.enabled = false;
                }
            }
        }
    }
}
