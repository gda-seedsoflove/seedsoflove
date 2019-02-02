using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteScript : MonoBehaviour {

    public GameObject Top;
    public GameObject Bottom;

    [HideInInspector]
    public GameObject top, bottom;

    public float speed;
    public bool held = false;
    public bool online;         //When to top of the hold note passes the line.

    public float length;

    private Vector2 pos;

    [HideInInspector]
    public LineRenderer lr;
    // Use this for initialization
    void Start () {
        top = (GameObject)Instantiate(Top,transform,true);
        bottom = (GameObject)Instantiate(Bottom, transform, true);
        bottom.transform.position = gameObject.transform.position;
        pos = bottom.transform.position;
        top.transform.position = new Vector2(pos.x, pos.y + length);

        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = new Color(.95f, .95f, .95f);
        //lr.SetColors(new Color(.95f,.95f,.95f), new Color(.95f, .95f, .95f));
        lr.startWidth = .75f;
        //lr.SetWidth(0.75f, 0.75f);
        lr.sortingLayerName = "Foreground";
        lr.SetPosition(0, bottom.transform.position);
        lr.SetPosition(1, top.transform.position);
    }


    // Update is called once per frame
    void Update () {
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
        }
        else
        {
            bottom.transform.position = gameObject.transform.position;
            pos = bottom.transform.position;
            //top.transform.position = new Vector2(pos.x, pos.y + length);
        }
    }
}
