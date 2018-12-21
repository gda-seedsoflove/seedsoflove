using Script.Song;
using UnityEngine;

namespace Tests.Interactive.NoteTimingTest
{
    public class NoteGenerator : NoteManager
    {
        public float SpawnTimeInterval;

        private float _nextNoteSpawnTime;

        public override float CurrentSongTime
        {
            get { return Time.time; }
        }

        protected override float[] LanePositions
        {
            get { return new[] {-6.2f, -5.4f, -4.6f, -3.8f}; }
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
    }
}
