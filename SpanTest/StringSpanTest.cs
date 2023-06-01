using NPOI.SS.Formula.PTG;
using System.Runtime.InteropServices;
using System.Text;

namespace SpanTest;

public static class StringSpanTest
{
    public static void Test()
    {
        Console.WriteLine("===================== IndexOf 完全匹配 ========================");
        string str11 = "Hello, world!";
        Span<char> span1 = str11.ToCharArray();

        char[] searchChars = { 'o', 'w' };
        // IndexOf 完全匹配字符 IndexOfAny 匹配任意一个字符
        int index = span1.IndexOf(searchChars);

        if (index != -1)
        {
            Console.WriteLine($"First match found at index: {index}");
        }
        else
        {
            Console.WriteLine("No match found.");
        }

        Console.WriteLine("===================== IndexOfAny 匹配任意一个 =======================");

        index = span1.IndexOfAny(searchChars);

        if (index != -1)
        {
            Console.WriteLine($"First match found at index: {index}");
        }
        else
        {
            Console.WriteLine("No match found.");
        }


        unsafe
        {
            var str = "women我们😊🙄";

            Console.WriteLine("===================== span转string指针不是同一个 =======================");
            var onlySpan = str.AsSpan();
            fixed (char* c = &MemoryMarshal.GetReference(onlySpan))
            {
                Console.WriteLine("-- 原始指针 --");
                // Console.WriteLine($"0x{$"{(long)c:x}".PadLeft(16, '0')}"); // 使用PadLeft
                Console.WriteLine($"0x{(IntPtr)c:x16}"); // 使用IntPtr和long强转都可以 只不过IntPtr可以知道是不是64位系统
                fixed (char* ptr = str)
                {
                    Console.WriteLine($"0x{(long)ptr:x16}");
                }


                Console.WriteLine("-- new string(c, 0, onlySpan.Length) --");
                var s = new string(c, 0, onlySpan.Length);
                fixed (char* char2 = &MemoryMarshal.GetReference(s.AsSpan()))
                {
                    fixed (char* ptr1 = s)
                    {
                        Console.WriteLine($"0x{(IntPtr)ptr1:x16}");
                    }

                    Console.WriteLine($"0x{(long)char2:x16}");
                }

                Console.WriteLine("-- new string(onlySpan) --");
                var str1 = new string(onlySpan);
                fixed (char* char3 = &MemoryMarshal.GetReference(str1.AsSpan()))
                {
                    fixed (char* ptr2 = str1)
                    {
                        Console.WriteLine($"0x{(IntPtr)ptr2:x16}");
                    }

                    Console.WriteLine($"0x{(long)char3:x16}");
                }
            }


            Console.WriteLine("===================== Encoding.UTF8.GetBytes MemoryMarshal.Cast<char, byte> byte数组大小 =======================");
            Console.WriteLine(str);
            Console.WriteLine("-- 字符串长度 --");
            Console.WriteLine(str.Length);
            var bytes = Encoding.UTF8.GetBytes(str);
            Console.WriteLine("-- Encoding.UTF8.GetBytes长度 --");
            Console.WriteLine(bytes.Length);
            var span = MemoryMarshal.Cast<char, byte>(str);
            Console.WriteLine("-- MemoryMarshal.Cast<char, byte>长度 = 字符串长度*2 --");
            Console.WriteLine(span.Length);

            Console.WriteLine("===================== MemoryMarshal.Cast<char, byte>byte数组转回字符串 =======================");
            var readOnlySpan = MemoryMarshal.Cast<byte, char>(span);
            fixed (char* point = &MemoryMarshal.GetReference(readOnlySpan))
            {
                Console.WriteLine("-- 原始指针 --");
                Console.WriteLine($"0x{(long)point:x16}");
                fixed (char* ptr = str)
                {
                    Console.WriteLine($"0x{(long)ptr:x16}");
                }

                Console.WriteLine("-- new string(point, 0, readOnlySpan.Length) --");
                var s = new string(point, 0, readOnlySpan.Length);
                fixed (char* char1 = &MemoryMarshal.GetReference(s.AsSpan()))
                {
                    fixed (char* ptr1 = s)
                    {
                        Console.WriteLine($"0x{(long)ptr1:x16}");
                    }

                    Console.WriteLine($"0x{(long)char1:x16}");
                }

                Console.WriteLine(s);
            }
        }
    }
}