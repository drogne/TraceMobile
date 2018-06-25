using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.Utilities.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassFailSample.Utilities
{
    public class ButtonItem
    {
        public string DisplayName { get; private set; }
        public ICommand Command { get; private set; }
        public Color Color { get; private set; }

        public ButtonItem(string displayName, ICommand command, Color color)
        {
            this.DisplayName = displayName;
            this.Command = command;
            this.Color = color;
        }
    }
}