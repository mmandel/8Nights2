//
//  This represents a path the player must walk along to get from the shamash to a beacon
//  It can be saved/loaded from an file or populated interactively using the vive controller
//

using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Nights2Path : MonoBehaviour 
{
    [Tooltip("Which candle does this path lead to?")]
    public Nights2Beacon LeadsToBeacon = null;
    public Material PreviewLineMat;
    public PathData Path = new PathData();
    
    private float _exitPortalDist = 0.0f; //dist along path of exit portal
    private int _exitPortalSegment = 0;
    private float _entrancePortalDist = 0.0f;
    private int _entrancePortalSegment = 0;
    private float _entrancePortal2Dist = 0.0f;
    private int _entrancePortal2Segment = 0;

    public enum PortalType
    {
       EntrancePortal, //go to first world
       EntrancePortal2, //go to second world
       ExitPortal       //back to room world
    }

    [System.Serializable]
    public class PathData
    {
        public PathEntry[] Points = new PathEntry[0];
    };

    [System.Serializable]
    public class PathEntry
    {

        public Vector3 GetPos() { return Spot.transform.position; }

        public Nights2Spot Spot = null;
        public SpotAction Action = SpotAction.PortalToAltWorld1;
    }

    public enum SpotAction
    {
        PortalToAltWorld1,
        PortalToAltWorld2,
        PortalToRoom,
        Beacon,
        Shamash,
        Treasure
    }

    private LineRenderer _previewRenderer = null;
    private bool _isEditting = false;

    public bool IsEditting() { return _isEditting; }

    public Nights2Spot GetTreasureSpot()
    {
        //meh, should probably cache this instead of looking it up
        for (int i = 0; i < Path.Points.Length - 1; i++)
        {
            if (Path.Points[i].Action == SpotAction.Treasure)
                return Path.Points[i].Spot;
        }

        return null;
    }

    public Vector3 GetPortalDir(PortalType portal)
    {
        if (portal == PortalType.EntrancePortal)
            return GetSegmentDirection(_entrancePortalSegment-1);
        else if (portal == PortalType.EntrancePortal2)
            return GetSegmentDirection(_entrancePortal2Segment);
        else
            return GetSegmentDirection(_exitPortalSegment);
    }

    public Vector3 GetPortalPos(PortalType portal)
    {
        int portalIdx = -1;
        if (portal == PortalType.EntrancePortal)
            portalIdx = _entrancePortalSegment;
        else if (portal == PortalType.EntrancePortal2)
            portalIdx = _entrancePortal2Segment;
        else if (portal == PortalType.ExitPortal)
            portalIdx = _exitPortalSegment;

        if (portalIdx >= 0)
            return Path.Points[portalIdx].GetPos();
        else
            return Vector3.zero;
    }

    //straight up distance from given position to portal pos
    public float DistToPortal(PortalType portal, Vector3 ptToTest)
    {
        ptToTest.y = 0.0f;
        Vector3 portalPos = GetPortalPos(portal);
        portalPos.y = 0.0f;

        return (portalPos - ptToTest).magnitude;
    }

    //get position relative to the portal, both along the vector in the forward direction of the portal, and normal to its direction
    public void GetPortalRelativePos(PortalType portal, Vector3 ptToTest, out float forwardDist, out float sideDist)
    {
        ptToTest.y = 0.0f;

        Vector3 portalPos = GetPortalPos(portal);
        portalPos.y = 0.0f;

        Vector3 portalDir = GetPortalDir(portal);

        Vector3 fromPortal = (portalPos - ptToTest);
        Vector3 projectedPos = Vector3.Project(fromPortal, portalDir);        
        forwardDist = Mathf.Sign(Vector3.Dot(fromPortal, portalDir))*projectedPos.magnitude;
        //Debug.Log("forward projected: " + projectedPos);

        Vector3 sideDir = Vector3.Cross(portalDir, Vector3.up);
        projectedPos = Vector3.Project(fromPortal, sideDir);
        //Debug.Log("side projected: " + projectedPos);
        sideDist = Mathf.Sign(Vector3.Dot(fromPortal, sideDir)) * projectedPos.magnitude;
    }

    public float ComputeTotalPathDist()
    {
        if ((Path == null) || (Path.Points.Length <= 1))
          return 0.0f;
       float totalDist = 0.0f;
       for (int i = 0; i < Path.Points.Length - 1; i++)
       {
           Vector3 a = GetWorldPoint(i);
           Vector3 b = GetWorldPoint(i + 1);
          totalDist += (b - a).magnitude;
       }
       return totalDist;
    }

    //take a 0..1 percentage of path and convert it to a distance
    public float UToDist(float u, out int segmentIdx)
    {
       u = Mathf.Clamp01(u);

       float totalDist = ComputeTotalPathDist();

       float resultDist = 0.0f;
       float uAccum = 0.0f;
       segmentIdx = Path.Points.Length - 1;
       for (int i = 0; i < Path.Points.Length - 1; i++)
       {
           Vector3 a = GetWorldPoint(i);
           Vector3 b = GetWorldPoint(i + 1);

          float segmentDist = (a - b).magnitude;
          float prevUAccum = uAccum;
          uAccum +=  (segmentDist / totalDist);

          if (uAccum >= u) //ok, its on this segment
          {
             resultDist += ((u - prevUAccum) * totalDist);
             segmentIdx = i;
             break;
          }
          else
             resultDist += segmentDist;
       }

       return resultDist;
    }

    public Vector3 UToPos(float u, out int segmentIdx)
    {
       u = Mathf.Clamp01(u);

       float totalDist = ComputeTotalPathDist();
       float uAccum = 0.0f;
       segmentIdx = Path.Points.Length - 1;
       for (int i = 0; i < Path.Points.Length - 1; i++)
       {
           Vector3 a = GetWorldPoint(i);
           Vector3 b = GetWorldPoint(i + 1);

          float segmentDist = (a - b).magnitude;
          float prevUAccum = uAccum;
          uAccum += (segmentDist / totalDist);

          if (uAccum >= u) //ok, its on this segment
          {
             float distAlongSeg =  ((u - prevUAccum) * totalDist);
             Vector3 segDir = (b - a).normalized;
             segmentIdx = i;
             return a + distAlongSeg * segDir;
          }
       }

       return Vector3.zero;
    }

    //find the closest point on the path to the given point
    public Vector3 ClosestPointOnPath(Vector3 testPoint, out int outPathSegment)
    {
        outPathSegment = -1;

        Vector3 result =  Vector3.zero;
        if ((Path == null) || (Path.Points.Length <= 1))
        {            
            return result;
        }

        //meh just test all the line segments and return the result with smallest dist to the test point
        float closestDist = float.MaxValue;
        for (int i = 0; i < Path.Points.Length - 1; i++)
        {
            Vector3 a = GetWorldPoint(i);
            Vector3 b = GetWorldPoint(i + 1);

            Vector3 closestPoint = Nights2Utl.ClosestPointOnLine(a, b, testPoint);
            float curDist = (testPoint - closestPoint).sqrMagnitude;
            if (curDist < closestDist)
            {
                closestDist = curDist;
                result = closestPoint;
                outPathSegment = i;
            }
        }

        return result;
    }

    //distance from the given point to the path
    public float DistToPath(Vector3 point)
    {
        int ignore;
        return (point - ClosestPointOnPath(point, out ignore)).magnitude;
    }

    //gives distance that given point is along the given path segment
    public float GetDistanceAlongSegment(Vector3 pt, int pathSegment)
    {
        float result = float.MinValue;

        if ((Path == null) || (pathSegment >= Path.Points.Length - 1))
        {
            return result;
        }

        Vector3 segA = GetWorldPoint(pathSegment);
        Vector3 segB = GetWorldPoint(pathSegment+1);

        Vector3 segDir = (segB - segA).normalized;

        //project point onto segment
        //Vector3 projectedPt = Vector3.Project(pt, segDir);
        //meh, not working. just assume pt is on segment for now
        Vector3 projectedPt = pt;

        //dist from beginning of segment is our result
        result = (projectedPt - segA).magnitude;

        return result;
    }

    //get the position a given distance along the given segment
    public Vector3 GetPositionOnSegment(float distAlongSegment, int pathSegment)
    {
        if ((Path == null) || (pathSegment >= Path.Points.Length - 1))
        {
            return Vector3.zero;
        }

        Vector3 segA = GetWorldPoint(pathSegment);
        Vector3 segB = GetWorldPoint(pathSegment + 1);

        Vector3 segDir = (segB - segA).normalized;

        return segA + (distAlongSegment*segDir);
    }

    public Vector3 GetSegmentDirection(int pathSegment)
    {
        //if last point leads to beacon, we can form the segment by pointing at the beacon itself
        if ((pathSegment == Path.Points.Length - 1) && ((Path.Points[pathSegment].Action == SpotAction.Beacon) || (Path.Points[pathSegment].Action == SpotAction.PortalToRoom)))
        {
            Vector3 a = GetWorldPoint(pathSegment);
            Vector3 b = transform.TransformPoint(LeadsToBeacon.transform.position);
            b.y = a.y;

            return (b - a).normalized;
        }

        if ((Path == null) || (pathSegment >= Path.Points.Length - 1))
        {
            return Vector3.forward;
        }

        Vector3 segA = GetWorldPoint(pathSegment);
        Vector3 segB = GetWorldPoint(pathSegment + 1);

        return (segB - segA).normalized;
    }

    public void FindFirstSegmentWithAction(SpotAction action, out int outSegment, out float outDist)
    {
        outSegment = -1;
        outDist = 0.0f;
        if (Path.Points.Length == 0)
            return;
        Vector3 _prevPoint = Path.Points[0].GetPos();
        for (int i = 0; i < Path.Points.Length; i++)
        {
            Vector3 curPoint = Path.Points[i].GetPos();
            outDist += (curPoint - _prevPoint).magnitude;
            if (action == Path.Points[i].Action)
            {
                outSegment = i;
                return;
            }
            _prevPoint = curPoint;
        }
    }

    void Start() 
    {
        //Let nights 2 mgr know which beacon this path leads to
        Nights2Mgr.Instance.RegisterPath(this, LeadsToBeacon);

        //get the distance to each portal, along with the path segment they are on 
        FindFirstSegmentWithAction(SpotAction.PortalToAltWorld1, out _entrancePortalSegment, out _entrancePortalDist);
        FindFirstSegmentWithAction(SpotAction.PortalToAltWorld2, out _entrancePortal2Segment, out _entrancePortal2Dist);
        FindFirstSegmentWithAction(SpotAction.PortalToRoom, out _exitPortalSegment, out _exitPortalDist);
    }

    void Update() 
    {

	}



    public void ShowPreview(bool b)
    {
        if (b)
        {
            RefreshPreview();
            Debug.Assert(_previewRenderer);
            _previewRenderer.gameObject.SetActive(true);
        }
        else
        {
            if (_previewRenderer != null)
                _previewRenderer.gameObject.SetActive(false);
        }
    }

    //updates line renderer with latest data
    void RefreshPreview()
    {
        if (_previewRenderer == null)
        {
            GameObject previewObj = new GameObject();
            previewObj.name = "_pathPreview";
            //add line renderer
            _previewRenderer = previewObj.AddComponent<LineRenderer>();
            _previewRenderer.SetWidth(.1f, .1f);
            _previewRenderer.material =  (PreviewLineMat != null) ? PreviewLineMat : new Material(Shader.Find("Unlit/Color"));
            _previewRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _previewRenderer.useLightProbes = false;
            _previewRenderer.useWorldSpace = false;
            if(PreviewLineMat == null)
                _previewRenderer.material.color = Color.green;

            previewObj.transform.parent = this.transform;
            previewObj.transform.localPosition = Vector3.zero;
        }

        //populate from data...
        Debug.Assert(_previewRenderer);
        if ((Path != null) && (Path.Points.Length > 0))
        {
            int numPoints = Path.Points.Length;
            _previewRenderer.SetVertexCount(numPoints);
            for (int i = 0; i < numPoints; i++)
            {
                _previewRenderer.SetPosition(i, Path.Points[i].GetPos());
            }
        }
    }

    //draw debug line version of path when selected in the editor...
    void OnDrawGizmosSelected()
    {
        if ((Path == null) || (Path.Points.Length <= 1))
            return;

        const float kSphereRadius = .05f;
        Vector3 prevPt = Path.Points[0].GetPos();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(prevPt, kSphereRadius);
        for (int i = 1; i < Path.Points.Length; i++)
        {
            Vector3 curPt = Path.Points[i].GetPos();
            Gizmos.DrawSphere(curPt, kSphereRadius);
            Gizmos.DrawLine(prevPt, curPt);

            prevPt = curPt;
        }

        //draw entrance portal
        Vector3 entrancePos = GetPortalPos(PortalType.EntrancePortal);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(entrancePos, 2.0f * kSphereRadius);

        //draw exit portal
        Vector3 exitPos = GetPortalPos(PortalType.ExitPortal);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(exitPos, 2.0f * kSphereRadius);
    }
    
    Vector3 GetWorldPoint(int idx)
    {
        //return transform.TransformPoint(Path.Points[idx].GetPos());
        return Path.Points[idx].GetPos();
    }

    //add point at whatever location the torch is right now
    /*void TriggerAddPoint()
    {
        if (!_isEditting)
            return;

        Vector3 newPt = Nights2CamMgr.Instance.GetTorchParent().position;
        newPt.y = 0.05f; //snap to ground

        Debug.Log("Adding point at: " + newPt + " new path length is: " + _pathData.Points.Length);

        System.Array.Resize(ref _pathData.Points, _pathData.Points.Length + 1);
        _pathData.Points[_pathData.Points.Length - 1] = new PathEntry(newPt);

        RefreshPreview();
    }

    //remove the last point in the list
    void TriggerRemovePoint()
    {
        if (!_isEditting)
            return;

        if (_pathData.Points.Length > 0)
        {
            System.Array.Resize(ref _pathData.Points, _pathData.Points.Length - 1);

            Debug.Log("Removing point, new length of path is: " + _pathData.Points.Length);

            RefreshPreview();
        }
    }*/
}
