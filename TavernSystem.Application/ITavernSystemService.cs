using TavernSystem.Models;

namespace TavernSystem.Application;

public interface ITavernSystemService
{
    IEnumerable<Adventurer> GetAdventurers();
    Adventurer? GetAdventurer(int id);
    bool AddAdventurer(Adventurer adventurer);
}