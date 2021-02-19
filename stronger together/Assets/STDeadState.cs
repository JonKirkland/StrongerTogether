using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STDeadState : State
{
    public override State RunCurrentState()
    {
        //just chill and be fucking dead

        return this;
    }
}
