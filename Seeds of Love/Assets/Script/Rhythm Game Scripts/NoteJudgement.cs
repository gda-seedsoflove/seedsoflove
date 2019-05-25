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

        [HideInInspector]
        public float bottomthreshhold;

        [HideInInspector]
        public bool missed;

        private Color c;

        private void Start()
        {
            // Disable this behaviour after the note is hit once. It doesn't
            // make sense to hit a note multiple times
            OnHit += _ => enabled = false;
            lastlane = 0;
            currentlane = 0;
            bottomthreshhold = -Note.HitTimeThreshold * 5 / 3;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(PlayerData.instance.spacekeybind)) && Note.Holding == false)
            {
                bufferedtime = .1f;
            }
            if ((Input.GetKeyUp(PlayerData.instance.spacekeybind)) && Note.Holding == true)
            {
                holdingspace = false;
                releasebuffertime = .15f;
            }


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
                        
                        NoteManager.addScore(1, 1);
                        OnHit(gameObject);
                    }
                    else
                    {
                        touched = true;
                    }
                    Note.Hit = true;
                }

                if(OnHit != null && touched && Note.Currtime <= Note.HitTimeThreshold*7/16) // If touch note and was "touched" then it is hit.
                {
                    NoteManager.addScore(2, 2);
                    OnHit(gameObject);
                }

                if (Note.Currtime < bottomthreshhold && missed == false && Note.Missed == false)
                {
                    missed = true;
                }
            }
            else if(Note.isHoldNote)
            {
                if (CanHit() && Note.Holding == true && (releasebuffertime > 0 || Input.GetKeyUp(PlayerData.instance.spacekeybind))) // Correctly releases at right time.
                {
                    Note.Holding = false;
                    GetComponent<HoldNoteScript>().top.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
                    Destroy(GetComponent<HoldNoteScript>().lr);
                    GetComponent<HoldNoteScript>().Release();
                    OnHit(gameObject);
                    NoteManager.addScore(10f, 10f); // completes the missing half of the note score.
                }
                else if (OnHit != null && Note.Holding == true && (releasebuffertime <= 0 || currentlane != Note.Lane) && (holdingspace == false || currentlane != Note.Lane))
                {
                    Note.Holding = false;                 
                    GetComponent<HoldNoteScript>().held = false;
                    GetComponent<HoldNoteScript>().hitcircle.SetActive(false);
                    //Destroy(GetComponent<HoldNoteScript>().lr);
                    //GetComponent<HoldNoteScript>().Release();
                    GetComponent<NoteMovement>().moving = true;
                    GetComponent<HoldNoteScript>().earlyrelease = true;
                    GetComponent<HoldNoteScript>().DestoryPulses();
                    Note.Currtime = bottomthreshhold;
                    if (missed == false)
                    {
                        NoteManager.addScore(0, 1.3f);
                        missed = true;
                    }
                }
                else if(OnHit != null && Note.Holding == true && Note.Currtime <= 0)
                {
                    GetComponent<HoldNoteScript>().online = true;
                }

                if(Note.Currtime < bottomthreshhold && missed == false && Note.Missed == false)
                {
                    NoteManager.addScore(0, 1.3f);
                    missed = true;
                }

                releasebuffertime -= Time.deltaTime;


                if (CanHit() && bufferedtime > 0 && Note.Holding == false) // Hold note start
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
                    NoteManager.addScore(1f,1f);
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
                && Note.Currtime > bottomthreshhold
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
