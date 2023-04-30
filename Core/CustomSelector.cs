namespace Bonsai.Core
{
    [BonsaiNode("Composites/", "Question")]
    public class CustomSelector : CustomComposite
    {
        public override Status Run()
        {
            while (true)
            {
                if (!CompositeIterator.IsRunning)
                {
                    var child = CurrentChild();
                    if (child == null) return Status.Failure;
                    CompositeIterator.Traverse(child);
                }
                CompositeIterator.Update();
                if (CompositeIterator.LastExecutedStatus != Status.Failure)
                {
                    return CompositeIterator.LastExecutedStatus;
                }
            }
        }
    }
}