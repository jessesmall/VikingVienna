using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {

    private bool _animationPaused;
    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Submit"))
        {
            animator.enabled = true;
        }
    }

    public void PauseAnimation()
    {
        animator.enabled = false;
    }





}
