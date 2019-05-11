using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

    //protected Rigidbody2D r2bd2;
    private Vector3 force;
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;
    private Vector3 j;
    private float speedMult = 6;
	
	// Update is called once per frame
	void FixedUpdate () {
        //gameObject.GetComponent<Rigidbody2D>().velocity = force;
        if (Input.GetMouseButtonDown(0))
        {

        }
	}

    void OnMouseDown()
    {
        a = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        b = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, a.z));
    }
    void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, a.z);
        float dist = Vector3.Distance(curPos, gameObject.transform.position);
        Debug.Log(dist);
        /*if (dist == 0)
        {
            force = Vector3.zero;
        }
        else if (dist > 0)
        {
            force = (Camera.main.ScreenToWorldPoint(curPos) - b) * speedMult;
        }
        else
        {
            force = (Camera.main.ScreenToWorldPoint(curPos) + b) * speedMult;
        }*/
        force = Camera.main.ScreenToWorldPoint(curPos) + b;
        //gameObject.GetComponent<Rigidbody2D>().velocity = force * speedMult;
        transform.position = force;
        //gameObject.GetComponent<Rigidbody2D>().velocity = force * speedMult;
    }

    void OnMouseUp()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = force * speedMult; 
    }
    /*
     *  x = Camera.main.WorldToScreenPoint(gameObject.transform.position);
     *  y = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, x.z));
     *
     *  Vector3 a = new Vector3(Input.mousePosition.x, Input.mousePosition.y, x.z);
     *  Vector3 b = Camera.main.ScreenToWorldPoint(a) + y;
     *  transform.position = b;
     * 
     */
}
