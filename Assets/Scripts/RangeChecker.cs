using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeChecker : MonoBehaviour {
    public List<string> tags;

    List<GameObject> _targets = new List<GameObject>();

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
        if (gameObject.GetComponent<EnemyAIMovement>().followPlayer == false)
        {
            gameObject.GetComponent<EnemyAIMovement>().followPlayer = true;
            //this.transform.parent.gameObject.GetComponent<EnemyAIMovement>().moveInPath = false;


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
