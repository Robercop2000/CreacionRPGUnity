using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje
{
    private string m_name;
    public string Name
    {
        get { return m_name; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Name can not be null");
            }
            else if (value.Length < 2 ||value.Length > 16)
            {
                throw new ArgumentException("Name is too long, try using between 2 and 16 characters.");
            }
            else
            {
                m_name = value;
            }
        }
    }

    private int m_level;
    public int Level
    {
        get { return m_level; }
        set
        {
            m_level = Math.Clamp(value, 1, 100);
        }
    }

    private int m_maxHealthPoints;

    public int MaxHealthPoints
    {
        get { return m_maxHealthPoints; }
        set
        {
            if (value <= 0)
                throw new ArgumentException($"{m_name} MaxLife can not be less than 1.");

            else
                m_maxHealthPoints = value;
        }
    }

    private int m_healthPoints;
    public int HealthPoints
    {
        get { return m_healthPoints; }
        set
        {
            m_healthPoints = Math.Clamp(value, 0, m_maxHealthPoints);
        }
    }

    private int m_maxMagicPoints;
    public int MaxMagicPoints
    {
        get { return m_maxMagicPoints; }
        set
        {
            if (value < 0)
                throw new ArgumentException($"{m_name} MaxMagicPoints can not be less than 0.");

            else
                m_maxMagicPoints = value;
        }
    }

    private int m_magicPoints;
    public int MagicPoints
    {
        get { return m_magicPoints; }
        set
        {
            m_magicPoints = Math.Clamp(value, 0, m_maxMagicPoints);
        }
    }

    private StatusCondition m_characterStatusCondition;
    public StatusCondition CharacterStatusCondition
    {
        get { return m_characterStatusCondition; }
        set { m_characterStatusCondition = value; }
    }

    [Tooltip("Inventory of the Character.")]
    public Inventory<Item> inventoryCharacter;

    //Manual creation of Personaje
    public Personaje(string name, int level, int maxHealthPoints, int maxMagicPoints, StatusCondition statusCondition, bool hasPersonalInventory)
    {
        Name = name;
        Level = level;

        //Set Health
        m_maxHealthPoints = maxHealthPoints;
        HealthPoints = m_maxHealthPoints;

        //Set Magic
        m_maxMagicPoints = maxMagicPoints;
        MagicPoints = m_maxMagicPoints;

        //StatusCondition
        CharacterStatusCondition = statusCondition;

        //Set personal inventory for character
        inventoryCharacter = hasPersonalInventory ? new() : null; 
        //Si fuera false, puede adjuntarse a un inventario general en un GameManager. 

    }

    //ScriptableObject Creation of Personaje
    public Personaje(PersonajeData data)
    {
        if (data != null)
        {
            Name = data.Name;
            Level = data.Level;

            //Set Health
            m_maxHealthPoints = data.MaxHealthPoints;
            HealthPoints = m_maxHealthPoints;

            //Set Magic
            m_maxMagicPoints = data.MaxMagicPoints;
            MagicPoints = m_maxMagicPoints;
            
            //StatusCondition
            CharacterStatusCondition = data.Condition;

            //Set personal inventory for character
            inventoryCharacter = data.hasPersonalInventory ? new() : null;
        }

        else
            throw new Exception($"There is no Scriptable Object attached to Player Definition");


    }
}

public interface IStatusCondition //Status controller
{
    bool IsPoisoned { get; }
    bool IsDead { get; }
    void SetPoisoned(bool poisoned);
    void SetDead(bool dead);

}

public class StatusCondition : IStatusCondition //Status controller class
{
    private bool isPoisoned;
    private bool isDead;

    public bool IsPoisoned => isPoisoned;
    public bool IsDead => isDead;

    public void SetPoisoned(bool poisoned)
    {
        isPoisoned = poisoned;
    }

    public void SetDead(bool dead)
    {
        isDead = dead;
    }
}
