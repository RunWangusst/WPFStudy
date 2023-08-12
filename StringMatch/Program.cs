// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using StringMatch;

Console.WriteLine("Hello, World!");

string str = "Hello, world! # @";

var a = BenchmarkRunner.Run<TestRegex>();


