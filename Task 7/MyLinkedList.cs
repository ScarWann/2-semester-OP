using System.Collections;
using System.Collections.Generic;

namespace Task_7;

public class MyLinkedList : IEnumerable<int>
{
    public MyLinkedList(int value, MyLinkedList? list = default)
    {
        this.Value = value;
        this.Next = list;
    }

    public MyLinkedList(IEnumerable<int> values)
    {
        ArgumentOutOfRangeException.ThrowIfZero(values.Count());
        this.Value = values.First();
        if (values.Count() == 1)
        {
            this.Next = null;
        }
        else
        {
            this.Next = new MyLinkedList(values.Skip(1));
        }
    }

    public int Value { get; set; }

    public MyLinkedList? Next { get; set; }

    public int Length => this.Count();

    public int this[int i]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, this.Length);
            if (i == 0)
            {
                return this.Value;
            }
            else
            {
                ArgumentNullException.ThrowIfNull(this.Next);
                return this.Next[i - 1];
            }
        }

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, this.Length);
            if (i == 0)
            {
                this.Value = value;
            }
            else if (i == 1 && this.Next == null)
            {
                this.Next = new MyLinkedList(value);
            }
            else
            {
                ArgumentNullException.ThrowIfNull(this.Next);
                this.Next[i - 1] = value;
            }
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        MyLinkedList? temp = this;
        while (temp != null)
        {
            yield return temp.Value;
            temp = temp.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public void Prepend(int value)
    {
        this.Next = new MyLinkedList(this);
        this.Value = value;
    }

    public void Pop(int i)
    {
        ArgumentNullException.ThrowIfNull(this.Next);
        ArgumentOutOfRangeException.ThrowIfNegative(i);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, this.Length);
        if (i == 1 && this.Next.Next == null)
        {
            this.Next = null;
        }
        else if (i == 0)
        {
            this.Value = this.Next.Value;
            this.Next = this.Next.Next;
        }
        else
        {
            this.Next.Pop(i - 1);
        }
    }

    public override string ToString()
    {
        return string.Join(", ", this);
    }
}
