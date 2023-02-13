namespace TMP_API.Helpers;


public class ApiResponse
{
    public ApiResponse() => Success = false;

    public bool Success { get; set; }
    public string Message { get; set; }
    public string Reason { get; set; }
}

public class ApiResponse<T>
{
    public ApiResponse() => Success = false;

    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class ApiPaginatedResponse<T>
{
    public int TotalCount { get; set; }
    public int Limit { get; set; }
    public int Current_Page { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
}


public struct ResponseMessages
{
    public static string BadRequest
    {
        get { return "400 Bad request."; }
    }

    public static string EntityNotFound
    {
        get { return "404 Not found."; }
    }

    public static string Created
    {
        get { return "Record(s) Created Successfully."; }
    }
    
    public static string Updated
    {
        get { return "Record(s) Updated Successfully."; }
    }
    
    public static string Deleted
    {
        get { return "Record Deleted Successfully."; }
    }
    
    public static string NoRecordFound
    {
        get { return "No Record Found."; }
    }
    
    public static string Exist
    {
        get { return "Record Already Exist."; }
    }

    public static string InternalServerError
    {
        get
        {
            return "Sorry, something went wrong while processing your request! " +
                        "We've noted it and we are going to fix this asap. " +
                        " Please try again later or contact support@tmp.com";
        }
    }
}

