using Script.Behaviour;
using UnityEngine;

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
        public float DisplayedTimeAfter;
        public float hitwindow;
        public float HitTimeThreshold;

        public float SpawnTimeInterval;

        public abstract float CurrentSongTime { get; }

        protected abstract float[] LanePositions { get; }

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
    }
}
