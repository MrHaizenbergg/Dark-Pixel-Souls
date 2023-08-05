using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class EquipManager : Singlton<EquipManager>
{
    Equipment[] currentEquipment;

    public delegate void OnEquipChanged(Equipment newItem, Equipment oldItem);
    public OnEquipChanged onEquipChanged;

    Inventory inventory;
    PixelEquipment pixelEquipment;

    private void Start()
    {
        inventory = Inventory.Instance;
        pixelEquipment = PixelEquipment.Instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        pixelEquipment.EquipItem(newItem.IdEquipment);
        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            pixelEquipment.UnEquipItem(oldItem.IdEquipment);
            inventory.Add(oldItem);
        }

        if (onEquipChanged != null)
        {
            onEquipChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;

    }

    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            pixelEquipment.UnEquipItem(oldItem.IdEquipment);

            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipChanged != null)
            {
                onEquipChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnEquipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            UnEquipAll();
    }
}