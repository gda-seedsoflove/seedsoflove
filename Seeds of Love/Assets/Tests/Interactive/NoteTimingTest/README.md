# NoteTimingTest

This test sets up an instance of a `NoteManager` that randomly spawns a note every `SpawnTimeInterval` (1) seconds.

The player can press `HitKey` (Space) to hit the notes if the current time is within `HitTimeThreshold` (0.2) seconds of the notes' times. When a note is hit, it will change its color to `ColorAfterHit` (a light orange).
