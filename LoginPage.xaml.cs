namespace Milestone;

public partial class LoginPage : ContentPage
{
    private readonly LocalDbService _dbService;

    public LoginPage()
    {
        InitializeComponent();
        _dbService = new LocalDbService();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Seed the database tables
        await _dbService.SeedStudentDatabase();
        await _dbService.SeedTermDatabase();
        await _dbService.SeedCourseDatabase();

        // Hide the nav bar
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        var username = usernameEntry.Text?.Trim();
        var password = passwordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter both username and password.", "OK");
            return;
        }

        var student = (await _dbService.GetStudents())
                        .FirstOrDefault(s => s.Username == username);

        if (student == null)
        {
            await DisplayAlert("Login Failed", "User not found.", "OK");
            return;
        }

        var isValid = SecurityHelper.VerifyPassword(password, student.PasswordHash, student.Salt);

        // Unsuccessful login
        if (!isValid)
        {
            await DisplayAlert("Login Failed", "Incorrect password.", "OK");
            return;
        }

        // Successful login
        await DisplayAlert("Welcome!", $"Logged in as {student.Name}", "OK");
        await SecureStorage.Default.SetAsync("studentId", student.StudentId.ToString());

        // Navigate to WelcomePage
        await Navigation.PushAsync(new WelcomePage());
    }


    private async void notificationsSettingsButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new NotificationsPage());
    }
    private async void courseListPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new CourseListPage());
    }

    private async void termListPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new TermListPage());
    }

}