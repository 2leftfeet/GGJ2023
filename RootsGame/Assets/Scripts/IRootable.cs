using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootable
{
   void RootInPlace();
   Collider2D GetCollider2D();
   bool IsRooted();
}
