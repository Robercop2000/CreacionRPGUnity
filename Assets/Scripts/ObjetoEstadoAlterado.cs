using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoEstadoAlterado : Item
{
    public override void GetItem(Personaje personaje, IInventory<Item> inventory)
    {
        if (personaje == null) return;

        Debug.Log($"{personaje.Name} found a {data.Name} that has the power of {data.HealType} and stored it.");

        if (inventory != null)
            inventory.AddItem(this);

        else
        {
            throw new ArgumentException($"{personaje.Name} Inventory given is not valid.");
        }
    }

    public override void UseItem(Personaje personaje, IInventory<Item> inventory)
    {
        if (personaje == null) return;

        if (inventory.ItemCount(this) == 0 || personaje.CharacterStatusCondition.IsPoisoned)
        {
            Debug.Log($"{personaje.Name} is already poisoned.");
            return;
        }

        switch (data.HealType)
        {
            case HealType.POISON:
                Debug.Log($"{personaje.Name} has consumed {data.Name} and has been poisoned.");
                personaje.CharacterStatusCondition.SetPoisoned(true);
                inventory.RemoveItem(this);
                break;

            default:
                throw new ArgumentException($"{data.Name} Status Condition must be a Negative one. For restoration conditions use ObjetoCuraciones.cs");
        }
    }
}
