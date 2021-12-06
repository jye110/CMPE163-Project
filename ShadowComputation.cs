using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowComputation : MonoBehaviour
{
    public GameObject plane;
    public GameObject Light;
    public Material material;
    Mesh m;
    void Start()
    {     
        GameObject polygon = new GameObject();
        polygon.name = "Shadow Polygon";
        polygon.AddComponent<MeshFilter>();
        polygon.AddComponent<MeshRenderer>();
        MeshFilter mf = polygon.GetComponent<MeshFilter>();
        MeshRenderer mr = polygon.GetComponent<MeshRenderer>();
        m = new Mesh();
        m.name = "Shadow Polygon";
        mr.material = material;
        // mr.material = new Material(Shader.Find("Sprites/Default"));
        mf.mesh = m;
        Update();
        InvokeRepeating("rotateThis", 0, 1);
    }

    void rotateThis() {
        transform.Rotate(Vector3.up * 3);
    }

    void Update() {
        if (!transform.hasChanged) return;

        transform.hasChanged = false;
        List<Vector3> vertices = new List<Vector3>();
        Vector3 planeNormal = plane.transform.up;
        Vector3 planeOrigin = plane.transform.position;
        Vector3 lightOrigin = Light.transform.position;
        foreach(Vector3 vertice in GetComponent<MeshFilter>().mesh.vertices) {
            Vector3 point = transform.position + transform.rotation * Vector3.Scale(vertice, transform.localScale);
            float lambda = Vector3.Dot(planeNormal, (planeOrigin - point)) / Vector3.Dot(planeNormal, (point - lightOrigin));
            vertices.Add(point + (point - lightOrigin) * lambda + planeNormal * 0.01f);
        }

        // for (int i = 0; i < points.Count; i++) {
        //     for (int j = i + 1; j < points.Count; j++) {
        //         DrawLine(points[i], points[j]);
        //     }
        // }
        m.vertices = vertices.ToArray();
        m.triangles = GetComponent<MeshFilter>().mesh.triangles;
        m.RecalculateNormals();
        m.RecalculateBounds();  

    }

    void DrawLine(Vector3 start, Vector3 end) {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        lr.startColor = Color.black;
        lr.endColor = Color.black;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, 1f);
    }
}
