using Script.Behaviour;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Song
{
    [RequireComponent(typeof(NoteMovement))]
    [RequireComponent(typeof(NoteJudgement))]
    public abstract class NoteManager : MonoBehaviour
    {
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
        protected Note CreateNote(float time, int lane, float speed, char type, float holdtime)
        {
            Note note = new Note
            {
                Time = time,
                Lane = lane,
                Speed = speed,
                Currtime = time,
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
                noteObject.GetComponent<HoldNoteScript>().interval = 1 / (BMReader.GetBps() * 2);

                HoldNoteScript holdscript = noteObject.GetComponent<HoldNoteScript>();
                holdscript.NoteManager = this;
                holdscript.Note = note;
            }
            else
            {
                noteObject = Instantiate(NotePrefab);
            }

            //Debug.Log("Lane:"+lane);
            noteObject.transform.position = new Vector2(LanePositions[lane], transform.position.y);

            NoteMovement noteMovement = noteObject.GetComponent<NoteMovement>();
            noteMovement.NoteManager = this;
            noteMovement.Note = note;

            NoteJudgement noteJudgement = noteObject.GetComponent<NoteJudgement>();
            noteJudgement.NoteManager = this;
            noteJudgement.Note = note;

            return note;
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
                float multiplier = Mathf.Clamp(4+((float)combo),5,100);
                moodmeter.GetComponent<MoodMeterScript>().AddMood(multiplier);
            }
            else if (combo == 0 && moodmeter != null)
            {
                moodmeter.GetComponent<MoodMeterScript>().AddMood(fullpoints * -5);
            }
            else
            {
                Debug.Log("Missing MoodMeter");
            }
        }

        public float GetScore()
        {
            return (score / fullscore);
        }
    }
}
