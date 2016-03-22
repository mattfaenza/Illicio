using UnityEngine;
using System.Collections;

public class ShatterAndHit : MonoBehaviour {
    public GameObject ThisButShattered, FallingThing;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player")) return;
        Transform temp = transform.parent;
        Instantiate(ThisButShattered,temp.position,temp.rotation);
        Vector3 pos = temp.position;
        pos.y += 8.0f + 0.5f / 2.0f;
        if (FallingThing != null) Instantiate(FallingThing, pos, temp.rotation);
        Destroy(gameObject);
    }
}
