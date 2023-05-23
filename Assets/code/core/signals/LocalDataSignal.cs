public struct LocalDataSignal
{
    public object Data { get; private set; }
    public string TypeName { get; private set; }
    public LocalDataState State { get; private set; }

    public LocalDataSignal( object data, LocalDataState state )
    {
        Data = data;
        TypeName = ( data != null ) ? data.GetType().Name : string.Empty;
        State = state;
    }
}