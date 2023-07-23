using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public enum EntityAnimation
    {
        Character_Idle,
        Character_Roll,
        Character_Run,
        Character_Attack,
    }

    public class EntityCustomize : MonoBehaviour
    {
        [SerializeField] Animator anim;

        public void PlayAnim(EntityAnimation animName, float transitionTime = 0.1f)
        {
            anim.CrossFade(animName.ToString(), transitionTime);
        }
    }
}