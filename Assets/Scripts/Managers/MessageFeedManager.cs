using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour
{
    private static MessageFeedManager instance;

    public static MessageFeedManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MessageFeedManager>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject messagePrefab;

    public void WriteMessage(string message)
    {
        GameObject gameObject = Instantiate(messagePrefab, transform);
        gameObject.GetComponent<Text>().text = message;
        gameObject.transform.SetAsFirstSibling();
        Destroy(gameObject, 2);
    }
}
