namespace Bonsai.Core
{
    [BonsaiNode("Composites/", "Arrow")]
    public class CustomSequence : CustomComposite
    {
        public override Status Run()
        {
            while (true)
            {
                if (!CompositeIterator.IsRunning)
                {
                    var child = CurrentChild();
                    if (child == null) return Status.Success;
                    CompositeIterator.Traverse(child);
                }
                CompositeIterator.Update();
                if (CompositeIterator.LastExecutedStatus != Status.Success)
                {
                    return CompositeIterator.LastExecutedStatus;
                }
            }
        }
    }
}