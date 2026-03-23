using Plugin.LocalNotification;

namespace Milestone
{
    public partial class NotificationsPage : ContentPage
    {

        public NotificationsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void closePageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void wgulogo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new WelcomePage());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            courseNotificationSwitch.IsToggled = AppSettings.CourseNotificationsEnabled;
            assessmentNotificationSwitch.IsToggled = AppSettings.AssessmentNotificationsEnabled;

            var deliveredNotifications = await LocalNotificationCenter.Current.GetDeliveredNotificationList();

            var notificationsListed = 10;
            var limited = deliveredNotifications
                .OrderByDescending(n => n.Schedule?.NotifyTime ?? DateTime.MinValue)
                .Take(notificationsListed)
                .ToList();

            deliveredListView.ItemsSource = limited;
        }

        private void OnCourseToggled(object sender, ToggledEventArgs e)
        {
            AppSettings.CourseNotificationsEnabled = e.Value;
        }

        private void OnAssessmentToggled(object sender, ToggledEventArgs e)
        {
            AppSettings.AssessmentNotificationsEnabled = e.Value;
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
}