# API Contracts and Data Transfer Objects (DTOs)

**Document Version:** 1.2
**Date:** 2025-08-13
**Author:** Gemini CLI Agent

---

## 1. Introduction

This document outlines the Data Transfer Objects (DTOs) and their associated validation rules using FluentValidation, which define the contracts for communication between the client applications (`SMS.UI`) and the API (`SMS.ApiService`). These DTOs are located in the `SMS.Contracts` project.

Using DTOs ensures a clear separation between the internal domain models and the external API representation, promoting security, flexibility, and maintainability. FluentValidation is used to enforce business rules and data integrity at the API boundary.

## 2. General Principles

-   **Input DTOs (Requests):** Used for data sent from the client to the API (e.g., `CreateStudentRequest`, `UpdateCourseRequest`). These typically include validation attributes.
-   **Output DTOs (Responses):** Used for data sent from the API to the client (e.g., `StudentResponse`, `CourseResponse`). These should only expose necessary information.
    -   **External ID Mapping:** For all response DTOs representing core entities, the internal `Guid ExternalId` property will be exposed as `Guid Id` to the client for cleaner API responses and consistency with common REST API patterns.
-   **Immutability:** Response DTOs should generally be immutable.
-   **Validation:** All input DTOs will have corresponding FluentValidation validators to ensure data integrity and adherence to business rules.

## 3. Authentication & Authorization Contracts

### 3.1. User Authentication

#### `LoginRequest`

Used for user login.

```csharp
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

**`LoginRequestValidator`**

```csharp
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
```

#### `AuthResponse`

Returned upon successful login.

```csharp
public class AuthResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; }
}
```

### 3.2. User Management

#### `CreateUserRequest`

Used to create a new user.

```csharp
public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RoleId { get; set; }
}
```

**`CreateUserRequestValidator`**

```csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters.");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("Role is required.");
    }
}
```

#### `UserResponse`

Returned when fetching user details.

```csharp
public class UserResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}
```

## 4. Student Management Contracts

### 4.1. Student Creation

#### `CreateStudentRequest`

Used to create a new student.

```csharp
public class CreateStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
```

**`CreateStudentRequestValidator`**

```csharp
public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now.AddYears(-5)).WithMessage("Student must be at least 5 years old.");
        RuleFor(x => x.Gender).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Address).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
    }
}
```

#### `StudentResponse`

Returned when fetching student details.

```csharp
public class StudentResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
```

## 5. Course Management Contracts

### 5.1. Course Creation

#### `CreateCourseRequest`

Used to create a new course.

```csharp
public class CreateCourseRequest
{
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public Guid TeacherExternalId { get; set; }
}
```

**`CreateCourseRequestValidator`**

```csharp
public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Credits).GreaterThan(0).WithMessage("Credits must be greater than 0.");
        RuleFor(x => x.TeacherExternalId).NotEmpty().WithMessage("Teacher is required.");
    }
}
```

#### `CourseResponse`

Returned when fetching course details.

```csharp
public class CourseResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public Guid TeacherExternalId { get; set; }
    public string TeacherFullName { get; set; }
}
```

## 6. Parent/Guardian Management Contracts

### 6.1. Parent/Guardian Creation

#### `CreateParentGuardianRequest`

Used to create a new parent/guardian.

```csharp
public class CreateParentGuardianRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
```

**`CreateParentGuardianRequestValidator`**

```csharp
public class CreateParentGuardianRequestValidator : AbstractValidator<CreateParentGuardianRequest>
{
    public CreateParentGuardianRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
    }
}
```

#### `ParentGuardianResponse`

Returned when fetching parent/guardian details.

```csharp
public class ParentGuardianResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
```

### 6.2. Parent/Guardian Update

#### `UpdateParentGuardianRequest`

Used to update an existing parent/guardian.

```csharp
public class UpdateParentGuardianRequest
{
    public Guid ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
```

**`UpdateParentGuardianRequestValidator`**

```csharp
public class UpdateParentGuardianRequestValidator : AbstractValidator<UpdateParentGuardianRequest>
{
    public UpdateParentGuardianRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
    }
}
```

## 7. Attendance Management Contracts

### 7.1. Attendance Creation

#### `CreateAttendanceRequest`

Used to record new attendance.

```csharp
public class CreateAttendanceRequest
{
    public Guid StudentExternalId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}
```

**`CreateAttendanceRequestValidator`**

```csharp
public class CreateAttendanceRequestValidator : AbstractValidator<CreateAttendanceRequest>
{
    public CreateAttendanceRequestValidator()
    {
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}
```

#### `AttendanceResponse`

Returned when fetching attendance details.

```csharp
public class AttendanceResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid StudentExternalId { get; set; }
    public string StudentFullName { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}
```

### 7.2. Attendance Update

#### `UpdateAttendanceRequest`

Used to update existing attendance.

```csharp
public class UpdateAttendanceRequest
{
    public Guid ExternalId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}
```

**`UpdateAttendanceRequestValidator`**

```csharp
public class UpdateAttendanceRequestValidator : AbstractValidator<UpdateAttendanceRequest>
{
    public UpdateAttendanceRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}
```

## 8. Assignment Management Contracts

### 8.1. Assignment Creation

#### `CreateAssignmentRequest`

Used to create a new assignment.

```csharp
public class CreateAssignmentRequest
{
    public Guid CourseExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
```

**`CreateAssignmentRequestValidator`**

```csharp
public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
{
    public CreateAssignmentRequestValidator()
    {
        RuleFor(x => x.CourseExternalId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
    }
}
```

#### `AssignmentResponse`

Returned when fetching assignment details.

```csharp
public class AssignmentResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid CourseExternalId { get; set; }
    public string CourseTitle { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
```

### 8.2. Assignment Update

#### `UpdateAssignmentRequest`

Used to update an existing assignment.

```csharp
public class UpdateAssignmentRequest
{
    public Guid ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
```

**`UpdateAssignmentRequestValidator`**

```csharp
public class UpdateAssignmentRequestValidator : AbstractValidator<UpdateAssignmentRequest>
{
    public UpdateAssignmentRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
    }
}
```

## 9. Assignment Submission Management Contracts

### 9.1. Assignment Submission Creation

#### `CreateAssignmentSubmissionRequest`

Used to submit an assignment.

```csharp
public class CreateAssignmentSubmissionRequest
{
    public string FileName { get; set; }
    public Guid AssignmentExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
}
```

**`CreateAssignmentSubmissionRequestValidator`**

```csharp
public class CreateAssignmentSubmissionRequestValidator : AbstractValidator<CreateAssignmentSubmissionRequest>
{
    public CreateAssignmentSubmissionRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AssignmentExternalId).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
    }
}
```

#### `AssignmentSubmissionResponse`

Returned when fetching assignment submission details.

```csharp
public class AssignmentSubmissionResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid AssignmentExternalId { get; set; }
    public string AssignmentTitle { get; set; }
    public Guid StudentExternalId { get; set; }
    public string StudentFullName { get; set; }
    public string FileName { get; set; }
    public DateTime SubmittedAt { get; set; }
}
```

## 10. Messaging Contracts

### 10.1. Message Creation

#### `CreateMessageRequest`

Used to send a new message.

```csharp
public class CreateMessageRequest
{
    public Guid RecipientExternalId { get; set; }
    public string Content { get; set; }
}
```

**`CreateMessageRequestValidator`**

```csharp
public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>
{
    public CreateMessageRequestValidator()
    {
        RuleFor(x => x.RecipientExternalId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}
```

#### `MessageResponse`

Returned when fetching message details.

```csharp
public class MessageResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid SenderExternalId { get; set; }
    public string SenderFullName { get; set; }
    public Guid RecipientExternalId { get; set; }
    public string RecipientFullName { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}
```

## 11. Announcement Management Contracts

### 11.1. Announcement Creation

#### `CreateAnnouncementRequest`

Used to create a new announcement.

```csharp
public class CreateAnnouncementRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string TargetAudience { get; set; }
}
```

**`CreateAnnouncementRequestValidator`**

```cscsharp
public class CreateAnnouncementRequestValidator : AbstractValidator<CreateAnnouncementRequest>
{
    public CreateAnnouncementRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.PublishDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(x => x.PublishDate).When(x => x.ExpiryDate.HasValue);
        RuleFor(x => x.TargetAudience).NotEmpty().MaximumLength(50);
    }
}
```

#### `AnnouncementResponse`

Returned when fetching announcement details.

```csharp
public class AnnouncementResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string TargetAudience { get; set; }
}
```

## 12. Notification Management Contracts

### 12.1. Notification Response

#### `NotificationResponse`

Returned when fetching notification details.

```csharp
public class NotificationResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid UserExternalId { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## 13. File Storage Contracts

### 13.1. File Upload Request

#### `FileUploadRequest`

Used to upload a file.

```csharp
public class FileUploadRequest
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Length { get; set; }
    public Dictionary<string, string> Metadata { get; set; } // Custom metadata
}
```

**`FileUploadRequestValidator`**

```csharp
public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ContentType).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Length).GreaterThan(0).WithMessage("File size must be greater than 0.");
    }
}
```

#### `FileStorageResponse`

Returned when fetching file storage details.

```csharp
public class FileStorageResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public Guid UploadedByUserExternalId { get; set; }
    public DateTime UploadedAt { get; set; }
    public string ContentType { get; set; }
    public long Length { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public int Version { get; set; }
}
```

### 13.2. File Metadata Update

#### `UpdateFileMetadataRequest`

Used to update metadata for an existing file.

```csharp
public class UpdateFileMetadataRequest
{
    public Guid Id { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}
```

**`UpdateFileMetadataRequestValidator`**

```csharp
public class UpdateFileMetadataRequestValidator : AbstractValidator<UpdateFileMetadataRequest>
{
    public UpdateFileMetadataRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Metadata).NotNull().WithMessage("Metadata cannot be null.");
    }
}
```

### 13.3. File Access Control

#### `UpdateFileAccessRequest`

Used to update access permissions for a file.

```csharp
public class UpdateFileAccessRequest
{
    public Guid Id { get; set; }
    public List<Guid> AllowedUserIds { get; set; }
    public List<int> AllowedRoleIds { get; set; }
}
```

**`UpdateFileAccessRequestValidator`**

```csharp
public class UpdateFileAccessRequestValidator : AbstractValidator<UpdateFileAccessRequest>
{
    public UpdateFileAccessRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AllowedUserIds).NotNull();
        RuleFor(x => x.AllowedRoleIds).NotNull();
    }
}
```

## 14. Audit Log Contracts

### 14.1. Audit Log Response

#### `AuditLogResponse`

Returned when fetching audit log details.

```csharp
public class AuditLogResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid UserExternalId { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public int EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}
```

---

This document will be expanded with DTOs and validation rules for all other core entities and complex workflows as development progresses.