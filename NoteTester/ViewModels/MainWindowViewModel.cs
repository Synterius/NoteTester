using NoteTester.Commands;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.ObjectModel;
using NoteTester.Models;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Linq;

namespace NoteTester.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Private fields
        private Visibility _isTrebleСlefVisible = Visibility.Visible;
        private Visibility _isNotesVisible = Visibility.Hidden;
        private int _selectedIndex = 0;
        private Random random = new Random();
        private int randIndex = -1;
        private int cache;

        // Pablic properties
        public ICommand Start_ClickCommand { get; }
        public ICommand CheckBox_ClickCommand { get; }
        public ObservableCollection<Note> ItemsToShowInCanvas { get; set; } = new ObservableCollection<Note>();
        public ObservableCollection<NotesLine> LinesToCanvas { get; set; } = new ObservableCollection<NotesLine>();

        public List<Note> notes = new List<Note>();

        public Visibility IsTrebleСlefVisible
        {
            get => _isTrebleСlefVisible;
            set
            {
                SetProperty(ref _isTrebleСlefVisible, value);
            }
        }

        public Visibility IsNotesVisible
        {
            get => _isNotesVisible;
            set
            {
                SetProperty(ref _isNotesVisible, value);
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
            }
        }

        public MainWindowViewModel()
        {
            // Initialize commands
            Start_ClickCommand = new BaseCommand(o => { Start(); });
            CheckBox_ClickCommand = new BaseCommand(o => { Change(o); });

            SetDefaultNoteLines();
        }

        void Start()
        {
            ItemsToShowInCanvas.Clear();

            // Calculation
            do
            {
                cache = randIndex;
                randIndex = random.Next(0, notes.Count);
            }
            while (randIndex == cache);

            ItemsToShowInCanvas.Add(notes[randIndex]);

            IsNotesVisible = Visibility.Visible;
        }

        void Change(object parameter)
        {
            if(parameter != null)
            {
                var param = (Tuple<object, object>)parameter;
                string name = param.Item1.ToString();
                bool isNeedToAdd = bool.Parse(param.Item2.ToString());

                if (isNeedToAdd)
                {
                    // Find mach
                    if (notes != null)
                    {
                        var list = notes.Where(x => x.Name == name);
                        if (list.Count() > 0)
                            return;
                    }

                    Note note = new Note { Name = name, Color = Brushes.Black, Left = 60 };        // 60 - temporary

                    switch (name)
                    {
                        case "До 1":
                            {
                                // Need additional line
                                note.Top = 65;
                                break;
                            }
                        case "Ре 1":
                            {
                                note.Top = 60;
                                break;
                            }
                        case "Ми 1":
                            {
                                note.Top = 55;
                                break;
                            }
                        case "Фа 1":
                            {
                                note.Top = 50;
                                break;
                            }
                        case "Соль 1":
                            {
                                note.Top = 45;
                                break;
                            }
                        case "Ля 1":
                            {
                                note.Top = 40;
                                break;
                            }
                        case "Си 1":
                            {
                                note.Top = 35;
                                break;
                            }
                        case "До 2":
                            {
                                note.Top = 30;
                                break;
                            }
                        case "Ре 2":
                            {
                                note.Top = 25;
                                break;
                            }
                        case "Ми 2":
                            {
                                note.Top = 20;
                                break;
                            }
                        case "Фа 2":
                            {
                                note.Top = 15;
                                break;
                            }
                        // Need additional line
                        case "Соль 2":
                            {
                                note.Top = 10;
                                break;
                            }
                        case "Ля 2":
                            {
                                note.Top = 5;
                                break;
                            }
                        case "Си 2":
                            {
                                note.Top = 0;
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("Что-то пошло не так, пожалуйста, перезапустите програму!");
                                break;
                            }
                    }

                    notes.Add(note);
                }
                else
                {
                    for(int i = 0; i < notes.Count; i++)
                    {
                        if (notes[i].Name.Equals(name))
                        {
                            notes.RemoveAt(i);
                        }
                    }
                }
            }
        }

        void SetDefaultNoteLines()
        {
            double x1 = 0;
            double x2 = 415;
            double left = 10;

            for(int i = 20; i <= 60; i += 10)
            {
                LinesToCanvas.Add(new NotesLine() { X1 = x1, X2 = x2, Left = left, Top = i });
            }
        }
    }
}
