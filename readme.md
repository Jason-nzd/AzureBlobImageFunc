# Azure Blob Image Func

This is an Azure Serverless Function that takes opaque images with white backgrounds and converts them to transparent images.

A Blob Trigger pointing to an existing Azure Storage Container is used to trigger the function. It is triggered when new images are placed in the container.

A Blob Output is used to store the resulting transparent image in a different container.

ImageMagick is used for image processing and the default output is a transparent 180x180 WebP format.

## Setup

The blob trigger binding and blob output binding can be changed in `BlobTrigger.cs`.

```c#
[BlobTrigger("trigger-container/{name}")] Stream original,

[Blob("processed-container/{name}", FileAccess.Write)] BlockBlobClient outClient,
```

The function can run locally using Azure Function Tools or deployed to an existing Azure Function using the .NET 6 runtime.

## Example Input & Output

![alt text](https://github.com/Jason-nzd/ImageTransparentFunc/blob/master/images/image-comparison.jpg?raw=true "Image Comparison")
