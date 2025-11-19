using Mongo2Go;

namespace HP.Clima.Infra.Services;

public class MongoInMemoryService : IDisposable
{
    private MongoDbRunner? _mongoDbRunner;
    public string ConnectionString { get; private set; } = string.Empty;

    public void Start()
    {
        _mongoDbRunner = MongoDbRunner.Start(
            singleNodeReplSet: false,
            additionalMongodArguments: "--quiet --logpath /dev/null"
        );
        ConnectionString = _mongoDbRunner.ConnectionString;
    }

    public void Stop()
    {
        _mongoDbRunner?.Dispose();
        _mongoDbRunner = null;
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}
