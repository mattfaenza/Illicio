using UnityEngine;
using System.Collections;
using System;

public class NormalProjectile : BaseProjectile {

    Vector3 _direction;
    bool _fired;
    GameObject _launcher;
    GameObject _target;
    int _damage;
	
	// Update is called once per frame
	void Update () {
        if(_fired)
        {
            transform.position += _direction * (speed * Time.deltaTime);
        }
	}

    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float attackSpeed)
    {
        if (launcher && target)
        {
            _direction = (target.transform.position - launcher.transform.position).normalized;
            _fired = true;
            _launcher = launcher;
            _target = target;
            _damage = damage;

            Destroy(gameObject, 5.0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            //Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.gameObject.tag == "Hologram")
        {
            //Will destroy hologram
            gameObject.SetActive(false);
            Destroy(gameObject);
            //Destroy(collision.collider.gameObject);
        }
    }
}
