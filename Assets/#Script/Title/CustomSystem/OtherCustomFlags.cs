using System;

namespace Title.Custom
{

    public struct OtherCustomFlags
    {
        private CustomOtherType _flags;

        private OtherCustomFlags(int value)
        {
            _flags = (CustomOtherType)value;
        }

        public static OtherCustomFlags Create(int value = 0)
        {
            return new OtherCustomFlags(value);
        }

        public void Add(CustomOtherType flags)
        {
            _flags |= flags;
        }

        public void Remove(CustomOtherType flags)
        {
            _flags &= ~flags;
        }

        public bool Has(CustomOtherType value)
        {
            return (_flags & value) != 0;
        }

        public override string ToString()
        {
            return _flags.ToString();
        }

        public int ToInt()
        {
            return (int)_flags;
        }
    }
}

[Flags]
public enum CustomOtherType
{
    None = 0,
    FieldEffect = 1 << 0,
    TapEffect = 1 << 1,
    StagePerformance = 1 << 2,
    MissPerformance = 1 << 3,
}