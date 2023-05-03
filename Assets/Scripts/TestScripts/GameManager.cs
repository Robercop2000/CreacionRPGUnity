using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;


//GameManager de Ejemplo para mostrar el uso de las clases
public class GameManager : MonoBehaviour
{
    [SerializeField] PersonajeData[] party;
    private List<Personaje> characters = new List<Personaje>();



    //Esta variable crea una instancia de un Inventario general para los personajes.
    //Además permite crear Inventarios para tiendas o cualquier otra cosa que requiera la interfaz de gestionar objetos.
    private static Inventory<Item> generalInventory;

    public static Inventory<Item> GeneralInventory
    {
        get
        {
            if (generalInventory == null)
            {
                generalInventory = new Inventory<Item>();
            }
            return generalInventory;
        }
    }

    //Creacion de personajes
    void Awake()
    {
        //Es posible crear Personajes manualmente si se añaden los parametros al crear la instancia de Personaje.
        Personaje character = new Personaje("Manuel", 25, 150, 150, new(), false);
        characters.Add(character);
        if (character.inventoryCharacter == null)
            character.inventoryCharacter = GeneralInventory;

        //Define el grupo de personajes usando los PersonajeData adjuntos
        foreach (PersonajeData data in party)
        {   
            //Tambien se pueden crear Personajes usando el Scriptable Object PersonajeData.
            character = new Personaje(data); 

            if (character.inventoryCharacter == null) 
               character.inventoryCharacter = GeneralInventory; //Si el personaje no tiene Inventario propio se le asigna el inventario general

            characters.Add(character);
        }
    }

    private void Start()
    {
        Debug.Log("Preparing the group to enter the dungeon...");

        StartCoroutine(CreateEvents());
    }

    private IEnumerator CreateEvents()
    {
        yield return new WaitForSeconds(2f);

        if (characters.Count > 0)
        {
            Debug.Log("The group is made up of: ");
            yield return new WaitForSeconds(0.5f);

            foreach (Personaje character in characters) //Nombra cada integrante del grupo
            {
                Debug.Log($"{character.Name}");
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(1.5f);

            Debug.Log("The group has entered the Dungeon...");

            yield return new WaitForSeconds(1.5f);
        }

        else //Si no hay ninguno no comienza el bucle
        {
            Debug.Log("There is not enough people to face the Dungeon, at least 1 character is needed.");
            Application.Quit();
        }

        while (true)
        {
            Item itemFound = GetRandomItem();

            if (itemFound != null && characters.Count > 0)
            {
                Personaje actualCharacter = characters[Random.Range(0, characters.Count)];
                switch (Random.Range(0, 5))
                {
                    case 0: //El personaje coge un objeto y se lo guarda
                        itemFound.GetItem(actualCharacter, actualCharacter.inventoryCharacter);
                        break;

                    case 1: //El personaje coge un objeto pero se le cae y lo pierde
                        itemFound.GetItem(actualCharacter, actualCharacter.inventoryCharacter);
                        Debug.Log($"{actualCharacter.Name} has dropped {itemFound.data.Name} found...");
                        actualCharacter.inventoryCharacter.RemoveItem(itemFound);
                        break;

                    case 2: //El personaje coge un objeto y se lo come
                        itemFound.GetItem(actualCharacter, actualCharacter.inventoryCharacter);
                        itemFound.UseItem(actualCharacter, actualCharacter.inventoryCharacter);
                        break;

                    case 3: //El personaje tiene hambre y buscara en su inventario por algo de comer y si lo encuentra, lo consume
                        Debug.Log($"{actualCharacter.Name} is hungry, trying to find something to eat in her bag...");
                        List<Item> itemsBag = actualCharacter.inventoryCharacter.ObtainItems().ToList();

                        if (itemsBag.Count != 0)
                        {
                            int rnd = Random.Range(0, itemsBag.Count);
                            Debug.Log($"{actualCharacter.Name} found a {itemsBag[rnd].data.Name}, it smells delicious!!!");
                            itemsBag[rnd].UseItem(actualCharacter, actualCharacter.inventoryCharacter);
                            break;
                        }

                        Debug.Log($"{actualCharacter.Name} could not find anything.");

                        break;

                    case 4: //El personaje se cae y recibe 25 puntos de daño
                        actualCharacter.HealthPoints -= 25;

                        FallEvent(actualCharacter);

                        break;
                }
            }

            else if(characters.Count == 0) //Si mueren todos los personajes se acaba el juego
            {
                Debug.Log("There is no remaining Characters in the party to face the Dungeon...");
                Application.Quit();
            }

            else if(itemFound == null) //Si no quedan más objetos usables se acaba el juego
            {
                Debug.Log("There is nothing interesting remaining in the Dungeon, let's go back.");
                Application.Quit();
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private void FallEvent(Personaje actualCharacter)
    {
        if (actualCharacter.HealthPoints > 0) //Buscará en la mochila un objeto curativo y si no lo encuentra no se curará
        {
            Debug.Log($"{actualCharacter.Name} has fallen and lost 25HP, having {actualCharacter.HealthPoints} HP. Searching for a healing item in his/her inventory...");

            Item healingItem = actualCharacter.inventoryCharacter.ObtainItems().FirstOrDefault(item => item.data.HealType == HealType.HEALTH);
            if (healingItem != null)
            {
                healingItem.UseItem(actualCharacter, actualCharacter.inventoryCharacter);
            }
            else
            {
                Debug.Log($"{actualCharacter.Name} could not find any item to heal...");
            }
        }

        else //Buscará en la mochila un objeto de resurrección y si no hay ninguno morirá y se borrará del grupo
        {
            Debug.Log($"{actualCharacter.Name} has fallen down and is fighting for his/her life. Searching for a reviving item in his/her inventory...");
            actualCharacter.CharacterStatusCondition.SetDead(true);

            Item revivingItem = actualCharacter.inventoryCharacter.ObtainItems().FirstOrDefault(item => item.data.HealType == HealType.RESURRECTION);
            if (revivingItem != null)
            {
                revivingItem.UseItem(actualCharacter, actualCharacter.inventoryCharacter);
                if (actualCharacter.CharacterStatusCondition.IsDead)
                {
                    Debug.Log($"{actualCharacter.Name} has eaten something strange. He/She is absolutely dead now!!!");
                    characters.Remove(actualCharacter);
                }
            }
            else
            {
                Debug.Log($"{actualCharacter.Name} could not find any item to revive, {actualCharacter.Name} is totally dead, bye {actualCharacter.Name}");
                characters.Remove(actualCharacter);
            }
        }
    }

    public Item GetRandomItem() //Encuentra items aleatorios con los que interactuar
    {
        Item[] items = FindObjectsOfType<Item>(false);

        if (items.Length == 0)
        {
            Debug.LogWarning("No items found in scene.");
            return null;
        }

        int randomIndex = Random.Range(0, items.Length);
        return items[randomIndex];
    }


}
