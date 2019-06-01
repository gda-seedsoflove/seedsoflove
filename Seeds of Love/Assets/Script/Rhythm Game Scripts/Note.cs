namespace Script.Song
{
    public class Note
    {
        public float Time { get; set; }
        public float Currtime { get; set; }
        public int Lane { get; set; }
        public float Speed { get; set; }

        public bool isHitNote { get; set; }
        public bool isTouchNote { get; set;}
        public int SpecialNum { get; set;} // Default 0, if not then its special!

        public bool isHoldNote { get; set; }
        public float Length { get; set; }
        public bool Holding { get; set; }

        public bool Hit { get; set; }
        public bool Missed { get; set; }

        public char convertType(int type)
        {
            if (type == 0)
            {
                return 'a';
            }
            else if (type == 1)
            {
                return 'b';
            }
            else if (type == 2)
            {
                return '1';
            }
            else
            {
                return '2';
            }
        }

        public void setType(char type)
        {
            if (type == 'a') {
                isHitNote = true;
            }
            else if(type == 'b')
            {
                isTouchNote = true;
            }
            else if (type == '1')
            {
                Length = 1;

                isHoldNote = true;
            }
            else if (type == '2')
            {
                //do nothing
            }
        }

        public void setHoldLength(float length)
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
