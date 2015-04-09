using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void ReadState()
        {
            OldPressedButtons = PressedButtons;
            PressedButtons = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                var state = Joystick.GetState(i);
                if (state.IsConnected)
                {
                    for (int button = 1; button < 32; button++)
                    {
                        if (state.GetButton((JoystickButton)button) == ButtonState.Pressed)
                        {
                            PressedButtons.Add("J" + i.ToString() + "B" + button.ToString("00"));
                            // TODO Remove this, exists only fortiming client BackCompat
                            PressedButtons.Add("J" + i.ToString() + "B" + button.ToString("0"));
                        }
                    }

                    var hat = state.GetHat(JoystickHat.Hat0);
                    if (hat.Position != HatPosition.Centered)
                    {
                        PressedButtons.Add("J" + i.ToString() + "HAT" + hat.Position.ToString());
                    }
                }
            }
        }

        public IEnumerable<string> GetInputs()
        {
            OldPressedButtons = PressedButtons;
            PressedButtons = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                for (int button = 1; button < 32; button++)
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