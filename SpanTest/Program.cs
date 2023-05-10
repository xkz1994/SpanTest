using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

uint[] array = { 1, 2, 3, 5, 3, 7, 0xFFFFFFFF };

// 1.
/*var span = MemoryMarshal.Cast<uint, byte>(array);

foreach (var item in span)
{
    Console.WriteLine(item);
}*/

// 2.
/*span[1] = 1;

foreach (var u in array)
{
    Console.WriteLine(u);
}*/

// 3.
/*var floatSpan = MemoryMarshal.Cast<uint, float>(array);

foreach (var item in floatSpan)
{
    Console.WriteLine(item);
}*/

// 4.
/*var list = new List<int>(10); // 倍增数组 log效率
Console.WriteLine(list.Capacity);
for (var i = 0; i < 11; i++)
{
    list.Add(i);
}

Console.WriteLine(list.Capacity);*/

// 5.
/*var list = new List<int>(10); // 倍增数组 log效率
Console.WriteLine(list.Capacity);
for (var i = 0; i < 10; i++)
{
    list.Add(0);
}

var sp2 = CollectionsMarshal.AsSpan(list);
for (var i = 0; i < 10; i++)
{
    list[i] = i;
}

Console.WriteLine("===========");
foreach (var item in sp2)
{
    Console.WriteLine(item);
}

Console.WriteLine("===========");
sp2[0] = 100;
foreach (var i in list)
{
    Console.WriteLine(i);
}

Console.WriteLine("===========");
Console.WriteLine(list.Capacity);
Console.WriteLine("===========");
list.Add(0); // 扩增后内部数组拷贝换了一个数组 但是sp2还是指向原来的数组 之前的数组会被释放 但是值还是那么些 没有被覆盖
foreach (var item in sp2)
{
    Console.WriteLine(item);
}*/

// var a = new Test();
// Console.WriteLine(a.Test2());
BenchmarkRunner.Run<Test>();

[MemoryDiagnoser]
public class Test
{
    // string 在堆中
    // span 只是指向string的一部分，而不是真正存在堆的内存。类似于安全指针
    private const string TestStr = "2010 02 27";

    private readonly List<int> list = new List<int>(10);

    public Test()
    {
        for (var i = 0; i < 10000; i++)
        {
            list.Add(i);
        }
    }

    // [Benchmark]
    public (int year, int month, int day) Test1()
    {
        var year = int.Parse(TestStr.Substring(0, 4));
        var month = int.Parse(TestStr.Substring(5, 2));
        var day = int.Parse(TestStr.Substring(8, 2));

        return (year, month, day);
    }

    // [Benchmark]
    public (int year, int month, int day) Test2()
    {
        ReadOnlySpan<char> span = TestStr;
        var year = int.Parse(span.Slice(0, 4));
        var month = int.Parse(span.Slice(5, 2));
        var day = int.Parse(span.Slice(8, 2));
        return (year, month, day);
    }

    [Benchmark]
    public int Test3()
    {
        var sp2 = CollectionsMarshal.AsSpan(list);
        var sum = 0;
        for (var i = 0; i < sp2.Length; i++)
        {
            sum += sp2[i];
        }

        return sum;
    }

    [Benchmark]
    public int Test4()
    {
        // var sp2 = CollectionsMarshal.AsSpan(list);
        var sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }

        return sum;
    }
}