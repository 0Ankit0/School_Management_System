using System.ComponentModel.DataAnnotations;

namespace SMS.UI.Shared.Models;

// Student Models
public class StudentResponse
{
    public Guid Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class StudentCreateRequest
{
    [Required]
    public string StudentId { get; set; } = string.Empty;
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    public bool IsActive { get; set; } = true;
}

// Academic Year Models
public class AcademicYearResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AcademicYearCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; } = DateTime.Today;
    
    [Required]
    public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(10);
    
    public bool IsCurrent { get; set; }
}

// Class Models
public class ClassResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public string? Room { get; set; }
    public Guid? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int StudentCount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ClassCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Range(1, 100)]
    public int Capacity { get; set; } = 30;
    
    public string? Room { get; set; }
    public Guid? TeacherId { get; set; }
    public bool IsActive { get; set; } = true;
}

// Subject Models
public class SubjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class SubjectCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Code { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Range(1, 10)]
    public int Credits { get; set; } = 3;
    
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
}

// Term Models
public class TermResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AcademicYearId { get; set; }
    public string? AcademicYearName { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TermCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; } = DateTime.Today;
    
    [Required]
    public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(3);
    
    [Required]
    public Guid AcademicYearId { get; set; }
    
    public bool IsCurrent { get; set; }
}

// Teacher Models
public class TeacherResponse
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TeacherCreateRequest
{
    [Required]
    public string EmployeeId { get; set; } = string.Empty;
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Position { get; set; }
    
    [Required]
    public DateTime HireDate { get; set; } = DateTime.Today;
    
    [Range(0, 1000000)]
    public decimal Salary { get; set; }
    
    public bool IsActive { get; set; } = true;
}

// Course Models
public class CourseResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public Guid SubjectId { get; set; }
    public string? SubjectName { get; set; }
    public Guid TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Grade Models
public class GradeResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string? StudentName { get; set; }
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public string Grade { get; set; } = string.Empty;
    public decimal Points { get; set; }
    public string? Comments { get; set; }
    public DateTime GradedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Assignment Models
public class AssignmentResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxPoints { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
