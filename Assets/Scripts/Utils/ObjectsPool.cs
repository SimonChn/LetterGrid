using System;
using System.Collections.Generic;

public class ObjectsPool<T>
{
    private bool isDestroyed = false;
    private int capacity;

    private Queue<T> elements;

    private Func<T> CreateFunc;
    private Action<T> ActionOnGet;
    private Action<T> ActionOnRelease;

    public ObjectsPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int startCapacity)
    {
        CreateFunc = createFunc;
        ActionOnGet = actionOnGet;
        ActionOnRelease = actionOnRelease;

        capacity = startCapacity;

        elements = new Queue<T>();
        for (int i = 0; i < capacity; i++)
        {
            AddNewObject();
        }
    }

    public Queue<T> DestroySelf()
    {
        isDestroyed = true;

        CreateFunc = null;
        ActionOnGet = null;
        ActionOnRelease = null;

        return elements;
    }

    public bool TryToGetPoolElement(out T poolElement)
    {
        if (isDestroyed)
        {
            poolElement = default(T);
            return false;
        }
        else
        {

            poolElement = GetElement();
            return true;
        }
    }

    private T GetElement()
    {
        if (elements.Count < 1)
        {
            for (int i = 0; i < capacity; i++)
            {
                AddNewObject();
            }

            capacity *= 2;
        }

        var poolElement = elements.Dequeue();       
        ActionOnGet?.Invoke(poolElement);

        return poolElement;
    }

    public void Release(T poolElement)
    {
        ActionOnRelease?.Invoke(poolElement);
        elements.Enqueue(poolElement);
    }

    private void AddNewObject()
    {
        var newObj = CreateFunc();
        elements.Enqueue(newObj);
    }
}