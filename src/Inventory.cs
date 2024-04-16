class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
        if(item.Weight < FreeWeight()){
            items.Add(itemName, item);
            return true;
        }

        return false;
    }
    
    public Item Get(string itemName)
    {
        if(items.ContainsKey(itemName)){
            Item item = items[itemName];
            items.Remove(itemName);
            return item;
        }
        
        return null;
    }

    public int TotalWeight()
    {
        int total = 0;

        foreach(Item item in items.Values){
            total += item.Weight;
        }

        return total;
    }
    
    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }
}