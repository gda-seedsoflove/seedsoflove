using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{

    private Vector3 a;
    // offset
    private Vector3 b;
    private float speedMult = 1f;
    private bool mouse;
    private float maxSpeed = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && mouse)
        {
            a = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            b = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, a.z)) - gameObject.transform.position;
            //Debug.Log(b);
            Vector2 prev = gameObject.GetComponent<Rigidbody2D>().velocity;
            Vector2 curr = new Vector2(prev.x * .83f + b.x * speedMult, prev.y * .83f + b.y * speedMult);
            if (curr.magnitude > maxSpeed)
            {
                float x = maxSpeed / curr.magnitude;
                curr = curr * x;
            }

            gameObject.GetComponent<Rigidbody2D>().velocity = curr;
        }
    }

    void OnMouseDown()
    {
        mouse = true;
    }

    private void OnMouseUp()
    {
        mouse = false;
    }



}
