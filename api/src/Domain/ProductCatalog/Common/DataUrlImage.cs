using System.Text.RegularExpressions;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// DataURL encoded image from the front end.
    /// </summary>
    public class DataUrlImage : IDataTransferObject {
        public string Name { get; set; } = null!;
        public string Data { get; set; } = null!;

        public DataUrlImage() { }

        public DataUrlImage(string name, byte[] data) {
            Name = name;
            Data = $"data:image/;base64,{System.Convert.ToBase64String(data)}";
        }

        public byte[] ToBinary() {
            // https://stackoverflow.com/questions/5714281/regex-to-parse-image-data-uri
            // can also access mime, or encoding data.
            var regex = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)", RegexOptions.Compiled);
            var match = regex.Match(Data);

            var base64Data = match.Groups["data"].Value;
            var binaryData = System.Convert.FromBase64String(base64Data);

            return binaryData;
        }
    }
}