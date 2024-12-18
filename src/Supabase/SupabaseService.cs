using dotenv.net;
using Supabase;
using Env = dotenv.net.Utilities.EnvReader;
using SupabaseClient = Supabase.Client;

namespace src.Supabase;

public class SupabaseService {
    public SupabaseClient? Client { get; private set; }

    public async Task intializeService() {
        try {
            DotEnv.Load();

            var options = new SupabaseOptions {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            var url = Env.GetStringValue("SUPABASE_URL");
            var key = Env.GetStringValue("SUPABASE_ANON_KEY");

            var client = new SupabaseClient(url, key, options);
            await client.InitializeAsync();

            Client = client;
        } catch (Exception ex) {
            throw new SupabaseException(
                "ERROR: Client initialization has failed!",
                ex.HResult,
                $"""
                Original Error Message: {ex.Message}
                Source: {ex.Source}
                Error-causing Method: {ex.TargetSite}
                Exception Instance: {ex.InnerException}
                Data: {ex.Data}
                Stacktrace: {ex.StackTrace}
                """);
        }
    }
}
