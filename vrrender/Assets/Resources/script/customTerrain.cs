using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class customTerrain : MonoBehaviour {

    int width = 17;
    int height = 17;
    float[] heightData;
	// Use this for initialization
	void Start () {
        int rowNum = height-1;
        int colNum = width-1;

        heightData = new float[width * height];
        readHeightData();

        int[] triangle = new int[rowNum*colNum*6];
        Vector3[] vertexs = new Vector3[rowNum * colNum * 4];
        Vector2[] uvs = new Vector2[rowNum*colNum*4];

        for (int row=0; row<rowNum; row++)
        {
            for (int col=0; col<colNum; col++)
            {
                int vertexIndex = (row*rowNum+col)*4;
                int heightDataIndex = row * width + col;
                vertexs[vertexIndex] = new Vector3(col, heightData[heightDataIndex], row);
                vertexs[vertexIndex + 1] = new Vector3(col + 1, heightData[heightDataIndex+1], row);
                vertexs[vertexIndex + 2] = new Vector3(col + 1, heightData[heightDataIndex+width], row + 1);
                vertexs[vertexIndex + 3] = new Vector3(col, heightData[heightDataIndex+width+1], row + 1);
                
                uvs[vertexIndex] = new Vector2(0.0f, 1.0f);
                uvs[vertexIndex+1] = new Vector2(1.0f, 1.0f);
                uvs[vertexIndex+2] = new Vector2(1.0f, 0.0f);
                uvs[vertexIndex+3] = new Vector2(0.0f, 0.0f);

                int triangleIndex = (row*rowNum+col)*6;
                triangle[triangleIndex] = vertexIndex+3;
                triangle[triangleIndex+1] = vertexIndex+2;
                triangle[triangleIndex+2] = vertexIndex+1;
                triangle[triangleIndex+3] = vertexIndex+1;
                triangle[triangleIndex+4] = vertexIndex;
                triangle[triangleIndex+5] = vertexIndex+3;
            }
        }



        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        //MeshRenderer meshRender = gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = meshFilter.mesh;
        mesh.vertices = vertexs;
        mesh.uv = uvs;
        mesh.triangles = triangle;
	}


    void readHeightData()
    {
        Texture2D heightMap = Resources.Load("height/output") as Texture2D;
        heightData = new float[heightMap.width * heightMap.height];
        for (int i=0; i<heightMap.height; i++)
        {
            for (int j=0; j<heightMap.width; j++)
            {
                heightData[i*heightMap.width+j] = heightMap.GetPixel(j, i).r*10/255;
            }
        }
    }


    void fractal()
    {
        float maxHeight = 2.0f;
        float minHeight = 0.0f;
        Vector2 up = new Vector2(0.0f, 1.0f);

        int iteratorTime = 20;
        float delta = (maxHeight - minHeight) / iteratorTime;

        for (int curIterator = 0; curIterator < iteratorTime; curIterator++)
        {
            int startX = Random.Range(0, width);
            int startZ = Random.Range(0, height);
            int endX = Random.Range(0, width);
            int endZ = Random.Range(0, height);

            Debug.Log(startX.ToString() + " " + startZ.ToString() + " " + endX.ToString() + " " +endZ.ToString());

            bool isUp = Random.Range(0, 1) > 0;

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2 dir = new Vector2(x - startX, z - startZ);
                    if (isUp)
                    {
                        if (Vector2.Dot(dir.normalized, up) > 0)
                            heightData[z * height + x] += delta;
                    }
                    else
                    {
                        if (Vector2.Dot(dir.normalized, up) < 0)
                            heightData[z * height + x] += delta;
                    }
                }
            }
        }
}
	
	// Update is called once per frame
	void Update () {
	
	}
}
