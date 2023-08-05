using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelEquipment : Singlton<PixelEquipment>
{
    public List<GameObject> equipped = new List<GameObject>();

    public void EquipItem(int IdItem)
    {
        for (int i = 0; i < equipped.Count; i++)
        {
            if (IdItem == i)
            {
                equipped[i].gameObject.SetActive(true);
            }
        }        
    }
    public void UnEquipItem(int IdItem)
    {
        for (int i = 0; i < equipped.Count; i++)
        {
            if (IdItem == i)
            {
                equipped[i].gameObject.SetActive(false);
            }
        }
    }
}