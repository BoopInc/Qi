using UnityEngine;
using System.Collections;

public class WavePlane : MonoBehaviour
{

    private Vector3[] meshVertices;
    private Vector3[] origVerticesPos;
    public int[] moveVertices; //for my mesh, I used 0 and 1 (upper two vertices)
    public float waveSpeed = 1;
    public float waveAmplitudeX = 0.001f;
    public float waveAmplitudeY = 0;
    private float xMove;
    private Mesh mesh;
    private float randomize;

    void Start()
    {

        MeshFilter myMF = this.GetComponent("MeshFilter") as MeshFilter;
        mesh = myMF.mesh;
        meshVertices = mesh.vertices;
        origVerticesPos = meshVertices;
        randomize = Random.Range(0, 100);
    }

    void Update()
    {
        xMove = Mathf.Sin((Time.time * waveSpeed) + randomize);
        foreach (int i in moveVertices)
        {
            meshVertices[i] = origVerticesPos[i] + new Vector3(xMove * waveAmplitudeX, xMove * waveAmplitudeY, 0);
        }
        mesh.vertices = meshVertices;
        mesh.RecalculateNormals();
    }
}