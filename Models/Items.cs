namespace AA1.Models;

public class Items{
    private List<string> _types = new List<string> {"Weapon", "Helmet", "Chestplate", "Boots", "Gloves", "Leggins"};
    private static int _seed = 0;
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public Dictionary<string, int> Stats { get; set; }
    public string Type { get; set; }
    
    public Items(string _name, int _value, Dictionary<string, int> _stats, string _type)
    {
        Id = _seed;
        Name = _name;
        Value = _value;
        Stats = _stats;
        Type = SetType(_type);
        IncraseSeed();
    }

    private static void IncraseSeed()
    {
        _seed++;
    }

    private string SetType(string _type)
    {
        if(_types.Contains(_type))
        {
            return _type;
        }
        else
        {
            throw new ArgumentException("Type not found");
        }
    }

    public override string ToString()
    {
        return $"Name: {Name},\nValue: {Value},\n{Stats}";   
    }
}