using UnityEngine;
using System.Collections;
using System;

public class FlexButtonComponent : FlexActionableComponent
{
    protected override void AssembleComponent()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SetupTriggerForwardingFor(transform.GetChild(i).gameObject);
        }
        State = 0;
    }

    protected override void StateChanged(uint _old, uint _new)
    {
        if (_new == 0)
        {
            transform.FindChild("Body").GetComponent<Renderer>().material.color = Color.white;
        }
        else if (_new == 2)
        {
            transform.FindChild("Body").GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.FindChild("Body").GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
