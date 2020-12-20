using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShrinkingPlatformController : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public bool isActive;
    public float platformTimer;
    public float threshold;

    public Transform spriteMask;
    public BoxCollider2D boxCollider;

    public AudioSource audioSourceShrink;
    public AudioSource audioSourceEnlarge;

    public PlayerBehaviour player;

    private Vector3 distance;
    private bool wait = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();

        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = false;
        distance = end.position - start.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {
            if (isActive)
            {
                platformTimer += Time.deltaTime;
                _Move();
                _Shrink();
            }
            else
            {
                _Enlarge();
            }
        }
    }

    private void _Move()
    {
        var distanceX = (distance.x > 0) ? start.position.x + Mathf.PingPong(platformTimer, distance.x) : start.position.x;
        var distanceY = (distance.y > 0) ? start.position.y + Mathf.PingPong(platformTimer, distance.y) : start.position.y;

        transform.position = new Vector3(distanceX, distanceY, 0.0f);
    }

    private void _Shrink()
    {
        if (spriteMask.localScale.x <= 0)
        {
            isActive = false;
            wait = true;
            Invoke("StopWait", 1.5f);
        }

            float test = 1.5f;
            spriteMask.localScale = new Vector3(spriteMask.transform.localScale.x - (test * Time.deltaTime), spriteMask.transform.localScale.y, spriteMask.transform.localScale.z);
            boxCollider.size = new Vector2(spriteMask.localScale.x, boxCollider.size.y);
    }

    private void _Enlarge()
    {
        if (spriteMask.localScale.x <= 3.0f)
        {
            float test = 1.5f;

            spriteMask.localScale = new Vector3(spriteMask.transform.localScale.x + (test * Time.deltaTime), spriteMask.transform.localScale.y, spriteMask.transform.localScale.z);
            boxCollider.size = new Vector2(spriteMask.localScale.x, boxCollider.size.y);
        }
    }

    public void StopWait()
    {
        Debug.Log("hi");
        wait = false;
    }

    public void Reset()
    {
        transform.position = start.position;
        platformTimer = 0;
    }
}
