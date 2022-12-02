using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{
    public Animator Animator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        Animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = animatorOverrideController;
    }

    public virtual void SetXAndY(float x, float y)
    {
        Animator.SetFloat("x", x);
        Animator.SetFloat("y", y);
    }

    public void ActivateAnimationLayer(string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }
        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        //if (isFlipped) spriteRenderer.flipX = true;
        animatorOverrideController["Attack_Down"] = animations[0];
        animatorOverrideController["Attack_Left"] = animations[1];
        animatorOverrideController["Attack_Right"] = animations[2];
        animatorOverrideController["Attack_Up"] = animations[3];
        animatorOverrideController["Idle_Down"] = animations[4];
        animatorOverrideController["Idle_Left"] = animations[5];
        animatorOverrideController["Idle_Right"] = animations[6];
        animatorOverrideController["Idle_Up"] = animations[7];
        animatorOverrideController["Run_Down"] = animations[8];
        animatorOverrideController["Run_Left"] = animations[10];
        animatorOverrideController["Run_Right"] = animations[10];
        animatorOverrideController["Run_Up"] = animations[11];
    }

    public void Dequip()
    {
        animatorOverrideController["Attack_Down"] = null;
        animatorOverrideController["Attack_Left"] = null;
        animatorOverrideController["Attack_Right"] = null;
        animatorOverrideController["Attack_Up"] = null;
        animatorOverrideController["Idle_Down"] = null;
        animatorOverrideController["Idle_Left"] = null;
        animatorOverrideController["Idle_Right"] = null;
        animatorOverrideController["Idle_Up"] = null;
        animatorOverrideController["Run_Down"] = null;
        animatorOverrideController["Run_Left"] = null;
        animatorOverrideController["Run_Right"] = null;
        animatorOverrideController["Run_Up"] = null;
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
    }
}
