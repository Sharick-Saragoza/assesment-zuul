using System.Drawing;
using System.Xml.XPath;

class Player
{
    // fields
    private Inventory backpack;
    // auto property
    public Room currentRoom { get; set; }

    // constructorr
    private int health;

    public Player()
    {
        health = 100;
        currentRoom = null;
        backpack = new Inventory(25);
    }

    // Zien dat de status is van je Health
    public int PlayerStatus()
    {
        if (health == 100)
        {
            Console.WriteLine("Health: " + health);
        }
        else if (health < 100)
        {
            Console.WriteLine("You're bleeding.");
            Console.WriteLine("Health: " + health);
        }
        return health;
    }

    // Player verliest wat health
    public int Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            isDead = true;
        }
        return health;
    }

    // Player's health word hersteld
    public void Heal()
    {
        health = 100;
    }

    // Checkt of de player levend is of niet
    public bool isDead = false;

    public bool IsAlive()
    {

        return isDead;
    }

    public bool TakeFromChest(string itemName)
    {

        Item item = currentRoom.Chest.Get(itemName);
        if (item != null)
        {
            if (backpack.Put(itemName, item))
            {
                Console.WriteLine("Picked up " + itemName);
            }
            else
            {
                Console.WriteLine("Failed to pick up " + itemName);
            }
        }
        else
        {
            Console.WriteLine("Couldn't find item");
            return true;
        }
        return false;
    }

    public bool DropToChest(string itemName)
    {
        Item item = backpack.Get(itemName);

        if (item != null)
        {
            if (currentRoom.Chest.Put(itemName, item))
            {
                Console.WriteLine("Dropped " + itemName);
            }
            else
            {
                backpack.Put(itemName, item);

                Console.WriteLine("Failed to drop" + itemName);
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    public string Use(Command command)
    {
        string ret = "";
        if (!command.HasSecondWord())
        {
            ret = "Use what item?";
            return ret;
        }
        
        string itemName = command.SecondWord;

        Item item = backpack.Get(itemName);
        if (item == null)
        {
            ret = $"You don't have {itemName}";
            return ret;
        }


        if (itemName == "bandage")
        {
            Heal();
            ret = "You feel much better";
        }
        
        if (itemName == "apple")
        {
            Damage(25);
            ret = "You feel sick";
        }

        if (itemName == "key")
        {
            if (currentRoom.IsLocked())
            {
                currentRoom.Unlock();
                ret = "You've unlocked this room";
            }else
            {
                ret = "Could not unlock this room";
            }
        }

        return ret;
  
    }

}

