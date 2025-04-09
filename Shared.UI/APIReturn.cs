namespace Shared.UI
{
    public class APIReturn<T>
    {
        public APIReturn(int count, T data)
        {
            Count = count;
            Data = data;
            Errors = new List<Error>();

        }

        public APIReturn()
        {
            Errors = new List<Error>();
        }


        public int Count { get; set; }

        public T Data { get; set; }

        public List<Error> Errors { get; set; }
        public List<string> ErrorMessages { get { return Errors.Select(x => x.ToString()).ToList(); } }

        public int PageCount { get; set; }
        public string SuccessMessage { get; set; }


        public APIReturn(int count, T data, List<Error> errors)
        {
            Count = count;
            Data = data;
            Errors = new List<Error>();
            Errors.AddRange(errors.ToList());
        }
        public string GetErrorsForHtml()
        {
            return string.Join("<br/>", Errors.Select(x => x.Message));
        }

        public bool HasErrors => Errors?.Count > 0;
    }
}
