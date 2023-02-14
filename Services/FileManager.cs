using tusdotnet.Interfaces;
using tusdotnet.Models.Configuration;
using tusdotnet.Models;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;

namespace WebAppTusDemo.Services
{
    public class FileManager
    {
        private readonly ILogger<FileManager> _logger;

        public FileManager(ILogger<FileManager> logger)
        {
            _logger = logger;
        }

        public async Task StoreTus(ITusFile file, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Debug, "Storing file", file.Id);
            using Stream content = await file.GetContentAsync(cancellationToken);
            Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(cancellationToken);
            string? filename = metadata.FirstOrDefault(m => m.Key == "filename").Value.GetString(System.Text.Encoding.UTF8);
            string? filetype = metadata.FirstOrDefault(m => m.Key == "filetype").Value.GetString(System.Text.Encoding.UTF8);
            bool isPublic = metadata.FirstOrDefault(m => m.Key == "ispublic").Value.GetString(System.Text.Encoding.UTF8) == "1" ? true : false;
            //await CreateAsync(new StoredFile { StoredFileId = file.Id, OriginalName = filename, Uploaded = DateTime.Now, ContentType = filetype });
            _logger.Log(LogLevel.Debug, "Stored file", filename, filetype);
        }

    }
}



