using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using Talent.Domain;
using Talent.DataAccess.Ado;

namespace WpfClient
{
    public partial class ComplexBindingWindow : Window
    {
        PersonRepository personRepository;
        ObservableCollection<Person> persons;

        public ComplexBindingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            personRepository = new PersonRepository();
            var people = personRepository.Fetch(null).OrderBy(d => d.LastFirstName);
            persons = new ObservableCollection<Person>(people);
            this.DataContext = persons;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Person selectedPerson = PersonListBox.SelectedItem as Person;
                if (selectedPerson != null)
                {
                    //selectedPerson.IsDirty = true;
                    PersonListBox.SelectedItem = personRepository
                        .Persist(selectedPerson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // To just cancel, we could simply not call Upsert, but we
            // want to update the data in the UI without losing which item
            // has focus in the list.
            Person selectedPerson =
                PersonListBox.SelectedItem as Person;
            if (selectedPerson != null)
            {
                // the Fetch method returns a new instance of the Person
                Person updatedPerson = personRepository
                    .Fetch(selectedPerson.Id).FirstOrDefault();

                // Replace the selectedPerson in the persons collection
                // with the refreshed instance.
                int index = persons.IndexOf(selectedPerson);
                persons.Insert(index, updatedPerson);
                persons.Remove(selectedPerson);
                PersonListBox.SelectedItem = updatedPerson;
            }
        }

        private void DOBIncrement_Click(object sender, RoutedEventArgs e)
        {
            Person selectedPerson =
                PersonListBox.SelectedItem as Person;
            if (selectedPerson != null)
            {
                selectedPerson.DateOfBirth = selectedPerson.DateOfBirth.Value.AddYears(1);
            }
        }
    }
}
