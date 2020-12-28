using System.Drawing;

namespace DetailingArsenal.Shared {
    public static class ImageUtils {
        static readonly ImageConverter converter = new ImageConverter();

        public static Image LoadFromBinary(byte[] data) {
            // https://stackoverflow.com/questions/3801275/how-to-convert-image-to-byte-array/16576471#16576471
            return (Image)converter.ConvertFrom(data);
        }

        public static byte[] ToBinary(Image i) {
            return (byte[])converter.ConvertTo(i, typeof(byte[]));
        }
    }
}