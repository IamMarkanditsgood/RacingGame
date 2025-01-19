public static class SaveManager
{
    public static ResourceSaveManager Resources { get; } = new ResourceSaveManager();
    public static PlayerPrefStorage PlayerPrefs { get; } = new PlayerPrefStorage();
    public static JsonStorage JsonStorage { get; } = new JsonStorage();
}