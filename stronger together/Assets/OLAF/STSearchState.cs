using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STSearchState : State
{
    public override State RunCurrentState()
    {
        //increased view distance
        //searches around the area last seen the player
        //returns pursue target state if player is found
        //returns patrol state if player isnt found by a certain time (or maybe a state to return to waypoint)

        return this;
    }
}
