namespace SpanTest;

public static class RefAndInTest
{
    public static unsafe void Test()
    {
        // Console.WriteLine(IntPtr.Size); // 64位系统8字节

        Console.WriteLine("------struct复制 慢------");
        var myStruct2 = new MyStruct2();
        // 结构进行了复制, 慢
        MyStruct2Test(myStruct2); // 输出1
        Console.WriteLine(myStruct2.Feild2); // 输出0

        Console.WriteLine("------class 快------");

        var myClass2 = new MyClass2();
        // 类没有进行复制
        MyClass2Test(myClass2); // 输出1
        Console.WriteLine(myClass2.Feild2); // 输出1

        Console.WriteLine("------ref函数参数 快 和 无防御性副本in一样快------");

        var myStruct21 = new MyStruct2();
        // 结构用ref不进行了复制 快
        MyStruct2TestRef(ref myStruct21); // 输出1
        Console.WriteLine(myStruct21.Feild2); // 输出1

        Console.WriteLine("------in 防御性副本 慢++------");

        var myStruct3 = new MyStruct3();
        MyStruct3* p = &myStruct3;
        // 输出的地址都是一样的
        Console.WriteLine("{0:x}", (int)p); // cd57e458 in 传入进去的
        // 因为内部有不是readonly属性（防御性副本）慢++
        // 1. 虽然输出的地址都是一样的，但是结构用in进行了复制，创建了防御性副本，影响效率，
        // 2. in 必须和readonly搭配使用不影响效率（1. readonly struct 2. 所有字段属性都是readonly的）
        MyStruct3TestIn(myStruct3); // 输出 cd57e458 \r\n 1
        // 加不加in都可以
        // MyStruct3TestIn(in myStruct3); // 输出1
        Console.WriteLine(myStruct3.Feild2); // 输出1
        Console.WriteLine(myStruct3.Feild2); // 输出2

        Console.WriteLine("------in readonly无防御性副本 快------");

        var myReadonlyStruct1 = new MyReadonlyStruct1();
        // readonly struct 无复制操作 快
        MyReadonlyStruct1TestIn(in myReadonlyStruct1); // 输出0
        Console.WriteLine(myReadonlyStruct1.Feild1); // 输出0
    }

    private static void MyStruct2Test(MyStruct2 myStruct2)
    {
        myStruct2.Feild2++;
        Console.WriteLine(myStruct2.Feild2);
    }

    private static void MyStruct2TestRef(ref MyStruct2 myStruct2)
    {
        myStruct2.Feild2++;
        Console.WriteLine(myStruct2.Feild2);
    }

    // 使用in就不能修改struct里面的值
    private static void MyStruct2TestIn(in MyStruct2 myStruct2)
    {
        // myStruct2.Feild2++;
        Console.WriteLine(myStruct2.Feild2);
    }

    private static void MyReadonlyStruct1TestIn(in MyReadonlyStruct1 myStruct2)
    {
        // myStruct2.Feild2++;
        Console.WriteLine(myStruct2.Feild1);
    }

    // 使用in就不能修改struct里面的值
    private static void MyStruct3TestIn(in MyStruct3 myStruct2)
    {
        unsafe
        {
            // fixed 语句可防止垃圾回收器重新定位可移动变量，并声明指向该变量的指针。 固定变量的地址在语句的持续时间内不会更改。 只能在相应的 fixed 语句中使用声明的指针。 声明的指针是只读的，无法修改
            fixed (MyStruct3* p = &myStruct2)
            {
                Console.WriteLine("{0:x}", (int)p);
            }
        }

        // myStruct2.Feild2++;
        // 不能修改struct里面的值，但是在定义里面自增了
        Console.WriteLine(myStruct2.Feild2);
    }

    private static void MyClass2Test(MyClass2 myStruct2)
    {
        myStruct2.Feild2++;
        Console.WriteLine(myStruct2.Feild2);
    }
}

public struct MyStruct1
{
    public readonly int Feild1;

    public int Property1 { get; }

    private int _feild2;
    private int _feild3;
    private int _feild4;
    private int _feild5;
    private int _feild6;

    public int Feild2
    {
        // readonly get => _feild2++; readonly get 内部不能修改_feild2的值
        // 对于class没有这个选项
        readonly get => _feild2;
        set => _feild2 = value;
    }

    // 对于class没有这个选项
    // readonly修饰函数名 内部变量不可修改
    public override readonly string ToString()
    {
        // Feild1++; 方法修饰符上有readonly的话，内部所有变量不能修改
        // Feild2++; 方法修饰符上有readonly的话，内部所有变量不能修改
        return base.ToString();
    }

    public override bool Equals(object? obj)
    {
        Feild2++; // 方法修饰符上没有readonly的话，内部所有变量能修改
        return base.Equals(obj);
    }
}

public struct MyStruct2
{
    private int _feild2;

    public int Feild2
    {
        // readonly get => _feild2++; readonly get 内部不能修改_feild2的值
        // 对于class没有这个选项
        get => _feild2;
        set => _feild2 = value;
    }
}

// 使用in 防御性副本
public struct MyStruct3
{
    private int _feild2;

    public int Feild2
    {
        // readonly get => _feild2++; readonly get 内部不能修改_feild2的值
        // 对于class没有这个选项
        get => ++_feild2;
        set => _feild2 = value;
    }
}

// 使用in 无防御性副本
// 结构是readonly的，里面所有的值都是readonly的
public readonly struct MyReadonlyStruct1
{
    // readonly struct 字段必须是readonly的
    public readonly int Feild1;

    // readonly struct 属性不能有set方法
    public int Property1 { get; }
}

public class MyClass2
{
    public readonly int Feild1;

    private int _feild2;

    public int Feild2
    {
        // readonly get => _feild2++; readonly get 内部不能修改_feild2的值
        // 对于class没有这个选项
        get => _feild2;
        set => _feild2 = value;
    }
}