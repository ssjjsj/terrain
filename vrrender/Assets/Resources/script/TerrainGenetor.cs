using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenetor {

    int width = 64;
    int height = 64;
    float[] heightData;
    meshData[] meshAry;
    Mesh[] meshs;
    GameObject prefabObj;

    public static int chunckWidth = 64;
    public static int chunckHeight = 64;

    class meshData
    {
        public Vector3[] vertices;
        public Vector2[] uv;
        public int[] triangles;
    }
	// Use this for initialization


    public void Init ()
    {
        prefabObj = (GameObject)Resources.Load("prefab/terrainblock");
    }

    public GameObject createBlock() 
    {
        heightData = new float[width * height];
        readHeightData();

        int rowNum = height - 1;
        int colNum = width - 1;
        int perMeshVertexCount = 65000;
        int perMeshIndexCount = perMeshVertexCount/4*6;
        int meshCount = rowNum*colNum*4/perMeshVertexCount+1;

        meshs = new Mesh[meshCount];
        meshAry = new meshData[meshCount];

        GameObject obj = null;
        for (int i=0; i<meshCount; i++)
        {
            obj = (GameObject)GameObject.Instantiate(prefabObj);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            meshs[i] = obj.GetComponent<MeshFilter>().mesh;
            meshAry[i] = new meshData();
            meshAry[i].vertices = new Vector3[perMeshVertexCount];
            meshAry[i].triangles = new int[perMeshIndexCount];
            meshAry[i].uv = new Vector2[perMeshVertexCount];
        }

        for (int row = 0; row < rowNum; row++)
        {
            for (int col = 0; col < colNum; col++)
            {
                int curIndex = (row * rowNum + col) * 4;
                int meshIndex = curIndex / perMeshVertexCount;
                int vertexIndex = curIndex % perMeshVertexCount;
                int heightDataIndex = row * width + col;
                meshAry[meshIndex].vertices[vertexIndex].x = col;
                meshAry[meshIndex].vertices[vertexIndex].y = heightData[heightDataIndex];
                meshAry[meshIndex].vertices[vertexIndex].z = row;

                meshAry[meshIndex].vertices[vertexIndex + 1].x = col+1;
                meshAry[meshIndex].vertices[vertexIndex + 1].y = heightData[heightDataIndex + 1];
                meshAry[meshIndex].vertices[vertexIndex + 1].z = row;

                meshAry[meshIndex].vertices[vertexIndex + 2].x = col + 1;
                meshAry[meshIndex].vertices[vertexIndex + 2].y = heightData[heightDataIndex + width+1];
                meshAry[meshIndex].vertices[vertexIndex + 2].z = row + 1;

                meshAry[meshIndex].vertices[vertexIndex + 3].x = col;
                meshAry[meshIndex].vertices[vertexIndex + 3].y = heightData[heightDataIndex + width];
                meshAry[meshIndex].vertices[vertexIndex + 3].z = row + 1;

                meshAry[meshIndex].uv[vertexIndex].x = 0.0f;
                meshAry[meshIndex].uv[vertexIndex].y = 1.0f;

                meshAry[meshIndex].uv[vertexIndex + 1].x = 1.0f;
                meshAry[meshIndex].uv[vertexIndex + 1].y = 1.0f;

                meshAry[meshIndex].uv[vertexIndex + 2].x = 1.0f;
                meshAry[meshIndex].uv[vertexIndex + 2].y = 0.0f;

                meshAry[meshIndex].uv[vertexIndex + 3].x = 0.0f;
                meshAry[meshIndex].uv[vertexIndex + 3].y = 0.0f;

                int triangleIndex = (row * rowNum + col) * 6 % perMeshIndexCount;
                meshAry[meshIndex].triangles[triangleIndex] = vertexIndex+2;
                meshAry[meshIndex].triangles[triangleIndex + 1] = vertexIndex + 1;
                meshAry[meshIndex].triangles[triangleIndex + 2] = vertexIndex;
               
                meshAry[meshIndex].triangles[triangleIndex + 3] = vertexIndex;
                meshAry[meshIndex].triangles[triangleIndex + 4] = vertexIndex + 3;
                meshAry[meshIndex].triangles[triangleIndex + 5] = vertexIndex+2;
            }
        }

        for (int i = 0; i < meshCount; i++)
        {
            meshs[i].vertices = meshAry[i].vertices;
            meshs[i].uv = meshAry[i].uv;
            meshs[i].triangles = meshAry[i].triangles;
        }

        return obj;
	}


    void readHeightData()
    {
        Texture2D heightMap = Resources.Load("height/output") as Texture2D;
        //heightData = new float[heightMap.width * heightMap.height];
        heightData = new float[chunckWidth* chunckHeight];
        for (int i=0; i<chunckHeight; i++)
        {
            for (int j=0; j<chunckWidth; j++)
            {
                //heightData[i*heightMap.width+j] = heightMap.GetPixel(j, i).r*100;
                //Debug.Log(heightMap.GetPixel(j,i));

                heightData[i * chunckWidth + j] = 0.0f;
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
}
