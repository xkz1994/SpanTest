# Span<T> ReadonlySpan<T> �ṹ ref readonly
---
- �ṩ�����ڴ�������������Ͱ�ȫ���ڴ氲ȫ��ʾ��ʽ
- NET Core 2.0 ������ `Span<T>`�� `ReadOnlvSpan<T>`, �����ǿ����йܻ���й��ڴ��ṩ֧�ֵ��������ڴ滺������������Щ����ֻ�ܴ����ڶ�ջ��
- ������ǲ������ڶ��ַ����������첽��������
- `Span<T>`����`stack`�϶������йܶ��Ϸ����`ref struct`
- `ref struct`���;���������ƣ�ʵ�����ڶ�ջ�Ϸ���ģ���ȷ�������޷�ת�嵽�йܶ�, �����޷�װ�䡢�޷����������`Object`��`dynamic`������`�κνӿ�`����
    1. `ref struct` �����������Ԫ�����͡�
    2. `ref struct` ���������� `ref struct` ���ֶε��������͡�
    3. `ref struct` ����ʵ�ֽӿڡ�
    4. `ref struct` ���ܱ�װ��Ϊ [System.ValueType](https://learn.microsoft.com/zh-cn/dotnet/api/system.valuetype) �� [System.Object](https://learn.microsoft.com/zh-cn/dotnet/api/system.object)��
    5. `ref struct` ���������Ͳ�����
    6. `ref struct` ���������� [Lambda ���ʽ](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/lambda-expressions)��[���غ���](https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/local-functions)����
    7. `ref struct` ���������� [`async`](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/async) ������ʹ�á� ���ǣ�������ͬ��������ʹ�� `ref struct` ���������磬�ڷ��� [Task](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.tasks.task) �� [Task](https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.tasks.task-1) �ķ����С�
    8. `ref struct` ����������[������](https://learn.microsoft.com/zh-cn/dotnet/csharp/iterators)��ʹ�á�
- `readonly struct` û�з����Ը���
- `readonly ref struct Span<T>` readonly ���η�����λ�� ref ���η�֮ǰ �������ŵ㣺				
	1. �ṹ��`readonly`�ģ��������е�ֵ����`readonly`
	2. �ṹ��`ref`�ģ�ȷ�������޷��������йܶ�, �����޷�װ�䡢�޷����������`Object`��`dynamic`������`�κνӿ�`����
- ���ǲ������������͵��ֶΣ����Ҳ��ܿ�`await`��`yield`ʹ��(�纯������)

# ��α�д�����ܸ��õĴ���
---
- C#�ṩ�˿ɱ�д���ܸ��õĿ���֤��ȫ����Ĺ��ܡ�`unsafe`
- ����ϸ��Ӧ����Щ����������Ҫ����ȫ����ķ�������(`span` `ref` `struct` `intptr(nint)`��)��
- ������Щ���ܣ��ɸ����׵ؽ���ֵ���͵������������������ͷ������ء���ȫ��ɺ���Щ�����������̶ȵؼ���ֵ���͵ĸ��Ʋ�����ͨ��ʹ��ֵ���ͣ�����ʹ������������չ��̵�����������͡�
- ʹ��ֵ���͵��ŵ�֮һ��ͨ���ɱ���ѷ��䡣ȱ�������ǰ�ֵ���и��ơ����ڴ����������ԣ���������Ż���Դ�������ִ�е��㷨��
---
- ��Щ����������Ŀ��֮��ȡ��ƽ��
	1. ����޶ȵؼ��ٶ��ϵķ���
		- �����������͵ı����������ڴ���λ�õ����ã����ҷ������йܶ��ϡ�������������Ϊ�������ݸ�������ӷ�������ʱ�������������á�ÿ���¶�����Ҫһ���µķ��䣬������������ա�����������ҪһЩʱ�䡣
	2. ����޶ȵؼ���ֵ�ĸ��ơ�`Intptr(64λϵͳ 8�ֽ�ָ��)`��`int double long(4-16�ֽ�)`
		- �������Ͳ���Ҫ`ref` ��ΪҲ�����ָ��8�ֽ��˷�
		- ����ֵ���͵ı���ֱ�Ӱ�����ֵ��ͨ���ڽ�ֵ���ݸ�������ӷ�������ʱ���临�ơ�����Ϊ�����ڵ��õ������ͽṹ���첽ʵ������ʱ����this��ֵ�����Ʋ�����ҪһЩʱ�䣬����ȡ�������͵Ĵ�С��

# in�����������η� �����Ը���
---
- ����ʹ��`readonly`���η�����`struct`�򷽷������ʸýṹ��`readonly`��Ա���Ž�����Ϊ`in`�������ݡ����򣬱������������������´���"�����Ը���"��ȷ������ת�����
- ���������Ϊ��������

# sealed
---
- Ӧ����ĳ����ʱ��`sealed`���η�����ֹ������̳��Ը��ࡣ�������ʾ���У���`B`�̳�����`A`����û������Լ̳�����`B`(��`B sealed`)
- `sealed`�ؼ���ֻ������`��`
- `sealed`�ؼ��������ǣ����ȷ���಻�ᱻ�̳У�������������ŵ�: 1. ���ٶ�̬�����ÿ� 2. is as�ؼ���ת���� 3. ���ܺò���Ҫ����ǲ����м̳�

# params
---
- ʹ��`params`�ؼ��ֿ���ָ��������Ŀ�ɱ�Ĳ����ķ����������������ͱ�����һά����
	- `printf("sadasd",15,156,156) // ������C++`
- `params int[] xxx`
- �ڷ��������е�`params`�ؼ���֮���������κ����������������ڷ���������ֻ������һ��`params`�ؼ���
- ʹ��params�������÷���ʱ�����Դ��룺
	- ����Ԫ�����͵Ĳ����Ķ��ŷָ��б�
	- ָ�����͵Ĳ���������
	- �޲��������δ�����κβ�������`params`�б�ĳ���Ϊ��

# in out ref
---
- `in` ָ���˲��������ô��ݣ���ֻ�ɵ��÷���`��ȡ`
	- ע������Ը���(ֻ��ʹ��`readonly struct`��`struct��Աȫ��reaonly`
- `ref`ָ���˲��������ô��ݣ������ɵ��÷���`��ȡ��д��`
	- `ref in`�����������ٴ���ֵ���͵Ŀ�������
	- `ref`��`cpp/c`dllͨ���кܳ���`[Dlllmport][Librarylmport]`
- `out`ָ���˲��������ô��ݣ��ɵ��÷���`д��`
	- `Try+`�����ǳ�����(`int.Try`, `float.Try`)
	- `JsonElement j = JsonDocument.Parse("").RootElement;` `j.TryGetDateTime, j.TryGetUInt16 ...`
- ���⻹��`return ref` `ref readonly`��`ReturnRefTest.cs`��ʾ����ָ��������C++��`int&`��`ref struct`��
- ���Ҳ��ܿ�`await`��`yield`ʹ��
