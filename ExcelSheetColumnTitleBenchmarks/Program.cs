using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace ExcelSheetColumnTitleBenchmarks
{
    [MemoryDiagnoser]
    public class ChallengeBenchmark
    {
        [Params(1, 703, int.MaxValue)]
        public int N { get; set; }

        private readonly ExcelSheetColumnTitle _challenge = new ExcelSheetColumnTitle();

        [Benchmark]
        public string Unsafe() => _challenge.ComputeColumnTitleUnsafe(N);

        [Benchmark(Baseline = true)]
        public string Safe() => _challenge.ComputeColumnTitle(N);

        [Benchmark]
        public string Simple() => _challenge.ComputeColumnTitleSimple(N);
    }

    public static class Program
    {
        public static void Main() {
            var challenge = new ExcelSheetColumnTitle();
            Console.WriteLine($@"
Sample in/output:

1 => {challenge.ComputeColumnTitleUnsafe(1)}

26 => {challenge.ComputeColumnTitleUnsafe(26)}

701 => {challenge.ComputeColumnTitleUnsafe(701)}
702 => {challenge.ComputeColumnTitleUnsafe(702)}
703 => {challenge.ComputeColumnTitleUnsafe(703)}

26^4 + 26^3 + 26^2 + 26^1 = {challenge.ComputeColumnTitleUnsafe((int)Math.Pow(26, 4) + (int)Math.Pow(26, 3) + (int)Math.Pow(26, 2) + (int)Math.Pow(26, 1))}

{int.MaxValue} = {challenge.ComputeColumnTitleUnsafe(int.MaxValue)}
");
            BenchmarkRunner.Run<ChallengeBenchmark>();
        }
    }
}