namespace Script.Song
{
    public class Note
    {
        public float Time { get; set; }
        public int Lane { get; set; }

        // The amount of time (in seconds) before and after the note during
        // which the note is able to be hit
        public float HitTimeThreshold { get; set; }
    }
}
