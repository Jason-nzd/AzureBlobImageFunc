using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ImageMagick;
using Azure.Storage.Blobs.Specialized;

public class ImageTransparentFunc
{
    [FunctionName("ImageTransparentFunc")]
    public static Task Run(
        // Trigger off opaque images placed into /countdownimages/ container
        [BlobTrigger("countdownimages/{name}")] Stream original,

        // Place processed images into /transparent-cd-images/ container
        [Blob("temp-images/{name}", FileAccess.Write)] BlockBlobClient outClient,

        string name,
        ILogger log)
    {
        // Check if transparent file already exists
        bool transparentImageAlreadyExists = outClient.Exists();

        // If image already exists we can end the task here
        const bool forceOverwriteImages = false;
        if (transparentImageAlreadyExists && !forceOverwriteImages)
        {
            log.LogInformation(name + " already has a transparent image processed");
            return Task.CompletedTask;
        }

        // Create write stream from client
        Stream outStream = outClient.OpenWrite(true);

        try
        {
            // Load into image magick
            using (var image = new MagickImage(original))
            {
                // The following converts white pixels into transparent pixels, with a fuzz of 3
                image.ColorFuzz = new Percentage(3);
                image.Alpha(AlphaOption.Set);
                image.BorderColor = MagickColors.White;
                image.Border(1);
                image.Settings.FillColor = MagickColors.Transparent;
                image.FloodFill(MagickColors.Transparent, 1, 1);
                image.Shave(1, 1);

                // Sets the output format to webp
                image.Quality = 60;
                image.Format = MagickFormat.WebP;

                // Write the image to azure storage blob
                image.Write(outStream);
                outStream.Dispose();

                // If image conversion is successful, log message
                long imageByteLength = outClient.OpenRead().Length;
                log.LogWarning("Image: " + name + " - width: " + image.Width +
                 "px - output size: " + formatBytesToString(imageByteLength));
            }
        }
        catch (System.Exception e)
        {
            log.LogError("Unable to process image: " + name + "\nError: " + e.ToString());
            throw;
        }
        return Task.CompletedTask;
    }

    // Takes a byte length such as 38260 and returns a nicer string such as 38 kb
    private static string formatBytesToString(long byteLength)
    {
        string longString = byteLength.ToString();
        if (byteLength < 1) return "0 kb";
        if (byteLength > 1 && byteLength < 1000) return "1 kb";
        if (byteLength > 1000 && byteLength < 1000000) return longString.Substring(0, longString.Length - 3) + " kb";
        else return longString.Substring(0, longString.Length - 6) + " mb";
    }
}
