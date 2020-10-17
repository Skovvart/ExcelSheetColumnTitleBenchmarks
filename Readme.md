# 168. Excel Sheet Column Title
Reference: https://leetcode.com/problems/excel-sheet-column-title/

Given a positive integer, return its corresponding column title as appear in an Excel sheet.

For example:
```
    1 -> A
    2 -> B
    3 -> C
    ...
    26 -> Z
    27 -> AA
    28 -> AB 
    ...
```
**Example 1:**
```
Input: 1
Output: "A"
```
**Example 2:**
```
Input: 28
Output: "AB"
```
**Example 3:**
```
Input: 701
Output: "ZY"
```

## Complications
Maximum result size for integers is 7 characters, which is why my solution always allocates this.
Worst case is that 6*2 bytes are overallocated, and the annoying fact that they are zero-initialized by default.
Circumventing this requires the `SkipLocalsInitAttribute`, but that requires allowing unsafe code in the compiler.

This solution benchmarks the impact of not zero-allocating the stack in the algorithm as well as the overall performance. It also compares it to a (perhaps) slightly more intuitive string-allocation implementation.

## Results
As my implementation is pretty damn good (;)), the actual improvement of the algorithm by not zero-initilizing the result array is actually pretty impressive at about a 20-40% difference in performance based on how many of the result chars that are used. Can't get exact allocation math to make sense, but logically it should be about `2*sizeof(int) + 7*sizeof(char) + sizeof(char)*resultSize + Marshal.SizeOf(^1)` (`sizeof(int)=4`, `sizeof(char)=2`, `Marshal.SizeOf(^1)=4`)
That works out for `resultSize=7 => 40`, but not for `resultSize=1 => 28` or `resultSize=2 => 30`. Byte packing or something? I don't know ¯\\_(ツ)_/¯. Explanations are welcome.

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.572 (2004/?/20H1)
Intel Core i5-4590 CPU 3.30GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.302
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT


```
| Method |          N |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
| **Unsafe** |          **1** |  **13.955 ns** | **0.1806 ns** | **0.1689 ns** |  **0.59** |    **0.01** | **0.0076** |     **-** |     **-** |      **24 B** |
|   Safe |          1 |  23.724 ns | 0.5521 ns | 0.5907 ns |  1.00 |    0.00 | 0.0076 |     - |     - |      24 B |
| Simple |          1 |   7.460 ns | 0.2020 ns | 0.1889 ns |  0.31 |    0.01 | 0.0076 |     - |     - |      24 B |
|        |            |            |           |           |       |         |        |       |       |           |
| **Unsafe** |        **703** |  **18.340 ns** | **0.1470 ns** | **0.1375 ns** |  **0.69** |    **0.01** | **0.0102** |     **-** |     **-** |      **32 B** |
|   Safe |        703 |  26.666 ns | 0.2885 ns | 0.2698 ns |  1.00 |    0.00 | 0.0101 |     - |     - |      32 B |
| Simple |        703 |  53.079 ns | 0.7447 ns | 0.6966 ns |  1.99 |    0.03 | 0.0433 |     - |     - |     136 B |
|        |            |            |           |           |       |         |        |       |       |           |
| **Unsafe** | **2147483647** |  **27.579 ns** | **0.2924 ns** | **0.2735 ns** |  **0.80** |    **0.01** | **0.0127** |     **-** |     **-** |      **40 B** |
|   Safe | 2147483647 |  34.395 ns | 0.4257 ns | 0.3982 ns |  1.00 |    0.00 | 0.0127 |     - |     - |      40 B |
| Simple | 2147483647 | 146.812 ns | 0.7838 ns | 0.6949 ns |  4.27 |    0.05 | 0.1197 |     - |     - |     376 B |


For more details see sample `results` folder.

## Improvements
If I could find a formula to calculate the stack-size based on `n`, that would be preferable, but my math skills are too weak.

I know that we need one char up to 26<sup>1</sup>, two up to 26<sup>1</sup> + 26<sup>2</sup>, three up to 26<sup>1</sup> + 26<sup>2</sup> + 26<sup>3</sup>  etc.

log<sub>26</sub>(n) is close, but it's not taking everything into account (it returns 3 for 26<sup>3</sup> and not for the desired 26<sup>1</sup> + 26<sup>2</sup> + 26<sup>3</sup>).
Pull requests are welcome.