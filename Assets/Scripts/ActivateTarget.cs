using UnityEngine;
using System.Collections;

public class ActivateTarget : MonoBehaviour {

    public GameObject targetToActivate;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            targetToActivate.SendMessage("Activate");
            Destroy(gameObject);
            //we need some sort of flourish here 
        }
    }
}