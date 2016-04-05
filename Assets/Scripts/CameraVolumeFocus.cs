/**
 * Attach to player character.
 * Set Main Camera to your Main Camera
 * Create room volumes, remove the renderer, set colliders to trigger, and tag them as 'Volume'
 * There should be a 'RoomVolumes' prefab as an example.
 * Ensure a Respawn object exists that is tagged 'Respawn'.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraVolumeFocus : MonoBehaviour {
    public float speed = 10.0f;
    public GameObject MainCamera;

    private GameObject Spawn;
    private Camera cam;
    private Vector3 destination;
    private List<GameObject> currentVolumes;

    private bool refPointsNull = true;
    private Vector3 top, bot, lft, rgt;

    private float v_fov;
    private float angle_from_hor;
    private float top_angle;
    private float bot_angle;

    void Start() {
        cam = MainCamera.GetComponent<Camera>();
        currentVolumes = new List<GameObject>();
        Spawn = GameObject.FindWithTag("Respawn");

        v_fov = cam.fieldOfView;
        angle_from_hor = MainCamera.transform.rotation.eulerAngles.x;
        top_angle = angle_from_hor - v_fov / 2;
        bot_angle = angle_from_hor + v_fov / 2;
    }
    void Update() {
        MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, destination, speed);
    }
    float horizontalFieldOfView() {
        return Mathf.Atan(cam.aspect * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
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
    void BakeRefPoints(GameObject vol) {
        Vector3 t_top, t_bot, t_lft, t_rgt;
        Vector3 pos = vol.transform.position;
        Vector3 scl = vol.transform.lossyScale;
        // set x comp (+ left MainCamera)
        t_top.x = t_bot.x = pos.x;
        t_lft.x = pos.x + scl.x / 2;
        t_rgt.x = pos.x - scl.x / 2;
        // set y comp (+ up MainCamera)
        t_bot.y = pos.y - scl.y / 2;
        t_top.y = t_lft.y = t_rgt.y = pos.y + scl.y / 2;
        // set z comp (+ into MainCamera)
        t_top.z = pos.z - scl.z / 2;
        t_bot.z = t_lft.z = t_rgt.z = pos.z + scl.z / 2;

        if (refPointsNull) {
            top = t_top;
            bot = t_bot;
            lft = t_lft;
            rgt = t_rgt;
            refPointsNull = false;
            return;
        }
        if (t_top.z < top.z) top = t_top;
        if (bot.z < t_bot.z) bot = t_bot;
        if (lft.x < t_lft.x) lft = t_lft;
        if (t_rgt.x < rgt.x) rgt = t_rgt;
    }
    void BakeCameraDestination() {
        //Vector3 camera_pos_vec = new Vector3(0.0f, Mathf.Tan(angle_from_hor * Mathf.Deg2Rad), 1.0f);
        Vector2 temp_v = intersection2d(
            new Vector2(top.y, top.z), new Vector2(Mathf.Tan(top_angle * Mathf.Deg2Rad), 1.0f),
            new Vector2(bot.y, bot.z), new Vector2(Mathf.Tan(bot_angle * Mathf.Deg2Rad), 1.0f));
        destination = new Vector3(top.x, temp_v.x, temp_v.y);
        Spawn.transform.position = transform.position;
    }
    void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("Volume")) return;
        //error here about the light not being accessible
        //col.gameObject.GetComponent<Light>().enabled = true;
        currentVolumes.Add(col.gameObject);
        BakeRefPoints(col.gameObject);
        BakeCameraDestination();
    }
    void OnTriggerExit(Collider col) {
        if (!col.CompareTag("Volume")) return;
        //col.gameObject.GetComponent<Light>().enabled = false;
        currentVolumes.Remove(col.gameObject);
        refPointsNull = true;
        foreach (GameObject vol in currentVolumes) BakeRefPoints(vol);
        BakeCameraDestination();
    }
}
