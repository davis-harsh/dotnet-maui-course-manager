
using System.Collections.ObjectModel;

namespace Milestone;


public partial class WelcomePage : ContentPage
{
    private readonly LocalDbService _dbService;

    // Lists for reports
    public ObservableCollection<Course> ActiveCourses { get; set; } = new();
    public ObservableCollection<Course> CompletedCourses { get; set; } = new();
    public bool HasActiveCourses => ActiveCourses.Any();
    public bool HasCompletedCourses => CompletedCourses.Any();

    private readonly List<ICourseReportSection> _sections;

    public WelcomePage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _dbService = new LocalDbService();

        _sections = new List<ICourseReportSection>
        {
            new ActiveCourseReport(ActiveCourses),
            new CompletedCourseReport(CompletedCourses)
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load greeting
        var idString = await SecureStorage.Default.GetAsync("studentId");
        if (int.TryParse(idString, out int studentId))
        {
            var student = await _dbService.GetByStudentId(studentId);
            greetingLabel.Text = student != null ? $"Welcome, {student.Name}" : "Welcome!";
        }

        // Polymorphically load each report section
        foreach (var section in _sections)
            await section.LoadAsync(_dbService);

        BindingContext = this;
    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
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