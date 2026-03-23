namespace Milestone;

public partial class AddAssessmentPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _courseId;

    public AddAssessmentPage(int courseId)
    {
        InitializeComponent();
        _dbService = new LocalDbService();

        _courseId = courseId;
    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(assessmentNameEntryField.Text) ||
            typePicker.SelectedItem is not AssessmentType selectedType)
        {
            await DisplayAlert("Validation Error", "Please fill out all required fields and enter a valid Term ID.", "OK");
            return;
        }

        if (endDatePicker.Date < startDatePicker.Date)
        {
            await DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        var existing = await _dbService.GetAssessmentsByCourseId(_courseId);
        if (existing.Any(a => a.Type == selectedType))
        {
            await DisplayAlert("Duplicate Type",
                $"This course already has a {selectedType} assessment. Only one of each type is allowed.",
                "OK");
            return;
        }

        var newAssessment = new Assessment
        {
            Name = assessmentNameEntryField.Text,
            Type = selectedType,
            StartDate = startDatePicker.Date,
            EndDate = endDatePicker.Date,
            CourseId = _courseId
        };

        await _dbService.Create(newAssessment);
        Notifications.AssessmentNotifications(newAssessment);

        await DisplayAlert("Success", "Assessment added successfully.", "OK");
        await Navigation.PopModalAsync();
    }

    private void cancelButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void termListPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new TermListPage());
    }

    private async void notificationsSettingsButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new NotificationsPage());
    }
    private async void courseListPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new CourseListPage());
    }
}