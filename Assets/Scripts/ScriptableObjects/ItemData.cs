using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Info", order = 51)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string      m_name;

    public string Name
    { 
        get { return m_name; } 
    }

    [SerializeField] 
    private int         m_sellPrice;

    public int SellPrice
    {
        get { return m_sellPrice; }
    }

    [SerializeField] 
    private int         m_buyPrice;

    public int BuyPrice
    {
        get { return m_buyPrice; }
    }

    [SerializeField] 
    private HealType    m_healType;
    public HealType HealType
    {
        get { return m_healType; }
    }
    

    [SerializeField] 
    private int         m_healAmmount;
    public int HealAmmount
    {
        get { return m_healAmmount; }
    }
}
