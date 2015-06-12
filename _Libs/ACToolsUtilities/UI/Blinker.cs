using System;

namespace ACToolsUtilities.UI
{
    public class AlertManager
    {
        private int alertRecurency = 100;

        public int AlertRecurency
        {
            get { return alertRecurency; }
            set { alertRecurency = value; }
        }

        private DateTime start;
        private bool started = false;

        public bool Started
        {
            set
            {
                if (started != value)
                {
                    this.start = DateTime.MinValue;
                }
                started = value;
            }
            get
            {
                return started;
            }
        }

        public bool Elapsed
        {
            get { return started && (DateTime.Now - start).TotalSeconds > alertRecurency; }
        }

        public void Reset()
        {
            if (this.Elapsed)
            {
                this.start = DateTime.Now;
            }
        }
    }

    public class Blinker
    {
        private int blinkTime = 100;

        public int BlinkTime
        {
            get { return blinkTime; }
            set { blinkTime = value; }
        }

        private DateTime start;
        private bool started = false;

        public bool Started
        {
            set
            {
                if (started != value)
                {
                    this.start = DateTime.Now;
                }
                started = value;
            }
            get
            {
                return started;
            }
        }

        public bool HasBeenStarted(int SinceMill)
        {
            return started || (DateTime.Now - start).TotalMilliseconds <= SinceMill;
        }

        public bool Blink
        {
            get
            {
                return started && (DateTime.Now - start).TotalMilliseconds % (blinkTime * 2) < blinkTime;
            }
        }
    }
}