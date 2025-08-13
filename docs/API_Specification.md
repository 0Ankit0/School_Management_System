# API Specification: School Management System (SMS)

**Document Version:** 2.0
**Date:** 2025-08-07
**Author:** Gemini CLI Agent

---

## 1. Introduction

This document provides a detailed specification for the RESTful API of the School Management System (SMS). It defines the API endpoints, request/response formats, authentication mechanisms, and error handling procedures. This API is designed to be consumed by the .NET MAUI Blazor Hybrid client application and potentially other authorized clients in the future.

## 2. Authentication

The API will use **JSON Web Token (JWT)** based authentication to secure all endpoints. The authentication flow is as follows:

1.  **Login:** A user submits their credentials (username/password) to a dedicated `/api/auth/login` endpoint.
2.  **Token Issuance:** If the credentials are valid, the server generates a JWT containing user claims (e.g., user ID, role) and returns it to the client.
3.  **Authenticated Requests:** The client must include the JWT in the `Authorization` header of all subsequent requests to protected endpoints, using the `Bearer` scheme.
    ```
    Authorization: Bearer <your_jwt_token>
    ```
4.  **Token Validation:** The server validates the JWT on each request to ensure its authenticity and integrity.

## 3. API Endpoints

The API will expose RESTful endpoints for managing students, teachers, courses, and enrollments. All endpoints will be versioned under the `/api/v1/` path.

### 3.0. Domain Setup APIs Overview

This API surface also includes domain setup endpoints for academic years/terms, classes, subjects, schedules, and grades to fully support workflows described in Workflow.md.

### 3.1. Authentication API

- **Base Path:** `/api/auth`

| HTTP Method | Endpoint  | Description                                                  | Request Body   | Response Body    |
| :---------- | :-------- | :----------------------------------------------------------- | :------------- | :--------------- |
| `POST`      | `/login`  | Authenticate a user and receive a JWT.                       | `LoginRequest` | `LoginResponse`  |
| `POST`      | `/logout` | (Optional) Invalidate the user's session on the server-side. | None           | No Content (204) |

### 3.2. Students API

- **Base Path:** `/api/v1/students`

| HTTP Method | Endpoint | Description                                     | Request Body           | Response Body        | Permissions                                                               |
| :---------- | :------- | :---------------------------------------------- | :--------------------- | :------------------- | :------------------------------------------------------------------------ |
| `GET`       | `/`      | Retrieve a paginated list of all students.      | None                   | `PagedList<Student>` | Administrator                                                             |
| `GET`       | `/{id}`  | Retrieve a specific student by their unique ID. | None                   | `Student`            | Administrator, Teacher (own students), Student (self), Parent (own child) |
| `POST`      | `/`      | Create a new student record.                    | `CreateStudentRequest` | `Student`            | Administrator                                                             |
| `PUT`       | `/{id}`  | Update an existing student's information.       | `UpdateStudentRequest` | `Student`            | Administrator                                                             |
| `DELETE`    | `/{id}`  | Delete a student record by their ID.            | None                   | No Content (204)     | Administrator                                                             |

### 3.3. Teachers API

- **Base Path:** `/api/v1/teachers`

| HTTP Method | Endpoint | Description                                     | Request Body           | Response Body        | Permissions                   |
| :---------- | :------- | :---------------------------------------------- | :--------------------- | :------------------- | :---------------------------- |
| `GET`       | `/`      | Retrieve a paginated list of all teachers.      | None                   | `PagedList<Teacher>` | Administrator                 |
| `GET`       | `/{id}`  | Retrieve a specific teacher by their unique ID. | None                   | `Teacher`            | Administrator, Teacher (self) |
| `POST`      | `/`      | Create a new teacher record.                    | `CreateTeacherRequest` | `Teacher`            | Administrator                 |
| `PUT`       | `/{id}`  | Update an existing teacher's information.       | `UpdateTeacherRequest` | `Teacher`            | Administrator, Teacher (self) |
| `DELETE`    | `/{id}`  | Delete a teacher record by their ID.            | None                   | No Content (204)     | Administrator                 |

### 3.4. Courses API

- **Base Path:** `/api/v1/courses`

| HTTP Method | Endpoint | Description                                  | Request Body          | Response Body       | Permissions   |
| :---------- | :------- | :------------------------------------------- | :-------------------- | :------------------ | :------------ |
| `GET`       | `/`      | Retrieve a paginated list of all courses.    | None                  | `PagedList<Course>` | All Roles     |
| `GET`       | `/{id}`  | Retrieve a specific course by its unique ID. | None                  | `Course`            | All Roles     |
| `POST`      | `/`      | Create a new course.                         | `CreateCourseRequest` | `Course`            | Administrator |
| `PUT`       | `/{id}`  | Update an existing course.                   | `UpdateCourseRequest` | `Course`            | Administrator |
| `DELETE`    | `/{id}`  | Delete a course by its ID.                   | None                  | No Content (204)    | Administrator |

### 3.5. Enrollments API

- **Base Path:** `/api/v1/enrollments`

| HTTP Method | Endpoint      | Description                                                    | Request Body              | Response Body      | Permissions                         |
| :---------- | :------------ | :------------------------------------------------------------- | :------------------------ | :----------------- | :---------------------------------- |
| `GET`       | `/`           | Retrieve a list of all enrollments (filtered by query params). | None                      | `List<Enrollment>` | Administrator                       |
| `POST`      | `/`           | Create a new enrollment (enroll a student in a course).        | `CreateEnrollmentRequest` | `Enrollment`       | Administrator                       |
| `PUT`       | `/{id}/grade` | Update the grade for a specific enrollment.                    | `UpdateGradeRequest`      | `Enrollment`       | Administrator, Teacher (own course) |
| `DELETE`    | `/{id}`       | Delete an enrollment by its ID.                                | None                      | No Content (204)   | Administrator                       |

### 3.6. Attendance API

- **Base Path:** `/api/v1/attendance`

| HTTP Method | Endpoint | Description                                                       | Request Body              | Response Body      | Permissions                                        |
| :---------- | :------- | :---------------------------------------------------------------- | :------------------------ | :----------------- | :------------------------------------------------- |
| `GET`       | `/`      | Retrieve attendance records (filterable by student, class, date). | None                      | `List<Attendance>` | Admin, Teacher, Student (self), Parent (own child) |
| `POST`      | `/`      | Mark attendance for a class or student.                           | `AttendanceRequest`       | `Attendance`       | Teacher                                            |
| `PUT`       | `/{id}`  | Update an attendance record.                                      | `AttendanceUpdateRequest` | `Attendance`       | Teacher, Admin                                     |
| `DELETE`    | `/{id}`  | Delete an attendance record.                                      | None                      | No Content (204)   | Admin                                              |

### 3.7. Assignments API

- **Base Path:** `/api/v1/assignments`

| HTTP Method | Endpoint       | Description                                           | Request Body                  | Response Body          | Permissions                                 |
| :---------- | :------------- | :---------------------------------------------------- | :---------------------------- | :--------------------- | :------------------------------------------ |
| `GET`       | `/`            | List all assignments (filterable by course, student). | None                          | `List<Assignment>`     | Teacher, Student (self), Parent (own child) |
| `POST`      | `/`            | Create a new assignment.                              | `CreateAssignmentRequest`     | `Assignment`           | Teacher                                     |
| `PUT`       | `/{id}`        | Update an assignment.                                 | `UpdateAssignmentRequest`     | `Assignment`           | Teacher                                     |
| `DELETE`    | `/{id}`        | Delete an assignment.                                 | None                          | No Content (204)       | Teacher                                     |
| `POST`      | `/{id}/submit` | Submit assignment (file upload).                      | `AssignmentSubmissionRequest` | `AssignmentSubmission` | Student                                     |

### 3.8. Messaging API

- **Base Path:** `/api/v1/messages`

| HTTP Method | Endpoint | Description                               | Request Body           | Response Body    | Permissions |
| :---------- | :------- | :---------------------------------------- | :--------------------- | :--------------- | :---------- |
| `GET`       | `/`      | List messages for the authenticated user. | None                   | `List<Message>`  | All Roles   |
| `POST`      | `/`      | Send a new message.                       | `CreateMessageRequest` | `Message`        | All Roles   |
| `GET`       | `/{id}`  | Get message details.                      | None                   | `Message`        | All Roles   |
| `DELETE`    | `/{id}`  | Delete a message.                         | None                   | No Content (204) | All Roles   |

### 3.9. Announcements API

- **Base Path:** `/api/v1/announcements`

| HTTP Method | Endpoint | Description                | Request Body                | Response Body        | Permissions |
| :---------- | :------- | :------------------------- | :-------------------------- | :------------------- | :---------- |
| `GET`       | `/`      | List all announcements.    | None                        | `List<Announcement>` | All Roles   |
| `POST`      | `/`      | Create a new announcement. | `CreateAnnouncementRequest` | `Announcement`       | Admin       |
| `PUT`       | `/{id}`  | Update an announcement.    | `UpdateAnnouncementRequest` | `Announcement`       | Admin       |
| `DELETE`    | `/{id}`  | Delete an announcement.    | None                        | No Content (204)     | Admin       |

### 3.10. Notifications API

- **Base Path:** `/api/v1/notifications`

| HTTP Method | Endpoint | Description                                    | Request Body                | Response Body        | Permissions    |
| :---------- | :------- | :--------------------------------------------- | :-------------------------- | :------------------- | :------------- |
| `GET`       | `/`      | List notifications for the authenticated user. | None                        | `List<Notification>` | All Roles      |
| `POST`      | `/`      | Send a notification.                           | `CreateNotificationRequest` | `Notification`       | Admin, Teacher |

### 3.11. User & Role Management API

- **Base Path:** `/api/v1/users`

| HTTP Method | Endpoint | Description               | Request Body        | Response Body    | Permissions |
| :---------- | :------- | :------------------------ | :------------------ | :--------------- | :---------- |
| `GET`       | `/`      | List all users.           | None                | `List<User>`     | Admin       |
| `POST`      | `/`      | Create a new user.        | `CreateUserRequest` | `User`           | Admin       |
| `PUT`       | `/{id}`  | Update user details.      | `UpdateUserRequest` | `User`           | Admin, Self |
| `DELETE`    | `/{id}`  | Delete/deactivate a user. | None                | No Content (204) | Admin       |
| `GET`       | `/roles` | List all roles.           | None                | `List<Role>`     | Admin       |

### 3.12. Parent/Guardian Management API

- **Base Path:** `/api/v1/parents`

| HTTP Method | Endpoint         | Description                     | Request Body          | Response Body    | Permissions   |
| :---------- | :--------------- | :------------------------------ | :-------------------- | :--------------- | :------------ |
| `GET`       | `/`              | List all parents/guardians.     | None                  | `List<Parent>`   | Admin         |
| `POST`      | `/`              | Create a new parent/guardian.   | `CreateParentRequest` | `Parent`         | Admin         |
| `PUT`       | `/{id}`          | Update parent/guardian details. | `UpdateParentRequest` | `Parent`         | Admin, Self   |
| `DELETE`    | `/{id}`          | Delete a parent/guardian.       | None                  | No Content (204) | Admin         |
| `GET`       | `/{id}/children` | List children for a parent.     | None                  | `List<Student>`  | Admin, Parent |

### 3.13. File Uploads API

- **Base Path:** `/api/v1/files`

| HTTP Method | Endpoint         | Description                                        | Request Body        | Response Body  | Permissions              |
| :---------- | :--------------- | :------------------------------------------------- | :------------------ | :------------- | :----------------------- |
| `POST`      | `/upload`        | Upload a file (assignment, profile picture, etc.). | Multipart/Form-Data | `FileMetadata` | All Roles (as permitted) |
| `GET`       | `/download/{id}` | Download a file by ID.                             | None                | File Stream    | All Roles (as permitted) |

### 3.14. Dashboards & Reports API

- **Base Path:** `/api/v1/reports`

| HTTP Method | Endpoint     | Description                                                | Request Body | Response Body   | Permissions |
| :---------- | :----------- | :--------------------------------------------------------- | :----------- | :-------------- | :---------- |
| `GET`       | `/dashboard` | Get dashboard data for the authenticated user.             | None         | `DashboardData` | All Roles   |
| `GET`       | `/custom`    | Generate a custom report (filterable by type, date, etc.). | Query Params | `ReportData`    | Admin       |

### 3.15. Academic Years API

- **Base Path:** `/api/v1/academic-years`

| HTTP Method | Endpoint | Description              | Request Body        | Response Body    | Permissions |
| :---------- | :------- | :----------------------- | :------------------ | :--------------- | :---------- |
| `GET`       | `/`      | List academic years.     | None                | `List<Year>`     | Admin       |
| `POST`      | `/`      | Create an academic year. | `CreateYearRequest` | `Year`           | Admin       |
| `PUT`       | `/{id}`  | Update an academic year. | `UpdateYearRequest` | `Year`           | Admin       |
| `DELETE`    | `/{id}`  | Delete an academic year. | None                | No Content (204) | Admin       |

### 3.16. Terms API

- **Base Path:** `/api/v1/terms`

| HTTP Method | Endpoint | Description    | Request Body        | Response Body    | Permissions |
| :---------- | :------- | :------------- | :------------------ | :--------------- | :---------- |
| `GET`       | `/`      | List terms.    | None                | `List<Term>`     | Admin       |
| `POST`      | `/`      | Create a term. | `CreateTermRequest` | `Term`           | Admin       |
| `PUT`       | `/{id}`  | Update a term. | `UpdateTermRequest` | `Term`           | Admin       |
| `DELETE`    | `/{id}`  | Delete a term. | None                | No Content (204) | Admin       |

### 3.17. Classes API

- **Base Path:** `/api/v1/classes`

| HTTP Method | Endpoint | Description     | Request Body         | Response Body    | Permissions |
| :---------- | :------- | :-------------- | :------------------- | :--------------- | :---------- |
| `GET`       | `/`      | List classes.   | None                 | `List<Class>`    | Admin       |
| `POST`      | `/`      | Create a class. | `CreateClassRequest` | `Class`          | Admin       |
| `PUT`       | `/{id}`  | Update a class. | `UpdateClassRequest` | `Class`          | Admin       |
| `DELETE`    | `/{id}`  | Delete a class. | None                 | No Content (204) | Admin       |

### 3.18. Subjects API

- **Base Path:** `/api/v1/subjects`

| HTTP Method | Endpoint | Description       | Request Body           | Response Body    | Permissions |
| :---------- | :------- | :---------------- | :--------------------- | :--------------- | :---------- |
| `GET`       | `/`      | List subjects.    | None                   | `List<Subject>`  | Admin       |
| `POST`      | `/`      | Create a subject. | `CreateSubjectRequest` | `Subject`        | Admin       |
| `PUT`       | `/{id}`  | Update a subject. | `UpdateSubjectRequest` | `Subject`        | Admin       |
| `DELETE`    | `/{id}`  | Delete a subject. | None                   | No Content (204) | Admin       |

### 3.19. Schedules API

- **Base Path:** `/api/v1/schedules`

| HTTP Method | Endpoint | Description                                                           | Request Body            | Response Body    | Permissions |
| :---------- | :------- | :-------------------------------------------------------------------- | :---------------------- | :--------------- | :---------- |
| `GET`       | `/`      | List schedules (filter by classId, teacherId, subjectId, date, term). | None                    | `List<Schedule>` | Admin       |
| `POST`      | `/`      | Create a schedule entry.                                              | `CreateScheduleRequest` | `Schedule`       | Admin       |
| `PUT`       | `/{id}`  | Update a schedule entry.                                              | `UpdateScheduleRequest` | `Schedule`       | Admin       |
| `DELETE`    | `/{id}`  | Delete a schedule entry.                                              | None                    | No Content (204) | Admin       |

### 3.20. Grades API

- **Base Path:** `/api/v1/grades`

| HTTP Method | Endpoint | Description                                                              | Request Body         | Response Body | Permissions                  |
| :---------- | :------- | :----------------------------------------------------------------------- | :------------------- | :------------ | :--------------------------- |
| `GET`       | `/`      | List grade items/entries (filter by courseId, studentId, term, subject). | None                 | `List<Grade>` | Teacher (own courses), Admin |
| `POST`      | `/`      | Create a grade entry (e.g., for an assignment/assessment).               | `CreateGradeRequest` | `Grade`       | Teacher (own courses), Admin |
| `PUT`       | `/{id}`  | Update a grade entry.                                                    | `UpdateGradeRequest` | `Grade`       | Teacher (own courses), Admin |
| `DELETE`    | `/{id}`  | Delete a grade entry.                                                    | None                 | No Content    | Teacher (own courses), Admin |

## 4. Request and Response Formats

All requests and responses will be in **JSON** format (`Content-Type: application/json`).

### 4.1. Common Data Structures

- **Error Response:**

  ```json
  {
    "statusCode": 400,
    "message": "A descriptive error message.",
    "details": "Optional detailed information about the error, such as validation failures."
  }
  ```

- **Pagination and Filtering:**
  List endpoints (`GET /api/v1/students`, etc.) will support pagination and filtering via query parameters:

  - `pageNumber`: The page number to retrieve (e.g., `?pageNumber=1`). Defaults to 1.
  - `pageSize`: The number of items per page (e.g., `?pageSize=20`). Defaults to 20.
  - `sortBy`: The field to sort by (e.g., `?sortBy=lastName`).
  - `sortOrder`: The sort order (`asc` or `desc`).
  - `filter`: A generic filter parameter for searching (e.g., `?filter=John`).

  The response for a paginated list will be structured as follows:

  ```json
  {
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 10,
    "totalRecords": 198,
    "data": [
      // ... list of items ...
    ]
  }
  ```

### 4.2. Key DTOs (schemas/examples)

Note: Types are illustrative. Use appropriate server-side validation and data annotations.

- Year

  - Year: { id: string, name: string, startDate: string(date), endDate: string(date) }
  - CreateYearRequest: { name: string, startDate: string(date), endDate: string(date) }
  - UpdateYearRequest: { name?: string, startDate?: string(date), endDate?: string(date) }

- Term

  - Term: { id: string, yearId: string, name: string, startDate: string(date), endDate: string(date) }
  - CreateTermRequest: { yearId: string, name: string, startDate: string(date), endDate: string(date) }
  - UpdateTermRequest: { name?: string, startDate?: string(date), endDate?: string(date) }

- Class

  - Class: { id: string, academicYearId: string, name: string, capacity: number, homeroomTeacherId?: string }
  - CreateClassRequest: { academicYearId: string, name: string, capacity?: number, homeroomTeacherId?: string }
  - UpdateClassRequest: { name?: string, capacity?: number, homeroomTeacherId?: string }

- Subject

  - Subject: { id: string, code: string, name: string, description?: string }
  - CreateSubjectRequest: { code: string, name: string, description?: string }
  - UpdateSubjectRequest: { code?: string, name?: string, description?: string }

- Schedule

  - Schedule: {
    id: string,
    classId: string,
    subjectId: string,
    teacherId: string,
    termId?: string,
    room?: string,
    dayOfWeek?: number(0-6),
    startTime?: string(HH:mm),
    endTime?: string(HH:mm),
    startDate?: string(date),
    endDate?: string(date)
    }
  - CreateScheduleRequest: { classId: string, subjectId: string, teacherId: string, termId?: string, room?: string, dayOfWeek?: number, startTime?: string, endTime?: string, startDate?: string, endDate?: string }
  - UpdateScheduleRequest: { room?: string, dayOfWeek?: number, startTime?: string, endTime?: string, startDate?: string, endDate?: string }
  - Notes: server must enforce conflict detection; return 409 on overlaps (teacher, room, or class)

- Grade

  - Grade: { id: string, enrollmentId?: string, assignmentId?: string, studentId: string, courseId?: string, termId?: string, score: number, maxScore?: number, letter?: string, weight?: number, recordedAt: string(datetime) }
  - CreateGradeRequest: { enrollmentId?: string, assignmentId?: string, score: number, maxScore?: number, letter?: string, weight?: number, termId?: string, comment?: string }
  - UpdateGradeRequest: { score?: number, maxScore?: number, letter?: string, weight?: number, comment?: string }
  - Rule: either enrollmentId or assignmentId must be provided

- Attendance

  - Attendance: { id: string, studentId: string, classId?: string, courseId?: string, date: string(date), period?: string, status: "Present"|"Absent"|"Late"|"Excused", notes?: string }
  - AttendanceRequest: { studentId: string, classId?: string, courseId?: string, date: string(date), period?: string, status: string, notes?: string }
  - AttendanceUpdateRequest: { status?: string, notes?: string }

- Assignment Submission
  - AssignmentSubmission: { id: string, assignmentId: string, studentId: string, submittedAt: string(datetime), fileId?: string, comment?: string, gradeId?: string }
  - AssignmentSubmissionRequest: multipart/form-data with fields: assignmentId, comment? and file:binary; or JSON with { fileId: string }

## 5. Error Handling

The API will use standard HTTP status codes to indicate the success or failure of a request.

| Status Code                 | Meaning      | Description                                                                                                                     |
| :-------------------------- | :----------- | :------------------------------------------------------------------------------------------------------------------------------ |
| `200 OK`                    | Success      | The request was successful.                                                                                                     |
| `201 Created`               | Success      | The resource was successfully created.                                                                                          |
| `204 No Content`            | Success      | The request was successful, but there is no content to return (e.g., for DELETE operations).                                    |
| `400 Bad Request`           | Client Error | The request was invalid or cannot be otherwise served. The request payload may be malformed or contain invalid data.            |
| `401 Unauthorized`          | Client Error | Authentication is required and has failed or has not yet been provided.                                                         |
| `403 Forbidden`             | Client Error | The authenticated user does not have the necessary permissions to access the resource.                                          |
| `404 Not Found`             | Client Error | The requested resource could not be found on the server.                                                                        |
| `409 Conflict`              | Client Error | The request could not be completed due to a conflict with the current state of the resource (e.g., creating a duplicate entry). |
| `500 Internal Server Error` | Server Error | An unexpected error occurred on the server.                                                                                     |

## 6. Security Considerations

- **HTTPS:** All API communication must be over HTTPS to ensure data is encrypted in transit.
- **Input Validation:** Strict input validation will be performed on all incoming requests to prevent common vulnerabilities like SQL Injection, XSS, and to ensure data integrity.
- **Rate Limiting:** The API will implement rate limiting to prevent abuse and ensure fair usage for all clients.
- **CORS:** Cross-Origin Resource Sharing (CORS) will be configured to allow requests only from authorized domains.

## 7. Versioning

API versioning will be handled via the URL path (e.g., `/api/v1/students`). This ensures that future changes to the API will not break existing client implementations.

## 8. Implementation Guidelines

### 8.1. Authentication Flow Details

#### JWT Token Structure

```json
{
  "sub": "user-uuid",
  "name": "John Doe",
  "email": "john.doe@school.edu",
  "role": "Teacher",
  "permissions": ["read:students", "write:grades", "read:attendance"],
  "exp": 1672531200,
  "iat": 1672444800,
  "iss": "SMS-API"
}
```

#### Authorization Headers

All authenticated requests must include:

```
Authorization: Bearer <jwt-token>
Content-Type: application/json
Accept: application/json
```

#### Permission Matrix

| Resource             | Administrator | Teacher                 | Student         | Parent                  |
| -------------------- | ------------- | ----------------------- | --------------- | ----------------------- |
| Users (CRUD)         | Full          | None                    | Read (self)     | Read (self)             |
| Students (CRUD)      | Full          | Read (assigned)         | Read (self)     | Read (children)         |
| Teachers (CRUD)      | Full          | Read/Update (self)      | Read            | Read (child's teachers) |
| Courses (CRUD)       | Full          | Read (assigned)         | Read (enrolled) | Read (child enrolled)   |
| Grades (CRUD)        | Full          | Full (assigned courses) | Read (self)     | Read (children)         |
| Attendance (CRUD)    | Full          | Full (assigned classes) | Read (self)     | Read (children)         |
| Messages (CRUD)      | Full          | Full                    | Full            | Full                    |
| Announcements (CRUD) | Full          | Create/Read             | Read            | Read                    |

### 8.2. API Response Standards

#### Success Response Format

```json
{
  "success": true,
  "data": {}, // or [] for arrays
  "message": "Operation completed successfully",
  "timestamp": "2025-08-09T10:30:00Z"
}
```

#### Error Response Format

```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input data",
    "details": [
      {
        "field": "email",
        "message": "Email format is invalid"
      }
    ]
  },
  "timestamp": "2025-08-09T10:30:00Z"
}
```

#### Pagination Response Format

```json
{
  "success": true,
  "data": [],
  "pagination": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalRecords": 98,
    "hasNextPage": true,
    "hasPreviousPage": false
  },
  "timestamp": "2025-08-09T10:30:00Z"
}
```

### 8.3. Error Codes Reference

| Error Code                | HTTP Status | Description                         | Resolution                            |
| ------------------------- | ----------- | ----------------------------------- | ------------------------------------- |
| INVALID_TOKEN             | 401         | JWT token is invalid or expired     | Refresh token or re-authenticate      |
| INSUFFICIENT_PERMISSIONS  | 403         | User lacks required permissions     | Contact administrator                 |
| RESOURCE_NOT_FOUND        | 404         | Requested resource doesn't exist    | Check resource ID                     |
| VALIDATION_ERROR          | 400         | Input validation failed             | Fix validation errors in request      |
| DUPLICATE_RESOURCE        | 409         | Resource already exists             | Use different unique identifiers      |
| SCHEDULE_CONFLICT         | 409         | Teacher/room schedule conflict      | Choose different time slot            |
| ENROLLMENT_LIMIT_EXCEEDED | 400         | Class capacity exceeded             | Choose different class                |
| GRADE_ALREADY_SUBMITTED   | 409         | Grade already exists for assignment | Update existing grade instead         |
| FILE_SIZE_EXCEEDED        | 400         | Uploaded file too large             | Reduce file size (max 10MB)           |
| INVALID_FILE_TYPE         | 400         | File type not allowed               | Use allowed types: PDF, DOC, JPG, PNG |

### 8.4. Rate Limiting

- **Unauthenticated requests**: 100 requests per hour per IP
- **Authenticated requests**: 1000 requests per hour per user
- **File upload**: 10 uploads per minute per user
- **Bulk operations**: 5 requests per minute per user

Rate limit headers:

```
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1672531200
```

### 8.5. File Upload Specifications

#### Supported File Types

- **Documents**: PDF, DOC, DOCX, TXT
- **Images**: JPG, JPEG, PNG, GIF
- **Archives**: ZIP, RAR

#### File Size Limits

- **Profile Pictures**: 2MB max
- **Assignment Files**: 10MB max
- **Document Uploads**: 5MB max

#### Upload Security

- All files are scanned for viruses
- Files are stored with UUID names
- Original filenames preserved in metadata
- Access controlled by user permissions

### 8.6. Database Constraints & Business Rules

#### Unique Constraints

- User email addresses
- Student email addresses
- Teacher email addresses
- Course codes
- Subject codes
- Student enrollment per course (composite)

#### Business Rule Validations

- Students must be 3-25 years old
- Academic year end date > start date
- Term dates must fall within academic year
- Schedule times: start time < end time
- Grade scores: 0 ≤ score ≤ maxScore
- Attendance date cannot be future date

### 8.7. Audit Logging Requirements

All CRUD operations on core entities must log:

```json
{
  "auditId": "uuid",
  "userId": "uuid",
  "action": "CREATE|UPDATE|DELETE",
  "entity": "Student|Teacher|Course|Grade|etc",
  "entityId": "uuid",
  "timestamp": "ISO datetime",
  "ipAddress": "IP address",
  "userAgent": "User agent string",
  "changes": {
    "before": {}, // Previous values for UPDATE
    "after": {} // New values for CREATE/UPDATE
  }
}
```

### 8.8. Environment Configuration

#### Required Environment Variables

```bash
# Database
DATABASE_CONNECTION_STRING=postgresql://user:pass@host:5432/sms_db
DATABASE_POOL_SIZE=20

# JWT
JWT_SECRET_KEY=your-256-bit-secret
JWT_EXPIRY_HOURS=24
JWT_ISSUER=SMS-API

# File Storage
FILE_STORAGE_PATH=/app/uploads
MAX_FILE_SIZE_MB=10

# Email (for notifications)
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=noreply@school.edu
SMTP_PASSWORD=app-password
SMTP_FROM_NAME=School Management System

# Security
CORS_ORIGINS=https://sms.school.edu,https://admin.school.edu
RATE_LIMIT_ENABLED=true
VIRUS_SCAN_ENABLED=true

# Logging
LOG_LEVEL=Information
LOG_FILE_PATH=/app/logs
```

### 8.9. API Testing Examples

#### cURL Examples

```bash
# Login
curl -X POST https://api.school.edu/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"teacher@school.edu","password":"password123"}'

# Get students (with pagination)
curl -X GET "https://api.school.edu/api/v1/students?pageNumber=1&pageSize=20" \
  -H "Authorization: Bearer <token>"

# Create student
curl -X POST https://api.school.edu/api/v1/students \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "dateOfBirth": "2010-05-15",
    "email": "john.doe@student.school.edu"
  }'

# Upload assignment file
curl -X POST https://api.school.edu/api/v1/files/upload \
  -H "Authorization: Bearer <token>" \
  -F "file=@assignment.pdf" \
  -F "description=Math Assignment 1"

# Submit assignment
curl -X POST https://api.school.edu/api/v1/assignments/uuid/submit \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"fileId": "file-uuid", "comment": "Completed assignment"}'
```

### 8.10. Performance Optimization Guidelines

#### Database Indexing Strategy

- Index all foreign key columns
- Composite indexes on frequently queried combinations
- Partial indexes for soft-deleted records
- Full-text search indexes for names and descriptions

#### Caching Strategy

- Cache user sessions (Redis)
- Cache frequently accessed reference data (roles, subjects)
- Cache computed grade summaries
- Cache file metadata

#### Query Optimization

- Use pagination for all list endpoints
- Implement lazy loading for navigation properties
- Use projection for large entities
- Batch database operations where possible

#### API Response Optimization

- Compress responses with gzip
- Use HTTP 304 for unchanged resources
- Implement API response caching headers
- Minimize payload sizes with selective field returns
