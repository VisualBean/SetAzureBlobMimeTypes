[![AppVeyor](https://img.shields.io/appveyor/ci/alexintime/setazureblobmimetypes.svg)](https://ci.appveyor.com/project/alexintime/setazureblobmimetypes)
[![GitHub](https://img.shields.io/github/license/VisualBean/SetAzureBlobMimeTypes.svg)](https://github.com/VisualBean/SetAzureBlobMimeTypes/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/SetBlobMimeTypes.svg)](https://www.nuget.org/packages/SetBlobMimeTypes)

# SetAzureBlobMimeTypes
A simple .Net Core Global tool for setting Content-Type of azure blob files.

## Installation
```
dotnet tools install --global SetBlobMimeTypes
```

## Usage
```
SetBlobMimeTypes -a <Storage Account Name> -k <Storage Account Key> -c <Container Name>
```
## MimeTypes
A list of supported MimeTypes be found here: [https://www.nuget.org/packages/SetBlobMimeTypes](https://www.nuget.org/packages/SetBlobMimeTypes)
