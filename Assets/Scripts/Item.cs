using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealType
{
    HEALTH,
    MAGIC,
    RESURRECTION,
    POISON
}

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    public ItemData data;

    private void Awake()
    {
        if (data == null)
        {
            throw new Exception($"There is no Scriptable Object attached to {gameObject.name}");
        }
    }
    public abstract void GetItem(Personaje personaje, IInventory<Item> inventory);
    public abstract void UseItem(Personaje personaje, IInventory<Item> inventory);
}
