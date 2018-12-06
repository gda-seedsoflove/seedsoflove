using UnityEngine;

public class Detector : MonoBehaviour
{
    private GameObject thing;
    private float time;

    private bool touching;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (touching)
        {
            time += Time.deltaTime;
        }

        if (time >= .5)
        {
            time = 0;
            Destroy(thing);
            touching = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        touching = true;
        thing = collision.gameObject;
    }
}
