using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SMS.Contracts.Users;

namespace SMS.UI.Shared.Pages
{
    public partial class UserDetails : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        private UserResponse? user;

        [Inject]
        public HttpClient Http { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            user = await Http.GetFromJsonAsync<UserResponse>($"http://localhost:5000/api/v1/users/{Id}");
        }
    }
}
