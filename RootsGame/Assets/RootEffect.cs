using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffect : MonoBehaviour
{
    //TODO: Change to IRootable
    public Collider2D target;
    public LayerMask groundMask;

    public AnimationCurve widthCurve;

    public Gradient colorGradient;
    public Material mat;

    public float averageRootSegmentLength;
    public float rootSegmentLengthVariance;

    void Start()
    {
        //Figure out the closest collider point to the target
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.transform.position, 50f, groundMask);

        Vector3 closestPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        for(int i = 0; i < colliders.Length; i++)
        {
            Vector3 point = colliders[i].ClosestPoint(target.transform.position);

            float distance = Vector2.Distance(point, target.transform.position);
            if(distance < closestDistance)
            {
                closestPoint = point;
                closestDistance = distance;
            }
        }

        closestPoint += (closestPoint - target.transform.position).normalized * 1f;
        
        for(int i = 0; i < 5; i ++)
        {
             //Create Gameobject and linerenderer
            GameObject root = new GameObject("Root to " + target.gameObject.name);
            LineRenderer lr = root.AddComponent<LineRenderer>();

            root.transform.parent = transform;

            Vector3 targetPoint = new Vector3(
                Random.Range(target.bounds.min.x, target.bounds.max.x),
                Random.Range(target.bounds.min.y, target.bounds.max.y),
                Random.Range(target.bounds.min.z, target.bounds.max.z)
            );

            targetPoint += (targetPoint - closestPoint).normalized * 0.5f;


            List<Vector3> rootPositions = new List<Vector3>();

            Vector3 rootTip = closestPoint + (Vector3)Random.insideUnitCircle * 0.5f;
            rootPositions.Add(rootTip);

            while(Vector3.Distance(rootTip, targetPoint) > 0.3f)
            {

                float rootSegmentLength = averageRootSegmentLength + Random.Range(-rootSegmentLengthVariance, rootSegmentLengthVariance);
                Vector3 dirToTarget = (targetPoint - rootTip).normalized;

                dirToTarget = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * dirToTarget;

                rootTip += rootSegmentLength * dirToTarget;

                rootPositions.Add(rootTip);
            }

            //set line renderer position
            Vector3[] lrPositions = rootPositions.ToArray();
            Debug.Log(rootPositions.Count);

            lr.positionCount = lrPositions.Length;
            lr.SetPositions(lrPositions);

            lr.widthCurve = widthCurve;

            //set lr properties
            lr.startWidth = 1f;
            lr.widthMultiplier = 0.1f;

            lr.startColor = lr.endColor = colorGradient.Evaluate(Random.Range(0f, 1f));

            lr.sortingOrder = 10;
            lr.material = mat;
        }
       
        
    }
}
