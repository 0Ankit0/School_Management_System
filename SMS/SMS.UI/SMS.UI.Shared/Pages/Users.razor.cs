using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Users;

namespace SMS.UI.Shared.Pages
{
    public partial class Users : ComponentBase
    {
        private List<UserResponse>? users;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            users = await Http.GetFromJsonAsync<List<UserResponse>>("http://localhost:5000/api/v1/users");
        }

        private async Task DeleteUser(Guid id)
        {
            var response = await Http.DeleteAsync($"http://localhost:5000/api/v1/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                users = await Http.GetFromJsonAsync<List<UserResponse>>("http://localhost:5000/api/v1/users");
            }
            else
            {
                // Handle error
            }
        }
    }
}
