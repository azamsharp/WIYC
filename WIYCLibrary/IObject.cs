namespace WIYCLibrary
{
    using System;

    public interface IObject
    {
        string Key { get; set; }

        double Size { get; set; }

        string TypeName { get; set; }

        object Value { get; set; }
    }
}

