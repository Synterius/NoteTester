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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoteTester.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Private fields
        private const int LINE_COUNT = 5;
        private const string OK = "../Resources/ok.png";
        private const string WRONG = "../Resources/wrong.png";
        private double topAdditionalLength = 70;
        private double bottomAdditionalLength = 10;
        private const double NOTE_LEFT_EDGE = 60;
        private const double LINE_LEFT_EDGE = 10;
        private const double OFFSET = 25;

        private Visibility _isTrebleСlefVisible = Visibility.Visible;
        private Visibility _isNotesVisible = Visibility.Hidden;
        private Visibility _isMessageVisible = Visibility.Hidden;

        private double _noteOffset = NOTE_LEFT_EDGE;
        private double _lineOffset = LINE_LEFT_EDGE;
        private int _selectedIndex = 0;
        private Random random = new Random();
        private int randIndex = -1;
        private int cache;
        private string _image;
        private string _message;
        //private bool _isDisabled = true;

        // Pablic properties
        public ICommand Exit_ClickCommand { get; }
        public ICommand Start_ClickCommand { get; }
        public ICommand CheckBox_ClickCommand { get; }
        public ICommand CheckBox_Oct1_SelectAll_ClicCommand { get; }
        public ICommand CheckBox_Oct2_SelectAll_ClicCommand { get; }
        public ICommand Button_CheckAnswer_ClickCommand { get; }
        public ObservableCollection<Note> ItemsToShowInCanvas { get; set; } = new ObservableCollection<Note>();
        public ObservableCollection<NotesLine> LinesToCanvas { get; set; } = new ObservableCollection<NotesLine>();
        public ObservableCollection<string> ButtonList { get; set; } = new ObservableCollection<string>();

        public List<Note> originalNotes = new List<Note>();
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

        public string Image
        {
            get => _image;
            set
            {
                SetProperty(ref _image, value);
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
            }
        }

        public Visibility IsMessageVisible
        {
            get => _isMessageVisible;
            set
            {
                SetProperty(ref _isMessageVisible, value);
            }
        }

        public MainWindowViewModel()
        {
            // Initialize commands
            Exit_ClickCommand = new BaseCommand(o => { Exit(); });
            Start_ClickCommand = new BaseCommand(o => { Start(); });    //, f => { return _isDisabled;
            CheckBox_ClickCommand = new BaseCommand(o => { Change(o); });       //, f => { return _isDisabled;
            CheckBox_Oct1_SelectAll_ClicCommand = new BaseCommand(o => { SelectAll(o, true); });
            CheckBox_Oct2_SelectAll_ClicCommand = new BaseCommand(o => { SelectAll(o, false); });
            Button_CheckAnswer_ClickCommand = new BaseCommand(o => { CheckAnswer(o); });
            PresetNotes();
            SetDefaultNoteLines();
        }

        private void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        private void PresetNotes()
        {
            originalNotes.Add(new Note { Name = "До 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 65 });
            originalNotes.Add(new Note { Name = "Ре 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 60 });
            originalNotes.Add(new Note { Name = "Ми 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 55 });
            originalNotes.Add(new Note { Name = "Фа 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 50 });
            originalNotes.Add(new Note { Name = "Соль 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 45 });
            originalNotes.Add(new Note { Name = "Ля 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 40 });
            originalNotes.Add(new Note { Name = "Си 1", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 35 });

            originalNotes.Add(new Note { Name = "До 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 30 });
            originalNotes.Add(new Note { Name = "Ре 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 25 });
            originalNotes.Add(new Note { Name = "Ми 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 20 });
            originalNotes.Add(new Note { Name = "Фа 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 15 });
            originalNotes.Add(new Note { Name = "Соль 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 10 });
            originalNotes.Add(new Note { Name = "Ля 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 5 });
            originalNotes.Add(new Note { Name = "Си 2", Color = Brushes.Black, Left = NOTE_LEFT_EDGE, Top = 0 });
        }


        private void SetOct(int octNumber)
        {
            var tempList = originalNotes.Where(x => x.Name.Contains(octNumber.ToString())).ToList();
            notes = notes.Union(tempList, new NotesComparer()).ToList();
        }

        private void RemoveOct(int octNumber)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Name.Contains(octNumber.ToString()))
                {
                    notes.RemoveAt(i);
                    --i;
                }
            }
        }

        private void SelectAll(object param, bool isOct1)
        {
            if (param != null)
            {
                if (bool.Parse(param.ToString()))   // add all
                {
                    if (isOct1)
                    {
                        SetOct(1);
                    }
                    else
                    {
                        SetOct(2);
                    }
                }
                else    // remove one octave
                {
                    if (isOct1)
                    {
                        RemoveOct(1);
                    }
                    else
                    {
                        RemoveOct(2);
                    }
                }
            }
        }

        private void Start()
        {
            if (notes.Count == 0)
            {
                MessageBox.Show("Не была выбрана ни одна нота!", "Внимание", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //_isDisabled = false;

            // Calculation
            if(notes.Count != 1)
            {
                do
                {
                    cache = randIndex;
                    randIndex = random.Next(0, notes.Count);
                }
                while (randIndex == cache);
            }
            else
            {
                randIndex = 0;
            }

            Note tempNote = notes[randIndex];

            if (SelectedIndex == 0)  // By one note
            {
                tempNote.Left = NOTE_LEFT_EDGE;
                ItemsToShowInCanvas.Clear();
            }
            else    // Sequence
            {
                if (cache != -1)
                {
                    _noteOffset += OFFSET;
                    tempNote.Left = _noteOffset;
                }
                if (ItemsToShowInCanvas.Count == 14)
                {
                    // Show result
                    

                    // Clear list
                    ItemsToShowInCanvas.Clear();

                    // Reset offsets
                    _noteOffset = NOTE_LEFT_EDGE;
                    _lineOffset = LINE_LEFT_EDGE;
                }
            }

            AddLine(tempNote);

            ItemsToShowInCanvas.Add(notes[randIndex]);

            IsNotesVisible = Visibility.Visible;

            // Generate buttons
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            ButtonList.Clear();
            foreach (Note note in notes)
            {
                ButtonList.Add(note.Name);
            }
        }

        private void CheckAnswer(object parameter)
        {
            string originalNote = notes[randIndex].Name;
            if (originalNote.Equals(parameter))
            {
                Image = OK;
                Message = $"Верно! Это нота была {originalNote}";
                notes[randIndex].Status = true;
            }
            else
            {
                Image = WRONG;
                Message = $"Неверно! Это была нота {originalNote}";
                notes[randIndex].Status = false;
            }

            IsMessageVisible = Visibility.Visible;
            Start();
        }

        private void Change(object parameter)
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

                    notes.Add(originalNotes.Single(x => x.Name == name));
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

        private void AddLine(Note pNote)
        {
            double add_x1 = 45;
            double add_x2 = 70;
            double left = LINE_LEFT_EDGE;
            
            if(SelectedIndex == 0)
            {
                left = LINE_LEFT_EDGE;
            }
            else
            {
                if(left != LINE_LEFT_EDGE)
                {
                    _lineOffset += OFFSET;
                    left = _lineOffset;
                }
            }

            switch (pNote.Name)
            {
                case "До 1":
                    {
                        if (LinesToCanvas.Count == LINE_COUNT)
                        {
                            // Insert index 5
                            LinesToCanvas.Insert(5, new NotesLine()
                            {
                                X1 = add_x1,
                                X2 = add_x2,
                                Left = left,
                                Top = topAdditionalLength
                            });
                        }
                        else
                        {
                            RemoveLine();
                        }
                        
                        break;
                    }
                case "Ля 2":
                case "Си 2":
                    {
                        if (LinesToCanvas.Count == LINE_COUNT)
                        {
                            // Insert index 0
                            LinesToCanvas.Insert(0, new NotesLine()
                            {
                                X1 = add_x1,
                                X2 = add_x2,
                                Left = left,
                                Top = bottomAdditionalLength
                            });
                        }
                        else
                        {
                            RemoveLine();
                        }
                        break;
                    }
                default:
                    {
                        RemoveLine();
                        break;
                    }
            }
        }

        private void RemoveLine()
        {
            for (int i = 0; i < LinesToCanvas.Count; i++)
            {
                if (LinesToCanvas[i].Top == 70)
                {
                    LinesToCanvas.RemoveAt(5);
                }
                else if (LinesToCanvas[i].Top == 10)
                {
                    LinesToCanvas.RemoveAt(0);
                }
            }
        }

        private void SetDefaultNoteLines()
        {
            double x1 = 0;
            double x2 = 415;
            double left = 10;

            for (int i = 20; i <= 60; i += 10)
            {
                LinesToCanvas.Add(new NotesLine() { X1 = x1, X2 = x2, Left = left, Top = i });
            }
        }
    }
}
