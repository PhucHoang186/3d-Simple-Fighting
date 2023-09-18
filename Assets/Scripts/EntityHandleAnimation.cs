using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public enum EntityAnimation
{
    Character_Idle,
    Character_Roll,
    Character_Run,
    Character_Walk,
    Character_Attack,
    Character_StartCasting,
    Character_Casting,
    Character_UnCasting,
    Character_GetHit,
    Character_Defeated,
    Character_Block,
    Character_UnBlock,
}

public class EntityHandleAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnim(EntityAnimation animName, float transitionTime = 0f)
    {
        // reset trigger
        if (NeedOverrideAnimationState(animName))
            anim.SetBool("BaseLayer", false);
        else
        {
            anim.SetBool("BaseLayer", true);
        }

        if (animName == EntityAnimation.Character_Block)
            anim.SetBool("IsBlocking", true);
        if (animName == EntityAnimation.Character_UnBlock)
            anim.SetBool("IsBlocking", false);

        if (animName == EntityAnimation.Character_UnCasting)
            anim.SetBool("IsCasting", false);
        if (animName == EntityAnimation.Character_StartCasting)
            anim.SetBool("IsCasting", true);

        anim.CrossFade(animName.ToString(), transitionTime);
    }

    private bool NeedOverrideAnimationState(EntityAnimation animName)
    {
        return animName == EntityAnimation.Character_Attack
        || animName == EntityAnimation.Character_StartCasting
        || animName == EntityAnimation.Character_Block;
    }
}
