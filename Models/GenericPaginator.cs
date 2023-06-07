namespace MVC.Models
{
    public class GenericPaginator<T> where T : class
    {
        public int PageNow { get; set; }
        public int RecordsPerPage{ get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
