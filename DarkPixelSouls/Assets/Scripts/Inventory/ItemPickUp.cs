using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        bool wasPickedUp = Inventory.Instance.Add(item);
        Debug.Log("PickUp item" + item.name);

        if (wasPickedUp)
            Destroy(gameObject);
    }
}