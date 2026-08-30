[![](https://img.shields.io/nuget/v/soenneker.queues.intrusive.abstractions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.queues.intrusive.abstractions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.queues.intrusive.abstractions/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.queues.intrusive.abstractions/actions/workflows/publish-package.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.queues.intrusive.abstractions/build-and-test.yml?label=Build&style=for-the-badge)](https://github.com/soenneker/soenneker.queues.intrusive.abstractions/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/nuget/dt/soenneker.queues.intrusive.abstractions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.queues.intrusive.abstractions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.queues.intrusive.abstractions/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.queues.intrusive.abstractions/actions/workflows/codeql.yml)

# Soenneker.Queues.Intrusive.Abstractions

Defines the forward-link contract used by Soenneker intrusive queues and provides a reusable node base class.

An intrusive queue stores linkage on the queued object itself. This avoids allocating a separate wrapper node, but it also means the queue owns that link while the object is enqueued.

## Install

```bash
dotnet add package Soenneker.Queues.Intrusive.Abstractions
```

## Define a node

The simplest implementation derives from `IntrusiveNode<TNode>`:

```csharp
using Soenneker.Queues.Intrusive.Abstractions;

public sealed class WorkItem : IntrusiveNode<WorkItem>
{
    public required string Payload { get; init; }
}
```

Use `IIntrusiveNode<TNode>` directly when the type already has its own base class:

```csharp
public sealed class WorkItem : IIntrusiveNode<WorkItem>
{
    private WorkItem? _next;

    public ref WorkItem? Next => ref _next;
}
```

`Next` must return a reference to a real, stable field. Lock-free queue implementations use that storage with `Volatile` and `Interlocked`; a computed value or temporary does not satisfy the contract.

## Ownership rules

- Do not read or write `Next` while the node is owned by a queue.
- Do not enqueue the same node twice, concurrently, or into two intrusive structures at once.
- Reuse a node only after the owning queue has removed it.
- Expect queue implementations to clear `Next` during enqueue, dequeue, or reuse preparation.

Violating these rules can corrupt queue linkage. The abstractions intentionally do not add per-node state or runtime checks that would change the lock-free data structure’s cost model.
