    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerData
    {
        private static PlayerData instance;
        public static PlayerData Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerData();
                return instance;
            }
        }
        

        public float BaseSpeed = 50f;
        public float WalkSpeed = 1f;
        public float RunSpeed = 3f;
        public float RotateSpeed = 10f;

        public float jumpForce = 5f;
        public float jumpCooldown = 0.25f;
        public float airMultiplier = 0.5f;
        public float groundDrag = 4f;

        public float playerHeight = 1.85f;

        public bool grounded;
        public bool walkToggle = false;
        public bool readyToJump = true;
        public bool builderMode = false;
        public bool isAiming = false;
        public bool equipBow = false;
        public bool equipSword = false;
        public bool isAttacking = false;
        public String currentEquipmentName = "";
        public bool canWalk = true;
            
        private PlayerData() { }
    }