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
    [Tooltip("Name of file to save/load our path from.  This will be in the Resources/paths/ directory")]
    public string SaveName = "test_path";
    [Tooltip("Which shamash does this path lead to?")]
    public Nights2Beacon LeadsToBeacon = null;

    [ScriptButton("Begin Editting!", "OnStartEditPressed")]
    public bool BeginEditDummy = false;
    [ScriptButton("End Editting!", "OnEndEditPressed")]
    public bool EndEditDummy = false;
    [ScriptButton("Save!", "OnSavePressed")]
    public bool SaveDummy = false;

    public class PathData
    {
        [XmlArrayAttribute("Points")]
        public PathEntry[] Points = new PathEntry[0];
    };

    public class PathEntry
    {
        public PathEntry() { Point = Vector3.zero; }
        public PathEntry(Vector3 pt) { Point = pt; }

        //NOTE: making this an attribute makes the xml more compact, but it won't deserialize unless I make my own vector class (oh well)
        //[XmlAttribute("Point")]
        public Vector3 Point;
    }

    private PathData _pathData = null; //our loaded path data

    private LineRenderer _previewRenderer = null;
    private bool _isEditting = false;

    public bool IsEditting() { return _isEditting; }

    //find the closest point on the path to the given point
    public Vector3 ClosestPointOnPath(Vector3 testPoint, out int outPathSegment)
    {
        outPathSegment = -1;

        Vector3 result =  Vector3.zero;
        if ((_pathData == null) || (_pathData.Points.Length <= 1))
        {            
            return result;
        }

        //meh just test all the line segments and return the result with smallest dist to the test point
        float closestDist = float.MaxValue;
        for (int i = 0; i < _pathData.Points.Length - 1; i++)
        {
            Vector3 a = _pathData.Points[i].Point;
            Vector3 b = _pathData.Points[i + 1].Point;

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

        if ((_pathData == null) || (pathSegment >= _pathData.Points.Length - 1))
        {
            return result;
        }

        Vector3 segA = _pathData.Points[pathSegment].Point;
        Vector3 segB = _pathData.Points[pathSegment+1].Point;

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
        if ((_pathData == null) || (pathSegment >= _pathData.Points.Length - 1))
        {
            return Vector3.zero;
        }

        Vector3 segA = _pathData.Points[pathSegment].Point;
        Vector3 segB = _pathData.Points[pathSegment + 1].Point;

        Vector3 segDir = (segB - segA).normalized;

        return segA + (distAlongSegment*segDir);
    }

    public Vector3 GetSegmentDirection(int pathSegment)
    {
        if ((_pathData == null) || (pathSegment >= _pathData.Points.Length - 1))
        {
            return Vector3.forward;
        }

        Vector3 segA = _pathData.Points[pathSegment].Point;
        Vector3 segB = _pathData.Points[pathSegment + 1].Point;

        return (segB - segA).normalized;
    }

	void Start () 
    {
        LoadFromXML();

        //Let nights 2 mgr know which beacon this path leads to
        Nights2Mgr.Instance.RegisterPath(this, LeadsToBeacon);

        //temp: generate fake data to test serialization
        /*_pathData = new PathData();
        _pathData.Points = new PathEntry[5];
        for (int i = 0; i < 5; i++)
        {
            _pathData.Points[i] = new PathEntry();
            _pathData.Points[i].Point = new Vector3(Random.Range(0.0f, 5.0f), 0.0f, Random.Range(0.0f, 5.0f));
        }*/

        //ShowPreview(true);
    }

    void Update() 
    {
        if (_isEditting)
        {
            //add a point if you pull the trigger
            if (Nights2InputMgr.Instance.TorchInfo().GetTriggerDown())
                TriggerAddPoint();
            //remove last point
            else if (Nights2InputMgr.Instance.TorchInfo().GetTouchpadDown())
                TriggerRemovePoint();
            //save if you hit the red button
            else if (Nights2InputMgr.Instance.TorchInfo().GetRedButtonDown())
                SaveToXML();
        }
	}

    void SaveToXML()
    {
        if (_pathData == null)
        {
            Debug.LogError("Cannot save path because there is no data!");
            return;
        }

        XmlSerializer deserializer = new XmlSerializer(typeof(PathData));
        string xmlPath = Application.dataPath + "/Resources/paths/" + SaveName + ".xml";
        Debug.Log("Saving path to: " + xmlPath);

        XmlSerializer serializer = new XmlSerializer(typeof(PathData));
        using ( TextWriter writer = new StreamWriter(xmlPath))
        {
            serializer.Serialize(writer, _pathData);
        }  
    }

    void LoadFromXML()
    {
        string xmlPath = "paths/" + SaveName;
        TextAsset xmlData = new TextAsset();
        xmlData = (TextAsset)Resources.Load(xmlPath, typeof(TextAsset));

        if (xmlData == null)
        {
            Debug.LogError("Couldn't read path from resource: " + xmlPath);
            return;
        }

        XmlSerializer deserializer = new XmlSerializer(typeof(PathData));
        TextReader reader = new StringReader(xmlData.text);
        object obj = deserializer.Deserialize(reader);
        _pathData = (PathData)obj;
        reader.Close();
    }

    public void OnSavePressed(string propPath)
    {
        SaveToXML();
    }

    public void OnStartEditPressed(string propPath)
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can't end path unless you are in play mode!");
            return;
        }

        _isEditting = true;
        Nights2Mgr.Instance.SetIsPathEditting(true);

        //start with fresh data
        _pathData = new PathData();
        RefreshPreview();

        ShowPreview(true);

        Debug.Log("Editting path began!  Hit the save button when you are done...");
    }

    public void OnEndEditPressed(string propPath)
    {
        Nights2Mgr.Instance.SetIsPathEditting(false);
        _isEditting = false;
        ShowPreview(false);
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
            _previewRenderer.material =  new Material(Shader.Find("Unlit/Color"));
            _previewRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _previewRenderer.useLightProbes = false;
            _previewRenderer.useWorldSpace = true;
            _previewRenderer.material.color = Color.green;

            previewObj.transform.parent = this.transform;
            previewObj.transform.localPosition = Vector3.zero;
        }

        if (_pathData == null)
            LoadFromXML();

        //populate from data...
        Debug.Assert(_previewRenderer);
        if ((_pathData != null) && (_pathData.Points.Length > 0))
        {
            int numPoints = _pathData.Points.Length;
            _previewRenderer.SetVertexCount(numPoints);
            for (int i = 0; i < numPoints; i++)
            {
                _previewRenderer.SetPosition(i, _pathData.Points[i].Point);
            }
        }
    }

    //draw debug line version of path when selected in the editor...
    void OnDrawGizmosSelected()
    {
        if (_pathData == null)
            LoadFromXML();

        if ((_pathData == null) || (_pathData.Points.Length <= 1))
            return;

        const float kSphereRadius = .05f;
        Vector3 prevPt = _pathData.Points[0].Point;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(prevPt, kSphereRadius);
        for (int i = 1; i < _pathData.Points.Length; i++)
        {
            Vector3 curPt = _pathData.Points[i].Point;
            Gizmos.DrawSphere(curPt, kSphereRadius);
            Gizmos.DrawLine(prevPt, curPt);

            prevPt = curPt;
        }
    }

    //add point at whatever location the torch is right now
    void TriggerAddPoint()
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
    }
}
