//
// Utility funcs
//

using UnityEngine;

public class Nights2Utl
{
    //compute closest point on a line segment from the given line
    public static Vector3 ClosestPointOnLine(Vector3 lineA, Vector3 lineB, Vector3 pointToTest)
    {
        var vVector1 = pointToTest - lineA;
        var vVector2 = (lineB - lineA).normalized;

        var d = Vector3.Distance(lineA, lineB);
        var t = Vector3.Dot(vVector2, vVector1);
 
        if (t <= 0)
            return lineA;
 
        if (t >= d)
            return lineB;
 
        var vVector3 = vVector2 * t;

        var vClosestPoint = lineA + vVector3;
 
        return vClosestPoint;
    }

}
