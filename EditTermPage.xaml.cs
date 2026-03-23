namespace Milestone;

public partial class EditTermPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editTermId;
    private Term _term;
    public EditTermPage(int termId)
    {
        InitializeComponent();
        _dbService = new LocalDbService();
        _editTermId = termId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _term = await _dbService.GetTermById(_editTermId);

        if (_term != null)
        {
            termNameEntryField.Text = _term.Name;
            startDatePicker.Date = _term.StartDate;
            endDatePicker.Date = _term.EndDate;
        }
        else
        {
            await DisplayAlert("Error", "Term not found.", "OK");
            await Navigation.PopModalAsync();
        }

    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void termSaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(termNameEntryField.Text))
        {
            await DisplayAlert("Validation Error", "Term name cannot be empty.", "OK");
            return;
        }

        if (endDatePicker.Date < startDatePicker.Date)
        {
            await DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        _term.Name = termNameEntryField.Text;
        _term.StartDate = startDatePicker.Date;
        _term.EndDate = endDatePicker.Date;

        await _dbService.Update(_term);
        await DisplayAlert("Success", "Term updated successfully.", "OK");
        await Navigation.PopModalAsync();
    }

    private void closePageButton_Clicked(object sender, EventArgs e)
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