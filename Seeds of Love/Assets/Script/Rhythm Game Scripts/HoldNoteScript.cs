using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Song;

public class HoldNoteScript : MonoBehaviour {

    public NoteManager NoteManager { get; set; }
    public Note Note { get; set; }

    public GameObject Top;
    public GameObject Bottom;

    [HideInInspector]
    public GameObject top, bottom;

    [HideInInspector]
    public float speed, length;

    [HideInInspector]
    public bool held, online;

    public GameObject pulse;
    [HideInInspector]
    public float interval,currinterval;

    private Vector2 pos;

    [HideInInspector]
    public LineRenderer lr;

    private Color c;
    // Use this for initialization
    void Start () {
        top = (GameObject)Instantiate(Top,transform,true);
        bottom = (GameObject)Instantiate(Bottom, transform, true);
        bottom.transform.position = gameObject.transform.position;
        pos = bottom.transform.position;
        top.transform.position = new Vector2(pos.x, pos.y + length);

        //Set COlors
        c = NoteManager.c;
        top.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 1);
        bottom.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 1);

        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Unlit/UnlitOutline"));
        lr.material.SetColor("_Color", new Color(1f, 1f, 1f));
        //lr.material.SetColor("_Outline", new Color(1f, 1f, 1f)); // this will set the outline color. default is RGBA(.1,.1,.1,1)
        //lr.material.SetFloat("_OutlineWidth", .02f); // this will set the outline width. default is .02. goes from 0 to 1
        lr.startWidth = .75f;
        lr.sortingLayerName = "Foreground";
        lr.SetPosition(0, bottom.transform.position);
        lr.SetPosition(1, top.transform.position);
    }

    // Update is called once per frame
    void Update () {
        lr = GetComponent<LineRenderer>();
        if (lr)
        {
            lr.SetPosition(0, bottom.transform.position);
            //lr.SetPosition(1, top.transform.position);
            lr.SetPosition(1, new Vector2(top.transform.position.x, top.transform.position.y +(.1f*speed/10)));
        }

        if (held)
        {
            if (online == false)
            {
                top.transform.position = new Vector2(top.transform.position.x, top.transform.position.y + speed * Time.deltaTime);
            }

            float distance = top.transform.position.y - bottom.transform.position.y;
            //Debug.Log(interval * speed);
            if (currinterval <= 0 && distance >= interval/2 * -speed) // Pulses in half beats in the hold notes
            {
                GameObject instance = (GameObject)Instantiate(pulse, transform.position, Quaternion.identity);
                instance.transform.parent = gameObject.transform;
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

    public void Hit(float distance)
    {
        Vector2 pos = bottom.transform.position;
        float topy = top.transform.position.y;
        //GetComponent<HoldNoteScript>().bottom.transform.position = new Vector2(pos.x, NoteManager.transform.position.y - NoteManager.EndY);
        GetComponent<HoldNoteScript>().held = true;
        transform.position = new Vector2(pos.x, distance);
        top.transform.position = new Vector2(pos.x, topy);
        top.GetComponent<SpriteRenderer>().color = new Color(.8f, .8f, .8f);
        lr.SetPosition(1, top.transform.position);
        lr.startColor = new Color(1f, 1f, 1f, 1);
        lr.endColor = new Color(1f, 1f, 1f, 1);
    }
}
