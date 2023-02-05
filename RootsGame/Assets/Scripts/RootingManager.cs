using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootingManager : MonoBehaviour
{
    [SerializeField] LayerMask mouseHitboxLayer = default;
    [SerializeField] RootEffect rootEffectPrefab = default;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos, mouseHitboxLayer);

            if(hitCollider)
            {

                Rootable rootable = hitCollider.GetComponentInParent<Rootable>();

                if(rootable != null && !rootable.IsRooted())
                {
                    RootEffects(rootable);
                    rootable.RootInPlace();
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos, mouseHitboxLayer);

            if(hitCollider)
            {
                Rootable rootable = hitCollider.GetComponentInParent<Rootable>();

                if(rootable != null && rootable.IsRooted())
                {
                    rootable.Unroot();
                }
            }
        }
    }

    void RootEffects(Rootable rootable)
    {
        RootEffect root = Instantiate(rootEffectPrefab, transform.position, Quaternion.identity);

        root.target = rootable.GetCollider2D();
        rootable.rootEffect = root;
    }
}
