using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace ACToolsUtilities
{
    public class JoystickManager
    {
        private List<string> OldPressedButtons = new List<string>();
        private List<string> PressedButtons = new List<string>();

        public List<string> ButtonPressed()
        {
            lock (this)
            {
                var result = PressedButtons.Where(i => !OldPressedButtons.Contains(i)).ToList();
                result.ForEach(i => Console.WriteLine(i));
                return result;
            }
        }

        public List<string> ButtonReleased()
        {
            lock (this)
            {
                var result = OldPressedButtons.Where(i => !PressedButtons.Contains(i)).ToList();
                result.ForEach(i => Console.WriteLine("Relased" + i));
                return result;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public void ReadState()
        {
            lock (this)
            {
                OldPressedButtons = PressedButtons;
                PressedButtons = new List<string>();

                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var state = Joystick.GetState(i);
                        if (state.IsConnected)
                        {
                            for (int button = 0; button < 32; button++)
                            {
                                if (state.GetButton((JoystickButton)button) == ButtonState.Pressed)
                                {
                                    PressedButtons.Add("J" + i.ToString() + "B" + button.ToString("00"));
                                    // TODO Remove this, exists only fortiming client BackCompat
                                    //PressedButtons.Add("J" + i.ToString() + "B" + button.ToString("0"));
                                }
                            }

                            var hat = state.GetHat(JoystickHat.Hat0);
                            if (hat.Position != HatPosition.Centered)
                            {
                                PressedButtons.Add("J" + i.ToString() + "HAT" + hat.Position.ToString());
                            }
                        }
                    }
                    PressedButtons = PressedButtons.Distinct().ToList();
                }
                catch { }
            }
        }


        public List<string> GetState()
        {
            lock (this)
            {
                
                var currentButtons = new List<string>();

                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var state = Joystick.GetState(i);
                        if (state.IsConnected)
                        {
                            for (int button = 0; button < 32; button++)
                            {
                                if (state.GetButton((JoystickButton)button) == ButtonState.Pressed)
                                {
                                    currentButtons.Add("J" + i.ToString() + "B" + button.ToString("00"));
                                    // TODO Remove this, exists only fortiming client BackCompat
                                    //PressedButtons.Add("J" + i.ToString() + "B" + button.ToString("0"));
                                }
                            }

                            var hat = state.GetHat(JoystickHat.Hat0);
                            if (hat.Position != HatPosition.Centered)
                            {
                                currentButtons.Add("J" + i.ToString() + "HAT" + hat.Position.ToString());
                            }
                        }
                    }
                    currentButtons = currentButtons.Distinct().ToList();
                }
                catch { }

                return currentButtons;
            }
        }

        public IEnumerable<string> GetInputs()
        {
            OldPressedButtons = PressedButtons;
            PressedButtons = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                for (int button = 0; button < 32; button++)
                {
                    yield return "J" + i.ToString() + "B" + button.ToString("00");
                }

                yield return "J" + i.ToString() + "HAT" + HatPosition.Down.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.DownLeft.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.DownRight.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.Left.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.Right.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.Up.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.UpLeft.ToString();
                yield return "J" + i.ToString() + "HAT" + HatPosition.UpRight.ToString();
            }
        }
    }
}