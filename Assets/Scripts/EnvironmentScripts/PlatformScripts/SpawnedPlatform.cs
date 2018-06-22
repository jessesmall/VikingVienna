using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPlatform : MonoBehaviour {

    private Transform _destination;
    private float _speed;

    private bool _isInitialized = false;

    public void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
        _isInitialized = true;
    }

    public void Update()
    {
        if (_isInitialized)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

            var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
            if (distanceSquared > 0.01f * 0.01f)
                return;

            var player = gameObject.GetComponentInChildren<PlayerController>();
            if(player != null)
            {
                player.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponentInParent<PlayerController>() != null && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(EnablePlatform());
        }
    }

    IEnumerator EnablePlatform()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
