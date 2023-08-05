using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public int IdEquipment;

    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();
        RemoveFromInventory();
        EquipManager.Instance.Equip(this);
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }