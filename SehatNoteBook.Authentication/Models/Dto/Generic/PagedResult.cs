namespace SehatNoteBook.Authentication
{
    public class PagedResult<T> : Result<List<T>>
    {
            public int Page { get; set; }
            public int ResultsCount { get; set; }
            public int ResultsPerPage { get; set; }
    }
}
