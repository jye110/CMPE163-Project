using UnityEngine;
using System.Collections;

public class tree : MonoBehaviour {

    // Material used for the connecting lines
    public Material lineMat;
    public float radius = .05f;
    public Mesh cylinderMesh;
    public float size = 1.0f;
    public float branch_ratio = .25f;
    public float leaf_length_ratio= .9f;
    public float side_angle = 35f;
    public int layer = 3;

    private const float PI = 3.14159265f;


    GameObject[] ringGameObjects;


    //for drawing line
    void DrawLine(Vector3 start_pt, Vector3 end_pt){

            // We make a offset gameobject to counteract the default cylindermesh pivot/origin being in the middle
            GameObject CylinderMeshObject = new GameObject();
            CylinderMeshObject.transform.parent = this.gameObject.transform;

            float dist = Vector3.Distance(start_pt, end_pt)*0.5f;
            float offsetX = (end_pt.x - start_pt.x)*0.5f;
            float offsetY = (end_pt.y - start_pt.y)*0.5f;
            float offsetZ = (end_pt.z - start_pt.z)*0.5f;
            Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

            // Offset the cylinder so that the pivot/origin is at the bottom in relation to the outer ring gameobject.
            CylinderMeshObject.transform.localPosition = start_pt + offset;
            // Set the radius
            CylinderMeshObject.transform.localScale = new Vector3(radius, dist, radius);
            // Make the cylinder look at the origin.
            // Since the cylinder is pointing up(y) and the forward is z, we need to offset by 90 degrees.
            // CylinderMeshObject.transform.LookAt(this.gameObject.transform.position);
            CylinderMeshObject.transform.rotation *= Quaternion.Euler(0, 0, -Mathf.Atan(offsetX/offsetY)* Mathf.Rad2Deg);

            // Create the the Mesh and renderer to show the connecting ring
            MeshFilter ringMesh = CylinderMeshObject.AddComponent<MeshFilter>();
            ringMesh.mesh = this.cylinderMesh;

            MeshRenderer ringRenderer = CylinderMeshObject.AddComponent<MeshRenderer>();
            ringRenderer.material = lineMat;

        // }
    }

    //for rotating computation
    Vector3 find_anchor(Vector3 p){
        return new Vector3(-p.x,-p.y,-p.z);
    }

    Vector3 translation(Vector3 p, Vector3 anchor){
        return new Vector3(p.x + anchor.x, p.y + anchor.y, 0f);
    }

    Vector3 rotation(Vector3 p, float angle){
        angle = angle*PI/180;

        return new Vector3( p.x*Mathf.Cos(angle)-p.y*Mathf.Sin(angle),
                            p.x*Mathf.Sin(angle)+p.y*Mathf.Cos(angle),
                            0f);
    }

    Vector3 post_process(Vector3 p, Vector3 anchor){
        return new Vector3(p.x - anchor.x, p.y - anchor.y, 0);
    }

    Vector3 compute(Vector3 p1, Vector3 p2, float angle){
        Vector3 anchor, new_p;

        anchor = find_anchor(p1);

        new_p = translation(p2, anchor);
        new_p = rotation(new_p, angle);
        new_p = post_process(new_p, anchor);

        return new_p;
    }

    void DrawTrunk(){
        DrawLine(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f));
    }

    void DrawBranch(Vector3 p0, Vector3 p1, int layer = 5){

        Vector3 dir = p1 - p0;
        Vector3 p2;
        Vector3 new_p2;

        if (layer < 1)
            return ;

        //tree leaf
        //middle
        p2 = new Vector3(p1.x + leaf_length_ratio*dir.x, p1.y + leaf_length_ratio*dir.y, p1.z + leaf_length_ratio*dir.z);
        DrawLine(p1,p2);
        DrawBranch(p1, p2, layer - 1);

        //left leaf
        new_p2 = compute(p1,p2,side_angle);
        DrawLine(p1, new_p2);
        DrawBranch(p1, new_p2, layer - 1);

        //right leaf
        new_p2 = compute(p1,p2,-side_angle);
        DrawLine(p1, new_p2);
        DrawBranch(p1, new_p2, layer - 1);
    }

    // Use this for initialization
    void Start () {
        Vector3 p0 = new Vector3(0f ,0f, 0f);
        Vector3 p1 = new Vector3(0f, size, 0f);

        //tree trunk
        DrawLine(p0, p1);

        p0 = new Vector3(p1.x, p1.y * (1-branch_ratio), p1.z);
        //tree leaves
        DrawBranch(p0, p1, layer);

    }
    
    // Update is called once per frame
    void Update () {

    }
}