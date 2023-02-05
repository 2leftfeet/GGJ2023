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

    public int rootCount;

    public float rootSpeed;
    private float rootSpreadAmount = 0f;
    
    public float targetOvershotAmount = 0.5f;

    List<LineRenderer> lineRenderers;

    public bool unrooting = false;

    void Start()
    {
        //Figure out the closest collider point to the target
        Vector3 closestPoint = FindClosestColliderPoint();

        //Retract root spawn point into geometry
        closestPoint += (closestPoint - target.transform.position).normalized * 1f;
        
        lineRenderers = new List<LineRenderer>();

        for(int i = 0; i < rootCount; i ++)
        {
            List<Vector3> rootPositions = new List<Vector3>();
            GenerateRootPositions(ref rootPositions, closestPoint);

            LineRenderer lr = CreateRoot(in rootPositions);
            lineRenderers.Add(lr);
        } 
    }

    void Update()
    {
        if(rootSpreadAmount < 1f)
        {
            foreach(LineRenderer line in lineRenderers)
            {
                AnimationCurve curve = line.widthCurve;
                Keyframe[] keys = curve.keys;

                keys[1].time = unrooting ? 1-rootSpreadAmount : rootSpreadAmount;
                curve.keys = keys;

                line.widthCurve = curve;
            }
        }
        
        rootSpreadAmount += Time.deltaTime * rootSpeed;
    }

    Vector3 FindClosestColliderPoint()
    {
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

        return closestPoint;
    }
    
    void GenerateRootPositions(ref List<Vector3> rootPositions, Vector3 closestPoint)
    {
        //target point is anywhere on  the rootable collider
        Vector3 targetPoint = new Vector3(
            Random.Range(target.bounds.min.x, target.bounds.max.x),
            Random.Range(target.bounds.min.y, target.bounds.max.y),
            Random.Range(target.bounds.min.z, target.bounds.max.z)
        );

        targetPoint += (targetPoint - closestPoint).normalized * targetOvershotAmount;

        //randomize position of the root tip
        Vector3 rootTip = closestPoint + (Vector3)Random.insideUnitCircle * 0.5f;
        rootPositions.Add(rootTip);

        //endless loop protections
        int iterations = 0;
        int maxIterations = 50;

        while(Vector3.Distance(rootTip, targetPoint) > 0.3f)
        {
            if(iterations++ > maxIterations)
             break;

            float rootSegmentLength = averageRootSegmentLength + Random.Range(-rootSegmentLengthVariance, rootSegmentLengthVariance);
            Vector3 dirToTarget = (targetPoint - rootTip).normalized;

            dirToTarget = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * dirToTarget;

            rootTip += rootSegmentLength * dirToTarget;

            rootPositions.Add(rootTip);
        }
    }

    LineRenderer CreateRoot(in List<Vector3> rootPositions)
    {
        GameObject root = new GameObject("Root to " + target.gameObject.name);
        LineRenderer lr = root.AddComponent<LineRenderer>();

        root.transform.parent = transform;

        //set line renderer position
        Vector3[] lrPositions = rootPositions.ToArray();
        Debug.Log(rootPositions.Count);

        lr.positionCount = lrPositions.Length;
        lr.SetPositions(lrPositions);


        AnimationCurve curve = widthCurve;
        Keyframe[] keys = curve.keys;

        keys[1].time = 0f;
        curve.keys = keys;

        lr.widthCurve = widthCurve;

        //set lr properties
        lr.startWidth = 1f;
        lr.widthMultiplier = 0.1f;

        lr.startColor = lr.endColor = colorGradient.Evaluate(Random.Range(0f, 1f));

        lr.sortingOrder = -10;
        lr.material = mat;

        return lr;
    }

    public void ResetEffect()
    {
        unrooting = true;
        rootSpreadAmount = 0f;

        Destroy(this.gameObject, 1f / rootSpeed);
    }
}
