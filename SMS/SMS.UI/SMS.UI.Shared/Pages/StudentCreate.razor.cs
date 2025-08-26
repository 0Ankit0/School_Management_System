using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Students;

namespace SMS.UI.Shared.Pages
{
    public partial class StudentCreate : ComponentBase
    {
        private CreateStudentRequest createStudentRequest = new CreateStudentRequest();
        private string? message;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        private async Task HandleCreate()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("http://localhost:5000/api/students", createStudentRequest);
                if (response.IsSuccessStatusCode)
                {
                    message = "Student created successfully!";
                    Navigation.NavigateTo("/students");
                }
                else
                {
                    message = "Student creation failed: " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                message = "An error occurred: " + ex.Message;
            }
        }
    }
}
