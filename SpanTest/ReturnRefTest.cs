namespace SpanTest;

public static class ReturnRefTest
{
    public static void Test()
    {
        Console.WriteLine("------原多维数组------");
        // 2行 3列({ 1, 2, 3 }最后一个参数才是具体单数组个数)
        var array = new int[2, 3] { { 0, 0, 0 }, { 0, 0, 0 } };
        // 等价于 int[,] array = new int[2, 3];
        Print2DArray(ref array);
        /*
         * 0     0       0
         * 0       0       0
        */

        Console.WriteLine("------var tmp = Find(array, s => s >= 0) 不生效------");
        var tmp = Find(array, s => s >= 0);
        tmp = 999;
        Print2DArray(ref array);
        /*
         * 0     0       0
         * 0       0       0
        */


        Console.WriteLine("------ref var ok = ref Find(array, s => s >= 0) 生效------");
        ref var ok = ref Find(array, s => s >= 0);
        ok = 999;
        Print2DArray(ref array);
        /*
         * 999     0       0
         * 0       0       0
        */

        Console.WriteLine("------array[0, 0] = -1; ref var ok2 = ref Find(array, s => s >= 0) 生效------");
        array[0, 0] = -1;
        ref var ok2 = ref Find(array, s => s >= 0);
        ok2 = 999;
        Print2DArray(ref array);
        /*
         * -1      999     0
         * 0       0       0
        */
    }

    /// <summary>
    /// 返回int指针(ref int类型相当于获取当前int的指针可以用于修改)
    /// </summary>
    public static ref int Find(int[,] matrix, Func<int, bool> predicate)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                if (predicate(matrix[i, j]))
                {
                    return ref matrix[i, j];
                }
            }
        }

        throw new InvalidOperationException("Not found");
    }

    public static void Print2DArray<T>(ref T[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++) // GetLength(0)获取第一维长度 行数
        {
            for (var j = 0; j < matrix.GetLength(1); j++) // GetLength(1)获取第二维长度 列数 内部具体数组个数
            {
                Console.Write($"{matrix[i, j]}\t");
            }

            Console.WriteLine();
        }
    }
}