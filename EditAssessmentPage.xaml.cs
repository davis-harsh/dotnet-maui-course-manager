
namespace Milestone;

public partial class EditAssessmentPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private AssessmentType _editAssessmentType;
    private int _editAssessmentId;
    private int _editCourseId;
    public EditAssessmentPage(Assessment assessment)
    {
        InitializeComponent();
        _dbService = new LocalDbService();

        BindingContext = assessment;

        _editAssessmentId = assessment.AssessmentId;
        _editCourseId = assessment.CourseId;

        assessmentNameEntryField.Text = assessment.Name;
        startDatePicker.Date = assessment.StartDate;
        endDatePicker.Date = assessment.EndDate;

    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(assessmentNameEntryField.Text))
        {
            await DisplayAlert("Validation Error", "Please fill out all required fields.", "OK");
            return;
        }

        // Check dates
        if (endDatePicker.Date < startDatePicker.Date)
        {
            await DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        if (_editAssessmentId == 0)
        {
            // Add Assessment
            var newAssessment = new Assessment
            {
                Name = assessmentNameEntryField.Text,
                Type = _editAssessmentType,
                StartDate = startDatePicker.Date,
                EndDate = endDatePicker.Date,
                CourseId = _editCourseId
            };
            await _dbService.Create(newAssessment);
            Notifications.AssessmentNotifications(newAssessment);

        }
        else
        {
            // Edit Assessment
            var updatedAssessment = new Assessment
            {
                AssessmentId = _editAssessmentId,
                Name = assessmentNameEntryField.Text,
                Type = _editAssessmentType,
                StartDate = startDatePicker.Date,
                EndDate = endDatePicker.Date,
                CourseId = _editCourseId
            };
            await _dbService.Update(updatedAssessment);
            Notifications.AssessmentNotifications(updatedAssessment);

            _editAssessmentId = 0;
        }

        await DisplayAlert("Success", "Assessment saved successfully.", "OK");
        await Navigation.PopModalAsync();
    }

    private async void closePageButton_Clicked(object sender, EventArgs e)
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