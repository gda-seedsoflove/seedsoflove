using Script.Song;
using UnityEditor;
using UnityEngine;

namespace Tests.Interactive.NoteTimingTest
{
    public class NoteGenerator : NoteManager
    {

        private float _nextNoteSpawnTime;

        public float spacing;

        public float notespeed;

        private float notetime;


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
                notetime = Mathf.Abs(EndY / notespeed);
                HitTimeThreshold = Mathf.Abs(notetime - (EndY + hitwindow) / notespeed);
                /**
                CreateNote(
                    _nextNoteSpawnTime + DisplayedTimeBefore,
                    Random.Range(0, LanePositions.Length), notespeed
                );
            */
                int type = Random.Range(0, 3);
                CreateNote(notetime, Random.Range(0, LanePositions.Length), -notespeed, type);

                _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval;
            }
        }

        private void OnDrawGizmosSelected()
        {
            notetime = Mathf.Abs(EndY / notespeed);
            //float bline = ((notetime + HitTimeThreshold) * notespeed);
            //float tline = EndY - (Mathf.Abs(EndY - bline));
            float bline = EndY + hitwindow;
            float tline = EndY - hitwindow;
            Handles.color = Color.grey;
            //Lane drawings
            for (int i =0; i< LanePositions.Length; i++)
            {
                Vector2 start = new Vector2(transform.position.x+ (spacing*i),transform.position.y);
                Vector2 end = new Vector2(transform.position.x + (spacing*i), transform.position.y - EndY);
                Handles.DrawLine(start, end);
            }
            Handles.color = Color.red;
            Handles.DrawLine(new Vector2(transform.position.x - spacing, transform.position.y - EndY), new Vector2(transform.position.x + (spacing * LanePositions.Length), transform.position.y - EndY));
            Handles.DrawLine(new Vector2(transform.position.x - spacing, transform.position.y - tline), new Vector2(transform.position.x + (spacing * LanePositions.Length), transform.position.y -tline ));
            Handles.DrawLine(new Vector2(transform.position.x - spacing, transform.position.y - bline), new Vector2(transform.position.x + (spacing * LanePositions.Length), transform.position.y - bline));
        }
    }
}
