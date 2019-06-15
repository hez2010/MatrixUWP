using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace MatrixUWP.Utils
{
    class HttpJsonContent<T> : IHttpContent
    {
        private readonly T json;
        private string serializedJson = "";
        private string SerializedJson => string.IsNullOrEmpty(serializedJson) ? serializedJson = JsonSerializer.ToString(json) : serializedJson;

        public HttpContentHeaderCollection Headers { get; }

        public HttpJsonContent(T json)
        {
            this.json = json;
            Headers = new HttpContentHeaderCollection
            {
                ContentType = new HttpMediaTypeHeaderValue("application/json")
                {
                    CharSet = "UTF-8"
                }
            };
        }

        public IAsyncOperationWithProgress<ulong, ulong> BufferAllAsync()
        {
            return AsyncInfo.Run<ulong, ulong>((cancellationToken, progress) => Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var length = GetLength();

                // Report progress.
                progress.Report(length);

                // Just return the size in bytes.
                return length;
            }));
        }

        public IAsyncOperationWithProgress<IBuffer, ulong> ReadAsBufferAsync()
        {
            return AsyncInfo.Run<IBuffer, ulong>((cancellationToken, progress) => Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var writer = new DataWriter();
                writer.WriteString(SerializedJson);

                // Make sure that the DataWriter destructor does not free the buffer.
                var buffer = writer.DetachBuffer();

                // Report progress.
                progress.Report(buffer.Length);

                return buffer;
            }));
        }

        public IAsyncOperationWithProgress<IInputStream, ulong> ReadAsInputStreamAsync()
        {
            return AsyncInfo.Run<IInputStream, ulong>(async (cancellationToken, progress) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var randomAccessStream = new InMemoryRandomAccessStream();
                var writer = new DataWriter(randomAccessStream);
                writer.WriteString(SerializedJson);

                var bytesStored = await writer.StoreAsync().AsTask(cancellationToken);

                // Make sure that the DataWriter destructor does not close the stream.
                writer.DetachStream();

                // Report progress.
                progress.Report(randomAccessStream.Size);

                return randomAccessStream.GetInputStreamAt(0);
            });
        }

        public IAsyncOperationWithProgress<string, ulong> ReadAsStringAsync()
        {
            return AsyncInfo.Run<string, ulong>((cancellationToken, progress) => Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Report progress (length of string).
                progress.Report((ulong)SerializedJson.Length);

                return SerializedJson;
            }));
        }

        public bool TryComputeLength(out ulong length)
        {
            length = GetLength();
            return true;
        }

        public IAsyncOperationWithProgress<ulong, ulong> WriteToStreamAsync(IOutputStream outputStream)
        {
            return AsyncInfo.Run<ulong, ulong>(async (cancellationToken, progress) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var writer = new DataWriter(outputStream);
                writer.WriteString(SerializedJson);
                var bytesWritten = await writer.StoreAsync().AsTask(cancellationToken);

                // Make sure that DataWriter destructor does not close the stream.
                writer.DetachStream();

                // Report progress.
                progress.Report(bytesWritten);

                return bytesWritten;
            });
        }

        public void Dispose() { }

        private ulong GetLength()
        {
            var writer = new DataWriter();
            writer.WriteString(SerializedJson);

            var buffer = writer.DetachBuffer();
            return buffer.Length;
        }
    }
}
