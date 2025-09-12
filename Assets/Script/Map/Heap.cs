using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeap<T>
{ 
    T[] items;
     int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst(){
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get { return currentItemCount; }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);     
    }
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = (item.heapIndex * 2) + 1;
            int childIndexRight = (item.heapIndex * 2) + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while (item.heapIndex > 0)
        {
            T parent = items[parentIndex];
            if (item.CompareTo(parent) > 0)
            {
                swap(item, parent);
            }
            else
            {
                break;
            }
            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    void swap(T item1, T item2)
    {
        items[item1.heapIndex] = item2;
        items[item2.heapIndex] = item1;
        int item1Index = item1.heapIndex;
        item1.heapIndex = item2.heapIndex;
        item2.heapIndex = item1Index;
    }
}


public interface IHeap<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}
