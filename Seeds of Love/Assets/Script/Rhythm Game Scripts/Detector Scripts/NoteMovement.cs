using Script.Song;
using UnityEngine;
using UnityEditor;

namespace Script.Behaviour
{
    public class NoteMovement : MonoBehaviour
    {
        public NoteManager NoteManager { get; set; }
        public Note Note { get; set; }
        public bool moving = true;

        private float timeafter;

        void Start()
        {
           
        }

        private void FixedUpdate()
        {

            // Self-destruct if the note is outside of the displayed time range
            if (Note.Currtime < -Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)
            {
                Destroy(gameObject);
            }
            else if (Note.Currtime <= -Note.HitTimeThreshold && Note.Hit == false && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Note_Hit_Animation") && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Fade_Out_Animation"))
            {
                GetComponent<Animator>().Play("Fade_Out_Animation");
            }
            else if (Note.isHoldNote && Note.Currtime < -Note.HitTimeThreshold && Note.Holding == false)
            {
                try
                {
                    Color c = GetComponent<HoldNoteScript>().lr.startColor;
                    //Debug.Log(1 - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().lr.startColor = new Color(c.r, c.g, c.b, 1f - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().lr.endColor = new Color(c.r, c.g, c.b, 1f - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                }
                catch { }
            }

            if (GetComponent<NoteJudgement>().missed && Note.isHoldNote == false && Note.Missed == false)
            {
                NoteManager.addScore(0, 1);
                Note.Missed = true;
            }

            Note.Currtime = Note.Currtime -= Time.deltaTime;

            // Set the current position of the note by interpolating between
            // the start and end positions
            /**
            transform.position = Vector2.Lerp(
                new Vector2(transform.position.x, NoteManager.transform.position.y),
                new Vector2(transform.position.x, NoteManager.transform.position.y+NoteManager.EndY),
                (NoteManager.CurrentSongTime - Note.Time + NoteManager.DisplayedTimeBefore) / (NoteManager.DisplayedTimeBefore + NoteManager.DisplayedTimeAfter)
            );
            */
            if (moving)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (Note.Speed * Time.deltaTime));
            }

        }

        void Update()
        {

            if (Note.Currtime < -Note.HitTimeThreshold)
            {
                GameObject child = transform.GetChild(0).gameObject;
                float alpha = Mathf.Clamp(1 - (2 * timeafter / (3*NoteManager.DisplayedTimeAfter)), 0, 255);
                try
                {
                    child.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                }
                catch { }
                timeafter += Time.deltaTime;
                if (Note.isHoldNote && GetComponent<HoldNoteScript>())
                {
                    HoldNoteScript hnote = GetComponent<HoldNoteScript>();
                    if (hnote.top)
                    {
                        hnote.top.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
                    }
                    try
                    {
                       
                        hnote.SetAlpha(alpha);
                        /**
                        hnote.bottom.GetComponent<Renderer>().material.color = new Color(hnote.c.r, hnote.c.g, hnote.c.b, alpha);
                        hnote.top.GetComponent<Renderer>().material.color = new Color(hnote.c.r, hnote.c.g, hnote.c.b, alpha);
                        hnote.bottom.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                        hnote.top.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
                        //hnote.lr.material.SetFloat("_Lightness", 1-alpha);
                        hnote.lr.material.SetColor("_Color", new Color(hnote.lr.material.color.r *(alpha), hnote.lr.material.color.g * (alpha), hnote.lr.material.color.b * ( alpha), alpha));
                        hnote.lr.material.SetColor("_Outline", new Color(hnote.lr.material.color.r * (alpha), hnote.lr.material.color.g * (alpha), hnote.lr.material.color.b * (alpha), alpha));
                        //hnote.lr.material.SetColor("_Color", new Color(0, 0, 0, alpha));
                        */
                    }
                    catch { }
                }
            }
        }
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Note.isHoldNote)
            {
                if (Note.Holding)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.blue;
                }
                //float length = NoteManager.SpawnTimeInterval * Note.Speed * Note.Length;
                float length = Note.Length;
                Handles.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + length));
                //Handles.DrawSphere(1,new Vector2(transform.position.x, transform.position.y - length +.2f),Quaternion.identity,.6f);
                Handles.DrawWireDisc(new Vector2(transform.position.x, transform.position.y + length), Vector3.back,.5f);
            }
        }
        #endif
    }
}
