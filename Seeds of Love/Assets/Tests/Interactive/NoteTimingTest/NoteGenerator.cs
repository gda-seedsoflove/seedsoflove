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

        /// Test variables
        private float lastlane = 0;
        private int touchspawning = 0;
        private int nextlane = 0; //Where the next note will spawn
        ///

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
            RandomSpawn();
        
        }

        /**
         * Randomly spawns notes in a certain pattern to mimic how the beatmap or rhythm game might play out
         * Spawns all types, and chance to spawn double touch and touch chaining
         */
        public void RandomSpawn()
        {
            if (touchspawning > 0 && _nextNoteSpawnTime <= CurrentSongTime) // While chaining, no other note should be spawning
            {
                int lane = nextlane;
                if (Mathf.Abs(lane - lastlane) >= 2)
                {
                    lane = Random.Range(1, LanePositions.Length - 1);
                }

                lastlane = lane;
                CreateNote(notetime, lane, -notespeed, 1);
                nextlane = Random.Range(0, LanePositions.Length);

                touchspawning = touchspawning - 1;
                if (touchspawning > 0)
                {
                    _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval / 3;
                }
                else
                {
                    _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval;
                }
            }
            else if (_nextNoteSpawnTime <= CurrentSongTime) // Normal random spawning
            {
                notetime = Mathf.Abs(EndY / notespeed);
                HitTimeThreshold = Mathf.Abs(notetime - (EndY + hitwindow) / notespeed); // Calculates the time frame the note has to be hit

                int type = Random.Range(0, 3);
                int lane = nextlane;
                // Determines the next lane the note will spawn in not 3 lanes apart.
                if (Mathf.Abs(lane - lastlane) >= 3)
                {
                    lane = Random.Range(1, LanePositions.Length - 1);
                }

                lastlane = lane;
                CreateNote(notetime, lane, -notespeed, type);
                nextlane = Random.Range(0, LanePositions.Length);

                if (type == 1)//touchnote
                {
                    if (Random.Range(0, 4) == 0)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            touchspawning = 3;
                        }
                        else
                        {
                            touchspawning = 6;
                        }
                        _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval / 3;
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 5)
                        {
                            lane = LaneNextTo(lane);
                            nextlane = lane;
                            CreateNote(notetime, lane, -notespeed, type);
                        }
                        _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval;
                    }
                }
                else if (type == 2) // Hold note, wait for 2 spawn intervals so a note wont be spawned next to the end
                {
                    _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval * 2;
                }
                else // Hit note
                {
                    _nextNoteSpawnTime = CurrentSongTime + SpawnTimeInterval;
                }
            }
        }

        /**
         * Gives a random lane next to the given lane. Used for tough chaining.
         */
        public int LaneNextTo(int lane)
        {
            if (lane == 0)
            {
                return 1;
            }
            else if (lane == 3)
            {
                return 2;
            }

            if (lane == 1)
            {
                if (Random.Range(0, 2) == 1)
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }

            if (lane == 2)
            {
                if (Random.Range(0, 2) == 1)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
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
