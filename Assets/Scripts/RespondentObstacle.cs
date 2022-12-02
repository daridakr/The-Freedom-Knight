using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondentObstacle : Obstacle
{
    private Color defaultColor;
    private Color fadedColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = SpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    public void FadeOut()
    {
        SpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        SpriteRenderer.color = defaultColor;
    }
}
