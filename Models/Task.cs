namespace AA1.Models;

public class Task
{
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
        Xp = CalculateXp(_difficulty);
        Gold = CalculateGold(_difficulty);
        lostLife = CalculateLostLife(_difficulty);
        difficulty = checkDifficulty(_difficulty);
        ExpirationDate = _lostTime;
        IncraseSeed();
        Completed = false;
        Expired = false;
    }

    public Task()
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
        return $"Title: {Title},\nDescription: {Description},\nDifficulty: {difficulty},\nExpiration Date: {ExpirationDate?.ToString() ?? "0"}";
    }

    private int CalculateGold(int difficulty = 1)
    {
        Random random = new Random();
        int _gold = random.Next(1, 5) * difficulty;
        return _gold;
    }

    private int CalculateXp(int difficulty = 1)
    {
        Random random = new Random();
        int _xp = random.Next(5, 20) * difficulty;
        return _xp;
    }

    private int CalculateLostLife(int difficulty = 1)
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