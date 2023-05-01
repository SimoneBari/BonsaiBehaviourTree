
using UnityEngine;

namespace Bonsai.Core
{
  public class BonsaiTreeComponent : MonoBehaviour
  {
    public void SetBlueprint(BehaviourTree blueprint) {
        treeInstance = BehaviourTree.Clone(blueprint);
        treeInstance.actor = gameObject;
        treeInstance.Start();
        treeInstance.BeginTraversal();
    }

    // Tree instance of the blueprint. This is a clone of the tree blueprint asset.
    // The tree instance is what runs in game.
    internal BehaviourTree treeInstance;
    
    public void Tick()
    {
      treeInstance.Update();
    }

    public bool IsRunning()
    {
      return treeInstance.IsRunning();
    }

    public void Reset()
    {
      treeInstance.Interrupt();
      treeInstance.Start();
      treeInstance.BeginTraversal();
    }
    
    void OnDestroy()
    {
      Destroy(treeInstance);
    }

    /// <summary>
    /// The tree instance running in game.
    /// </summary>
    public BehaviourTree Tree
    {
      get { return treeInstance; }
    }
  }
}