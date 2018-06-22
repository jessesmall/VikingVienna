using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController character;
        private InventoryManager inventory;
        private bool allowInput = true;

        private void Start()
        {
            inventory = GetComponent<InventoryManager>();
            character = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (allowInput)
                HandleInput();
        }

        private void HandleInput()
        {
            var rawVert = Input.GetAxisRaw("Vertical");
            bool ducking = character.HandleVert(rawVert);
            if (ducking)
                return;

            Vector2 directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            character.HandleDirection(directionalInput);

            if (Input.GetButtonDown("Jump"))
            {
                character.HandleJump();
            }
            if (Input.GetButtonDown("Fire1") && character.canMeleeIn <= 0)
            {
                StartCoroutine(character.HandleAttack());
            }
            if (Input.GetButtonDown("Fire2") && character.canFireIn <= 0)
            {
                StartCoroutine(character.HandleRangedAttack());
            }
            if (Input.GetButtonDown("UseItem"))
            {
                inventory.UseItem();
            }
            if (Input.GetButtonDown("Interact"))
            {
                character.InteractWith();
            }
            for(int i = 1; i <= 8; i++)
            {
                if (Input.GetButtonDown("Item" + i))
                {
                    inventory.UpdateCurrentItemIndex(i);
                }
            }
            var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if(scrollWheel > 0)
            {
                inventory.UpdateCurrentItemIndex(true);
            }
            else if(scrollWheel < 0)
            {
                inventory.UpdateCurrentItemIndex(false);
            }
        }

        public void DisablePlayerInput()
        {
            allowInput = false;
            character.InputDisabled();
        }

        public void EnablePlayerInput()
        {
            allowInput = true;
        }
    }
}
