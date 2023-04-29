
using UnityEngine;

namespace Bonsai.Core
{
  public class BonsaiTreeComponent : MonoBehaviour
  {
    /// <summary>
    /// The tree blueprint asset used.
    /// </summary>
    public BehaviourTree TreeBlueprint;

    // Tree instance of the blueprint. This is a clone of the tree blueprint asset.
    // The tree instance is what runs in game.
    internal BehaviourTree treeInstance;

    void Start()
    {
      treeInstance = BehaviourTree.Clone(TreeBlueprint);
      treeInstance.actor = gameObject;
      treeInstance.Start();
      treeInstance.BeginTraversal();
    }

    void Update()
    {
      treeInstance.Update();
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