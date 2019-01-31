using Script.Song;
using UnityEngine;

namespace Script.Behaviour
{
    public class NoteJudgement : MonoBehaviour
    {
        public KeyCode HitKey;

        public NoteManager NoteManager { get; set; }
        public Note Note { get; set; }

        public delegate void HitHandler(GameObject note);

        public event HitHandler OnHit;

        private float bufferedtime; //Buffering the input to avoid misregistered presses

        private float releasebuffertime; //Buffering the release input to avoid misregistered presses

        private bool touched;       //Will automatically "Hit" the note at hit time

        private void Start()
        {
            // Disable this behaviour after the note is hit once. It doesn't
            // make sense to hit a note multiple times
            OnHit += _ => enabled = false;

        }

        private void Update()
        {
            if (Input.GetKeyDown(HitKey) && Note.Holding == false)
            {
                bufferedtime = .1f;
            }
            if (Input.GetKeyUp(HitKey) && Note.Holding == true)
            {
                releasebuffertime = .1f;
            }
            /**
            if (OnHit != null
                && Input.GetKeyDown(HitKey)
                && NoteManager.CurrentSongTime > Note.Time - Note.HitTimeThreshold
                && NoteManager.CurrentSongTime < Note.Time + Note.HitTimeThreshold
                && dlane == Note.Lane)
            {
                OnHit(gameObject);
            }
            */

        }

        void FixedUpdate()
        {
            int dlane = 0;
            if (GameObject.FindGameObjectWithTag("Detector"))
            {
                // Looks for the Detector and checks if it has the Note detector component;
                dlane = GameObject.FindGameObjectWithTag("Detector").GetComponent<NoteDetector>().Lane;
            }

            if (!Note.isHoldNote)
            {
                if (OnHit != null && (bufferedtime > 0 || Note.isTouchNote)
                && Note.Currtime > -Note.HitTimeThreshold
                && Note.Currtime < Note.HitTimeThreshold
                && dlane == Note.Lane)
                {
                    if (!Note.isTouchNote)
                    {
                        OnHit(gameObject);
                    }
                    else
                    {
                        touched = true;
                    }
                }

                if(OnHit != null && touched && Note.Currtime <= Note.HitTimeThreshold*7/16)
                {
                    OnHit(gameObject);
                }
            }
            else
            {
                if (OnHit != null && Note.Holding == true
                && ((Note.Currtime > -Note.HitTimeThreshold
                && Note.Currtime < Note.HitTimeThreshold) && releasebuffertime >0)
                && dlane == Note.Lane)
                {
                    Note.Holding = false;
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    Destroy(GetComponent<HoldNoteScript>().lr);
                    OnHit(gameObject);
                }
                else if (OnHit != null && Note.Holding == true && (releasebuffertime > 0 || dlane != Note.Lane))
                {
                    Note.Holding = false;
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    Destroy(GetComponent<HoldNoteScript>().lr);
                    //OnHit(gameObject);
                }
                else if(OnHit != null && Note.Holding == true && Note.Currtime <= 0)
                {
                    GetComponent<HoldNoteScript>().online = true;
                }

                if (OnHit != null && bufferedtime > 0 && Note.Holding == false
                && Note.Currtime > -Note.HitTimeThreshold
                && Note.Currtime < Note.HitTimeThreshold
                && dlane == Note.Lane)
                {
                    bufferedtime = 0;
                    Note.Currtime = Note.Currtime + NoteManager.SpawnTimeInterval/2 * Note.Length;
                    Note.Holding = true;
                    if (GetComponent<HoldNoteScript>())
                    {
                        Vector2 pos = GetComponent<HoldNoteScript>().bottom.transform.position;
                        float topy = GetComponent<HoldNoteScript>().top.transform.position.y;
                        //GetComponent<HoldNoteScript>().bottom.transform.position = new Vector2(pos.x, NoteManager.transform.position.y - NoteManager.EndY);
                        GetComponent<HoldNoteScript>().held = true;
                        transform.position = new Vector2(pos.x, NoteManager.transform.position.y - NoteManager.EndY);
                        GetComponent<HoldNoteScript>().top.transform.position = new Vector2(pos.x, topy);
                        GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().color = new Color(1,1,.8f);
                        GetComponent<HoldNoteScript>().lr.SetPosition(1, GetComponent<HoldNoteScript>().top.transform.position);
                        GetComponent<HoldNoteScript>().lr.SetColors(new Color(1f, 1f, 1f), new Color(1f, 1f, 1f));
                    }
                    if (GetComponent<NoteMovement>())
                    {
                        GetComponent<NoteMovement>().moving = false;
                        GetComponent<HoldNoteScript>().bottom.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                    }
                }

            }
            bufferedtime -= Time.deltaTime;
        }
    }
}
