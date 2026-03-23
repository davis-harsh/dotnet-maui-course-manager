using System.Collections.ObjectModel;

namespace Milestone;

public partial class CourseDetailPage : ContentPage
{
    private ObservableCollection<string> _notes = new ObservableCollection<string>();
    private readonly LocalDbService _dbService;
    private int _courseId;
    private Course _course;
    private int _editNoteIndex;



    public CourseDetailPage(int courseId)
    {
        InitializeComponent();
        _dbService = new LocalDbService();
        _courseId = courseId;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourseAsync();
        NotesListView.ItemsSource = _notes;
        await LoadNotes();

        var course = await _dbService.GetCourseById(_courseId);


        var assessments = await _dbService.GetAssessmentsByCourseId(_courseId);

        if (!assessments.Any())
        {
            var perf = new Assessment
            {
                Name = $"{course.Name} (PA{course.CourseId})",
                CourseId = _courseId,
                Type = AssessmentType.Performance,
                StartDate = course.StartDate,
                EndDate = course.StartDate.AddDays(5)
            };

            var obj = new Assessment
            {
                Name = $"{course.Name} (OA{course.CourseId})",
                CourseId = _courseId,
                Type = AssessmentType.Objective,
                StartDate = course.StartDate,
                EndDate = course.StartDate.AddDays(5)
            };

            await _dbService.Create(perf);
            await _dbService.Create(obj);

            assessments = new List<Assessment> { perf, obj };
        }

        assessmentsView.ItemsSource = assessments;
    }

    private async void wgulogo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new WelcomePage());
    }

    private async void OnAssessmentTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Assessment assessment)
        {
            var action = await DisplayActionSheet("Assessment Options", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Navigation.PushModalAsync(new EditAssessmentPage(assessment));
                    break;

                case "Delete":
                    var confirm = await DisplayAlert("Delete Assessment", $"Are you sure you want to delete \"{assessment.Name}\"?", "Yes", "Cancel");
                    if (confirm)
                    {
                        await _dbService.Delete(assessment);
                        await DisplayAlert("Deleted", "The assessment has been deleted.", "OK");

                        var updated = await _dbService.GetAssessmentsByCourseId(_courseId);
                        assessmentsView.ItemsSource = updated;
                    }
                    break;
            }
        }
    }

    private async void editCourseDetailPageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new EditCourseDetailPage(_course.CourseId));
    }
    private async void closePageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
    private async void notificationSettingsButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new NotificationsPage());
    }

    private async void deleteCourseButton_Clicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Delete Course", $"Are you sure you want to delete \"{_course.Name}\"?", "Yes", "Cancel");
        if (confirm)
        {
            await _dbService.Delete(_course);
            await DisplayAlert("Deleted", "The course has been removed.", "OK");
            await Navigation.PopModalAsync();
        }
    }


    private async Task ShareNote(string note)
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = note,
            Title = "Share Note"
        });
    }

    private HashSet<string> _hiddenNotes = new HashSet<string>();

    private async void ToggleNoteVisibility(string noteContent)
    {
        var notes = await _dbService.GetNotes(_courseId, includeHidden: true);
        var targetNote = notes.FirstOrDefault(n => n.Content == noteContent);

        if (targetNote != null)
        {
            targetNote.IsHidden = !targetNote.IsHidden;
            await _dbService.Update(targetNote);

            var visibleNotes = await _dbService.GetNotes(_courseId);
            _notes.Clear();
            foreach (var note in visibleNotes)
            {
                _notes.Add(note.Content);
            }

            NotesListView.ItemsSource = _notes;
        }
    }

    private async void OnShowAllNotes_Clicked(object sender, EventArgs e)
    {
        var allNotes = await _dbService.GetNotes(_courseId, includeHidden: true);

        _notes.Clear();
        foreach (var note in allNotes)
        {
            _notes.Add(note.Content);
        }

        NotesListView.ItemsSource = _notes;
    }
    private void RefreshNotesList()
    {
        NotesListView.ItemsSource = _notes.Where(n => !_hiddenNotes.Contains(n)).ToList();
    }


    private async Task LoadCourseAsync()
    {
        if (_courseId > 0)
        {
            _course = await _dbService.GetCourseById(_courseId);
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

    private async Task LoadNotes()
    {
        List<Note> savedNotes = await _dbService.GetNotes(_courseId);

        _notes.Clear();
        foreach (var note in savedNotes)
        {
            _notes.Add(note.Content);
        }

        OnPropertyChanged(nameof(_notes));
    }

    private async void OnAddNote_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NotesEntry.Text))
        {
            Note newNote = new Note { CourseId = _courseId, Content = NotesEntry.Text };
            await _dbService.AddNote(newNote);

            _notes.Add(NotesEntry.Text);
            NotesEntry.Text = string.Empty;
            RefreshNotesList();
        }
    }
    private async void notesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is string noteContent)
        {
            var allNotes = await _dbService.GetNotes(_courseId, includeHidden: true);
            var note = allNotes.FirstOrDefault(n => n.Content == noteContent);

            if (note == null)
                return;

            var action = await DisplayActionSheet("Action", "Cancel", null, "Share", note.IsHidden ? "Unhide" : "Hide", "Delete");

            switch (action)
            {
                case "Share":
                    await ShareNote(note.Content);
                    break;

                case "Hide":
                case "Unhide":
                    note.IsHidden = !note.IsHidden;
                    await _dbService.Update(note);
                    var updatedNotes = await _dbService.GetNotes(_courseId);
                    _notes.Clear();
                    foreach (var n in updatedNotes)
                        _notes.Add(n.Content);
                    NotesListView.ItemsSource = _notes;
                    break;

                case "Delete":
                    await _dbService.DeleteNote(note.NoteId);
                    _notes.Remove(note.Content);
                    NotesListView.ItemsSource = _notes;
                    break;
            }
        }
    }

    private async void addAssessmentButton_Clicked(object sender, EventArgs e)
    {
        var assessments = await _dbService.GetAssessmentsByCourseId(_courseId);

        if (assessments.Count >= 2)
        {
            await DisplayAlert("Assessment Limit Reached", "Each course may only have 2 assessments.", "OK");
            return;
        }

        await Navigation.PushModalAsync(new AddAssessmentPage(_courseId));
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

    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        scrollToTopButton.IsVisible = e.ScrollY > 50;
    }

    private async void scrollToTopButton_Clicked(object sender, EventArgs e)
    {
        await courseDetailScrollView.ScrollToAsync(0, 0, true);
    }
}
