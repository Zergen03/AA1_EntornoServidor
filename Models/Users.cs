﻿namespace AA1.Models;
public class Users
{
    private static int _seed = 0;
    public int Id { get; set; }
    public string Name { get; set; }
    public string password { get; set; }
    public int life { get; set; }
    public int xp { get; set; }
    public int level { get; set; }
    public int gold { get; set; }
    public Dictionary<int, string> equippedItems { get; set; } = new Dictionary<int, string>();
    public Dictionary<int, string> items { get; set; } = new Dictionary<int, string>();
    public Dictionary<int, string> Tasks { get; set; } = new Dictionary<int, string>();

    public Users(string _name, string _password)
    {
        Id = _seed;
        Name = _name;
        password = _password;
        life = 10;
        xp = 0;
        level = 1;
        gold = 0;
        IncraseSeed();
    }

    public Users()
    {
        Id = _seed;
        IncraseSeed();
    }

    private static void IncraseSeed()
    {
        _seed++;
    }

    public override string ToString()
    {
        return $"Name: {Name},\nLife: {life},\nXp: {xp},\nLevel: {level},\nGold: {gold}";
    }
};