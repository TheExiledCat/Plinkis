using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hud : MonoBehaviour
{
    int score;
    float multiplier;
    int lines;
    [SerializeField]
    GameObject HeldObject;
    Sprite held;
    Image image;
    [SerializeField]
    GameObject[] slots = new GameObject[2];
    private void Start()
    {
        Field.Grid.OnHold += SetHeld;
        Field.Grid.spawner.OnSpawn += UpdateOrder;
        image = HeldObject.GetComponent<Image>();
        UpdateOrder();
    }
    public void SetHeld(string g)
    {
        if (!image.enabled) image.enabled = true;
        held = Resources.Load<Sprite>("Sprites/" + g);
        image.sprite = held;
    }
    public void UpdateOrder()
    {
        print("Updating order");
        for(int i =0; i < slots.Length; i++)
        {
            slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/"+Field.Grid.spawner.GetSpawns()[i]);
        }
    }
}
