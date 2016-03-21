using UnityEngine;
using System.Collections;

public class ChangePosition : MonoBehaviour {

    public Vector3 newPos;
    private bool activated;
    public Vector3 pos;

    void Start()
    {
        activated = false;
        pos = transform.position;
    }

    void Activate()
    {
        activated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            pos = newPos;
        }
    }
}
