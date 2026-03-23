using System.Text.RegularExpressions;

namespace Milestone;

public partial class AddCoursePage : ContentPage
{
    private readonly LocalDbService _dbService;
    private const int MaxCoursesPerTerm = 6;
    public AddCoursePage()
    {
        InitializeComponent();
        _dbService = new LocalDbService();
    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        //Null checks & data validation

        if (string.IsNullOrWhiteSpace(courseNameEntryField.Text) ||
            string.IsNullOrWhiteSpace(courseInstructorEntryField.Text) ||
            string.IsNullOrWhiteSpace(coursePhoneEntryField.Text) ||
            !int.TryParse(termIdEntryField.Text, out int termId) ||
            statusPicker.SelectedItem is not CourseStatus selectedStatus)
        {
            await DisplayAlert("Validation Error", "Please fill out all required fields and enter a valid Term ID.", "OK");
            return;
        }

        if (!Regex.IsMatch(courseEmailEntryField.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
            return;
        }

        if (endDatePicker.Date < startDatePicker.Date)
        {
            await DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        var term = await _dbService.GetTermById(termId);
        if (term == null)
        {
            await DisplayAlert("Invalid Term", $"No term with ID {termId} exists.", "OK");
            return;
        }

        if (!int.TryParse(courseCUsEntryField.Text?.Trim(), out int parsedCUs))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid number of credit units (CUs).", "OK");
            return;
        }

        var existingCourses = await _dbService.GetCoursesByTerm(termId);
        if (existingCourses.Count >= MaxCoursesPerTerm)
        {
            await DisplayAlert("Term Capacity Reached",
                $"Term {termId} already has {MaxCoursesPerTerm} courses. No additional courses can be added.",
                "OK");
            return;
        }

        int newCourseParsedCUs = 0;
        int.TryParse(courseCUsEntryField.Text, out newCourseParsedCUs);
        var newCourse = new Course
        {
            Name = courseNameEntryField.Text,
            Summary = courseSummaryEntryField.Text,
            Description = courseDescriptionEntryField.Text,
            Instructor = courseInstructorEntryField.Text,
            Phone = coursePhoneEntryField.Text,
            Email = courseEmailEntryField.Text,
            StartDate = startDatePicker.Date,
            EndDate = endDatePicker.Date,
            Status = selectedStatus,
            TermId = termId,
            CUs = newCourseParsedCUs
        };

        await _dbService.Create(newCourse);

        await DisplayAlert("Success", "Course added successfully.", "OK");
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