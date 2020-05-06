using System;

namespace Tamagotchi.Model
{
    public class StatsChanged : EventArgs
    {
        public string PropertyName { get; }

        public StatsChanged(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
