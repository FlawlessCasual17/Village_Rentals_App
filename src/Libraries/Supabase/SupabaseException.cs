namespace Libraries.Supabase;

public class SupabaseException(
    string message,
    int errorCode = 0,
    string errorDetails = ""
) : Exception(message) {
    public int ErrorCode { get; init; } = errorCode;
    public string ErrorDetails { get; init; } = errorDetails;
}

// public class SupabaseFetchException(
//     string message,
//     int errorCode = 0,
//     string errorDetails = ""
//     ) : SupabaseException(
//         $"Supabase Fetch Error: {message}", errorCode, errorDetails);
//
// public class SupabaseDatabaseException(
//     string message,
//     int errorCode = 0,
//     string errorDetails = ""
//     ) : SupabaseException(
//         $"Supabase Database Error: {message}", errorCode, errorDetails);
//
// public class SupabaseAuthException(
//     string message,
//     int errorCode = 0,
//     string errorDetails = ""
//     ) : SupabaseException(
//         $"Supabase Authentication Error: {message}", errorCode, errorDetails);