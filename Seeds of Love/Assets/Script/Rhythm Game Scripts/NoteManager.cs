using Script.Behaviour;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Song
{
    [RequireComponent(typeof(NoteMovement))]
    [RequireComponent(typeof(NoteJudgement))]
    public abstract class NoteManager : MonoBehaviour
    {
        /// <Descriptiom>
        /// Note manager is the backend system for managing the notes that will spawn in the rhythm game.
        /// It keeps track of the combo, score, and rhythm game characteristics like hitwindows and threshholds
        /// It also does the acutal spawning/creation of the notes and adjusts them accordingly given their type.
        ///

        public GameObject NotePrefab;
        public GameObject TouchNotePrefab;
        public GameObject HoldNotePrefab;

        public float EndY;

        //[HideInInspector]
        public float DisplayedTimeAfter, hitwindow, HitTimeThreshold;

        public float SpawnTimeInterval;

        protected BeatmapReader BMReader;

        private float score;
        private float fullscore;
        protected int combo;

        public Slider moodmeter;

        public abstract float CurrentSongTime { get; }

        protected abstract float[] LanePositions { get; }

        public Color c;

        //Types: 0 = Normal Note (On top of and hit) 1 = Touch Note (Just on top of) 2 = Hold Note (On top of and hold)
        protected Note CreateNote(float time, int lane, float speed, char type, float holdtime, int specialnum)
        {
            Note note = new Note
            {
                Time = time,
                Lane = lane,
                Speed = speed,
                Currtime = time,
                SpecialNum = specialnum,
                HitTimeThreshold = HitTimeThreshold              
            };

            note.setType(type);
            GameObject noteObject = null;
            if (note.isTouchNote)
            {           
                noteObject = Instantiate(TouchNotePrefab);
            }
            else if (note.isHoldNote)
            {
                noteObject = Instantiate(HoldNotePrefab);
                noteObject.GetComponent<HoldNoteScript>().speed = speed;
                noteObject.GetComponent<HoldNoteScript>().length = -speed * holdtime;
                note.setHoldLength(noteObject.GetComponent<HoldNoteScript>().length);
                //Debug.Log(1 / (BMReader.GetBps() * 2));
                noteObject.GetComponent<HoldNoteScript>().interval = 1 / (BMReader.GetBps() * 2);

                HoldNoteScript holdscript = noteObject.GetComponent<HoldNoteScript>();
                holdscript.NoteManager = this;
                holdscript.Note = note;
            }
            else if(note.isHitNote)
            {
                noteObject = Instantiate(NotePrefab);
            }
            else
            {
                //Do nothing
            }

            //Debug.Log("Lane:"+lane);
            if (noteObject)
            {
                noteObject.transform.position = new Vector2(LanePositions[lane], transform.position.y);

                NoteMovement noteMovement = noteObject.GetComponent<NoteMovement>();
                noteMovement.NoteManager = this;
                noteMovement.Note = note;

                NoteJudgement noteJudgement = noteObject.GetComponent<NoteJudgement>();
                noteJudgement.NoteManager = this;
                noteJudgement.Note = note;

                return note;
            }

            return null;
        }

        /**
        * Adds score out of fullpoints. 1 and 1 is 100% 3 and 10 is 30%
        */
        public void addScore(float points, float fullpoints)
        {
            score += points;
            fullscore += fullpoints;

            if (points>=fullpoints)
            {
                combo++;
            }
            else
            {
                combo = 0;
            }

            if (combo > 0 && moodmeter != null)
            {
                float multiplier = Mathf.Clamp(points+((float)combo),0,10);
                if(moodmeter.GetComponent<MoodMeterScript>().targetvalue < 20 && combo <= 1) // hidden comeback mood
                {
                    multiplier += 10;
                }
                else if (moodmeter.GetComponent<MoodMeterScript>().targetvalue < 40 && combo <= 1) // hidden comeback mood
                {
                    multiplier += 5;
                }
                else if (moodmeter.GetComponent<MoodMeterScript>().targetvalue >= 75)
                {
                    multiplier = Mathf.Clamp(Mathf.Clamp(points,1,6) + ((float)combo)/10, 0, 6f);
                }
                moodmeter.GetComponent<MoodMeterScript>().AddMood(multiplier);
            }
            else if (combo == 0 && moodmeter != null)
            {
                if (moodmeter.GetComponent<MoodMeterScript>().targetvalue < 12) // Reduces points loss when low mood
                {
                    fullpoints *= .5f;
                }
                else if (moodmeter.GetComponent<MoodMeterScript>().targetvalue < 24) // Reduces points loss when low mood
                {
                    fullpoints *= .75f;
                }
                moodmeter.GetComponent<MoodMeterScript>().AddMood(fullpoints * -12);
            }
            else
            {
                Debug.Log("Missing MoodMeter");
            }
            //Debug.Log(moodmeter.GetComponent<MoodMeterScript>().GetMood());
        }

        public float GetScore()
        {
            return (score / fullscore);
        }
    }
}
