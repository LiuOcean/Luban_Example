using System;

namespace Sirenix.OdinInspector;

public class LabelTextAttribute : Attribute
{
    public string name;

    public LabelTextAttribute(string name) => this.name = name;
}