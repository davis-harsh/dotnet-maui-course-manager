using System.Text.RegularExpressions;

namespace Milestone;

public partial class EditCourseDetailPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editCourseId;
    private Course _course;
    private string _editCourseSummary;
    private int _editTermId;
    private int _editCourseCUs;



    public EditCourseDetailPage(int courseId)
    {
        InitializeComponent();
        _dbService = new LocalDbService();
        _editCourseId = courseId;

    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _course = await _dbService.GetCourseById(_editCourseId);

        if (_course != null)
        {
            _editTermId = _course.TermId;
            _editCourseSummary = _course.Summary;
            _editCourseCUs = _course.CUs;
            courseNameEntryField.Text = _course.Name;
            courseDescriptionEntryField.Text = _course.Description;
            courseInstructorEntryField.Text = _course.Instructor;
            coursePhoneEntryField.Text = _course.Phone;
            courseEmailEntryField.Text = _course.Email;
            startDatePicker.Date = _course.StartDate;
            endDatePicker.Date = _course.EndDate;
            statusPicker.SelectedItem = _course.Status;
        }
        else
        {
            await DisplayAlert("Error", "Course not found.", "OK");
            await Navigation.PopModalAsync();
        }
    }

    private async Task LoadCourseAsync()
    {
        if (_editCourseId > 0)
        {
            _course = await _dbService.GetCourseById(_editCourseId);
            if (_course != null)
            {
                BindingContext = _course;
            }
            else
            {
                await DisplayAlert("Error", "Course not found.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private async void closePageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void courseSaveButton_Clicked(object sender, EventArgs e)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(courseNameEntryField.Text) ||
            string.IsNullOrWhiteSpace(courseInstructorEntryField.Text) ||
            string.IsNullOrWhiteSpace(coursePhoneEntryField.Text) ||
            statusPicker.SelectedItem is not CourseStatus selectedStatus)
        {
            await DisplayAlert("Validation Error", "Please fill out all required fields.", "OK");
            return;
        }
        // Validate email format
        if (!Regex.IsMatch(courseEmailEntryField.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
            return;
        }

        // Check dates
        if (endDatePicker.Date < startDatePicker.Date)
        {
            await DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        if (_editCourseId == 0)
        {
            //Add Course
            var newCourse = new Course
            {
                Name = courseNameEntryField.Text,
                Description = courseDescriptionEntryField.Text,
                Instructor = courseInstructorEntryField.Text,
                Phone = coursePhoneEntryField.Text,
                Email = courseEmailEntryField.Text,
                StartDate = startDatePicker.Date,
                EndDate = endDatePicker.Date,
                TermId = 6,
                Status = selectedStatus,
                CompletedOn = selectedStatus == CourseStatus.Completed ? DateTime.Now : null
            };
            await _dbService.Create(newCourse);
            await DisplayAlert("Success", "Course added successfully.", "OK");
            Notifications.CourseNotifications(newCourse);
        }

        else
        {
            //Edit Course
            var updatedCourse = new Course
            {
                CourseId = _editCourseId,
                Summary = _editCourseSummary,
                CUs = _editCourseCUs,
                TermId = _editTermId,
                Name = courseNameEntryField.Text,
                Description = courseDescriptionEntryField.Text,
                Instructor = courseInstructorEntryField.Text,
                Phone = coursePhoneEntryField.Text,
                Email = courseEmailEntryField.Text,
                StartDate = startDatePicker.Date,
                EndDate = endDatePicker.Date,
                Status = selectedStatus,
                CompletedOn = selectedStatus == CourseStatus.Completed && _course.Status != CourseStatus.Completed
                      ? DateTime.Now
                      : _course.CompletedOn

            };
            await _dbService.Update(updatedCourse);
            await DisplayAlert("Success", "Course updated successfully.", "OK");
            Notifications.CourseNotifications(updatedCourse);

            _editCourseId = 0;
        }
        await Navigation.PopModalAsync();
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
