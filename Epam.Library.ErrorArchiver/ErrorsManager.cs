using FluentValidation.Results;

namespace Epam.Library.ErrorArchiver
{
    public static class ErrorsManager
    {
        public static void AddFalseResponse(ref Response response, string errorMessage)
        {
            response.IsSuccess = false;
            response.ErrorCollection.Add(new Error(errorMessage));
        }
        public static void AddFalseResponse(ref Response response, ValidationResult validationResult)
        {
            response.IsSuccess = false;
            foreach (var error in validationResult.Errors)
            {
                response.ErrorCollection.Add(new Error(error.ErrorMessage));
            }
        }

        public static void AddTrueResponse(ref Response response, string errorMessage)
        {
            response.IsSuccess = true;
        }
    }
}
