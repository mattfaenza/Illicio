using UnityEngine;
using System.Collections;

public class PlayerDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {

		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Hologram"  )
        {
//			this.transform.parent.gameObject.GetComponent<BullAIMovement> ().attackPlayer ();


            if (this.transform.parent.gameObject.GetComponent<BullAIMovement>().followPlayer == false)
            {
                this.transform.parent.gameObject.GetComponent<BullAIMovement>().followPlayer = true;
                this.transform.parent.gameObject.GetComponent<BullAIMovement>().moveInPath = false;


            }

        }

    }
}
