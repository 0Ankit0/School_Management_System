using AutoMapper;
using SMS.Contracts.AcademicYears;
using SMS.Contracts.Assignments;
using SMS.Contracts.AssignmentSubmissions;
using SMS.Contracts.Attendances;
using SMS.Contracts.AuditLogs;
using SMS.Contracts.Classes;
using SMS.Contracts.Courses;
using SMS.Contracts.Enrollments;
using SMS.Contracts.Grades;
using SMS.Contracts.ParentGuardians;
using SMS.Contracts.Schedules;
using SMS.Contracts.Students;
using SMS.Contracts.Subjects;
using SMS.Contracts.Teachers;
using SMS.Contracts.Terms;
using SMS.Microservices.SchoolCore.Models;

namespace SMS.Microservices.SchoolCore.MappingProfiles;

public class SchoolCoreMappingProfile : Profile
{
    public SchoolCoreMappingProfile()
    {
        CreateMap<Student, StudentResponse>();
        CreateMap<Teacher, TeacherResponse>();
        CreateMap<Course, CourseResponse>();
        CreateMap<Enrollment, EnrollmentResponse>();
        CreateMap<ParentGuardian, ParentGuardianResponse>();
        CreateMap<Attendance, AttendanceResponse>();
        CreateMap<Assignment, AssignmentResponse>();
        CreateMap<AssignmentSubmission, AssignmentSubmissionResponse>();
        CreateMap<AuditLog, AuditLogResponse>();
        CreateMap<AcademicYear, AcademicYearResponse>();
        CreateMap<Term, TermResponse>();
        CreateMap<Class, ClassResponse>();
        CreateMap<Subject, SubjectResponse>();
        CreateMap<Schedule, ScheduleResponse>();
        CreateMap<Grade, GradeResponse>();
    }
}