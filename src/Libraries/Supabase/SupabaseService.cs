using DotNetEnv;
using Supabase;
using SupabaseClient = Supabase.Client;

namespace Libraries.Supabase;

public class SupabaseService {
    SupabaseClient? client;

    public async  Task intialize() {
        try {
            Env.TraversePath().Load();

            var options = new SupabaseOptions {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            // var connection = new npgSq

            const string msg = "Failed to get the environmental value!";
            var supabaseUrl = Env.GetString("SUPABASE_URL", msg);
            var supabaseKey = Env.GetString("SUPABASE_KEY", msg);

            var newClient = new SupabaseClient(supabaseUrl, supabaseKey, options);
            await newClient.InitializeAsync();

            client = newClient;
        } catch (Exception ex) {
            throw new SupabaseException(
                "ERROR: Client initialization has failed!",
                ex.HResult,
                $"""
                Original Error Message: {ex.Message}
                Source: {ex.Source}
                Error-causing Method: {ex.TargetSite}
                Data: {ex.Data}
                Stacktrace: {ex.StackTrace}
                Exception Instance: {ex.InnerException}
                """);
        }
    }

    public SupabaseClient getClient()
        => client ?? throw new InvalidOperationException();
}
