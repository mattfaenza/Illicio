using UnityEngine;
using System.Collections;

public class ShatterAndHit : MonoBehaviour {
    public GameObject ThisButShattered;
    public float shatterLifeTime = 4.0f;
    void OnCollisionEnter(Collision col) {
        if (!col.gameObject.CompareTag("Enemy")) return;
        GameObject temp = (GameObject)Instantiate(ThisButShattered, transform.position, transform.rotation);
        temp.transform.localScale = transform.localScale;
        Destroy(temp, shatterLifeTime);
        Destroy(gameObject);
    }
}
