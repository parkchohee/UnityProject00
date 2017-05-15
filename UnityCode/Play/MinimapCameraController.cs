using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    Transform target;
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
            return;

        gameObject.transform.position = target.transform.position + new Vector3(0, 10, 0);

    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
