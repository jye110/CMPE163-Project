using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSquare : MonoBehaviour
{
    List<Vector3> topLeft = new List<Vector3>();
    List<Vector3> topRight = new List<Vector3>();
    List<Vector3> bottomLeft = new List<Vector3>();
    List<Vector3> bottomRight = new List<Vector3>();
    public float length = 2f;
    public float lambda = 0.1f;
    public float period = 1f;
    public int steps = 10;
    public Color color = Color.black;
    public Material material;

    void Start()
    {
        init();
        InvokeRepeating("drawRotatedSquare", 0, period);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init() {
        topLeft.Clear();
        topRight.Clear();
        bottomLeft.Clear();
        bottomRight.Clear();

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        Vector3 zAxis = transform.forward;
        Vector3 xAxis = transform.right;
        topLeft.Add(new Vector3(
            x + length / 2 * (-xAxis.x + zAxis.x),
            y + length / 2 * (-xAxis.y + zAxis.y),
            z + length / 2 * (-xAxis.z + zAxis.z)
        ));
        topRight.Add(new Vector3(
            x + length / 2 * (xAxis.x + zAxis.x),
            y + length / 2 * (xAxis.y + zAxis.y),
            z + length / 2 * (xAxis.z + zAxis.z)
        ));
        bottomLeft.Add(new Vector3(
            x + length / 2 * (-xAxis.x - zAxis.x),
            y + length / 2 * (-xAxis.y - zAxis.y),
            z + length / 2 * (-xAxis.z - zAxis.z)
        ));
        bottomRight.Add(new Vector3(
            x + length / 2 * (xAxis.x - zAxis.x),
            y + length / 2 * (xAxis.y - zAxis.y),
            z + length / 2 * (xAxis.z - zAxis.z)
        ));
    }

    void drawRotatedSquare() {    
        int pre = topLeft.Count - 1;
        topLeft.Add(new Vector3(
            topLeft[pre].x + lambda * (topRight[pre].x - topLeft[pre].x),
            topLeft[pre].y + lambda * (topRight[pre].y - topLeft[pre].y),
            topLeft[pre].z + lambda * (topRight[pre].z - topLeft[pre].z)
        ));
        topRight.Add(new Vector3(
            topRight[pre].x + lambda * (bottomRight[pre].x - topRight[pre].x),
            topRight[pre].y + lambda * (bottomRight[pre].y - topRight[pre].y),
            topRight[pre].z + lambda * (bottomRight[pre].z - topRight[pre].z)
        ));
        bottomRight.Add(new Vector3(
            bottomRight[pre].x + lambda * (bottomLeft[pre].x - bottomRight[pre].x),
            bottomRight[pre].y + lambda * (bottomLeft[pre].y - bottomRight[pre].y),
            bottomRight[pre].z + lambda * (bottomLeft[pre].z - bottomRight[pre].z)
        ));
        bottomLeft.Add(new Vector3(
            bottomLeft[pre].x + lambda * (topLeft[pre].x - bottomLeft[pre].x),
            bottomLeft[pre].y + lambda * (topLeft[pre].y - bottomLeft[pre].y),
            bottomLeft[pre].z + lambda * (topLeft[pre].z - bottomLeft[pre].z)
        ));  
        for (int i = 0; i < topLeft.Count; i++) {
            DrawLine(topLeft[i], topRight[i]);
            DrawLine(topRight[i], bottomRight[i]);
            DrawLine(bottomRight[i], bottomLeft[i]);
            DrawLine(bottomLeft[i], topLeft[i]);
        }
        if (topLeft.Count == steps) {
            init();
        }
    }

    void DrawLine(Vector3 start, Vector3 end) {
        GameObject myLine = new GameObject();
        myLine.name = "RotateSquareLine";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, period);
    }
}
