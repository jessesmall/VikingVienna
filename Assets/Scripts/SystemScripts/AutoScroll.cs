using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour {

    public Transform StartOfLevel;
    public Transform EndOfLevel;

    public BoxCollider2D CameraBounds;

    /// <summary>
    /// (tiles/second)
    /// </summary>
    public float AutoScrollSpeed;

    public enum ScrollType { Horizontal, Vertical };
    public ScrollType ScrollDirection;

    private float _totalDistance;
    private float _totalTime;
    private float _countingTime = 0;

    // Use this for initialization
    void Start () {
        _totalDistance = EndOfLevel.position.x - StartOfLevel.position.x;
        _totalTime = _totalDistance / AutoScrollSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _countingTime += Time.fixedDeltaTime;
        if(_countingTime != 0)
        {
            var x = Mathf.Lerp(StartOfLevel.position.x, EndOfLevel.position.x, _countingTime / _totalTime);
            var deltaX = x - transform.position.x;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            CameraBounds.transform.position = new Vector2(CameraBounds.transform.position.x + deltaX, CameraBounds.transform.position.y);
        }
	}
}
