
using UnityEngine;

namespace Bonsai.Core
{
  /// <summary>
  /// The base class for all composite nodes.
  /// </summary>
  public abstract class CustomComposite : Composite
  {
    protected BehaviourIterator CompositeIterator;

    public override void OnStart()
    {
      CompositeIterator = new BehaviourIterator(Tree, levelOrder + 1);
      var count = ChildCount();
      for (int i = 0; i < count; i++)
      {
        foreach (var node in TreeTraversal.PreOrderSkipChildren(GetChildAt(i), n => n is ParallelComposite || n is CustomComposite))
        {
          node.Iterator = CompositeIterator;
        }
      }
    }

    public override void OnEnter()
    {
      CurrentChildIndex = 0;
      var next = CurrentChild();
      
      if (next)
      {
        CompositeIterator.Traverse(next);
      }
    }

    public override void OnExit()
    {
        if (CompositeIterator.IsRunning)
        {
          BehaviourTree.Interrupt(Children[CurrentChildIndex]);
        }
    }

    /// <summary>
    /// <para>Set the children for the composite node.</para>
    /// <para>This should be called when the tree is being built.</para>
    /// <para>It should be called before Tree Start() and never during Tree Update()</para>
    /// <note>To clear children references, pass an empty array.</note>
    /// </summary>
    /// <param name="nodes">The children for the node. Should not be null.</param>
    public override void SetChildren(BehaviourNode[] nodes)
    {
      children = nodes;
      // Set index orders.
      for (int i = 0; i < children.Length; i++)
      {
        children[i].indexOrder = i;
      }

      // Set parent references.
      foreach (BehaviourNode child in children)
      {
        child.Parent = this;
        child.Iterator = CompositeIterator;
      }
    }

    /// <summary>
    /// Called when a composite node has a child that activates when it aborts.
    /// </summary>
    /// <param name="child"></param>
    public override void OnAbort(int childIndex)
    {
      // The default behaviour is to set the current child index of the composite node.
      // TODO
      CurrentChildIndex = childIndex;
    }

    /// <summary>
    /// Default behaviour is sequential traversal from first to last.
    /// </summary>
    /// <returns></returns>
    public override void OnChildExit(int childIndex, Status childStatus)
    {
      CurrentChildIndex++;
      lastChildExitStatus = childStatus;
    }
  }
}