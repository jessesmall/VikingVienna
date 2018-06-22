﻿using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {

    public Transform[] Backgrounds;
    public float ParallaxScale;
    public float ParallaxReductionFactor;
    public float Smoothing;

    private Vector3 lastPosition;

    public void Start()
    {
        lastPosition = transform.position;
    }

    public void Update()
    {
        var parallax = (lastPosition.x - transform.position.x) * ParallaxScale;

        for(var i = 0; i < Backgrounds.Length; i++)
        {
            var backgroundTargetPosition = Backgrounds[i].position.x + parallax * (i * ParallaxReductionFactor + 1);
            Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, //from
            new Vector3(backgroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z), //to
            Smoothing * Time.deltaTime);
        }

        lastPosition = transform.position;
    }
}
