namespace RPG.Control
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), .25f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).transform.position;
        }
        public int GetNextIndex(int i)
        {
            if (i < transform.childCount - 1)
            {
                return i + 1;
            }
            return 0;
        }
    }
}
