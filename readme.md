# Image Transparent Func

This is an Azure Serverless Function that takes opaque images with white backgrounds and converts them to transparent images. Image Magick is used for image processing and the default output is tranparent WEBP format.

A Blob Trigger pointing to an existing Azure Storage Container is used to start the function. This is triggered when new images are placed in the container.

A Blob Output is used to store the resulting transparent image in a different container.

## Setup

The blob trigger binding and blob output binding can be changed in `BlobTrigger.cs`.

```c#
[BlobTrigger("containerNameToWatch/{name}")] Stream original,

[Blob("transparentImagesGoHere/{name}", FileAccess.Write)] BlockBlobClient outClient,
```

`local.settings.json` should contain at least:

```json
{
  "Values": {
    "AzureWebJobsStorage": "<azure storage connection string>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

## Example Input & Output

![alt text](https://github.com/Jason-nzd/ImageTransparentFunc/blob/master/images/image-comparison.jpg?raw=true "Image Comparison")
