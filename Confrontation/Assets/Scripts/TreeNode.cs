using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeNode<T> : MonoBehaviour
{
    [SerializeField] private T _value;
    [SerializeField] private List<TreeNode<T>> _children = new List<TreeNode<T>>();

    private void Awake()
    {
        foreach (var c in _children) 
            c.Parent = this;
    }

    public TreeNode<T> Parent { get; private set; }

    public T Value => _value;

    public List<TreeNode<T>> Children => _children;

    public void Traverse(Action<T> action)
    {
        action(Value);
        foreach (var child in _children)
            child.Traverse(action);
    }
    
    public void Traverse(Action<TreeNode<T>> action)
    {
        action(this);
        foreach (var child in _children)
            child.Traverse(action);
    }
    
    public TreeNode<T> FindNode(Predicate<T> predicate)
    {
        return predicate(Value) ? 
            this : 
            _children.Select(c => c.FindNode(predicate)).FirstOrDefault();
    }
}