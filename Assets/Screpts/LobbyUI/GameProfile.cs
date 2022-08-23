using PlayFab.ClientModels;

public static class GameProfile
{
    private static string _playerName;
    private static string _gunId;
    private static int _gunDamage;
    private static int _gunRange;
    private static int _maxZombies = 6;
    private static GameMode _gameMode;
    private static GetAccountInfoResult _result1;
    private static GetUserInventoryResult _result2;
    private static GetCatalogItemsResult _result3;
    private static bool _endGameFlag = false;

    public static string PlayerName { get => _playerName; set => _playerName = value; }
    public static string GunId { get => _gunId; set => _gunId = value; }
    public static int GunDamage { get => _gunDamage; set => _gunDamage = value; }
    public static int GunRange { get => _gunRange; set => _gunRange = value; }

    public static SubscriptionProperty<SubscriptionData> DamageForZombie = new SubscriptionProperty<SubscriptionData>();

    public static SubscriptionProperty<SubscriptionData> DamageForPlayer = new SubscriptionProperty<SubscriptionData>();

    public static SubscriptionProperty<bool> FlagGameOff = new SubscriptionProperty<bool>();
    public static int MaxZombies { get => _maxZombies; set => _maxZombies = value; }
    public static GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public static GetAccountInfoResult ResultGetAccountInfo { get => _result1; set => _result1 = value; }
    public static GetUserInventoryResult ResultGetUserInventory { get => _result2; set => _result2 = value; }
    public static GetCatalogItemsResult ResultGetCatalogItems { get => _result3; set => _result3 = value; }
    public static bool EndGameFlag { get => _endGameFlag; set => _endGameFlag = value; }
}
