using UnityEngine;
using System.Collections;

public class ActivateTarget : MonoBehaviour {

    public GameObject target1ToActivate;
    public GameObject target2ToActivate;
    public GameObject target3ToActivate;
    public GameObject target4ToActivate;
    public GameObject target5ToActivate;

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
  //          target1ToActivate.SendMessage("Activate");
//            target2ToActivate.SendMessage("Activate");
            target3ToActivate.SendMessage("Activate");
            target4ToActivate.SendMessage("Activate");
            target5ToActivate.SendMessage("boostAllowed");
            Destroy(gameObject);
            //we need some sort of flourish here 
        }
    }
}