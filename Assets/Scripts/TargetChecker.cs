using System;
using UnityEngine;

public class TargetChecker
{
    public bool CheckAvailableTarget(Transform from, Transform target, float angle, float distance)
    {
        bool result = false;
        if (from != null && Vector3.Distance(from.position, target.position) <= distance)
        {
            Vector3 direction = (target.position - from.position);
            float dot = Vector3.Dot(from.forward, direction.normalized);

            if (dot < 1)
            {
                float angleRadians = Mathf.Acos(dot);
                float angleDeg = angleRadians * Mathf.Rad2Deg;

                if (angleDeg <= angle)
                    from.LookAt(target.position);
                
                result = (angleDeg <= angle);
            }
            else
                result = true;
        }
        return result;
    }
}

