using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretAI : MonoBehaviour {
    public enum AiStates { NEAREST, FURTHEST};

    public AiStates aiState = AiStates.NEAREST;

    TrackingSystem _tracker;
    ShootingSystem _shooter;
    RangeChecker _range;

    float dist;

    // Use this for initialization
    void Start()
    {
        _tracker = GetComponent<TrackingSystem>();
        _shooter = GetComponent<ShootingSystem>();
        _range = GetComponent<RangeChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tracker || !_shooter || !_range)
            return;

        switch (aiState)
        {
            case AiStates.NEAREST:
                TargetNearest();
                break;
            case AiStates.FURTHEST:
                TargetFurthest();
                break;
        }
    }

    void TargetNearest()
    {
        List<GameObject> validTargets = _range.GetValidTargets();

        GameObject curTarget = null;
        float closestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            if (validTargets[i] == null)
                continue;
            if (validTargets[i].activeSelf == false)
                continue;

            dist = Vector3.Distance(transform.position, validTargets[i].transform.position);
            if (!curTarget || dist < closestDist)
            {
                curTarget = validTargets[i];
                closestDist = dist;
            }
        }

        _tracker.SetTarget(curTarget);
        _shooter.SetTarget(curTarget);
    }

    void TargetFurthest()
    {
        List<GameObject> validTargets = _range.GetValidTargets();

        GameObject curTarget = null;
        float furthestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist > furthestDist)
            {
                curTarget = validTargets[i];
                furthestDist = dist;
            }
        }

        _tracker.SetTarget(curTarget);
        _shooter.SetTarget(curTarget);
    }
}
