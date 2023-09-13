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
    Character_GetHit,
    Character_Defeated,
    Character_Block,
}

public class EntityHandleAnimation : MonoBehaviour
{
    // private float animTransitionTime;
    // private float curAnimTransitionTime;
    // private float desAnimLayerWeight;
    // private float curAnimLayerWeight;
    // private bool finishedUpdateAnimWeight;
    private Animator anim;
    // private Entity.Entity entity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        // entity = GetComponent<Entity.Entity>();
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
        // UpdateAnimLayerWeight();
    }

    // private void UpdateAnimLayerWeight()
    // {
    //     if (finishedUpdateAnimWeight)
    //         return;
    //     if (curAnimTransitionTime > animTransitionTime)
    //     {
    //         finishedUpdateAnimWeight = true;
    //         return;
    //     }

    //     if (animTransitionTime == 0)
    //     {
    //         curAnimLayerWeight = desAnimLayerWeight;
    //     }
    //     else
    //     {
    //         curAnimTransitionTime += Time.deltaTime;
    //         curAnimLayerWeight = Mathf.Lerp(curAnimLayerWeight, desAnimLayerWeight, curAnimTransitionTime / animTransitionTime);
    //     }

    //     anim.SetLayerWeight(1, curAnimLayerWeight);
    //     anim.SetLayerWeight(2, curAnimLayerWeight);
    // }

    public void PlayAnim(EntityAnimation animName, float transitionTime = 0f)
    {
        // reset trigger
        if (!NeedOverrideAnimationState(animName))
            anim.SetTrigger("BaseLayer");
        else
            anim.ResetTrigger("BaseLayer");
        
        
        anim.CrossFade(animName.ToString(), transitionTime);
        // desAnimLayerWeight = NeedOverrideAnimationState(animName, transitionTime) ? 1f : 0f;
        // curAnimTransitionTime = 0;
        // finishedUpdateAnimWeight = false;
    }

    private bool NeedOverrideAnimationState(EntityAnimation animName, float transitionTime = 0f)
    {
        // animTransitionTime = transitionTime;
        return animName == EntityAnimation.Character_Attack || animName == EntityAnimation.Character_StartCasting || animName == EntityAnimation.Character_Block;
    }
}
