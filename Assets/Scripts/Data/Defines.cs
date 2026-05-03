public enum DataType
{
    Cookie,
    Gear,
}

public enum Grade
{
    Common,
    Rare,
    Epic,
}

public static class Variables
{
    public static event System.Action OnLanguageChanged;
    private static DataType dataType = DataType.Cookie;

    public static DataType DataTypes
    {
        get
        {
            return dataType;
        }
        set
        {
            if (dataType == value)
            {
                return;
            }

            dataType = value;
            DataTableManager.ChangeDataType(dataType);
            OnLanguageChanged?.Invoke();
        }
    }


}

public static class DataTableIds
{
    public static readonly string[] StringTableIds =
    {
        "StringTableCookie",
        "StringTableGear",
    };

    public static string String => StringTableIds[(int)Variables.DataTypes];
    public static readonly string Gear = "GearTable";
    public static readonly string Cookie = "CookieTable";

}