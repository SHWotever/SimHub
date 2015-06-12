using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACToolsUtilities.Input
{
    public class InputManager
    {
        private class InputTracking
        {
            public DateTime Date { get; set; }

            public bool Invalidated { get; set; }
        }

        private int LongPressLength = 750;

        private Dictionary<string, InputTracking> CurrentInputs = new Dictionary<string, InputTracking>();

        private List<string> NewInputs = new List<string>();
        private List<string> ReleasedInputs = new List<string>();
        private List<string> LongInputs = new List<string>();
        private List<string> ShortInputs = new List<string>();

        public List<string> GetNewInputs()
        {
            return NewInputs.ToList();
        }

        public List<string> GetReleasedInputs()
        {
            return ReleasedInputs.ToList();
        }

        public List<string> GetShortInputs()
        {
            return ShortInputs.ToList();
        }

        public delegate void LongPressDelegate(string input);

        public event LongPressDelegate LongPress;

        public void SetCurrentInputs(List<string> inputs)
        {
            DateTime refTime = DateTime.Now;
            lock (this)
            {
                NewInputs = inputs.Where(i => !CurrentInputs.ContainsKey(i)).ToList();

                var tmpValidReleasedInputs = CurrentInputs.Where(i => !inputs.Exists(j => j == i.Key) && !i.Value.Invalidated).ToList();
                var tmpReleasedInputs = CurrentInputs.Where(i => !inputs.Exists(j => j == i.Key)).ToList();

                ReleasedInputs.Clear();
                ShortInputs.Clear();

                foreach (var tmp in tmpValidReleasedInputs)
                {
                    ShortInputs.Add(tmp.Key);
                }

                foreach (var tmp in tmpReleasedInputs)
                {
                    ReleasedInputs.Add(tmp.Key);
                }

                foreach (var input in tmpReleasedInputs)
                {
                    CurrentInputs.Remove(input.Key);
                }

                foreach (var input in CurrentInputs)
                {
                    if ((refTime - input.Value.Date).TotalMilliseconds > LongPressLength)
                    {
                        LongInputs.Add(input.Key);
                        input.Value.Invalidated = true;
                    }
                }

                foreach (var input in inputs)
                {
                    if (!CurrentInputs.ContainsKey(input))
                    {
                        var mapping = new InputTracking { Date = refTime, Invalidated = false };
                        CurrentInputs.Add(input, mapping);

                        Task.Delay(LongPressLength).ContinueWith((k) =>
                        {
                            lock (this)
                            {
                                if (CurrentInputs.ContainsKey(input) && CurrentInputs[input] == mapping)
                                {
                                    var item = CurrentInputs.First(i => i.Value == mapping);
                                    item.Value.Invalidated = true;
                                    if (LongPress != null)
                                    {
                                        LongPress(item.Key);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}