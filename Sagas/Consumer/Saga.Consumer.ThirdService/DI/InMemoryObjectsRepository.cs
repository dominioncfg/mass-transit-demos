﻿namespace Saga.Consumer.ThirdService;

public class InMemoryObjectsRepository : IObjectsRepository
{
    private static readonly List<object> objects = new();

    public void Add(object o) => objects.Add(o);
}