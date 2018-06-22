using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationManager : MonoBehaviour {

    private Animator animator;

    void Awake()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start () {
        animator.Play("OpeningScene");
    }
}
