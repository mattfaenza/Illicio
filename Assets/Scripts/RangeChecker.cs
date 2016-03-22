using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeChecker : MonoBehaviour {
    public List<string> tags;

    List<GameObject> _targets = new List<GameObject>();
    public GameObject exclamation;  // When the enemy spots the player, this will show up
    LizardAIMovement _AI;
    void Start()
    {
        exclamation.SetActive(false);
        _AI = GetComponent<LizardAIMovement>();
    }
    //void Update()
    //{
    //    Debug.Log(_targets.Count);
    //    for (int i = 0; i < _targets.Count; i++)
    //    {
    //        if (_targets[i] == null)
    //            _targets.RemoveAt(i);
    //        if (_targets[i].activeSelf == false)
    //            _targets.RemoveAt(i);
    //    }
    //}

    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            exclamation.SetActive(true);
        }
        else if (target.gameObject.tag == "Untagged")
        {
            _AI.SetDestination(_AI.getWaypoints()[0].position);
        }
        bool invalid = true;

        for (int i = 0; i < tags.Count; i++)
        {
            if (target.CompareTag(tags[i]))
            {
                invalid = false;
                break;
            }
        }

        if (invalid)
            return;

        _targets.Add(target.gameObject);
    }

    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            exclamation.SetActive(false);
        }
        for (int i = 0; i < _targets.Count; i++)
        {
            if (target.gameObject == _targets[i])
            {
                _targets.Remove(target.gameObject);
                return;
            }
        }
    }

    public List<GameObject> GetValidTargets()
    {
        return _targets;
    }

    public bool InRange(GameObject other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other == _targets[i])
                return true;
        }

        return false;
    }
}
