using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Users;
using SMS.Contracts.Roles;

namespace SMS.UI.Shared.Pages
{
    public partial class UserCreate : ComponentBase
    {
        private CreateUserRequest createUserRequest = new CreateUserRequest();
        private List<RoleDto>? roles;
        private string? message;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            roles = await Http.GetFromJsonAsync<List<RoleDto>>("http://localhost:5000/api/v1/users/roles");
        }

        private async Task HandleCreate()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("http://localhost:5000/api/users", createUserRequest);
                if (response.IsSuccessStatusCode)
                {
                    message = "User created successfully!";
                    Navigation.NavigateTo("/users");
                }
                else
                {
                    message = "User creation failed: " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                message = "An error occurred: " + ex.Message;
            }
        }
    }
}
