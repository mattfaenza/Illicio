using UnityEngine;
using System.Collections;

public class ActivateIce : MonoBehaviour
{

    private enum IceState { IDLE, ON, OFF };
    private IceState state = IceState.IDLE;

    private float onTime = 2.0f, scaleFactor = 10.0f;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (state == IceState.ON) IceOn();
        if (state == IceState.OFF) IceOff();
    }

    void Scale(float factor)
    {
        transform.localScale += new Vector3(factor,factor,factor);
    }
    void IceOn()
    {
        Scale(0.2F);
        if (transform.localScale.x >= 2)
        {
            state = IceState.IDLE;
        }

    }

    void IceOff()
    {
        Scale(-0.2F);
        if (transform.localScale.x <= 0.02f)
        {
            state = IceState.IDLE;
        }
        
    }

    void Activate()
    {
        Debug.Log("activated");

                state = IceState.ON;
    }

    void Deactivate()
    {

        state = IceState.OFF;
    }

}

