using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lamar a la interfaz para tener un inventario común
public interface IInventory<T> where T : Item 
{
    void AddItem(T item);
    void RemoveItem(T item);
    void CleanInventory();
    int ItemCount(T item);
    IEnumerable<T> ObtainItems();
}

//Crear clase para tener un inventario especifico para cada personaje
public class Inventory<T> : IInventory<T> where T : Item 
{
    private Dictionary<T, int> inventory = new Dictionary<T, int>();

    public void AddItem(T item)
    {
        inventory[item] = inventory.ContainsKey(item) ? inventory[item] + 1 : 1;
    }

    public void RemoveItem(T item)
    {
        if (inventory.ContainsKey(item))
        {
            if (inventory[item] > 1)
            {
                inventory[item]--;
            }
            else
            {
                inventory.Remove(item);
            }
        }
    }

    public void CleanInventory()
    {
        inventory.Clear();
    }

    public int ItemCount(T item)
    {
        return inventory.TryGetValue(item, out int count) ? count : 0;
    }

    public IEnumerable<T> ObtainItems()
    {
        return inventory.Keys;
    }
}
