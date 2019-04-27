using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

    private Vector3 x;
    private Vector3 y;
    private Vector3 z;
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    void OnMouseDown()
    {
        x = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        y = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, x.z));
    }
    void OnMouseDrag()
    {
        Vector3 a = new Vector3(Input.mousePosition.x, Input.mousePosition.y, x.z);
        Vector3 b = Camera.main.ScreenToWorldPoint(a) + y;
        transform.position = b;
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
