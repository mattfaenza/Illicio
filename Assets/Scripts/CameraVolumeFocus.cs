/**
 * Attach to player character.
 * Set Main Camera to your Main Camera
 * Create room volumes, remove the renderer, set colliders to trigger, and tag them as 'Volume'
 * There should be a 'RoomVolumes' prefab as an example.
 * Ensure a Respawn object exists that is tagged 'Respawn'.
 */

using UnityEngine;
using System.Collections;

public class CameraVolumeFocus : MonoBehaviour {
    public float speed = 10.0f;
    public GameObject MainCamera;
    private Vector3 destination;
    private bool newRoom;
    public GameObject playerChar;
    private GameObject Spawn;
    private bool Dead;
    private Vector3 playerPos;

    void Start()
    {
        Dead = false;
    }

    void Update() {
        MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, destination, speed);
        if (newRoom)
        {
            Spawn = GameObject.FindWithTag("Respawn");
            //update spawn position using player's current position
            //playerChar = GameObject.FindWithTag("Player");
            if (!Dead)
            {
                Spawn.transform.position = playerChar.transform.position;
            }
            newRoom = false;
        }
    }

    void isDead() {
        Dead = true;
    }

    void isNotDead()
    {
        Dead = false;
    }

    Vector2 intersection2d(Vector2 a_p, Vector2 a_v, Vector2 b_p, Vector2 b_v) {
        float y = (b_p.x - a_p.x + a_v.x * a_p.y / a_v.y - b_v.x * b_p.y / b_v.y) / (a_v.x / a_v.y - b_v.x / b_v.y);
        float x = a_p.x + a_v.x * (y - a_p.y) / a_v.y;
        return new Vector2(x, y);
    }
    Vector3 intersection3d(Vector3 a_p, Vector3 a_v, Vector3 b_p, Vector3 b_v) {
        if (a_p.x == b_p.x) {
            Vector2 temp = intersection2d(
                new Vector2(a_p.y, a_p.z),
                new Vector2(a_v.y, a_v.z),
                new Vector2(b_p.y, b_p.z),
                new Vector2(b_v.y, b_v.z));
            return new Vector3(a_p.x, temp.x, temp.y);
        } else if (a_p.y == b_p.y) {
            Vector2 temp = intersection2d(
                new Vector2(a_p.x, a_p.z),
                new Vector2(a_v.x, a_v.z),
                new Vector2(b_p.x, b_p.z),
                new Vector2(b_v.x, b_v.z));
            return new Vector3(temp.x, a_p.y, temp.y);
        } else if (a_p.z == b_p.z) {
            Vector2 temp = intersection2d(
                new Vector2(a_p.x, a_p.y),
                new Vector2(a_v.x, a_v.y),
                new Vector2(b_p.x, b_p.y),
                new Vector2(b_v.x, b_v.y));
            return new Vector3(temp.x, temp.y, a_p.z);
        }
        return Vector3.zero;
    }
    void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("Volume")) return;
        newRoom = true;
        Vector3 pos, scl, top, bot, lft, rgt;
        pos = col.gameObject.transform.position;
        scl = col.gameObject.transform.lossyScale;
        // set x comp (+ left MainCameraera)
        top.x = bot.x = pos.x;
        lft.x = pos.x + scl.x / 2;
        rgt.x = pos.x - scl.x / 2;
        // set y comp (+ up MainCameraera)
        bot.y = pos.y - scl.y / 2;
        top.y = lft.y = rgt.y = pos.y + scl.y / 2;
        // set z comp (+ into MainCameraera)
        top.z = pos.z - scl.z / 2;
        bot.z = lft.z = rgt.z = pos.z + scl.z / 2;

        Vector2 temp_v = intersection2d(
            new Vector2(  top.y, top.z),
            new Vector2(0.2679f,  1.0f),
            new Vector2(  bot.y, bot.z),
            new Vector2(   1.0f,  1.0f));
        destination = new Vector3(top.x, temp_v.x, temp_v.y);
    }
}
