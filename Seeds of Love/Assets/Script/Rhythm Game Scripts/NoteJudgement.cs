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

        private bool holdingspace;

        private int lastlane;

        private int currentlane;

        private float lastlanebuffer;

        private Color c;

        private void Start()
        {
            // Disable this behaviour after the note is hit once. It doesn't
            // make sense to hit a note multiple times
            OnHit += _ => enabled = false;
            lastlane = 0;
            currentlane = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(HitKey) && Note.Holding == false)
            {
                bufferedtime = .1f;
            }
            if (Input.GetKeyUp(HitKey) && Note.Holding == true)
            {
                holdingspace = false;
                releasebuffertime = .15f;
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
            c = NoteManager.c;
            GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 1);

        }

        void FixedUpdate()
        {
            CheckLanes();

            if (!Note.isHoldNote)
            {
                if (CanHit() && (bufferedtime > 0 || Note.isTouchNote)) // Hitting the note correctly or touched the note.
                {
                    if (!Note.isTouchNote)
                    {
                        OnHit(gameObject);
                    }
                    else
                    {
                        touched = true;
                    }
                    NoteManager.addScore(1, 1);
                    Note.Hit = true;
                }

                if(OnHit != null && touched && Note.Currtime <= Note.HitTimeThreshold*7/16) // If touch note and was "touched" then it is hit.
                {
                    OnHit(gameObject);
                }
            }
            else if(Note.isHoldNote)
            {
                if (CanHit() && Note.Holding == true && releasebuffertime > 0) // Correctly releases at right time.
                {
                    Note.Holding = false;
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
                    Destroy(GetComponent<HoldNoteScript>().lr);
                    GetComponent<HoldNoteScript>().Release();
                    OnHit(gameObject);
                    NoteManager.addScore(.5f, 0); // completes the missing half of the note score.
                }
                else if (OnHit != null && Note.Holding == true && (releasebuffertime <= 0 || currentlane != Note.Lane) && (holdingspace == false || currentlane != Note.Lane))
                {
                    Note.Holding = false;
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
                    GetComponent<HoldNoteScript>().held = false;
                    Destroy(GetComponent<HoldNoteScript>().lr);
                    GetComponent<HoldNoteScript>().Release();
                }
                else if(OnHit != null && Note.Holding == true && Note.Currtime <= 0)
                {
                    GetComponent<HoldNoteScript>().online = true;
                }
                releasebuffertime -= Time.deltaTime;


                if (CanHit() && bufferedtime > 0 && Note.Holding == false)
                {
                    holdingspace = true;
                    Note.Currtime = Note.Currtime +  Note.Length/-Note.Speed;
                    Note.Holding = true;
                    GetComponent<EffectsManager>().PlaySoundEffect();
                    if (GetComponent<HoldNoteScript>())
                    {
                        GetComponent<HoldNoteScript>().Hit(NoteManager.transform.position.y - NoteManager.EndY);
                    }
                    if (GetComponent<NoteMovement>())
                    {
                        GetComponent<NoteMovement>().moving = false;
                        GetComponent<HoldNoteScript>().bottom.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0);
                    }
                    NoteManager.addScore(.5f,1);
                    Note.Hit = true;
                }

            }
            bufferedtime -= Time.deltaTime;

            

        }

        /**
         *  Returns true if the note is in the correct time and lane.
         */
        public bool CanHit()
        {
            if (OnHit != null
                && Note.Currtime > -Note.HitTimeThreshold * 5 / 3
                && Note.Currtime < Note.HitTimeThreshold
                && (currentlane == Note.Lane || (Note.Lane == lastlane && lastlanebuffer > 0)))
            {
                return true;
            }
                return false;
        }

        public int CheckLanes()
        {
            int dlane = 0;
            if (GameObject.FindGameObjectWithTag("Detector"))
            {
                // Looks for the Detector and checks if it has the Note detector component;
                dlane = GameObject.FindGameObjectWithTag("Detector").GetComponent<NoteDetector>().Lane;
                if (lastlanebuffer >= 0)
                {
                    lastlanebuffer -= Time.deltaTime;
                    if (lastlanebuffer <= 0)
                    {
                        lastlane = dlane;
                    }
                }
                if (dlane != currentlane)
                {
                    lastlane = currentlane;
                    currentlane = dlane;
                    lastlanebuffer = .08f;
                }
                return dlane;
            }
            return 0;
        }
    }
}
