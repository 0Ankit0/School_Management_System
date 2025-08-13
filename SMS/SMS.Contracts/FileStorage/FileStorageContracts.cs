using FluentValidation;
using System;

namespace SMS.Contracts.FileStorage;

public class FileUploadRequest
{
    public string FileName { get; set; }
}

public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
    }
}

public class FileStorageResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public Guid UploadedByUserExternalId { get; set; }
    public DateTime UploadedAt { get; set; }
}