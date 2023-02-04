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

                IRootable rootable = hitCollider.GetComponentInParent<IRootable>();

                if(rootable != null && !rootable.IsRooted())
                {
                    RootEffects(rootable);
                    rootable.RootInPlace();
                    //Do effects
                }
            }
        }
    }

    void RootEffects(IRootable rootable)
    {
        RootEffect root = Instantiate(rootEffectPrefab, transform.position, Quaternion.identity);

        root.target = rootable.GetCollider2D();
    }
}
