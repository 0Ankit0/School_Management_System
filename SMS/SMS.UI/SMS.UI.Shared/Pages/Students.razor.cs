using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Students;

namespace SMS.UI.Shared.Pages
{
    public partial class Students : ComponentBase
    {
        private List<StudentResponse>? students;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            students = await Http.GetFromJsonAsync<List<StudentResponse>>("http://localhost:5000/api/students");
        }

        private async Task DeleteStudent(Guid id)
        {
            var response = await Http.DeleteAsync($"http://localhost:5000/api/students/{id}");
            if (response.IsSuccessStatusCode)
            {
                students = await Http.GetFromJsonAsync<List<StudentResponse>>("http://localhost:5000/api/students");
            }
            else
            {
                // Handle error
            }
        }
    }
}
