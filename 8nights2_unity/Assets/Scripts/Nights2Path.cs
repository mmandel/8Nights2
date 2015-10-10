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

    [ScriptButton("Begin Editting!", "OnStartEditPressed")]
    public bool BeginEditDummy = false;
    [ScriptButton("Save!", "OnSavePressed")]
    public bool SaveDummy = false;

    public class PathData
    {
        [XmlArrayAttribute("Points")]
        public PathEntry[] Points = new PathEntry[0];
    };

    public class PathEntry
    {
        //NOTE: making this an attribute makes the xml more compact, but it won't deserialize unless I make my own vector class (oh well)
        //[XmlAttribute("Point")]
        public Vector3 Point;
    }

    private PathData _pathData = null; //our loaded path data

    private LineRenderer _previewRenderer = null;

	void Start () 
    {
        LoadFromXML();

        //temp: generate fake data to test serialization
        /*_pathData = new PathData();
        _pathData.Points = new PathEntry[5];
        for (int i = 0; i < 5; i++)
        {
            _pathData.Points[i] = new PathEntry();
            _pathData.Points[i].Point = new Vector3(Random.Range(0.0f, 5.0f), Random.Range(0.0f, 5.0f), 0.0f);
        }*/
	}

    void Update() 
    {
	
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
        //TODO: confirm play mode, turn on preview mesh, clear existing data, etc...
    }

    void ShowPreview(bool b)
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
            //add line renderer
            _previewRenderer = previewObj.AddComponent<LineRenderer>();
            _previewRenderer.SetWidth(.1f, .1f);
            _previewRenderer.material =  new Material(Shader.Find("Diffuse"));
            _previewRenderer.material.color = Color.green;

            previewObj.transform.parent = this.transform;
            previewObj.transform.localPosition = Vector3.zero;
        }

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
        //TODO
        //Gizmos.DrawLine(
    }
}
