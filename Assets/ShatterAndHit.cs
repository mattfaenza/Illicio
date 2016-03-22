using UnityEngine;
using System.Collections;

public class ShatterAndHit : MonoBehaviour {
    public GameObject ThisButShattered;
    public float shatterLifeTime = 10.0f;
    void OnCollisionEnter(Collision col) {
        if (!col.gameObject.CompareTag("Enemy")) return;
        Transform tmp = transform.parent;
        col.gameObject.SendMessage("HitPillar", tmp.position);
        GameObject temp = (GameObject)Instantiate(ThisButShattered,tmp.position,tmp.rotation);
        Destroy(temp, shatterLifeTime);
        Destroy(gameObject);
    }
}
