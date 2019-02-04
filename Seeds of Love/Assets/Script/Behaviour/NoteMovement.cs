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
            else if (Note.Currtime <= 0 && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Note_Hit_Animation") && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Fade_Out_Animation"))
            {
                GetComponent<Animator>().Play("Fade_Out_Animation");
            }
            else if (Note.isHoldNote && Note.Currtime < -Note.HitTimeThreshold && Note.Holding == false)
            {
                try
                {
                    Color c = GetComponent<HoldNoteScript>().lr.startColor;
                    //Debug.Log(1 - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().lr.startColor = new Color(c.r, c.g, c.b, .8f - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().lr.endColor = new Color(c.r, c.g, c.b, .8f - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .8f - (Note.Currtime / (-Note.HitTimeThreshold * 4 - NoteManager.DisplayedTimeAfter)));
                }
                catch { }
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

        }

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
    }
}
