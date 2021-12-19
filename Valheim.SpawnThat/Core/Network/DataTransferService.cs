using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Core.Network;

internal partial class DataTransferService : IDisposable
{
    private static DataTransferService _service;

    public static DataTransferService Service => _service ??= new();

    static DataTransferService()
    {
        StateResetter.Subscribe(() =>
        {
            _service = null;
        });
    }

    public Dictionary<string, Queue<QueueItem>> SocketQueues { get; private set; } = new();

    public Task AddToQueueAsync(Func<ZPackage> preparePackage, string rpcRoute, ZRpc zrpc)
    {
        var zpackage = preparePackage();

        AddToQueue(zpackage, rpcRoute, zrpc);

        return Task.CompletedTask;
    }

    public void AddToQueue(ZPackage package, string rpcRoute, long playerId)
    {
        var peers = ZNet.instance.GetConnectedPeers();
        var zrpc = peers.FirstOrDefault(x => x.m_uid == playerId)?.m_rpc;

        AddToQueue(package, rpcRoute, zrpc);
    }

    public void AddToQueue(ZPackage package, string rpcRoute, ZRpc zrpc)
    {
        try
        {
            var item = new QueueItem
            {
                Package = package,
                Target = rpcRoute,
                ZRpc = zrpc,
            };

            string queueIdentifier = item.ZRpc.GetSocket().GetEndPointString();
            lock (SocketQueues)
            {
                if (SocketQueues.TryGetValue(queueIdentifier, out var queue))
                {
                    queue.Enqueue(item);
                }
                else
                {
                    var newQueue = new Queue<QueueItem>();
                    newQueue.Enqueue(item);
                    SocketQueues[queueIdentifier] = newQueue;
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError("Failed to queue package.", e);
        }
    }

    public void Dispose()
    {
        SocketQueues = null;
    }
}
