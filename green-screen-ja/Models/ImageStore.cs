using System.Drawing;


namespace green_screen_ja.Models
{
    internal class ImageStore
    {
        public Bitmap InputImage { get; set; }

        public Bitmap OutputImage { get; set; }

        public byte[] Pixels { get; set; }

        public int GetInputHeight() => InputImage.Height;
        public int GetInputWidth() => InputImage.Width;
        public int GetPixelsSize() => Pixels.Length;
    }
}
