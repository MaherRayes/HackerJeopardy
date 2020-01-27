using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step
{

    protected Step() { }

    public abstract void Undo();

    public abstract bool BoardMode();



}
