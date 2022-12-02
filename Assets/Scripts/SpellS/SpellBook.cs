using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;

    public static SpellBook Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<SpellBook>();
            return instance;
        }
    }

    /// <summary>
    /// A reference to the spell name on the casting bar
    /// </summary>
    [SerializeField]
    private Text currentSpell;

    /// <summary>
    /// A reference to the casting time on the casting bar
    /// </summary>
    [SerializeField]
    private Text castTime;

    /// <summary>
    /// A reference to the icon on the casting bar
    /// </summary>
    [SerializeField]
    private Image icon;

    /// <summary>
    /// A canvas group that is sitting on the casting bar, 
    /// </summary>
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image castingBar;

    /// <summary>
    /// All spells in the spell book
    /// </summary>
    [SerializeField]
    private Spell[] spells;

    /// <summary>
    /// A reference to the coroutine that throws spells
    /// </summary>
    private Coroutine spellRoutine;

    /// <summary>
    /// A reference to the coroutine that fades out the bar
    /// </summary>
    private Coroutine fadeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Cast a spell at an enemy
    /// </summary>
    /// <param name="spellName">The name of the spell that need to cast</param>
    /// <returns></returns>
    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);
        castingBar.fillAmount = 0;
        castingBar.color = spell.BarColor;
        currentSpell.text = spell.Name;
        icon.sprite = spell.Icon;
        spellRoutine = StartCoroutine(Progress(spell));
        fadeRoutine = StartCoroutine(FadeBar());
        
        return spell;
    }

    /// <summary>
    /// Updates the casting bar according to the current progress of the cast
    /// </summary>
    /// <param name="spell">Spell to cast</param>
    /// <returns></returns>
    private IEnumerator Progress(Spell spell)
    {
        // How much time has passed since we started casting the spell
        float timePassed = Time.deltaTime;
        // How fast are we going to move the bar
        float rate = 1.0f / spell.CastTime; // 1 - max
        // The current progress of the cast
        float progress = 0.0f;
        while (progress <= 1.0f)
        {
            // Updates the bar based on the progress
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            // Increase the progress
            progress += rate * Time.deltaTime;
            // Updates the time passed
            timePassed += Time.deltaTime;
            castTime.text = (spell.CastTime - timePassed).ToString("F2"); // two signs after ,
            // Makes sure that the time doesnt go bellow 0
            if (spell.CastTime - timePassed < 0) castTime.text = "0.00";
            yield return null;
        }
        StopCasting();
    }

    /// <summary>
    /// Fades the bar in on the screen when we start casting
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.30f; // how fast it was faded, slower - higher, faster - lower
        float progress = 0.0f;
        while (progress <= 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Stops a cast
    /// </summary>
    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);
        return spell;
    }
}
