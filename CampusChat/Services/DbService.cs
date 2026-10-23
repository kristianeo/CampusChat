using System.Text.Json;

namespace CampusChat.Services;

public interface IEntity
{
    string Id { get; }
}

public abstract class DbService<T>
    where T : IEntity
{
    protected abstract string FilePath { get; }

    protected List<T> Load()
    {
        if (!File.Exists(FilePath))
            return [];

        string json = File.ReadAllText(FilePath);

        return JsonSerializer.Deserialize<List<T>>(json)
               ?? [];
    }

    protected void Save(List<T> items)
    {
        File.WriteAllText(
            FilePath,
            JsonSerializer.Serialize(
                items,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
    }

    protected T? Get(string id)
    {
        return Load().FirstOrDefault(x => x.Id == id);
    }

    protected void Add(T item)
    {
        List<T> items = Load();

        int index = items.FindIndex(x => x.Id == item.Id);

        if (index >= 0)
            items[index] = item;
        else
            items.Add(item);

        Save(items);
    }
    protected void Delete(string id)
    {
        List<T> items = Load();

        items.RemoveAll(x => x.Id == id);

        Save(items);
    }
}