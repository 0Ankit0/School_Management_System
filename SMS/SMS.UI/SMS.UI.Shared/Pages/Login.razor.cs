using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Authentication;

namespace SMS.UI.Shared.Pages
{
    public partial class Login : ComponentBase
    {
        private LoginRequest loginRequest = new LoginRequest();
        private string? message;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        private async Task HandleLogin()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("http://localhost:5000/api/auth/login", loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    // Store token and user info (e.g., in local storage or a state management service)
                    message = "Login successful!";
                    Navigation.NavigateTo("/"); // Redirect to home or dashboard
                }
                else
                {
                    message = "Login failed: " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                message = "An error occurred: " + ex.Message;
            }
        }
    }
}
