namespace Shared.Domain
{
    public class ServiceReturn<TData>
    {
        public enum ConstructorErrors
        {
            NotFound,
            Error,
        }

        public ServiceReturn(int count, TData data)
        {
            Count = count;
            Data = data;
            Errors = new List<Error>();
        }

        public ServiceReturn(int count, TData data, List<Error> errors)
        {
            Count = count;
            Data = data;
            Errors = new List<Error>();
            Errors.AddRange(errors.ToList());
        }

        public int PageCount { get; set; }
        public int Count { get; set; }

        public ServiceReturn()
        {
            Errors = new List<Error>();
        }

        public ServiceReturn(List<Error> errors)
        {
            Errors = errors.ToList();
        }

        public ServiceReturn(ConstructorErrors errorType)
        {
            if (Errors == null)
                Errors = new List<Error>();
            switch (errorType)
            {
                case ConstructorErrors.NotFound:
                    AddNotFoundError();
                    break;
                case ConstructorErrors.Error:
                    AddTechnicalError();
                    break;
                default:
                    break;
            }
        }

        public ServiceReturn(ConstructorErrors errorType, TData data)
        {
            if (Errors == null)
                Errors = new List<Error>();
            switch (errorType)
            {
                case ConstructorErrors.NotFound:
                    Data = data;
                    AddNotFoundError();
                    break;
                case ConstructorErrors.Error:
                    Data = data;
                    AddTechnicalError();
                    break;
                default:
                    break;
            }
        }

        public TData Data { get; set; }
        public List<Error> Errors { get; set; }
        public string SuccessMessage { get; set; } 

        public bool HasErrors => Errors.Count > 0;

        public void AddError(Error error)
        {
            Errors.Add(error);
        }

        public void AddError(string error)
        {
            Errors.Add(new Error(error));
        }

        public void AddTechnicalError()
        {
            Errors.Add(new Error("خطأ أثناء تنفيذ العملية"));
        }

        public void AddTechnicalDeleteError()
        {
            Errors.Add(new Error("خطأ أثناء حذف البيانات"));
        }

        public void AddTechnicalInsertError()
        {
            Errors.Add(new Error("خطأ أثناء إضافة البيانات"));
        }

        public void AddTechnicalUpdateError()
        {
            Errors.Add(new Error("خطأ أثناء تحديث البيانات"));
        }

        public void AddfiredError()
        {
            Errors.Add(new ServiceError("هذا الطالب مفصول و لا يحق له التسجيل "));
        }

        public void AddGraduateError()
        {
            Errors.Add(new ServiceError("هذا الطالب قد تخرج و لا يحق له التسجيل "));
        }

        public void AddRegistrationDateError()
        {
            Errors.Add(new ServiceError("لم يفتح التسجيل "));
        }

        public void AddErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                AddError(error);
            }
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                AddError(error);
            }
        }

        public void AddErrors(List<Error> errors)
        {
            foreach (var error in errors)
            {
                AddError(error);
            }
        }

        public void AddNotFoundError()
        {
            Errors.Add(new Error("لا توجد بيانات"));
        }

        public string GetUIErrors()
        {
            return string.Join("<br/>", Errors.Select(x => x.Message).ToArray());
        }

        public ServiceReturn<TViewModel> ToViewModel<TViewModel>(TViewModel viewModel)
        {
            var mutated = new ServiceReturn<TViewModel>(Errors)
            {
                Data = viewModel,
                Count = Count,
                SuccessMessage = SuccessMessage 
            };
            return mutated;
        }
    }

    public class Error
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public Error()
        {
        }

        public Error(string message)
        {
            Message = message;
        }
    }

    public class ServiceError : Error
    {
        public ServiceError(string message) : base(message)
        {
        }
    }
}
