using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoverScript : MonoBehaviour {

    /*
     * Object mover is a script that moves a object around it's original position with bounds
     * Spread - How far apart each section is from each other
     * Changespeed - Speed in which the object swtiches from each section
     * Just place the object into the Object slot in the editor when viewing the script
     */

    public GameObject Object;
    Vector2 origin;
    public float spread;
    public float changespeed;
    float speed = 6;

    // Distance bound of how far the Object can movefrom the starting point
    public float leftbound;
    public float rightbound;
    public float upbound;
    public float downbound;

    public bool freezex;
    public bool freezey;

    bool changex = true;
    bool changey = true;

    public Vector2 pos; //position in terms of a grid. (0,0) is the origin coordinate
    //Setting the origin point and position of the mover to the object
    void Awake()
    {
        pos = new Vector2(0, 0);
        transform.position = Object.transform.position;
        origin = transform.position;
    }
    // Use this for initialization
	void Start () {
        origin = Object.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Object.transform.position; // Stay on the object
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        checkPos(x,y);
        MoveToPosition();

        // Speed System.
        //Rigidbody2D rb = Object.GetComponent<Rigidbody2D>();
        //x = xspeed(x);
        //y = yspeed(y);
        //rb.velocity = checkVelocity(x,y);
    }

    public void checkPos(float x, float y)
    {

        if (freezex == false && changex == false)
        {
            if (x > 0)
            {
                pos.x++;
            }
            else if (x < 0)
            {
                pos.x--;
            }
        }
        if(x == 0)
        {
            changex = false;
        }
        else
        {
            changex = true;
        }

        if (freezey == false && changey == false)
        {
            if (y > 0)
            {
                pos.y++;
            }
            else if (y < 0)
            {
                pos.y--;
            }
        }
        if (y == 0)
        {
            changey = false;
        }
        else
        {
            changey = true;
        }

        if (pos.x > rightbound)
        {
            pos.x = rightbound;
        }
        if(pos.x < -leftbound)
        {
            pos.x = -leftbound;
        }

        if(pos.y > upbound)
        {
            pos.y = upbound;
        }
        if(pos.y < -downbound)
        {
            pos.y = -downbound;
        }
    }

    public void MoveToPosition()
    {
        Object.transform.position = Vector2.MoveTowards(Object.transform.position, new Vector2(origin.x + (pos.x * spread), origin.y + (pos.y * spread)), changespeed);
    }

    public float xspeed(float x)
    {
        if (freezex == false)
        {
            if (x > 0)
            {
                x = speed;
            }
            else if (x < 0)
            {
                x = -speed;
            }
            else
            {
                x = 0;
            }
        }
        else
        {
            x = 0;
        }
        return x;
    }

    public float yspeed(float y)
    {
        if (freezey == false)
        {
            if (y > 0)
            {
                y = speed;
            }
            else if (y < 0)
            {
                y = -speed;
            }
            else
            {
                y = 0;
            }
        }
        else
        {
            y = 0;
        }
        return y;
    }

    /*
     * Checks the whether or not the current position of the object + the current speed would exceed the bounds.  
     * If the future position exceeds the bounds then the object will be placed on the boundary and the speed will be set back to zero.
     */
    public Vector2 checkVelocity(float x, float y)
    {
        Vector2 vel = new Vector2(x, y);
        Vector2 objpos = Object.transform.position;
        if(origin.x - (objpos.x) > 0 && Mathf.Abs(origin.x - (objpos.x)) > leftbound)
        {
            Object.transform.position = new Vector2(origin.x - leftbound, objpos.y);
            x = 0;
        }
        else if (origin.x - (objpos.x) < 0 && Mathf.Abs(origin.x - (objpos.x)) > rightbound)
        {
            Object.transform.position = new Vector2(origin.x + rightbound, objpos.y);
            x = 0;
        }

        if (origin.y - (objpos.y) < 0 && Mathf.Abs(origin.y - (objpos.y)) > upbound)
        {
            Object.transform.position = new Vector2(objpos.x, origin.y + upbound);
            y = 0;
        }
        else if (origin.y - (objpos.y) > 0 && Mathf.Abs(origin.y - (objpos.y)) > downbound)
        {
            Object.transform.position = new Vector2(objpos.x, origin.y - downbound);
            y = 0;
        }

        vel = new Vector2(x, y);
        return vel;
    }

    /*
     * Draws the bounds of the object denoted by lines
     *
     */
    private void OnDrawGizmos()
    {
        if (origin != new Vector2(0, 0)) {
            UnityEditor.Handles.color = Color.grey;
            UnityEditor.Handles.DrawWireDisc(Object.transform.position, Vector3.back, .02f);
            if (freezex == false)
            {
                UnityEditor.Handles.DrawLine(new Vector2(origin.x - leftbound, origin.y), new Vector2(origin.x + rightbound, origin.y));
            }
            if (freezey == false)
            {
                UnityEditor.Handles.DrawLine(new Vector2(origin.x, origin.y - downbound), new Vector2(origin.x, origin.y + upbound));
            }
        }
    }
}
