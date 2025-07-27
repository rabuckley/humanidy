using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, DefaultConfig.Instance);
