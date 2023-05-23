using System.Collections.Generic;

public class LocalDataContainer
{
    public Dictionary<string, System.Object> Data { get; private set; }

    public LocalDataContainer( Dictionary<string, System.Object> data )
    {
        Data = data;
    }
}