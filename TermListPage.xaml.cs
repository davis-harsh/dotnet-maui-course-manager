namespace Milestone;

public partial class TermListPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editTermId;

    public TermListPage()
    {
        InitializeComponent();
        _dbService = new LocalDbService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var terms = await _dbService.GetTerms();
        termListView.ItemsSource = terms;

    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void termListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var term = (Term)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                _editTermId = term.TermId;
                await Navigation.PushModalAsync(new EditTermPage(term.TermId));

                break;
            case "Delete":
                var confirm = await DisplayAlert("Delete Term", $"Are you sure you want to delete \"{term.Name}\"?", "Yes", "Cancel");
                if (confirm)
                {
                    await _dbService.Delete(term);
                    await DisplayAlert("Deleted", "The term has been deleted.", "OK");
                    termListView.ItemsSource = await _dbService.GetTerms();
                }
                break;
        }
    }

    private async void coursePageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new CourseListPage());
    }

    private async void closePageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void addTermButton_Clicked(object sender, EventArgs e)
    {
        var existingTerms = await _dbService.GetTerms();

        // Generate next term number
        int nextTermNumber = existingTerms.Count + 1;

        // Get latest end date
        var latestTerm = existingTerms
            .OrderByDescending(t => t.EndDate)
            .FirstOrDefault();

        DateTime suggestedStart = latestTerm != null
            ? latestTerm.EndDate.AddDays(1)
            : DateTime.Today;

        var newTerm = new Term
        {
            Name = $"Term {nextTermNumber}",
            StartDate = suggestedStart,
            EndDate = suggestedStart.AddMonths(6).AddDays(-1)
        };

        await _dbService.Create(newTerm);

        await DisplayAlert("Term Added", $"'{newTerm.Name}' has been created.", "OK");

        // Refresh list
        var updatedTerms = await _dbService.GetTerms();
        termListView.ItemsSource = updatedTerms;
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