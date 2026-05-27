namespace KarAfarin.Application.Common.Models.ResultOperation
{
    public enum ResultStatus
    {
        Success = 1,
        ValidationError = -1,
        DuplicateData = -2,
        NotFound = -3,
        Faild = -4,
        Unauthorized = -5,
    }
}
