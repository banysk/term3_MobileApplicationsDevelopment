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
    public class Note
    {
        // Fields
        public int id { get; private set; }
        public String text { get; set; }
        public DateTime date { get; set; }

        // Methods
        public Note()
        {
            text = "";
            id = ++App.globalID;
            Application.Current.Properties["globalID"] = App.globalID;
        }
    }

    public partial class MainPage : ContentPage
    {
        // Variables
        bool bOpened = false;
        public static ObservableCollection<Note> leftNotes = new ObservableCollection<Note>();
        public static ObservableCollection<Note> rightNotes = new ObservableCollection<Note>();
        public static ObservableCollection<Note> leftNotes_search = new ObservableCollection<Note>();
        public static ObservableCollection<Note> rightNotes_search = new ObservableCollection<Note>();

        public MainPage()
        {
            InitializeComponent();
            // Get globalID
            if (Application.Current.Properties.ContainsKey("id"))
            {
                App.globalID = int.Parse(Application.Current.Properties["globalID"].ToString());
            }
            else
            {
                Application.Current.Properties["globalID"] = App.globalID;
            }
            // Binds
            BindableLayout.SetItemsSource(left, leftNotes);
            BindableLayout.SetItemsSource(right, rightNotes);
            BindableLayout.SetItemsSource(left_search, leftNotes_search);
            BindableLayout.SetItemsSource(right_search, rightNotes_search);
            // Get data
            if (Application.Current.Properties.ContainsKey("leftNotes"))
            {
                var json = Application.Current.Properties["leftNotes"];
                foreach (Note note in JsonConvert.DeserializeObject<ObservableCollection<Note>>(json.ToString()))
                {
                    leftNotes.Add(note);
                }
            }
            if (Application.Current.Properties.ContainsKey("rightNotes"))
            {
                var json = Application.Current.Properties["rightNotes"];
                foreach (Note note in JsonConvert.DeserializeObject<ObservableCollection<Note>>(json.ToString()))
                {
                    rightNotes.Add(note);
                }
            }
            // Update data
            leftNotes.CollectionChanged += (s, ev) =>
            {
                Application.Current.Properties["leftNotes"] = JsonConvert.SerializeObject(leftNotes);
            };
            rightNotes.CollectionChanged += (s, es) =>
            {
                Application.Current.Properties["rightNotes"] = JsonConvert.SerializeObject(rightNotes);
            };
        }


        private void Add(object sender, EventArgs e)
        {
            if (!bOpened)
            {
                bOpened = true;
                var page = new NotePage();
                Navigation.PushAsync(page);
                page.Disappearing += (s, ev) =>
                {
                    bOpened = false;
                };
            }
        }

        async private void swipe(object sender, SwipedEventArgs e)
        {
            // Variables
            bool isOpened = false;
            // magic
            int find_id = int.Parse(e.Parameter.ToString());
            // not magic
            Note note = find_note(find_id);
            if (!bOpened)
            {
                isOpened = true;
                if (await DisplayAlert("Alert!", "Are you sure?", "Yes", "No"))
                {
                    leftNotes.Remove(note);
                    rightNotes.Remove(note);
                    leftNotes_search.Remove(note);
                    rightNotes_search.Remove(note);
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
            }
        }

        private void left_tap(object sender, EventArgs e)
        {
            tap(sender, e as TappedEventArgs, "left");
        }

        private void right_tap(object sender, EventArgs e)
        {
            tap(sender, e as TappedEventArgs, "right");
        }

        private void tap(object sender, TappedEventArgs e, string side)
        {
            if (!bOpened)
            {
                // magic
                int find_id = int.Parse(e.Parameter.ToString());
                // not magic
                Note note = null;
                NotePage page;
                bOpened = true;
                note = find_note(find_id);
                page = new NotePage(note, side);
                page.Disappearing += (s, ev) =>
                {
                    bOpened = false;
                };
                Navigation.PushAsync(page);
            }
        }

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

        private void search(object sender, TextChangedEventArgs e) // fixed
        {
            leftNotes_search.Clear();
            rightNotes_search.Clear();
            if (editor.Text.Length == 0)
            {
                main.IsVisible = true;
                find.IsVisible = false;
            }
            else
            {
                main.IsVisible = false;
                find.IsVisible = true;
                ObservableCollection<Note> notes = new ObservableCollection<Note>();
                foreach (Note note in leftNotes.Select(a => { return a; }))
                {
                    notes.Add(note);
                }
                foreach (Note note in rightNotes.Select(a => { return a; }))
                {
                    notes.Add(note);
                }
                int i = 0;
                foreach (Note note in notes.Select(a => { return a; }).Where(a => a.text.IndexOf(editor.Text) != -1))
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
    }
}
