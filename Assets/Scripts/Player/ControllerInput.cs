using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {

    private bool _allowInput = true;
    public bool AllowInput
    {
        get { return _allowInput; }
        set
        {
            _allowInput = value;
            if (!_allowInput)
            {
                controller.HandleVerticalInput(0);
                controller.HandleHorizontalInput(0);
            }
        }
    }

    private BaseController controller;

    List<string> ControllerInterfaces;

    private void Awake()
    {
        controller = GetComponent<BaseController>();
    }

    private void Update()
    {
        if (AllowInput)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        var rawVert = Input.GetAxisRaw("Vertical");
        var rawHorz = Input.GetAxisRaw("Horizontal");

        controller.HandleVerticalInput(rawVert);
        controller.HandleHorizontalInput(rawHorz);

        if (Input.GetButtonDown("Jump"))
        {
            IJump jumper = controller as IJump;
            if(jumper != null)
                jumper.HandleJump();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            IMelee melee = controller as IMelee;
            if (melee != null)
                melee.HandleMelee();
        }
    }
}
