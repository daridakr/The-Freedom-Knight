using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacterPrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject charaterPrefab;

    [SerializeField]
    private Image charaterPosition;

    public void LoadCharacter()
    {
        var character = new GameObject("Character", typeof(RectTransform));
        var image = character.AddComponent<Image>();
        character.transform.SetParent(transform);
        image.sprite = character.GetComponent<Sprite>();
        // charaterPrefab.transform.SetParent(charaterPosition.transform);
        // charaterPrefab.transform.localPosition = new Vector3(a, b, 100);
    }
}
