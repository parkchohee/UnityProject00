using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private RaycastHit rayHit;  
    private Ray ray;
    private float distance = 30.0f;

    public Transform target = null;

    private Transform tr;
    private float dist = 0;
    private float maxDist = 5;
    private float height = 5;

    public void init ()
    {
        ray = new Ray();
        tr = this.gameObject.transform;
        target = GameObject.FindWithTag("Player").transform;
        ray.origin = this.transform.position;

        GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().SetTarget(target);
    }
	
	void Update ()
    {
        if (target == null)
            return;

        ray.origin = this.transform.position;
        ray.direction = (target.position + new Vector3(0, 0.5f, 0)) - this.transform.position;

        //if (isPlayerRayCast())
        //{
        //    if (dist > 0)
        //        dist -= 0.05f;
        //    else
        //        dist = 0f;
        //}
        //else
        //{
        //    dist += 0.05f;
        //}



        //if (Physics.Raycast(ray.origin, ray.direction, out rayHit, distance))
        //{
        //    if (rayHit.collider.gameObject.tag != "Player")
        //    {

        //    }
        //    else
        //    {

        //    }
        //}


        tr.position = target.position - (new Vector3(0, 0, 1) * (maxDist - dist)) + (new Vector3(0, 1, 0) * (height - dist));
        tr.LookAt(target);

    }

    bool isPlayerRayCast()
    {
        if (Physics.Raycast(ray.origin, ray.direction, out rayHit, distance))
        {
            if (rayHit.collider.gameObject.tag == "Player")
                return true;                
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        // Ray를 그려줌..Scene에서만 확인 가능.
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

    }
}
