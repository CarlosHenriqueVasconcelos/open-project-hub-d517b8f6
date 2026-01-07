namespace PlataformaGestaoIA.ViewModel
{
    public class ResultViewModel<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(List<string> errors)
        {
            Errors = errors;
        }

        public ResultViewModel(string error)
        {
            Errors.Add(error);
        }

        public ResultViewModel(T data, int totalRecords, int totalPages, int currentPage, int pageSize, string error)
        {
            Data = data;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Errors.Add(error);
        }

        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new();
    }
}
