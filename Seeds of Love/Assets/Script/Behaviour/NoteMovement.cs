using Script.Song;
using UnityEngine;

namespace Script.Behaviour
{
    public class NoteMovement : MonoBehaviour
    {
        public NoteManager NoteManager { get; set; }
        public Note Note { get; set; }
        public bool Moving = true;

        private void Update()
        {
            // Self-destruct if the note is outside of the displayed time range
            if (Note.Time < NoteManager.CurrentSongTime - NoteManager.DisplayedTimeAfter)
                Destroy(gameObject);

            // Set the current position of the note by interpolating between
            // the start and end positions
            if (Moving)
            {
                transform.position = Vector2.Lerp(
                    new Vector2(transform.position.x, NoteManager.StartY),
                    new Vector2(transform.position.x, NoteManager.EndY),
                    (NoteManager.CurrentSongTime - Note.Time + NoteManager.DisplayedTimeBefore) / (NoteManager.DisplayedTimeBefore + NoteManager.DisplayedTimeAfter)
                );
            }
        }
    }
}
