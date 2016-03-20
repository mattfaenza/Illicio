using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingSystem : MonoBehaviour {
    public float fireRate;
    public int damage;
    public float fieldOfView;
    public GameObject projectile;
    public GameObject _target;
    public List<GameObject> projectileSpawns;

    List<GameObject> _lastProjectiles = new List<GameObject>();
    float _fireTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (!_target)
        {
            return;
        }
        else {
            _fireTimer += Time.deltaTime;

            if (_fireTimer >= fireRate)
            {
                float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_target.transform.position - transform.position));

                if (angle < fieldOfView)
                {
                    SpawnProjectiles();

                    _fireTimer = 0.0f;
                }
            }
        }
    }

    void SpawnProjectiles()
    {
        if (!projectile)
        {
            return;
        }
        _lastProjectiles.Clear();

        for (int i = 0; i < projectileSpawns.Count; i++)
        {
            if (projectileSpawns[i])
            {
                GameObject proj = Instantiate(projectile, projectileSpawns[i].transform.position, Quaternion.Euler(projectileSpawns[i].transform.forward)) as GameObject;
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], _target, damage, fireRate);

                _lastProjectiles.Add(proj);
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    void RemoveLastProjectiles()
    {
        while (_lastProjectiles.Count > 0)
        {
            Destroy(_lastProjectiles[0]);
            _lastProjectiles.RemoveAt(0);
        }
    }
}
