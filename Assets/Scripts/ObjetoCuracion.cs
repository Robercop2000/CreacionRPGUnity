using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjetoCuracion : Item
{
    public override void GetItem(Personaje personaje, IInventory<Item> inventory)
    {
        if (personaje == null) return;

        Debug.Log($"{personaje.Name} found a {data.Name} that has the power of {data.HealType} and has stored it.");

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

        if (inventory.ItemCount(this) == 0) {
            return;
        }

        switch (data.HealType)
        {
            case HealType.HEALTH:
                Debug.Log($"{personaje.Name} has consumed {data.Name} and got healed {data.HealAmmount} HP!!!");
                personaje.HealthPoints += data.HealAmmount;
                Debug.Log($"{personaje.Name} now has {personaje.HealthPoints} HP.");
                inventory.RemoveItem(this);
                break;

            case HealType.MAGIC:
                Debug.Log($"{personaje.Name} has consumed {data.Name} and got magic restored {data.HealAmmount} MP!!!");
                personaje.MagicPoints += data.HealAmmount;
                Debug.Log($"{personaje.Name} now has {personaje.MagicPoints} MP.");
                inventory.RemoveItem(this);
                break;

            case HealType.RESURRECTION:
                if (personaje.CharacterStatusCondition.IsDead)
                {
                    personaje.CharacterStatusCondition.SetDead(false);
                    personaje.HealthPoints += data.HealAmmount;
                    Debug.Log($"{personaje.Name} has consumed {data.Name} and was revived with {personaje.HealthPoints} HP!!!");
                }
                else
                {
                    Debug.Log($"{personaje.Name} has consumed {data.Name}, but nothing happened...");
                }

                inventory.RemoveItem(this);

                break;

            default:
                throw new ArgumentException($"{data.Name} StatusCondition must be a Restoration one. For negative conditions use ObjetoEstadoAlterado.cs");
        }
    }
}

