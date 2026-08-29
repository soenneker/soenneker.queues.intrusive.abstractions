namespace Soenneker.Queues.Intrusive.Abstractions;

/// <inheritdoc cref="IIntrusiveNode{TNode}"/>
/// <typeparam name="TNode">
/// The concrete node type. This is typically the deriving type itself (self-referential generic constraint).
/// </typeparam>
/// <remarks>
/// Intrusive contract:
/// <list type="bullet">
/// <item>A node must not be inserted while it is already part of any intrusive structure.</item>
/// <item>The <see cref="Next"/> link is owned and manipulated by the structure while the node is linked.</item>
/// <item>Nodes may be reused only after they are removed by the owning structure.</item>
/// </list>
/// </remarks>
public abstract class IntrusiveNode<TNode> : IIntrusiveNode<TNode>
    where TNode : class, IIntrusiveNode<TNode>
{
    private TNode? _next;

    public ref TNode? Next => ref _next;
}