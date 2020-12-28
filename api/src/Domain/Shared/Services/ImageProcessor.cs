using System;
using System.Drawing;
using System.Text.RegularExpressions;
using DetailingArsenal.Shared;

namespace DetailingArsenal.Domain.Shared {
    public interface IImageProcessor : IService {
        ProcessedImage Process(string fileName, string fileData);
    }

    public class ImageProcessor : IImageProcessor {
        const int ThumbnailSize = 100;

        public ProcessedImage Process(string fileName, string fileData) {
            Image full = LoadFromDataUrl(fileData);
            Image thumb = GenerateThumbnail(full);

            return new ProcessedImage(fileName, full.GetMimeType(), full, thumb);
        }

        Image LoadFromDataUrl(string data) {
            // https://stackoverflow.com/questions/5714281/regex-to-parse-image-data-uri
            // can also access mime, or encoding data.
            var regex = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)", RegexOptions.Compiled);
            var match = regex.Match(data);

            var base64Data = match.Groups["data"].Value;
            var binaryData = System.Convert.FromBase64String(base64Data);

            Image i = ImageUtils.LoadFromBinary(binaryData);
            return i;
        }

        Image GenerateThumbnail(Image full) {
            return full.GetThumbnailImage(ThumbnailSize, ThumbnailSize, () => false, IntPtr.Zero);
        }
    }
}