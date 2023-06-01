# Span<T> ReadonlySpan<T> 结构 ref readonly
---
- 提供任意内存连续区域的类型安全与内存安全表示形式
- NET Core 2.0 引入了 `Span<T>`和 `ReadOnlvSpan<T>`, 它们是可由托管或非托管内存提供支持的轻量级内存缓冲区。由于这些类型只能存诸在堆栈上
- 因此它们不适用于多种方案，包括异步方法调用
- `Span<T>`是在`stack`上而不是托管堆上分配的`ref struct`
- `ref struct`类型具有许多限制，实例是在堆栈上分配的，以确保它们无法转义到托管堆, 包括无法装箱、无法分配给类型`Object`，`dynamic`变量或`任何接口`类型
    1. `ref struct` 不能是数组的元素类型。
    2. `ref struct` 不能是类或非 `ref struct` 的字段的声明类型。
    3. `ref struct` 不能实现接口。
    4. `ref struct` 不能被装箱为 [System.ValueType](https://learn.microsoft.com/zh-cn/dotnet/api/system.valuetype) 或 [System.Object](https://learn.microsoft.com/zh-cn/dotnet/api/system.object)。
    5. `ref struct` 不能是类型参数。
    6. `ref struct` 变量不能由 [Lambda 表达式](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/lambda-expressions)或[本地函数](https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/local-functions)捕获。
    7. `ref struct` 变量不能在 [`async`](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/async) 方法中使用。 但是，可以在同步方法中使用 `ref struct` 变量，例如，在返回 [Task](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.tasks.task) 或 [Task](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.tasks.task-1) 的方法中。
    8. `ref struct` 变量不能在[迭代器](https://learn.microsoft.com/zh-cn/dotnet/csharp/iterators)中使用。
- `readonly struct` 没有防御性副本
- `readonly ref struct Span<T>` readonly 修饰符必须位于 ref 修饰符之前 有以上优点：				
	1. 结构是`readonly`的，里面所有的值都是`readonly`
	2. 结构是`ref`的，确保它们无法提升到托管堆, 包括无法装箱、无法分配给类型`Object`，`dynamic`变量或`任何接口`类型
- 它们不能是引用类型的字段，并且不能跨`await`或`yield`使用(跨函数调用)

# 如何编写出性能更好的代码
---
- C#提供了可编写性能更好的可验证安全代码的功能。`unsafe`
- 若仔细地应用这些技术，则需要不安全代码的方案更少(`span` `ref` `struct` `intptr(nint)`等)。
- 利用这些功能，可更轻易地将对值类型的引用用作方法参数和方法返回。安全完成后，这些技术可以最大程度地减少值类型的复制操作。通过使用值类型，可以使分配和垃圾回收过程的数量降至最低。
- 使用值类型的优点之一是通常可避免堆分配。缺点是它们按值进行复制。由于存在这种折衷，因此难以优化针对大量数据执行的算法。
---
- 这些技术在两个目标之间取得平衡
	1. 最大限度地减少堆上的分配
		- 属于引用类型的变量包含对内存中位置的引用，并且分配在托管堆上。将引用类型作为参数传递给方法或从方法返回时，将仅复制引用。每个新对象都需要一个新的分配，并且随后必须回收。垃圾回收需要一些时间。
	2. 最大限度地减少值的复制。`Intptr(64位系统 8字节指针)`，`int double long(4-16字节)`
		- 基本类型不需要`ref` 因为也会造成指针8字节浪费
		- 属于值类型的变量直接包含其值，通常在将值传递给方法或从方法返回时将其复制。此行为包括在调用迭代器和结构的异步实例方法时复制this的值。复制操作需要一些时间，具体取决于类型的大小。

# in函数参数修饰符 防御性副本
---
- 仅当使用`readonly`修饰符声明`struct`或方法仅访问该结构的`readonly`成员，才将其作为`in`参数传递。否则，编译器必须在许多情况下创建"防御性副本"以确保不会转变参数
- 输入参数作为副本保留

# sealed
---
- 应用于某个类时，`sealed`修饰符可阻止其他类继承自该类。在下面的示例中，类`B`继承自类`A`，但没有类可以继承自类`B`(类`B sealed`)
- `sealed`关键字只能修饰`类`
- `sealed`关键字作用是：如果确认类不会被继承，必须用这个，优点: 1. 减少多态，调用快 2. is as关键字转换快 3. 性能好不需要检查是不是有继承

# params
---
- 使用`params`关键字可以指定采用数目可变的参数的方法参数。参数类型必须是一维数组
	- `printf("sadasd",15,156,156) // 类似与C++`
- `params int[] xxx`
- 在方法声明中的`params`关键字之后不允许有任何其他参数，并且在方法声明中只充许有一个`params`关键字
- 使用params参数调用方法时，可以传入：
	- 数组元素类型的参数的逗号分隔列表
	- 指定类型的参数的数组
	- 无参数。如果未发送任何参数，则`params`列表的长度为零

# in out ref
---
- `in` 指定此参数由引用传递，但只由调用方法`读取`
	- 注意防御性副本(只能使用`readonly struct`和`struct成员全部reaonly`
- `ref`指定此参数由引用传递，可能由调用方法`读取或写入`
	- `ref in`可以用来减少传入值类型的拷贝消耗
	- `ref`与`cpp/c`dll通信中很常用`[Dlllmport][Librarylmport]`
- `out`指定此参数由引用传递，由调用方法`写入`
	- `Try+`方法非常常见(`int.Try`, `float.Try`)
	- `JsonElement j = JsonDocument.Parse("").RootElement;` `j.TryGetDateTime, j.TryGetUInt16 ...`
- 此外还有`return ref` `ref readonly`见`ReturnRefTest.cs`表示返回指针类似于C++中`int&`，`ref struct`等
- 并且不能跨`await`或`yield`使用
