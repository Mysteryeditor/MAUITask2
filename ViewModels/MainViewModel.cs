using MauiAppTask2CRUD.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppTask2CRUD.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Person> People { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _age;
        public string Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                SetProperty(ref _selectedPerson, value);
                if (_selectedPerson != null)
                {
                    Name = _selectedPerson.Name;
                    Age = _selectedPerson.Age.ToString();
                }
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public MainViewModel()
        {
            People = new ObservableCollection<Person>();
            AddPersonCommand = new Command(async () => await AddPerson());
            DeletePersonCommand = new Command(async () => await DeletePerson());

            LoadPeople();
        }

        // Load people from the database
        async Task LoadPeople()
        {
            var peopleFromDb = await App.Database.GetPeopleAsync();
            People.Clear();
            foreach (var person in peopleFromDb)
            {
                People.Add(person);
            }
        }

        // Add a new person or update an existing one
        async Task AddPerson()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Age) && int.TryParse(Age, out int age))
            {
                var person = new Person { Name = Name, Age = age };

                if (SelectedPerson != null) // Update existing person
                {
                    person.Id = SelectedPerson.Id;
                    await App.Database.SavePersonAsync(person);

                    // Update the person in the ObservableCollection
                    var index = People.IndexOf(SelectedPerson);
                    if (index != -1)
                    {
                        People[index] = person;
                    }
                }
                else // Add new person
                {
                    await App.Database.SavePersonAsync(person);
                    People.Add(person); // Directly add to ObservableCollection
                }

                // Clear inputs
                Name = string.Empty;
                Age = string.Empty;
                SelectedPerson = null; // Reset selection after save
            }
        }

        // Delete the selected person
        async Task DeletePerson()
        {
            if (SelectedPerson != null)
            {
                await App.Database.DeletePersonAsync(SelectedPerson);

                // Remove from the ObservableCollection
                People.Remove(SelectedPerson);

                // Clear selection and input fields
                Name = string.Empty;
                Age = string.Empty;
                SelectedPerson = null;
            }
        }
    }
}
