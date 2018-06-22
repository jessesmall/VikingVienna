using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform Player;
    public Vector2 Margin;
    public Vector2 Smoothing;
    public BoxCollider2D Bounds;

    private Vector3 min;
    private Vector3 max;

    public bool IsFollowing;

    private bool _isAutoScroll;

    public void Start()
    {
        min = Bounds.bounds.min;
        max = Bounds.bounds.max;
        if(GetComponent<AutoScroll>() != null)
        {
            _isAutoScroll = true;
        }
    }

    public void FixedUpdate()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);

        if (IsFollowing)
        {
            if (!_isAutoScroll)
            {
                if (Mathf.Abs(x - Player.position.x) > Margin.x)
                    x = Mathf.Lerp(x, Player.position.x, Smoothing.x * Time.deltaTime);
                x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
            }

            if (Mathf.Abs(y - Player.position.y) > Margin.y)
                y = Mathf.Lerp(y, Player.position.y, Smoothing.y * Time.deltaTime);
            y = Mathf.Clamp(y, min.y + GetComponent<Camera>().orthographicSize, max.y - GetComponent<Camera>().orthographicSize);
        }

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
