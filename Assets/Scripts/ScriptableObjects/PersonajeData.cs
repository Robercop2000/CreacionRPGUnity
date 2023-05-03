using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Characters/Character", order = 51)]
public class PersonajeData : ScriptableObject
{
    [SerializeField]
    private string m_name;

    public string Name
    {
        get { return m_name; }
    }

    [SerializeField]
    private int m_level;

    public int Level
    {
        get { return m_level; }
    }

    [SerializeField]
    private int m_maxHealthPoints;

    public int MaxHealthPoints
    {
        get { return m_maxHealthPoints; }
    }

    [SerializeField]
    private int m_maxMagicPoints;

    public int MaxMagicPoints
    {
        get { return m_maxMagicPoints; }
    }

    StatusCondition m_condition = new();

    public StatusCondition Condition 
    { 
        get { return m_condition; } 
    }

    [SerializeField]
    public bool hasPersonalInventory;
}
