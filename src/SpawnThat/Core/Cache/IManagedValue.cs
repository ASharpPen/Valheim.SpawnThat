namespace SpawnThat.Core.Cache;

internal interface IManagedValue
{
    void OnCreate();

    void OnDestroy();
}
