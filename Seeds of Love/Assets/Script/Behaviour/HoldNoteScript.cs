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
        lr.SetColors(Color.white, Color.white);
        lr.SetWidth(0.5f, 0.5f);
        lr.sortingLayerName = "Foreground";
        lr.SetPosition(0, bottom.transform.position);
        lr.SetPosition(1, top.transform.position);
    }


    // Update is called once per frame
    void Update () {
        if (lr)
        {
            lr.SetPosition(0, bottom.transform.position);
            lr.SetPosition(1, top.transform.position);
        }

        if (held)
        {
            top.transform.position = new Vector2(top.transform.position.x, top.transform.position.y + speed * Time.deltaTime);
        }
        else
        {
            bottom.transform.position = gameObject.transform.position;
            pos = bottom.transform.position;
            top.transform.position = new Vector2(pos.x, pos.y + length);
        }
    }
}
