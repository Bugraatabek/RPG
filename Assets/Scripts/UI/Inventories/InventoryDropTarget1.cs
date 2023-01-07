using System.Collections;
using System.Collections.Generic;
using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

public class InventoryDropTarget : MonoBehaviour,  IDragDestination<InventoryItem>
{
    public void AddItems(InventoryItem item, int number)
    {
        throw new System.NotImplementedException();
    }

    public int MaxAcceptable(InventoryItem item)
    {
        return 0;
    }
}

