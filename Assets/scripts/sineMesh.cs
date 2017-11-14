using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineMesh : MonoBehaviour {
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;
    [SerializeField]
    private float depth;
    [SerializeField]
    private int precision;
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private int waves;
    [SerializeField]
    public int invert;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    // Use this for initialization
    private void Start()
    {
    }

    public void create () {
        mesh = new Mesh();
        vertices = new Vector3[precision * 4 + 8];
        triangles = new int[(precision + 1) * 30 + 6];
        float maxSinX = Mathf.PI * 2 * waves;

        createQuad(0, 0, 0, 0);
        int i = 0;
        while (++i < precision + 1)
            createQuad(invert * Mathf.Sin(maxSinX / (precision + 1) * i) * amplitude, 0, depth / (precision + 1) * i, i);
        createQuad(0, 0, depth, i);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void createQuad(float posX, float posY, float posZ, int quadPos)
    {
        //Create the vertices of the new face
        vertices[quadPos * 4 + 0] = new Vector3(posX - width / 2, posY - height / 2, posZ);
        vertices[quadPos * 4 + 1] = new Vector3(posX - width / 2, posY + height / 2, posZ);
        vertices[quadPos * 4 + 2] = new Vector3(posX + width / 2, posY + height / 2, posZ);
        vertices[quadPos * 4 + 3] = new Vector3(posX + width / 2, posY - height / 2, posZ);
        linkQuad(quadPos);
    }

    void linkQuad(int quadPos)
    {
        if (quadPos == 0)
        {
            //Create the triangles for the new face
            triangles[quadPos * 30 + 0] = quadPos * 4 + 0;
            triangles[quadPos * 30 + 1] = quadPos * 4 + 1;
            triangles[quadPos * 30 + 2] = quadPos * 4 + 2;
            triangles[quadPos * 30 + 3] = quadPos * 4 + 0;
            triangles[quadPos * 30 + 4] = quadPos * 4 + 2;
            triangles[quadPos * 30 + 5] = quadPos * 4 + 3;
        }
        else
        {
            //Create the triangles for the new face
            triangles[quadPos * 30 + 0] = quadPos * 4 + 2;
            triangles[quadPos * 30 + 1] = quadPos * 4 + 1;
            triangles[quadPos * 30 + 2] = quadPos * 4 + 0;
            triangles[quadPos * 30 + 3] = quadPos * 4 + 2;
            triangles[quadPos * 30 + 4] = quadPos * 4 + 0;
            triangles[quadPos * 30 + 5] = quadPos * 4 + 3;
            //Link the new face with the old one.
            //Left
            triangles[(quadPos - 1) * 30 + 6] = (quadPos - 1) * 4 + 1;
            triangles[(quadPos - 1) * 30 + 7] = (quadPos - 1) * 4 + 0;
            triangles[(quadPos - 1) * 30 + 8] = quadPos * 4 + 0;
            triangles[(quadPos - 1) * 30 + 9] = (quadPos - 1) * 4 + 1;
            triangles[(quadPos - 1) * 30 + 10] = quadPos * 4 + 0;
            triangles[(quadPos - 1) * 30 + 11] = quadPos * 4 + 1;

            //right
            triangles[(quadPos - 1) * 30 + 12] = (quadPos - 1) * 4 + 3;
            triangles[(quadPos - 1) * 30 + 13] = (quadPos - 1) * 4 + 2;
            triangles[(quadPos - 1) * 30 + 14] = quadPos * 4 + 2;
            triangles[(quadPos - 1) * 30 + 15] = (quadPos - 1) * 4 + 3;
            triangles[(quadPos - 1) * 30 + 16] = quadPos * 4 + 2;
            triangles[(quadPos - 1) * 30 + 17] = quadPos * 4 + 3;

            //bottom
            triangles[(quadPos - 1) * 30 + 18] = (quadPos - 1) * 4 + 0;
            triangles[(quadPos - 1) * 30 + 19] = (quadPos - 1) * 4 + 3;
            triangles[(quadPos - 1) * 30 + 20] = quadPos * 4 + 3;
            triangles[(quadPos - 1) * 30 + 21] = (quadPos - 1) * 4 + 0;
            triangles[(quadPos - 1) * 30 + 22] = quadPos * 4 + 3;
            triangles[(quadPos - 1) * 30 + 23] = quadPos * 4 + 0;

            //top
            triangles[(quadPos - 1) * 30 + 24] = (quadPos - 1) * 4 + 2;
            triangles[(quadPos - 1) * 30 + 25] = (quadPos - 1) * 4 + 1;
            triangles[(quadPos - 1) * 30 + 26] = quadPos * 4 + 1;
            triangles[(quadPos - 1) * 30 + 27] = (quadPos - 1) * 4 + 2;
            triangles[(quadPos - 1) * 30 + 28] = quadPos * 4 + 1;
            triangles[(quadPos - 1) * 30 + 29] = quadPos * 4 + 2;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
