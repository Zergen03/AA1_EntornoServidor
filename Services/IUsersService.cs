﻿using AA1.Models;
using AA1.DTOs;

namespace AA1.Services;

public interface IUsersService
{
    LoginResultDTO Login(string name, string password);
    LoginResultDTO Register(string name, string password);
    Users AddTask(int id, AA1.Models.Task task);
    Dictionary<int, string> GetTasks(int id);
    Users DeleteTask(int id, int taskId);
    Dictionary<int,string> AddItemToInventory(int id, Items item);
    Dictionary<int,string> GetInventory(int id);
    Dictionary<int,string> GetEquippedItems(int id);
    Users EquipItem(int id, int itemId);
    Users UnEquipItem(int id, int itemId);
    Users Heal(int id, int heal);
    Users GainXp(int id, int xp);
    Users GainGold(int id, int gold);
}