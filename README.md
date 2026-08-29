[![](https://img.shields.io/nuget/v/soenneker.queues.intrusive.abstractions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.queues.intrusive.abstractions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.queues.intrusive.abstractions/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.queues.intrusive.abstractions/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.queues.intrusive.abstractions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.queues.intrusive.abstractions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.queues.intrusive.abstractions/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.queues.intrusive.abstractions/actions/workflows/codeql.yml)

# Soenneker.Queues.Intrusive.Abstractions

Defines the intrusive linkage required by an intrusive node. Implementations must provide stable storage for a single forward link that can be accessed by reference to support lock-free publication using `System.Threading.Volatile` and `System.Threading.Interlocked`.

## Install

```bash
dotnet add package Soenneker.Queues.Intrusive.Abstractions
```

## What you get

- `IIntrusiveNode<TNode>` — Defines the intrusive linkage required by an intrusive node. Implementations must provide stable storage for a single forward link that can be accessed by reference to support lock-free publication using `System.Threading.Volatile` and `System.Threading.Interlocked`.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IIntrusiveNode<TNode>.Next` | Gets a reference to the next node in the intrusive structure. | This must return a reference to the underlying storage location so that lock-free algorithms can safely perform atomic and volatile operations on it. |

## Important behavior

- `IIntrusiveNode<TNode>`: Intrusive contract: The returned reference must point to real, stable storage (typically a field), not a computed or temporary value. The `Next` link is owned by the intrusive data structure while the node is enqueued and must not be modified by user code during that time. A node must not be enqueued more than once concurrently or while it is already part of any intrusive structure. Implementations should assume the structure may set `Next` to `null` during enqueue/dequeue and when preparing a node for reuse.
- `IIntrusiveNode<TNode>.Next`: This must return a reference to the underlying storage location so that lock-free algorithms can safely perform atomic and volatile operations on it.
