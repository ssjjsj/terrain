using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class WorldMap : MonoBehaviour
{
    static int blockCount = 9;
    static int blockWidth = 3;
    TerrainBlock[] terrainBlocks = new TerrainBlock[blockCount];
    void Start()
    {
        TerrainGenetor g = new TerrainGenetor();
        g.Init();

        for (int i=0; i<9; i++)
        {
            GameObject block = g.createBlock();
            terrainBlocks[i] = block.GetComponent<TerrainBlock>();
            terrainBlocks[i].SetBlockIndex(i);
            terrainBlocks[i].SetCenterPos(new Vector2(TerrainGenetor.chunckHeight / 2 + (i / blockWidth) * TerrainGenetor.chunckHeight,
                TerrainGenetor.chunckHeight / 2 + (i % blockWidth) * TerrainGenetor.chunckHeight));
        }
    }


    public void OnCenterChange(int x, int y)
    {
        float width = TerrainGenetor.chunckHeight;
        TerrainBlock centerBlock = terrainBlocks[4];
        Vector2 center = centerBlock.GetCenterPos();
        Vector2 min = new Vector2(center.x-width/2, center.y-width/2);
        Vector2 max = new Vector2(center.x + width / 2, center.y + width / 2);

        if (x<min.x && y<min.y)
        {

        }
    }
}
