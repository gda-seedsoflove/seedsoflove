using Script.Behaviour;
using UnityEngine;

namespace Script.Song
{
    [RequireComponent(typeof(NoteMovement))]
    [RequireComponent(typeof(NoteJudgement))]
    public abstract class NoteManager : MonoBehaviour
    {
        public GameObject NotePrefab;

        public float StartY;
        public float EndY;
        public float DisplayedTimeBefore;
        public float DisplayedTimeAfter;
        public float HitTimeThreshold;

        public abstract float CurrentSongTime { get; }

        protected abstract float[] LanePositions { get; }

        protected void CreateNote(float time, int lane)
        {
            Note note = new Note
            {
                Time = time,
                Lane = lane,
                HitTimeThreshold = HitTimeThreshold
            };

            GameObject noteObject = Instantiate(NotePrefab);
            noteObject.transform.position = new Vector2(LanePositions[lane], StartY);

            NoteMovement noteMovement = noteObject.GetComponent<NoteMovement>();
            noteMovement.NoteManager = this;
            noteMovement.Note = note;

            NoteJudgement noteJudgement = noteObject.GetComponent<NoteJudgement>();
            noteJudgement.NoteManager = this;
            noteJudgement.Note = note;
        }
    }
}
