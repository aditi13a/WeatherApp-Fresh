using Microsoft.AspNetCore.Components;

public class ProtectedPageBase : ComponentBase
{
    [Inject] public Supabase.Client Supabase { get; set; } = default!;
    [Inject] public NavigationManager Nav { get; set; } = default!;

    protected string UserEmail = "";

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;

        var session = Supabase.Auth.CurrentSession;

        if (session == null || session.User == null || string.IsNullOrWhiteSpace(session.User.Email))
        {
            // Not logged in → Redirect to login
            Nav.NavigateTo("/auth", forceLoad: true);
        }
        else
        {
            // Save email to use inside your pages
            UserEmail = session.User.Email;
        }
    }
}
