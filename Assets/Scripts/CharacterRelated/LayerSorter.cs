using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRenderer;

    private List<Obstacle> obstacles = new List<Obstacle>();

    // Start is called before the first frame update
    void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When the player hits an obstacle
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we hit an obstacle
        if (collision.tag == "Obstacle")
        {
            // creates a reference to the obstacle
            Obstacle obstacle = collision.GetComponent<Obstacle>();
            // if we aren't colliding with anything else or we are colliding with ..., change the sortorder to be behind what we just hit
            if (obstacles.Count == 0 || obstacle.SpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder) parentRenderer.sortingOrder = obstacle.SpriteRenderer.sortingOrder - 1;
            // add the obstacle to the list, so that we can keep track of it
            obstacles.Add(obstacle);
        }
        else if(collision.tag == "RespondentObstacle")
        {
            // creates a reference to the obstacle
            RespondentObstacle obstacle = collision.GetComponent<RespondentObstacle>();
            obstacle.FadeOut();
            // if we aren't colliding with anything else or we are colliding with ..., change the sortorder to be behind what we just hit
            if (obstacles.Count == 0 || obstacle.SpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder) parentRenderer.sortingOrder = obstacle.SpriteRenderer.sortingOrder - 1;
            // add the obstacle to the list, so that we can keep track of it
            obstacles.Add(obstacle);
        }
    }

    /// <summary>
    /// When we stop colliding with a obstacle
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // if we stop colliding with an obstacle
        if(collision.tag == "Obstacle")
        {
            // creates a reference to the obstacle
            Obstacle obstacle = collision.GetComponent<Obstacle>();
            // remove the obstacle from the list
            obstacles.Remove(obstacle);
            // we don't have any obstacles 
            if (obstacles.Count == 0) parentRenderer.sortingOrder = 200;
            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].SpriteRenderer.sortingOrder - 1;
            }
        }
        else if (collision.tag == "RespondentObstacle")
        {
            // creates a reference to the obstacle
            RespondentObstacle obstacle = collision.GetComponent<RespondentObstacle>();
            obstacle.FadeIn();
            // remove the obstacle from the list
            obstacles.Remove(obstacle);
            // we don't have any obstacles 
            if (obstacles.Count == 0) parentRenderer.sortingOrder = 200;
            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].SpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
