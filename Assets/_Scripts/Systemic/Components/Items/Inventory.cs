using UnityEngine;
using UnityEngine.UI;

public class Inventory : EntityComponent
{
    [SerializeField]
    public ItemSlotUI[] items;
    private int selectedItemIndex = 0;

    public void NextSelectedItem()
    {
        items[selectedItemIndex].outline.enabled = false;
        if (selectedItemIndex < items.Length - 1)
        {
            selectedItemIndex++;
        }
        else
        {
            selectedItemIndex = 0;
        }
        items[selectedItemIndex].outline.enabled = true;
    }

    public void PreviousSelectedItem()
    {
        items[selectedItemIndex].outline.enabled = false;
        if (selectedItemIndex > 0)
        {
            selectedItemIndex--;
        }
        else
        {
            selectedItemIndex = items.Length - 1;
        }
        items[selectedItemIndex].outline.enabled = true;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].potionPrefab == null)
            {
                items[i].potionPrefab = item.potionPrefab;
                items[i].image.sprite = item.potionImage;
                items[i].image.enabled = true;
                return;
            }
        }
    }

    public ItemSlotUI RemoveItem()
    {
        items[selectedItemIndex].potionPrefab = null;
        items[selectedItemIndex].image.enabled = false;
        //Instantiate on drop ?
        return items[selectedItemIndex];
    }

    public void SwitchItem(Item item)
    {
        RemoveItem();
        items[selectedItemIndex].potionPrefab = item.potionPrefab;
        items[selectedItemIndex].image.sprite = item.potionImage;
        items[selectedItemIndex].image.enabled = true;
    }

    public GameObject CurrentItem()
    {
        return (items[selectedItemIndex].potionPrefab);
    }

}