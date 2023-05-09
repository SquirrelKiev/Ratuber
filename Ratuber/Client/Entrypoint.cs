namespace Ratuber.Client
{
    public static class Entrypoint
    {
        static void Main()
        {
            using var client = new TubeClient();
            client.Run();
        }
    }
}
