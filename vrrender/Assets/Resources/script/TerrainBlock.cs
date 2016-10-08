using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TerrainBlock : MonoBehaviour
{
    private int blockIndex = 0;
    private Vector2 centerPos;

    public int GetBlockIndex()
    {
        return blockIndex;
    }

    public void SetBlockIndex(int blockIndex)
    {
        this.blockIndex = blockIndex;
    }

    public Vector2 GetCenterPos()
    {
        return centerPos;
    }

    public void SetCenterPos(Vector2 centerPos)
    {
        this.centerPos = centerPos;

        gameObject.transform.position = new Vector3(centerPos.x - TerrainGenetor.chunckWidth, 0.0f, centerPos.y - TerrainGenetor.chunckHeight);
    }
}
