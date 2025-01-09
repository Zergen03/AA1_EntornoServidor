﻿namespace AA1.Models;
public class Users
{
    private static int _seed = 0;
    public int Id { get; set; }
    public string Name { get; set; }
    public int life { get; set; }
    public int xp { get; set; }
    public int level { get; set; }
    public int gold { get; set; }
    public List<Items> equippedItems = new List<Items>(6);
    public List<Items>? items { get; set; }
    public List<Task>? tasks { get; set; }
    
    public Users(string _name)
    {
        Id = _seed;
        Name = _name;
        life = 10;
        xp = 0;
        level = 1;
        gold = 0;
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