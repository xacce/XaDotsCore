using System;

namespace Core.Runtime
{
    [Flags]
    public enum XaObjectType
    {
        Nothing = 0,
        Dynamic = 1,
        Static = 2,
        Character = 4,
        Trigger = 8,
    }
}