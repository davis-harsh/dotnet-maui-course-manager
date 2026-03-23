
namespace Milestone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var dbService = new LocalDbService();

            MainPage = new NavigationPage(new LoginPage());


        }
    }
}

//<a href="https://www.flaticon.com/free-icons/project" title="project icons">Project icons created by Freepik - Flaticon</a>
//<a href="https://www.flaticon.com/free-icons/up-arrow" title="up arrow icons">Up arrow icons created by IconKanan - Flaticon</a>
//<a href="https://www.flaticon.com/free-icons/milestone" title="milestone icons">Milestone icons created by Maan Icons - Flaticon</a>
//<a href="https://www.flaticon.com/free-icons/house" title="house icons">House icons created by Freepik - Flaticon</a>
//<a href="https://www.flaticon.com/free-icons/stack" title="stack icons">Stack icons created by Catalin Fertu - Flaticon</a>
//< a href = "https://www.flaticon.com/free-icons/notification" title = "notification icons" > Notification icons created by ghufronagustian - Flaticon</a>