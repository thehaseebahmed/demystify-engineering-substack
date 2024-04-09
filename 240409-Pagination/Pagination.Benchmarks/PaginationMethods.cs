using BenchmarkDotNet.Attributes;
using FilteringPagination.Domain.Entities;
using FilteringPagination.Infrastructure.Repositories;
using Pagination.Benchmarks;

namespace FilteringPagination
{
    public class PaginationMethods
    {
        private readonly PostRepository _repository;
        private const int _pageSize = 10;

        public PaginationMethods()
        {
            _repository = new(DataHelpers.GetDbContext());
        }

        [Benchmark]
        public (Post[] data, int count) WithOffsetPaginationGetStartingPage()
        {
            var thirdPage = _pageSize * 2;
            return _repository.GetAll(thirdPage, _pageSize);
        }

        [Benchmark]
        public (Post[] data, int count) WithKeysetPaginationGetStartingPage()
        {
            const int secondPageLastItemId = _pageSize * 2;
            (DateTime lastDate, int lastId) cursor = new(
                DataHelpers.StartingDate.AddMinutes(secondPageLastItemId),
                secondPageLastItemId
                );

            return _repository.GetAll(cursor, _pageSize);
        }

        [Benchmark]
        public (Post[] data, int count) WithOffsetPaginationGetMiddlePage()
        {
            var middlePage = DataHelpers.TotalRecords / 2;
            return _repository.GetAll(middlePage, _pageSize);
        }

        [Benchmark]
        public (Post[] data, int count) WithKeysetPaginationGetMiddlePage()
        {
            const int middleItemId = DataHelpers.TotalRecords / 2;
            (DateTime lastDate, int lastId) cursor = new(
                DataHelpers.StartingDate.AddMinutes(middleItemId),
                middleItemId
                );

            return _repository.GetAll(cursor, _pageSize);
        }

        [Benchmark]
        public (Post[] data, int count) WithOffsetPaginationGetEndingPage()
        {
            var thirdLastPage = DataHelpers.TotalRecords - (_pageSize * 2);
            return _repository.GetAll(thirdLastPage, _pageSize);
        }

        [Benchmark]
        public (Post[] data, int count) WithKeysetPaginationGetEndingPage()
        {
            const int thirdLastPageFirstItemId = DataHelpers.TotalRecords - (_pageSize * 2);
            (DateTime lastDate, int lastId) cursor = new(
                DataHelpers.StartingDate.AddMinutes(thirdLastPageFirstItemId),
                thirdLastPageFirstItemId
                );

            return _repository.GetAll(cursor, _pageSize);
        }
    }
}
