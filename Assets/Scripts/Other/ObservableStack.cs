using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    /// <summary>
    /// Event that is raised when we push something
    /// </summary>
    public event UpdateStackEvent OnPush;

    /// <summary>
    /// Event that is raised when we pop something
    /// </summary>
    public event UpdateStackEvent OnPop;

    /// <summary>
    /// Event that is raised when we clear the stack
    /// </summary>
    public event UpdateStackEvent OnClear;

    public ObservableStack(ObservableStack<T> items) : base(items)
    {

    }

    public ObservableStack()
    {

    }

    public new void Push(T item)
    {
        base.Push(item);
        if (OnPush != null) OnPush();
    }

    public new T Pop()
    {
        T item = base.Pop();
        if (OnPop != null) OnPop();
        return item;
    }

    public new void Clear()
    {
        base.Clear();
        if (OnClear != null) OnClear();
    }
}
