using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Entity
{
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
        Character_Attack_Deflected,
    }

    public class EntityHandleAnimation : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void UpdateAnimationBaseOnState(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Entity_Idle:
                    PlayAnim(EntityAnimation.Character_Idle, 0.1f);
                    break;
                case EntityState.Entity_Move:
                    PlayAnim(EntityAnimation.Character_Run, 0.1f);
                    break;
                case EntityState.Entity_Attack_Short:
                    PlayAnim(EntityAnimation.Character_Attack, 0.1f);
                    break;
                case EntityState.Entity_Attack_Long:
                    PlayAnim(EntityAnimation.Character_Idle);
                    PlayAnim(EntityAnimation.Character_StartCasting);
                    break;
                case EntityState.Entity_UnAttack_Long:
                    PlayAnim(EntityAnimation.Character_UnCasting);
                    break;
                case EntityState.Entity_Block:
                    PlayAnim(EntityAnimation.Character_Block);
                    break;
                case EntityState.Entity_UnBlock:
                    PlayAnim(EntityAnimation.Character_UnBlock);
                    break;
                case EntityState.Entity_GetHit:
                case EntityState.Entity_Blocking_GetHit:
                    PlayAnim(EntityAnimation.Character_GetHit);
                    break;
                case EntityState.Entity_Attack_Deflected:
                    PlayAnim(EntityAnimation.Character_Attack_Deflected);
                    break;
                case EntityState.Entity_Destroy:
                    PlayAnim(EntityAnimation.Character_Defeated);
                    break;
                default:
                    break;
            }
        }

        public void PlayAnim(EntityAnimation animName, float transitionTime = 0f)
        {
            UpdateTriggerAnim(animName);
            anim.CrossFade(animName.ToString(), transitionTime);
        }

        private void UpdateTriggerAnim(EntityAnimation animName)
        {
            // reset trigger
            if (NeedOverrideAnimationState(animName))
                anim.SetBool("BaseLayer", false);
            else
                anim.SetBool("BaseLayer", true);

            if (animName == EntityAnimation.Character_Block)
                anim.SetBool("IsBlocking", true);
            if (animName == EntityAnimation.Character_UnBlock)
                anim.SetBool("IsBlocking", false);

            if (animName == EntityAnimation.Character_UnCasting)
                anim.SetBool("IsCasting", false);
            if (animName == EntityAnimation.Character_StartCasting)
                anim.SetBool("IsCasting", true);
        }

        private bool NeedOverrideAnimationState(EntityAnimation animName)
        {
            return animName == EntityAnimation.Character_Attack
            || animName == EntityAnimation.Character_StartCasting
            || animName == EntityAnimation.Character_Attack_Deflected
            || animName == EntityAnimation.Character_Block;
        }
    }
}