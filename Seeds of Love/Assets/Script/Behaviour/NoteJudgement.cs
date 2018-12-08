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

        private void Start()
        {
            // Disable this behaviour after the note is hit once. It doesn't
            // make sense to hit a note multiple times
            OnHit += _ => enabled = false;
        }

        private void Update()
        {
            if (OnHit != null
                && Input.GetKeyDown(HitKey)
                && NoteManager.CurrentSongTime > Note.Time - Note.HitTimeThreshold
                && NoteManager.CurrentSongTime < Note.Time + Note.HitTimeThreshold)
                OnHit(gameObject);
        }
    }
}
