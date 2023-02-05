using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rootable : MonoBehaviour
{
    public RootEffect rootEffect;

    public abstract void RootInPlace();
    public abstract Collider2D GetCollider2D();
    public abstract bool IsRooted();
    public abstract void Unroot();
}
