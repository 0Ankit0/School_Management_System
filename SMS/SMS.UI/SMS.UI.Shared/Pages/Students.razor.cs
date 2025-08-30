using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Students;

namespace SMS.UI.Shared.Pages
{
    public partial class Students : ComponentBase
    {
        private List<StudentResponse>? students;
        private string searchTerm = "";

        [Inject]
        public HttpClient Http { get; set; } = default!;

        private IEnumerable<StudentResponse> filteredStudents => students?.Where(FilterStudents) ?? new List<StudentResponse>();

        protected override async Task OnInitializedAsync()
        {
            try 
            {
                students = await Http.GetFromJsonAsync<List<StudentResponse>>("http://localhost:5000/api/students");
            }
            catch
            {
                // Fallback to sample data for demo
                students = new List<StudentResponse>
                {
                    new StudentResponse
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@school.com",
                        Phone = "123-456-7890",
                        DateOfBirth = new DateTime(2008, 5, 15),
                        Gender = "Male",
                        Address = "123 Main St"
                    },
                    new StudentResponse
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "jane.smith@school.com",
                        Phone = "123-456-7891",
                        DateOfBirth = new DateTime(2009, 3, 22),
                        Gender = "Female",
                        Address = "456 Oak Ave"
                    }
                };
            }
        }

        private bool FilterStudents(StudentResponse student)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return true;

            return (student.FirstName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (student.LastName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (student.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private async Task DeleteStudent(Guid id)
        {
            try
            {
                var response = await Http.DeleteAsync($"http://localhost:5000/api/students/{id}");
                if (response.IsSuccessStatusCode)
                {
                    students = await Http.GetFromJsonAsync<List<StudentResponse>>("http://localhost:5000/api/students");
                }
                else
                {
                    // Handle error - for demo, just remove from local list
                    students?.RemoveAll(s => s.Id == id);
                }
            }
            catch
            {
                // Fallback for demo - just remove from local list
                students?.RemoveAll(s => s.Id == id);
            }
            StateHasChanged();
        }
    }
}
