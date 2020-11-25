using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace HW2
{
    public class Note // Note
    {
        // Fields
        public int id { get; private set; }
        public String text { get; set; }
        public DateTime date { get; set; }

        // Methods
        public Note()
        {
            text = "";
            id = ++MainPage.globalID;
            Application.Current.Properties["globalID"] = MainPage.globalID;
        }
    }

    public partial class MainPage : ContentPage // MainPage
    {
        // Fields
        bool bOpened = false;
        public static ObservableCollection<Note> leftNotes = new ObservableCollection<Note>();
        public static ObservableCollection<Note> rightNotes = new ObservableCollection<Note>();
        public static ObservableCollection<Note> leftNotes_search = new ObservableCollection<Note>();
        public static ObservableCollection<Note> rightNotes_search = new ObservableCollection<Note>();
        public static int globalID;

        // Methods
        public MainPage() // Default constructor
        {
            InitializeComponent();
            // Get globalID
            if (Application.Current.Properties.ContainsKey("globalID"))
            {
                globalID = int.Parse(Application.Current.Properties["globalID"].ToString());
            }
            else
            {
                globalID = 0;
                Application.Current.Properties["globalID"] = globalID;
            }
            // Set title
            update_title();
            // Set binds
            BindableLayout.SetItemsSource(left, leftNotes);
            BindableLayout.SetItemsSource(right, rightNotes);
            BindableLayout.SetItemsSource(left_search, leftNotes_search);
            BindableLayout.SetItemsSource(right_search, rightNotes_search);
            // Get data from local storage
            if (Application.Current.Properties.ContainsKey("leftNotes"))
            {
                var json = Application.Current.Properties["leftNotes"].ToString();
                foreach (Note note in JsonConvert.DeserializeObject<ObservableCollection<Note>>(json))
                {
                    leftNotes.Add(note);
                }
            }
            if (Application.Current.Properties.ContainsKey("rightNotes"))
            {
                var json = Application.Current.Properties["rightNotes"].ToString();
                foreach (Note note in JsonConvert.DeserializeObject<ObservableCollection<Note>>(json))
                {
                    rightNotes.Add(note);
                }
            }
            // Create rules for update data in local storage
            leftNotes.CollectionChanged += (s, ev) =>
            {
                Application.Current.Properties["leftNotes"] = JsonConvert.SerializeObject(leftNotes);
            };
            rightNotes.CollectionChanged += (s, es) =>
            {
                Application.Current.Properties["rightNotes"] = JsonConvert.SerializeObject(rightNotes);
            };
        }


        private void Add(object sender, EventArgs e) // Calls NotePage
        {
            if (!bOpened)
            {
                bOpened = true;
                var page = new NotePage();
                page.Disappearing += (s, ev) =>
                {
                    bOpened = false;
                    update_title();
                };
                Navigation.PushAsync(page);
            }
        }

        async private void swipe(object sender, SwipedEventArgs e) // Remove note
        {
            // Variables
            bool isOpened = false;
            int find_id = int.Parse(e.Parameter.ToString());
            Note note = find_note(find_id);
            string side = find_side(note);
            int direction;
            // Animation
            if (side == "left")
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            (sender as Frame)?.TranslateTo(direction * 100, 0, 300, Easing.CubicInOut);
            // Removing data
            if (!bOpened)
            {
                isOpened = true;
                if (await DisplayAlert("Alert!", "Are you sure?", "Yes", "No"))
                {
                    await (sender as Frame)?.FadeTo(0, 500, Easing.CubicInOut);
                    // Removing from collections
                    leftNotes.Remove(note);
                    rightNotes.Remove(note);
                    leftNotes_search.Remove(note);
                    rightNotes_search.Remove(note);
                    // Fix collections' sizes
                    int size = 0;
                    if (leftNotes.Count < rightNotes.Count)
                    {
                        size = rightNotes.Count;
                        leftNotes.Add(rightNotes[size - 1]);
                        rightNotes.RemoveAt(size - 1);
                    }
                    if (leftNotes.Count - rightNotes.Count > 1)
                    {
                        size = leftNotes.Count;
                        rightNotes.Add(leftNotes[size - 1]);
                        leftNotes.RemoveAt(size - 1);
                    }
                    if (leftNotes_search.Count < rightNotes_search.Count)
                    {
                        size = rightNotes_search.Count;
                        leftNotes_search.Add(rightNotes_search[size - 1]);
                        rightNotes_search.RemoveAt(size - 1);
                    }
                    if (leftNotes_search.Count - rightNotes_search.Count > 1)
                    {
                        size = leftNotes_search.Count;
                        rightNotes_search.Add(leftNotes_search[size - 1]);
                        leftNotes_search.RemoveAt(size - 1);
                    }
                }
                else
                {
                    await (sender as Frame)?.TranslateTo(0, 0, 300, Easing.CubicInOut);
                }
            }
        }

        private void tap(object sender, EventArgs e) // open note for updating and reading
        {
            if (!bOpened)
            {
                int find_id = int.Parse((e as TappedEventArgs)?.Parameter.ToString());
                Note note = find_note(find_id);
                string side = find_side(note);
                NotePage page;
                bOpened = true;
                page = new NotePage(note, side);
                page.Disappearing += (s, ev) =>
                {
                    bOpened = false;
                };
                Navigation.PushAsync(page);
            }
        }

        private void search(object sender, TextChangedEventArgs e) // find note with text
        {
            btn.IsEnabled = (editor.Text.Length > 0);
            // Clear collections
            leftNotes_search.Clear();
            rightNotes_search.Clear();
            // Blocks' visibility 
            if (editor.Text.Length == 0)
            {
                main.IsVisible = true;
                find.IsVisible = false;
            }
            else
            {
                main.IsVisible = false;
                find.IsVisible = true;
                // Merge collections
                ObservableCollection<Note> notes = new ObservableCollection<Note>();
                foreach (Note note in leftNotes.Select(a => { return a; }))
                {
                    notes.Add(note);
                }
                foreach (Note note in rightNotes.Select(a => { return a; }))
                {
                    notes.Add(note);
                }
                // Distribute
                int i = 0;
                foreach (Note note in notes.Select(a => { return a; }).Where(a => a.text.ToLower().IndexOf(editor.Text.ToLower()) != -1))
                {
                    if (i % 2 == 0)
                    {
                        leftNotes_search.Add(note);
                    }
                    else
                    {
                        rightNotes_search.Add(note);
                    }
                    i++;
                }
            }
        }

        // Returns Note with id
        private Note find_note(int cur_id)
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            foreach (Note note in leftNotes)
            {
                notes.Add(note);
            }
            foreach (Note note in rightNotes)
            {
                notes.Add(note);
            }
            return notes.Select(a => { return a; }).Where(a => a.id == cur_id).ToList()[0];
        }

        // Returns side(string) with Note
        private string find_side(Note note)
        {
            if (leftNotes.Contains(note))
            {
                return "left";
            }
            else
            {
                return "right";
            }
        }

        // Update title
        private void update_title()
        {
            this.Title = "Notes | globalID:" + globalID.ToString();
        }

        async private void clean(object sender, EventArgs e)
        {
            editor.Text = "";
            await btn.FadeTo(0, 200, Easing.CubicInOut);
            await btn.FadeTo(1, 200, Easing.CubicInOut);
        }
    }
}
