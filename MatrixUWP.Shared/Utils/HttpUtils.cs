#nullable enable
namespace MatrixUWP.Shared.Utils
{
    public static class HttpUtils
    {
        public static MatrixJsonHttpRequestClient MatrixHttpClient = new MatrixJsonHttpRequestBuilder().Build();
    }
}
