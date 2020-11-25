using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HW2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        // Fields
        string mode;
        string side;
        Note note;
        DateTime date;
        bool isTapped = false;

        // Methods
        public NotePage() // Default constructor for creating note
        {
            InitializeComponent();
            date = DateTime.Now;
            mode = "Create";
            label.Text = date.ToString() + " | 0";
        }

        public NotePage(Note cur_note, string cur_side) // Constructor for updating and reading note
        {
            InitializeComponent();
            note = cur_note;
            date = note.date;
            mode = "Update";
            label.Text = date.ToString() + " | 0";
            editor.Text = note.text;
            side = cur_side;
        }

        private void Update(object sender, EventArgs e) // Shows information
        {
            label.Text = date.ToString() + " | " + editor.Text.Length;
        }

        private void Save(object sender, EventArgs e) // Save new Note
        {
            if (!isTapped)
            {
                if (editor.Text != null && editor.Text.Length != 0)
                {
                    isTapped = true;
                    if (mode == "Create")
                    {
                        if (MainPage.leftNotes.Count > MainPage.rightNotes.Count)
                        {
                            MainPage.rightNotes.Add(new Note() { text = editor.Text, date = DateTime.Now });
                        }
                        else
                        {
                            MainPage.leftNotes.Add(new Note() { text = editor.Text, date = DateTime.Now });
                        }
                    }

                    if (mode == "Update")
                    {
                        if (note.text != editor.Text)
                        {
                            note.text = editor.Text;
                            note.date = DateTime.Now;
                            if (side == "left")
                            {
                                int index = MainPage.leftNotes.IndexOf(note);
                                MainPage.leftNotes.RemoveAt(index);
                                MainPage.leftNotes.Add(note);
                            }

                            if (side == "right")
                            {
                                int index = MainPage.rightNotes.IndexOf(note);
                                MainPage.rightNotes.RemoveAt(index);
                                MainPage.rightNotes.Add(note);
                            }
                        }
                    }
                    Navigation.PopAsync();
                }
            }
          
        }
    }
}