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

        if (col.gameObject.tag == "Player")
        {

            if (this.transform.parent.gameObject.GetComponent<EnemyAIMovement>().followPlayer == false)
            {
                this.transform.parent.gameObject.GetComponent<EnemyAIMovement>().followPlayer = true;
                this.transform.parent.gameObject.GetComponent<EnemyAIMovement>().moveInPath = false;


            }

        }

    }
}
