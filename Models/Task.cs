namespace AA1.Models;

public class Task{
    private static int _seed = 0;
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Xp { get; set; }
    public int Gold { get; set; }
    public int difficulty { get; set; }
    public int lostLife { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool Completed { get; set; }
    public bool Expired { get; set; }

    public Task(string _title, string _description, int _difficulty, DateTime? _lostTime = null)
    {
        Id = _seed;
        Title = _title;
        Description = _description;
        Xp = CalculateXp();
        Gold = CalculateGold();
        lostLife = CalculateLostLife();
        difficulty = checkDifficulty(_difficulty);
        ExpirationDate = _lostTime;
        IncraseSeed();
        Completed = false;
        Expired = false;
    }

    private static void IncraseSeed()
    {
        _seed++;
    }

    public override string ToString()
    {
        return $"Title: {Title},\nDescription: {Description},\nXp: {Xp},\nGold: {Gold},\nDifficulty: {difficulty}, LostLife: {ExpirationDate?.ToString() ?? "0"}";   
    }

    private int CalculateGold()
    {
        Random random = new Random();
        int _gold = random.Next(1, 5) * difficulty;
        return _gold;
    }

    private int CalculateXp()
    {
        Random random = new Random();
        int _xp = random.Next(5, 20) * difficulty;
        return _xp;
    }

    private int CalculateLostLife()
    {
        Random random = new Random();
        int _lostLife = random.Next(1, 5) * difficulty;
        return _lostLife;
    }

    private int checkDifficulty(int _difficulty)
    {
        if (_difficulty < 1 || _difficulty > 5)
        {
            throw new System.Exception("Difficulty must be between 1 and 5");
        }
        return _difficulty;
    }
}