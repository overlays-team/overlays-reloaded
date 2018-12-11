using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * a simple container of Data for our Inventoy Items
 */
[System.Serializable]
public class InventoryItem
{
    public GameObject blockObjectPrefab;
    public int blockAmount;
    public Sprite icon;
    Stack<BlockObject> aviableBlockObjects = new Stack<BlockObject>();
    HashSet<BlockObject> takenBlockObjects = new HashSet<BlockObject>();

    public void AddBlockObject(BlockObject block)
    {
        aviableBlockObjects.Push(block);
        if (takenBlockObjects.Contains(block)) takenBlockObjects.Remove(block);
    }

    public void ReturnBlockObject(BlockObject block)
    {
        aviableBlockObjects.Push(block);
        if (takenBlockObjects.Contains(block)) takenBlockObjects.Remove(block);
        blockAmount++;
    }

    public BlockObject RemoveBlockObject()
    {
        BlockObject block = aviableBlockObjects.Pop();
        takenBlockObjects.Add(block);
        blockAmount--;
        return block;
    }

    public bool CheckIfBlockBelongsToThisItem(BlockObject block)
    {
        if (takenBlockObjects.Contains(block)) return true;
        else return false;
    }
}
