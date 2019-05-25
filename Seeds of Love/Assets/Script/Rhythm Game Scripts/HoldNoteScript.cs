using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Song;

public class HoldNoteScript : MonoBehaviour {

    public NoteManager NoteManager { get; set; }
    public Note Note { get; set; }

    public GameObject Top;
    public GameObject Bottom;
    public GameObject HitCircle;

    [HideInInspector]
    public GameObject top, bottom, hitcircle;

    private Vector2 end;

    [HideInInspector]
    public float speed, length, timeafter;

    [HideInInspector]
    public bool held, online, earlyrelease;

    [HideInInspector]
    public float interval, currinterval;

    public GameObject pulse;

    [HideInInspector]
    public GameObject pulseobject;

    private Vector2 pos;

    [HideInInspector]
    public LineRenderer lr;

    [HideInInspector]
    public Color c;
    // Use this for initialization
    void Start () {
        top = (GameObject)Instantiate(Top,transform,true);
        bottom = (GameObject)Instantiate(Bottom, transform, true);
        bottom.transform.position = gameObject.transform.position;
        pos = bottom.transform.position;
        top.transform.position = new Vector2(pos.x, pos.y + length);
        hitcircle = (GameObject)Instantiate(HitCircle, transform, true);
        hitcircle.transform.position = new Vector3(top.transform.position.x, top.transform.position.y, 0);
        hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
        pulseobject =GameObject.FindGameObjectWithTag("Detector");

        //Set COlors
        c = NoteManager.c;
        top.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1);
        bottom.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1);

        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Unlit/UnlitOutline"));
        lr.material.SetColor("_Color", new Color(.6f, .2f, .2f));
        lr.material.SetColor("_Outline", new Color(.9f, .9f, .9f, 1)); // this will set the outline color. default is RGBA(.1,.1,.1,1)
        lr.material.SetFloat("_OutlineWidth", .1f); // this will set the outline width. default is .02. goes from 0 to 1
        lr.startWidth = .75f;
        lr.sortingLayerName = "Foreground";
        lr.SetPosition(0, bottom.transform.position);
        lr.SetPosition(1, top.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate () {
        lr = GetComponent<LineRenderer>();
        if (lr)
        {
            lr.SetPosition(0, bottom.transform.position);
            //lr.SetPosition(1, top.transform.position);
            lr.SetPosition(1, new Vector2(top.transform.position.x, top.transform.position.y +(.1f*speed/10)));
        }

        if (held)
        {
            if (bottom.GetComponentInChildren<SpriteRenderer>())
            {
                Destroy(bottom.GetComponentInChildren<SpriteRenderer>());
            }
            if (top != null)
            {
                top.transform.position = new Vector2(top.transform.position.x, top.transform.position.y + speed * Time.deltaTime);
            }

            if (online == true && lr)
            {
                lr.enabled = false;
            }

            float distance = 0;
            if (top != null)
            {
                distance = top.transform.position.y - bottom.transform.position.y;
            }

            if (hitcircle.gameObject)
            {
                //hitcircle.transform.position = new Vector3(top.transform.position.x, top.transform.position.y, 0);
                hitcircle.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                float maxscale = 5;
                float scale = Mathf.Lerp(.3f, maxscale, distance / -speed);
                hitcircle.transform.localScale = new Vector3(scale, scale, scale);

                if (scale >= maxscale)
                {
                    hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
                }
                else if (scale > 3)
                {
                    hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .05f);
                }
                else if (scale >= 1.5)
                {
                    hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(.2f,.05f, scale-1));
                }
                else if(scale > 1)
                {
                    hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
                }
                else
                {
                    hitcircle.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Clamp(scale - .3f, 0, .5f));
                }
            }

            //Debug.Log(interval * speed);
            if (currinterval <= 0 && distance >= 3/2 * interval * -speed && earlyrelease == false) // Pulses in half beats in the hold notes
            {
                GameObject instance = (GameObject)Instantiate(pulse, transform.position, Quaternion.identity);
                instance.transform.parent = pulseobject.transform;
                Destroy(instance, 1f);
                currinterval = interval;
            }
            else
            {
                currinterval -= Time.deltaTime;
            }

        }
        else
        {
            bottom.transform.position = gameObject.transform.position;
            pos = bottom.transform.position;
            //top.transform.position = new Vector2(pos.x, pos.y + length);
        }

 
    }

    void Update()
    {
        if (earlyrelease)
        {
            float alpha = Mathf.Clamp(1 - (2 * timeafter / (3 * NoteManager.DisplayedTimeAfter)), 0, 255);
            timeafter += Time.deltaTime;
            SetAlpha(alpha);
        }
    }

    public void SetAlpha(float alpha)
    {
        if (bottom.GetComponent<SpriteRenderer>())
        {
            bottom.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, alpha);
            bottom.transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, alpha);
        }
        top.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, alpha);
        top.transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(alpha, alpha, alpha, alpha);
        //Debug.Log(lr);
        if (lr)
        {
            lr.material.SetColor("_Color", new Color(lr.material.color.r * (alpha), lr.material.color.g * (alpha), lr.material.color.b * (alpha), alpha));
            lr.material.SetColor("_Outline", new Color(lr.material.color.r * (alpha), lr.material.color.g * (alpha), lr.material.color.b * (alpha), alpha));
        }
    }

    public void DestoryPulses()
    {
        //remove pulses
    }

    /**
     * Trigger effects that happen when the hold note is hit
     */
    public void Hit(float distance)
    {
        Vector2 pos = bottom.transform.position;
        try
        {
            float topy = top.transform.position.y;
            //GetComponent<HoldNoteScript>().bottom.transform.position = new Vector2(pos.x, NoteManager.transform.position.y - NoteManager.EndY);
            GetComponent<HoldNoteScript>().held = true;
            transform.position = new Vector2(pos.x, distance);
            top.transform.position = new Vector2(pos.x, topy);
            top.GetComponent<SpriteRenderer>().color = new Color(c.r * .8f, c.g * .6f, c.b * .6f);
        }
        catch { }

        lr.SetPosition(1, top.transform.position);
        lr.material.SetColor("_Color", new Color(.8f, .4f, .4f));

    }

    public void Release()
    {
        if (top.GetComponentInChildren<SpriteRenderer>())
        {
            Transform child = top.transform.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
            Destroy(top);
            Destroy(hitcircle);
        }
        if (bottom.GetComponentInChildren<SpriteRenderer>())
        {
            Destroy(bottom.transform.GetChild(0).gameObject);
        }
    }
}
