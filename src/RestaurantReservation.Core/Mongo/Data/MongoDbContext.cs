﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestaurantReservation.Core.Mongo.Exceptions;

namespace RestaurantReservation.Core.Mongo.Data;

public class MongoDbContext : IMongoDbContext
{
    public IClientSessionHandle? Session { get; set; }
    public IMongoDatabase Database { get; }
    public IMongoClient MongoClient { get; }
    protected readonly IList<Func<Task>> commands;

    protected MongoDbContext(IOptions<MongoOptions> options)
    {
        this.MongoClient = new MongoClient(options.Value.ConnectionString);
        var databaseName = options.Value.DatabaseName;
        this.Database = this.MongoClient.GetDatabase(databaseName);

        // Every command will be stored and it'll be processed at SaveChanges
        this.commands = new List<Func<Task>>();
    }

    protected MongoDbContext(string? connectionString, string? databaseName)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new NullReferenceException(nameof(connectionString));
        if (string.IsNullOrEmpty(databaseName)) throw new NullReferenceException(nameof(databaseName));

        this.MongoClient = new MongoClient(connectionString);
        this.Database = this.MongoClient.GetDatabase(databaseName);

        this.commands = new List<Func<Task>>();
    }

    public IMongoCollection<T> GetCollection<T>(string? name = null)
    {
        if (!string.IsNullOrEmpty(name) && !this.CollectionExists(name)) throw new CollectionNameDoesNotExist(name);
        return this.Database.GetCollection<T>(name ?? typeof(T).Name.ToLower());
    }

    public void Dispose()
    {
        while (this.Session is { IsInTransaction: true })
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = this.commands.Count;

        using (this.Session = await this.MongoClient.StartSessionAsync(cancellationToken: ct))
        {
            this.Session.StartTransaction();

            try
            {
                var commandTasks = this.commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await this.Session.CommitTransactionAsync(ct);
            }
            catch (Exception)
            {
                await this.Session.AbortTransactionAsync(ct);
                this.commands.Clear();
                throw;
            }
        }

        this.commands.Clear();
        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        this.Session = await this.MongoClient.StartSessionAsync(cancellationToken: ct);
        this.Session.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (this.Session is { IsInTransaction: true })
            await this.Session.CommitTransactionAsync(ct);

        this.Session?.Dispose();
    }

    public async Task RollbackTransaction(CancellationToken ct = default)
    {
        await this.Session?.AbortTransactionAsync(ct)!;
    }

    public void AddCommand(Func<Task> func)
    {
        this.commands.Add(func);
    }

    public async Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken ct = default)
    {
        await this.BeginTransactionAsync(ct);
        try
        {
            await action();

            await this.CommitTransactionAsync(ct);
        }
        catch
        {
            await this.RollbackTransaction(ct);
            throw;
        }
    }

    public async Task<T> ExecuteTransactionalAsync<T>(
        Func<Task<T>> action,
        CancellationToken ct = default)
    {
        await this.BeginTransactionAsync(ct);
        try
        {
            var result = await action();

            await this.CommitTransactionAsync(ct);

            return result;
        }
        catch
        {
            await this.RollbackTransaction(ct);
            throw;
        }
    }

    private bool CollectionExists(string collectionName) =>
        this.Database.ListCollectionNames().ToList().Contains(collectionName);
}
