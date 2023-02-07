# Image Remove Background

This is an Azure Function that takes opaque images with white backgrounds and converts them to transparent images.

A Blob Trigger pointing to an existing Azure Storage Container is used to start the function. An output Blob is used to store the resulting transparent image.

## Setup

The blob trigger container name and blob output container name bindings can be changed in `Function.cs`.

## Example Input & Output

![alt text](https://github.com/Jason-nzd/ImageRemoveBackgroundFunction/blob/master/imgs/image-comparison.jpg?raw=true "Image Comparison")
