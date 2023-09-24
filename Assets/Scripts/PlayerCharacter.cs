using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerCharacter : Entity
    {
        [Header("Player Settings")]
        [SerializeField] float lookSensitive;
        [Header("Player References")]
        [SerializeField] Transform playerCamera;

        private Transform lockingTarget;
        private float xRotation;
        private float yRotation;

        protected override void Start()
        {
            base.Start();
            // Locks the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}