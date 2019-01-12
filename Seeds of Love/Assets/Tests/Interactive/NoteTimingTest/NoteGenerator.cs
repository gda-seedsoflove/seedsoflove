using Script.Song;
using UnityEditor;
using UnityEngine;

namespace Tests.Interactive.NoteTimingTest
{
    public class NoteGenerator : NoteManager
    {
        public float SpawnTimeInterval;

        private float _nextNoteSpawnTime;

        public float spacing;


        public override float CurrentSongTime
        {
            get { return Time.time; }
        }

        protected override float[] LanePositions
        {
            get { return new[] { gameObject.transform.position.x, gameObject.transform.position.x + spacing, gameObject.transform.position.x + (2 * spacing), gameObject.transform.position.x + (3 * spacing) }; }
        }

        private void Start()
        {
            _nextNoteSpawnTime = Time.time;
        }

        private void Update()
        {
            // If the next note to be spawned is overdue, spawn it, and queue
            // the time for the next note
            if (_nextNoteSpawnTime <= CurrentSongTime)
            {
                CreateNote(
                    _nextNoteSpawnTime + DisplayedTimeBefore,
                    Random.Range(0, LanePositions.Length)
                );

                _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.grey;
            for (int i =0; i< LanePositions.Length; i++)
            {

            }
            //Handles.DrawLine();
        }
    }
}
