using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }
    
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // disable all items
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        
        // active 3 items randomly
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int idx = 0; idx < ran.Length; ++idx)
        {
            Item ranItem = items[ran[idx]];
            // replace max level item with consumable item
            if (ranItem.level == ranItem.data.damages.Length)
            {
                // Energy Drink
                items[4].gameObject.SetActive(true);
            }
            else{
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
