using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoteDetector : MonoBehaviour
{
    // Basically this script is the object moverscript but just includes a lane number with it.

    /*
    * A script that moves a object around it's original position with bounds
    * Spread - How far apart each section is from each other
    * Changespeed - Speed in which the object swtiches from each section
    * Just place the object into the Object slot in the editor when viewing the script
    */

    public int Lane { get; set; }       // Current lane the notedetector is on
    private float speed = 6;
    public float changespeed;
    private float originalspeed;

    private bool changex = true;
    private bool changey = true;
    public float downbound;

    public bool freezex;
    public bool freezey;

    // Distance bound of how far the Object can movefrom the starting point
    public float leftbound;

    public GameObject Object;
    private Vector2 origin;

    public Vector2 pos; //position in terms of a grid. (0,0) is the origin coordinate
    public float rightbound;
    public float spread;

    public float upbound;
    private float whitetime; // time the detector lights up after pressing space;
    private bool moving;
    private float currscale = 1;
    private float middle;

    [HideInInspector]
    public bool leftdown, rightdown, leftup, rightup;

    [HideInInspector]
    public bool holdingshift, holdingleft, holdingright;

    [HideInInspector]
    public float shiftbuffer, leftbuffer, rightbuffer;

    //Setting the origin point and position of the mover to the object
    private void Awake()
    {
        pos = new Vector2(0, 0);
        transform.position = Object.transform.position;
        origin = transform.position;
    }

    // Use this for initialization
    private void Start()
    {
        origin = Object.transform.position;
        originalspeed = changespeed;

    }

    // Update is called once per frame
    private void Update()
    {
        Lane = (int)((Object.transform.position.x - origin.x) + 0.5f);  // Determines the lane the detector is on. 0.5f to make it round to the nearest int
        //Debug.Log((int)((Object.transform.position.x - origin.x) + 0.5f));

        transform.position = Object.transform.position; // Stay on the object

        bool leftkey = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.J);
        bool leftkeyup = Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.J);

        bool rightkey = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.L);
        bool rightkeyup = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.L);

        bool dashkey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow);
        bool dashkeyup = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.UpArrow);

        if (leftkey)
        {
            leftdown = true;
        }
        if (leftup)
        {
            leftup = false; // since this is above the true setter, this value is only true for one frame
        }
        if (leftkeyup)
        {
            leftup = true;

        }

        if (rightkey)
        {
            rightdown = true;
        }
        if (rightup)
        {
            rightup = false; // since this is above the true setter, this value is only true for one frame
        }
        if (rightkeyup)
        {
            rightup = true;
        }



        float movement = 0;

        if (leftdown)
        {
            holdingleft = true;
            leftbuffer = .2f;
            movement -= 1;
            if (shiftbuffer > 0)
            {
                changespeed = 40;
                movement = -4;
            }
            leftdown = false;
        }
        else if (leftup)
        {
            holdingleft = false;
        }

        if (rightdown)
        {
            holdingright = true;
            rightbuffer = .2f;
            movement += 1;
            if(shiftbuffer > 0)
            {
                changespeed = 40;
                movement = 4;
            }
            rightdown = false;
        }
        else if(rightup)
        {
            holdingright = false;
        }


        if (Input.GetKey(KeyCode.LeftShift)|| dashkey)
        {
            if(rightbuffer > 0)
            {
                changex = false;
                movement = 4;
                rightbuffer = 0;
            }
            if (leftbuffer > 0)
            {
                changex = false;
                movement = -4;
                leftbuffer = 0;
            }
            holdingshift = true;
            shiftbuffer = .15f;
            changespeed = 40;
        }
        else if(Input.GetKeyUp("left shift") || dashkeyup)
        {
            holdingshift = false;
            changespeed = originalspeed;
        }

        /**
        if ((holdingshift || shiftbuffer > 0) && rightbuffer > 0 && (rightbuffer > leftbuffer))
        {
            movement = 4;
            rightbuffer = 0;
            shiftbuffer = 0;
        }
        if ((holdingshift || shiftbuffer > 0) && leftbuffer > 0 && (leftbuffer > rightbuffer))
        {
            movement = -4;
            leftbuffer = 0;
            shiftbuffer = 0;
        }
        */

        checkPos(movement, 0);

        if(shiftbuffer > 0)
        {
            shiftbuffer -= Time.deltaTime;
        }
        else
        {
            changespeed = originalspeed;
            shiftbuffer = 0;
        }

        if (leftbuffer > 0)
        {
            leftbuffer -= Time.deltaTime;
        }
        else
        {
            leftbuffer = 0;
        }
        if (rightbuffer > 0)
        {
            rightbuffer -= Time.deltaTime;
        }
        else
        {
            rightbuffer = 0;
        }


        if (rightkey || leftkey)
        {
            Vector2 currentpos = new Vector2(origin.x + pos.x * spread, origin.y + pos.y * spread);
            middle = ((currentpos.x - transform.position.x) / 2f) + transform.position.x;
        }

        bool spacekeydown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K);
        bool spacekey = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.K);

        if (spacekeydown)
        {
            transform.Find("PressEffect").GetComponent<Animator>().Play("Press_Effect",0,0f);
            transform.Find("Outline").GetComponent<SpriteRenderer>().color = new Color(1,1,1,.7f);
            whitetime = .1f;
            transform.localScale = new Vector3(.85f, .85f, 1);
        }
        else if (!spacekey)
        {
            transform.Find("Outline").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            //transform.localScale = new Vector3(1,1,1);
        }


        if (whitetime >= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,.8f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
        whitetime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        MoveToPosition();
        Vector2 targetpos = new Vector2(origin.x + pos.x * spread, origin.y + pos.y * spread);
        if ((Vector2)Object.transform.position != targetpos)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        bool spacekeydown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K);

        if (moving && !spacekeydown)
        {
            float distance = Mathf.Abs(middle - Object.transform.position.x);
            float scale = Mathf.Clamp(((distance * 2) / spread), .3f, 1);

           // Debug.Log("Position:" + Object.transform.position.x + " Target:" + targetpos.x + " Middle:" + middle + " Scale:"+scale);
            float speed = 0;
            currscale = Mathf.SmoothDamp(currscale, scale, ref speed, .012f);
            transform.localScale = new Vector3(1, currscale, 1);
        }
        else if(!moving && !spacekeydown)
        {
            currscale = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void checkPos(float x, float y)
    {
        if (freezex == false && changex == false)
        {
            pos.x += (int)x;
        }

        if (x == 0)
        {
            changex = false;
        }
        else
        {
            changex = true;
        }

        if (freezey == false && changey == false)
        {
            pos.y += (int)y;
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

        if (pos.x < -leftbound)
        {
            pos.x = -leftbound;
        }

        if (pos.y > upbound)
        {
            pos.y = upbound;
        }

        if (pos.y < -downbound)
        {
            pos.y = -downbound;
        }
    }

    public void MoveToPosition()
    {
        Object.transform.position = Vector2.MoveTowards(Object.transform.position,
            new Vector2(origin.x + pos.x * spread, origin.y + pos.y * spread), changespeed*Time.deltaTime);

       // Debug.Log((Object.transform.position.x - origin.x));
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
        var vel = new Vector2(x, y);
        Vector2 objpos = Object.transform.position;
        if (origin.x - objpos.x > 0 && Mathf.Abs(origin.x - objpos.x) > leftbound)
        {
            Object.transform.position = new Vector2(origin.x - leftbound, objpos.y);
            x = 0;
        }
        else if (origin.x - objpos.x < 0 && Mathf.Abs(origin.x - objpos.x) > rightbound)
        {
            Object.transform.position = new Vector2(origin.x + rightbound, objpos.y);
            x = 0;
        }

        if (origin.y - objpos.y < 0 && Mathf.Abs(origin.y - objpos.y) > upbound)
        {
            Object.transform.position = new Vector2(objpos.x, origin.y + upbound);
            y = 0;
        }
        else if (origin.y - objpos.y > 0 && Mathf.Abs(origin.y - objpos.y) > downbound)
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
    /**
    private void OnDrawGizmos()
    {
        if (origin != new Vector2(0, 0))
        {
            Handles.color = Color.grey;
            Handles.DrawWireDisc(Object.transform.position, Vector3.back, .02f);
            if (freezex == false)
            {
                Handles.DrawLine(new Vector2(origin.x - leftbound, origin.y),
                    new Vector2(origin.x + rightbound, origin.y));
            }

            if (freezey == false)
            {
                Handles.DrawLine(new Vector2(origin.x, origin.y - downbound),
                    new Vector2(origin.x, origin.y + upbound));
            }
        }
    }
    */
}
