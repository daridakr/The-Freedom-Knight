using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;

    public static GameManager Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Layer mask number code with clickable objects
    /// </summary>
    private int clickableLayerMaskCode = 512;

    /// <summary>
    /// A reference to the player object
    /// </summary>
    [SerializeField]
    private Player player;

    private Enemy currentTarget;

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // makes a raycats from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMaskCode);
            if (hit.collider != null && hit.collider.tag == "Enemy") // if we hit smth
            {
                if (currentTarget != null) // if we have a current target 
                {
                    currentTarget.DeSelect(); // deselect the current target
                }
                currentTarget = hit.collider.GetComponent<Enemy>(); // selects the new target
                player.Target = currentTarget.Select(); // gives the player the new target
                UIManager.Instance.ShowTargetFrame(currentTarget);
            }
            else
            {
                UIManager.Instance.HideTargetFrame();
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.Target = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMaskCode);
            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.Interactable)
                player.Interact();
        }
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null) killConfirmedEvent(character);
    }
}