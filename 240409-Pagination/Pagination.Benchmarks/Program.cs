// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using FilteringPagination;
using Pagination.Benchmarks;

DataHelpers.SeedDataIfEmpty();
BenchmarkRunner.Run<PaginationMethods>();