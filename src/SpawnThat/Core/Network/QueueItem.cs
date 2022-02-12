namespace SpawnThat.Core.Network;

internal class QueueItem
{
    public ZPackage Package { get; set; }

    public ZRpc ZRpc { get; set; }

    public string Target { get; set; }

    public int Retries { get; set; }
}
