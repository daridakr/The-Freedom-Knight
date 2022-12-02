using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    /// <summary>
    /// The obstacles sprite renderer
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; set; }

    /// <summary>
    /// Compare to, that is used for sorting the obstacles
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Obstacle other)
    {
        if (SpriteRenderer.sortingOrder > other.SpriteRenderer.sortingOrder) return 1;
        else if (SpriteRenderer.sortingOrder < other.SpriteRenderer.sortingOrder) return -1;
        return 0;
    } 

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
