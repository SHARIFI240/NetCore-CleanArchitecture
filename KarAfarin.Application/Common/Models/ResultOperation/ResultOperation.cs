namespace KarAfarin.Application.Common.Models.ResultOperation
{
    public class ResultOperation
    {
        public string Message { get; init; }
        public object? Data { get; init; }
        public ResultStatus Status { get; set; }

        #region Success
        public static ResultOperation Ok(object? data, string message = null)
        => new()
        {
            Data = data,
            Status = ResultStatus.Success,
            Message = message
        };

        #endregion

        #region Fail

        public static ResultOperation Fail(
        string message)
        => new()
        {
            Status = ResultStatus.Faild,
            Message = message,
        };

        #endregion

        #region Validation Error

        public static ResultOperation ValidationError(
            string? message)
            => new()
            {
                Status = ResultStatus.ValidationError,
                Message = message,
            };

        #endregion

        #region Duplicated Data

        public static ResultOperation Duplicate(
            string message)
            => new()
            {
                Status = ResultStatus.DuplicateData,
                Message = message
            };

        #endregion

        #region Not Found

        public static ResultOperation NotFound(
            string message = "Data not found")
            => new()
            {
                Status = ResultStatus.NotFound,
                Message = message
            };

        #endregion

        #region Unauthorized

        public static ResultOperation Unauthorized(
            string message)
            => new()
            {
                Status = ResultStatus.Unauthorized,
                Message = message
            };

        #endregion


    }
}
