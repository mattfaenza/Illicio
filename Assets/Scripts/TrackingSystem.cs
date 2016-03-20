using UnityEngine;
using System.Collections;

public class TrackingSystem : MonoBehaviour {
    public float speed = 3.0f;

    GameObject _target = null;
    Vector3 _lastKnownPosition = Vector3.zero;
    Quaternion _lookAtRotation;

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            if (_lastKnownPosition != _target.transform.position)
            {
                _lastKnownPosition = _target.transform.position;
                _lookAtRotation = Quaternion.LookRotation(_lastKnownPosition - transform.position);
                _lookAtRotation.x = 0.0f;
                _lookAtRotation.z = 0.0f;
            }

            if (transform.rotation != _lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookAtRotation, speed * Time.deltaTime);
            }
        } else
        {
            //transform.Rotate(0, 1.5f, 0, Space.World);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
