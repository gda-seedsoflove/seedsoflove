namespace Script.Song
{
    public class Note
    {
        public float Time { get; set; }
        public float Currtime { get; set; }
        public int Lane { get; set; }
        public float Speed { get; set; }

        public bool isTouchNote { get; set;}

        public bool isHoldNote { get; set; }
        public int Length { get; set; }
        public bool Holding { get; set; }

        public void setType(int type)
        {
            if (type == 0) {
                //Do nothing, its a normal not
            }
            else if(type == 1)
            {
                isTouchNote = true;
            }
            else if (type == 2)
            {
                Length = 1;
                isHoldNote = true;
            }
        }

        public void setHoldLength(int length)
        {
            if (length>0 && isHoldNote)
            {
                this.Length = length;
            }
        }

        // The amount of time (in seconds) before and after the note during
        // which the note is able to be hit
        public float HitTimeThreshold { get; set; }
    }
}
