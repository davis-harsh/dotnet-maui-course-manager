namespace Milestone;

public partial class CourseListPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private List<TermGroup> _allGroupedCourses;
    private int _editCourseId;
    private string _editCourseSummary;
    private int _editTermId;

    public CourseListPage()
    {
        InitializeComponent();
        _dbService = new LocalDbService();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        courseSearchBar.Text = string.Empty;

        var courses = await _dbService.GetCourses();
        var terms = await _dbService.GetTerms();

        var grouped = terms
            .Select(term => new TermGroup(
                term.Name,
                term.StartDate,
                term.EndDate,
                courses.Where(c => c.TermId == term.TermId)))
            .ToList();

        courseListView.ItemsSource = grouped;


    }

    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var query = e.NewTextValue?.ToLower()?.Trim();

        if (string.IsNullOrWhiteSpace(query))
        {
            searchListView.ItemsSource = null;
            return;
        }

        var allCourses = await _dbService.GetCourses();
        var filtered = allCourses
            .Where(c => !string.IsNullOrWhiteSpace(c.Name) && c.Name.ToLower().Contains(query))
            .ToList();

        searchListView.ItemsSource = filtered;
    }

    private async void courseListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var course = (Course)e.Item;
        await Navigation.PushModalAsync(new CourseDetailPage(course.CourseId));

    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void addCourseButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddCoursePage());
    }

    private async void termListPageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new TermListPage());
    }


    private async void closePageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }


    private async void notificationsSettingsButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new NotificationsPage());
    }
    private async void courseListPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new CourseListPage());
    }

    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        scrollToTopButton.IsVisible = e.ScrollY > 100;
    }

    private async void scrollToTopButton_Clicked(object sender, EventArgs e)
    {
        await coursesScrollView.ScrollToAsync(0, 0, true);
    }
}