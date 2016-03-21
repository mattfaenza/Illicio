using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float MinX;
    public float MaxX;
    public float ConY;
    public float MinZ;
    public float MaxZ;

    public GameObject player;
    private Vector3 offset;

	void Start () {
        offset = player.transform.position - transform.position;
	}
	
	void Update () {
        Vector3 temp = player.transform.position;
        temp.x = Mathf.Max(temp.x, MinX);
        temp.x = Mathf.Min(temp.x, MaxX);
        temp.y = ConY;
        temp.z = Mathf.Max(temp.z, MinZ);
        temp.z = Mathf.Min(temp.z, MaxZ);

        transform.position = temp - offset;
    }
}
