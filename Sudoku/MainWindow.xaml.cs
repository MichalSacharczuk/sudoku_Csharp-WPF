using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle[,] bigRectArray = new Rectangle[3, 3];
        Rectangle[,] smallRectArray = new Rectangle[9, 9];

        #region variables for sudoku full-numbered generator
        Label[,] labelNumbersArrayInRectGrid = new Label[9, 9];

        int[,] howManyPermutations = new int[3, 3];

        List<string> nineElementList = new List<string>();
        List<string> elementListFrom1To9 = new List<string>();
        List<string> decreasedElementList = new List<string>();

        List<string> numberCheckingList = new List<string>();

        List<List<string>>[] listOfAllPermutationsOfElementsFromGroup1 = new List<List<string>>[4];
        List<List<string>>[] listOfAllPermutationsOfElementsFromGroup2 = new List<List<string>>[4];
        List<List<string>>[] listOfAllPermutationsOfElementsFromGroup3 = new List<List<string>>[4];
        List<List<string>>[] listOfAllPermutationsOfElementsFromGroup4 = new List<List<string>>[4];
        #endregion

        Button randomizeButton;

        int checkNr = 1;

        string[,] arrayOfElementsFromSolvedSudoku = new string[9, 9];

        Brush colorOfCellsFilledAtStart = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ddd"));
        Brush colorOfCellsToFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4F4F5"));
        Brush colorOfCellOnGuide = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4f4"));
        int iClicked;
        int jClicked;


        public MainWindow()
        {
            for (int i = 1; i < 10; i++)
            {
                nineElementList.Add((i).ToString());
                elementListFrom1To9.Add((i).ToString());
            }

            InitializeComponent();

            this.SizeToContent = SizeToContent.WidthAndHeight;

            randomizeButton = new Button()
            {
                Content = "Generuj!",
                Width = 70,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(312 - 35, 0, 0, 5)
            };
            randomizeButton.Click += randomizeButton_Click;
            grid1.Children.Add(randomizeButton);

            CreateSudokuGrid();

            FillSudokuGridWithLabelsForNumbers();


        }

        private void CreateSudokuGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bigRectArray[i, j] = new Rectangle()
                    {
                        Fill = colorOfCellsToFill,
                        Stroke = Brushes.Black,
                        Width = 181,
                        Height = 181,
                        StrokeThickness = 2,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new Thickness(40 + j * 181, 40 + i * 181, 0, 0)
                    };
                    grid1.Children.Add(bigRectArray[i, j]);
                }
            }

            int marginICorrection;
            int marginJCorrection;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i < 3) marginICorrection = 0;
                    else if (i > 2 && i < 6) marginICorrection = 2;
                    else marginICorrection = 4;
                    if (j < 3) marginJCorrection = 0;
                    else if (j > 2 && j < 6) marginJCorrection = 2;
                    else marginJCorrection = 4;

                    smallRectArray[i, j] = new Rectangle()
                    {
                        Fill = colorOfCellsToFill,
                        Stroke = Brushes.Gray,
                        Width = 60,
                        Height = 60,
                        StrokeThickness = 1,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new Thickness(40 + j * 60 + marginJCorrection, 40 + i * 60 + marginICorrection, 0, 0),
                        Name = "rect" + i.ToString() + j.ToString()
                    };
                    grid1.Children.Add(smallRectArray[i, j]);
                }
            }
        }

        private void FillSudokuGridWithLabelsForNumbers()
        {
            int bigCellICorrection;
            int bigCellJCorrection;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i < 3) bigCellICorrection = 0;
                    else if (i > 2 && i < 6) bigCellICorrection = 1;
                    else bigCellICorrection = 2;
                    if (j < 3) bigCellJCorrection = 0;
                    else if (j > 2 && j < 6) bigCellJCorrection = 1;
                    else bigCellJCorrection = 2;

                    labelNumbersArrayInRectGrid[i, j] = new Label()
                    {
                        Content = "",
                        FontSize = 30,
                        Width = 60,
                        Height = 60,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new Thickness(57 + j * 60 + bigCellJCorrection * 2, 45 + i * 60 + bigCellICorrection * 2, 0, 0),
                        Name = "labl" + i.ToString() + j.ToString()
                    };
                    grid1.Children.Add(labelNumbersArrayInRectGrid[i, j]);
                }
            }
        }

        private void GenerateFullFilledSudoku()
        {
            checkNr = 1;
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    smallRectArray[i, j].Stroke = Brushes.Gray;
                    smallRectArray[i, j].Fill = colorOfCellsToFill;
                    smallRectArray[i, j].StrokeThickness = 1;
                    labelNumbersArrayInRectGrid[i, j].FontWeight = FontWeights.Normal;
                    labelNumbersArrayInRectGrid[i, j].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000"));
                }
            }

            RandomizeSudokuGridNumbersBetter();
        }

        private void StoreFullFilledSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    arrayOfElementsFromSolvedSudoku[i, j] = labelNumbersArrayInRectGrid[i, j].Content.ToString();
                }
            }
        }

        private void ShowFullFilledSudoku() 
        {
            int howManyWrongFilledCells = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    smallRectArray[i, j].Stroke = Brushes.Gray;
                    smallRectArray[i, j].StrokeThickness = 1;
                    labelNumbersArrayInRectGrid[i, j].FontWeight = FontWeights.Normal;
                    if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == arrayOfElementsFromSolvedSudoku[i, j]
                        && smallRectArray[i, j].Fill != colorOfCellsFilledAtStart)
                    {
                        smallRectArray[i, j].Fill = colorOfCellsToFill;
                        labelNumbersArrayInRectGrid[i, j].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008000"));
                    }

                    else if (labelNumbersArrayInRectGrid[i, j].Content.ToString() != arrayOfElementsFromSolvedSudoku[i, j] &&
                        labelNumbersArrayInRectGrid[i, j].Content.ToString() != "" && 
                        smallRectArray[i, j].Fill != colorOfCellsFilledAtStart)
                    {
                        smallRectArray[i, j].Fill = colorOfCellsToFill;
                        labelNumbersArrayInRectGrid[i, j].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e60000"));
                        howManyWrongFilledCells++;
                    }
                    else if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == "" &&
                        smallRectArray[i, j].Fill != colorOfCellsFilledAtStart)
                    {
                        smallRectArray[i, j].Fill = colorOfCellsToFill;
                        labelNumbersArrayInRectGrid[i, j].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bb8800"));
                        howManyWrongFilledCells++;
                    }

                    labelNumbersArrayInRectGrid[i, j].Content = arrayOfElementsFromSolvedSudoku[i, j];
                }
            }

            timer.Stop();
            if (howManyWrongFilledCells == 0)
            {
                MessageBox.Show("BRAWO! Poprawnie rozwiązane sudoku! \r\n Czas: " + timeOfPlay);
                if (guideUsed)
                    MessageBox.Show("Szkoda tylko, że potrzebna była podpowiedź.");
                else
                    MessageBox.Show("I to bez podpowiedzi! Brawo!");
            }
            else if (howManyWrongFilledCells > 4)
                MessageBox.Show("Niestety, " + howManyWrongFilledCells + " pól nie zostało poprawnie uzupełnionych.");
            else if (howManyWrongFilledCells > 1)
                MessageBox.Show("Prawie! Tylko " + howManyWrongFilledCells + " pola nie zostały poprawnie uzupełnione.");
            else
                MessageBox.Show("Prawie! Tylko jedno pole nie zostało poprawnie uzupełnione.");

            RemoveEventFromClickedRects();
            btn_answer_name.IsEnabled = false;
            btn_guide_name.IsEnabled = false;
            btn_pause_name.IsEnabled = false;
        }

        private void randomizeButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime timeBefore;
            DateTime timeAfter;
            timeBefore = DateTime.Now;
            
            GenerateFullFilledSudoku();

            StoreFullFilledSudoku();

            EraseElementsFromSudokuGrid();

            MakeColorOfStartCellsDarker();

            AddEventToClickedRects();

            timeAfter = DateTime.Now;
            lbl_generationTime.Content = "Wygenerowano w: " + Math.Round((timeAfter - timeBefore).TotalSeconds, 3) + " s";

            StartTimer();

            HideGeneratedTime();

            guideUsed = false;
            btn_answer_name.IsEnabled = true;
            btn_guide_name.IsEnabled = true;
            btn_pause_name.IsEnabled = true;
        }

        private async void HideGeneratedTime()
        {
            await Task.Delay(3000);
            string genTimeString = lbl_generationTime.Content.ToString();
            string tempGenTimeString = "";
            while (genTimeString != "")
            {
                for (int i = 0; i < genTimeString.Length - 1; i++)
                {
                    tempGenTimeString += genTimeString[i];
                }
                genTimeString = tempGenTimeString;
                tempGenTimeString = "";
                lbl_generationTime.Content = genTimeString;
                await Task.Delay(15);
            }
        }

        DispatcherTimer timer = new DispatcherTimer();
        TimeSpan timeOfPlay;
        
        private void StartTimer()
        {
            timeOfPlay = new TimeSpan(0, 0, 0);
            lbl_time.Content = "Czas gry: " + timeOfPlay;
            timer.Tick -= new EventHandler(timer_tick);
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timeOfPlay += timer.Interval;
            lbl_time.Content = "Czas gry: " + timeOfPlay;
        }



        // ***********************************************************************************************
        // ***********************************************************************************************
        // ***********************************************************************************************

        #region Generate full-numbered sudoku
        private void FillFirst6BigCells()
        {
            do
            {
                do
                {
                    for (int jBig = 0; jBig < 3; jBig++)
                    {
                        ClearCellsInBigCell(0, jBig);
                    }

                    FillingFirstRow();
                    FillingSecondRow();
                    FillingThirdRow();
                    FillingBigCellAt2ndRow1stColumn();

                } while (!FillingBigCellAt2ndRow2ndColumnAndReturnIfFilledFully());

            } while (!FillingCustomBigCellAndReturnIfFilledFullyBetter(2, 0, 1));
        }

        private void FillingFirstRow()
        {
            Methods.RandomizeList(nineElementList);
            for (int j = 0; j < 9; j++)
            {
                labelNumbersArrayInRectGrid[0, j].Content = nineElementList[j];
            }
        }

        private void FillingSecondRow()
        {
            int i = 1;
            decreasedElementList.Clear();
            // creating decreased list with 6 elements:
            for (int j = 3; j < 9; j++)
            {
                decreasedElementList.Add(nineElementList[j]);
            }
            Methods.RandomizeList(decreasedElementList);
            // adding to first 3 sudoku cells:
            for (int j = 0; j < 3; j++)
            {
                labelNumbersArrayInRectGrid[i, j].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0); // 3 elements left
            }
            // decreasing the list:
            decreasedElementList.Remove(nineElementList[3]);
            decreasedElementList.Remove(nineElementList[4]);
            decreasedElementList.Remove(nineElementList[5]);
            // creating new List containing first 3 elements from nineElementsList and randomizing it:
            List<string> threeFirstElementFromNineElementsList = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                threeFirstElementFromNineElementsList.Add(nineElementList[j]);
            }
            Methods.RandomizeList(threeFirstElementFromNineElementsList);
            // adding elements to decreasing list if has less than 3 elements:
            int id = 0;
            while (decreasedElementList.Count < 3)
            {
                decreasedElementList.Add(threeFirstElementFromNineElementsList[id]);
                id++;
            }
            Methods.RandomizeList(decreasedElementList);
            // adding to second 3 sudoku cells:
            for (int j = 3; j < 6; j++)
            {
                labelNumbersArrayInRectGrid[i, j].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0);
            }
            decreasedElementList.Clear();
            // filling last 3 cells
            for (int j = 0; j < 9; j++)
            {
                decreasedElementList.Add(nineElementList[j]);
            }
            for (int j = 0; j < 6; j++)
            {
                decreasedElementList.Remove(labelNumbersArrayInRectGrid[i, j].Content.ToString());
            }
            Methods.RandomizeList(decreasedElementList);
            for (int j = 6; j < 9; j++)
            {
                labelNumbersArrayInRectGrid[i, j].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0); // 0 elements left
            }
        }

        private void FillingThirdRow()
        {
            for (int jBig = 0; jBig < 3; jBig++)
            {
                decreasedElementList.Clear();
                for (int j = 0; j < 9; j++)
                {
                    decreasedElementList.Add(elementListFrom1To9[j]);
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        decreasedElementList.Remove(labelNumbersArrayInRectGrid[i, j + jBig * 3].Content.ToString());
                    }
                }
                Methods.RandomizeList(decreasedElementList);
                for (int j = 0; j < 3; j++)
                {
                    labelNumbersArrayInRectGrid[2, j + jBig * 3].Content = decreasedElementList[0];
                    decreasedElementList.RemoveAt(0);
                }
            }

        }

        private void FillingBigCellAt2ndRow1stColumn()
        {
            nineElementList.Clear();
            decreasedElementList.Clear();
            // creating decreased list with 6 elements:
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    nineElementList.Add(labelNumbersArrayInRectGrid[i, j].Content.ToString());
                }
            }
            for (int j = 3; j < 9; j++)
            {
                decreasedElementList.Add(nineElementList[j]);
            }
            Methods.RandomizeList(decreasedElementList);
            // adding to first 3 sudoku cells:
            for (int i = 3; i < 6; i++)
            {
                labelNumbersArrayInRectGrid[i, 0].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0); // 3 elements left
            }
            // decreasing the list:
            decreasedElementList.Remove(nineElementList[3]);
            decreasedElementList.Remove(nineElementList[4]);
            decreasedElementList.Remove(nineElementList[5]);
            // creating new List containing first 3 elements from nineElementsList and randomizing it:
            List<string> threeFirstElementFromNineElementsList = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                threeFirstElementFromNineElementsList.Add(nineElementList[j]);
            }
            Methods.RandomizeList(threeFirstElementFromNineElementsList);
            // adding elements to decreasing list if has less than 3 elements:
            int id = 0;
            while (decreasedElementList.Count < 3)
            {
                decreasedElementList.Add(threeFirstElementFromNineElementsList[id]);
                id++;
            }
            Methods.RandomizeList(decreasedElementList);
            // adding to second 3 sudoku cells:
            for (int i = 3; i < 6; i++)
            {
                labelNumbersArrayInRectGrid[i, 1].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0);
            }
            decreasedElementList.Clear();
            // filling last 3 cells
            for (int j = 0; j < 9; j++)
            {
                decreasedElementList.Add(nineElementList[j]);
            }
            for (int j = 0; j < 2; j++)
            {
                for (int i = 3; i < 6; i++)
                {
                    decreasedElementList.Remove(labelNumbersArrayInRectGrid[i, j].Content.ToString());
                }
            }
            Methods.RandomizeList(decreasedElementList);
            for (int i = 3; i < 6; i++)
            {
                labelNumbersArrayInRectGrid[i, 2].Content = decreasedElementList[0];
                decreasedElementList.RemoveAt(0); // 0 elements left
            }
        }

        private bool FillingBigCellAt2ndRow2ndColumnAndReturnIfFilledFully()
        {
            List<string> sixElementList = new List<string>();
            bool cannotFillProperly;
            int howManyTries = 0;

            do
            {
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 3; j < 6; j++)
                    {
                        labelNumbersArrayInRectGrid[i, j].Content = "";
                    }
                }

                cannotFillProperly = false;
                for (int i = 3; i < 6; i++)
                {
                    sixElementList.Clear();
                    for (int n = 0; n < 9; n++)
                    {
                        sixElementList.Add(elementListFrom1To9[n]);
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        sixElementList.Remove(labelNumbersArrayInRectGrid[i, j].Content.ToString());
                    }
                    for (int j = 3; j < 6; j++)
                    {
                        Creating6ElementsListDecreasedByElementsFromUpperBigCell(sixElementList, j);
                        for (int i2 = 3; i2 < i + 1; i2++)
                        {
                            for (int j2 = 3; j2 < 6; j2++)
                            {
                                decreasedElementList.Remove(labelNumbersArrayInRectGrid[i2, j2].Content.ToString());
                            }
                        }
                        if (decreasedElementList.Count > 0) labelNumbersArrayInRectGrid[i, j].Content = decreasedElementList[0];
                        else
                        {
                            cannotFillProperly = true;
                            break;
                        }
                    }
                    if (cannotFillProperly)
                    {
                        howManyTries++;
                        break;
                    }
                }
                if (howManyTries > 10) // więcej???
                    return false;
            } while (cannotFillProperly);
            return true;
        }

        private void Creating6ElementsListDecreasedByElementsFromUpperBigCell(List<string> sixElementList, int jj)
        {
            decreasedElementList.Clear();
            for (int n = 0; n < 6; n++)
            {
                decreasedElementList.Add(sixElementList[n]);
            }
            for (int ii = 0; ii < 3; ii++)
            {
                decreasedElementList.Remove(labelNumbersArrayInRectGrid[ii, jj].Content.ToString());
            }
            Methods.RandomizeList(decreasedElementList);
        }

        private void ClearCellsInBigCell(int iBig, int jBig)
        {
            for (int i = 0 + 3 * iBig; i < 3 + 3 * iBig; i++)
            {
                for (int j = 0 + 3 * jBig; j < 3 + 3 * jBig; j++)
                {
                    labelNumbersArrayInRectGrid[i, j].Content = "";
                }
            }
        }

        private bool IfAllElementsArePlacedProperly()
        {
            for (int i = 0; i < 9; i++)
            {
                numberCheckingList.Clear();
                for (int j = 0; j < 9; j++)
                {
                    numberCheckingList.Add(labelNumbersArrayInRectGrid[i, j].Content.ToString());
                }
                numberCheckingList.Sort();
                for (int n = 0; n < 9; n++)
                {
                    if (numberCheckingList[n] != elementListFrom1To9[n]) return false;
                }
            }
            for (int j = 0; j < 9; j++)
            {
                numberCheckingList.Clear();
                for (int i = 0; i < 9; i++)
                {
                    numberCheckingList.Add(labelNumbersArrayInRectGrid[i, j].Content.ToString());
                }
                numberCheckingList.Sort();
                for (int n = 0; n < 9; n++)
                {
                    if (numberCheckingList[n] != elementListFrom1To9[n]) return false;
                }
            }
            for (int iBig = 0; iBig < 3; iBig++)
            {
                for (int jBig = 0; jBig < 3; jBig++)
                {
                    numberCheckingList.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            numberCheckingList.Add(labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content.ToString());
                        }
                    }
                    numberCheckingList.Sort();

                    for (int i = 0; i < 9; i++)
                    {
                        if (numberCheckingList[i] != elementListFrom1To9[i]) return false;
                    }
                }
            }
            return true;
        }

        private bool IfSomeCellHasOnlyOneElementThenRemoveItFromTheOtherCellsAndReturnIfOK(List<string>[,] arrayOfListOfAvailableElemensts)
        {
            int allAvailableElementsInBigCellBefore;
            int allAvailableElementsInBigCellAfter;
            do
            {
                allAvailableElementsInBigCellBefore = 0;
                allAvailableElementsInBigCellAfter = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        allAvailableElementsInBigCellBefore += arrayOfListOfAvailableElemensts[i, j].Count;
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((arrayOfListOfAvailableElemensts[i, j].Count == 0)) return false;
                        else if (arrayOfListOfAvailableElemensts[i, j].Count == 1)
                        {
                            arrayOfListOfAvailableElemensts[i, j].Add(arrayOfListOfAvailableElemensts[i, j][0]);
                            for (int ii = 0; ii < 3; ii++)
                            {
                                for (int jj = 0; jj < 3; jj++)
                                {
                                    arrayOfListOfAvailableElemensts[ii, jj].Remove(arrayOfListOfAvailableElemensts[i, j][0]);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        allAvailableElementsInBigCellAfter += arrayOfListOfAvailableElemensts[i, j].Count;
                    }
                }
            } while (allAvailableElementsInBigCellBefore != allAvailableElementsInBigCellAfter);
            return true;
        }

        private bool FillingCustomBigCellAndReturnIfFilledFullyBetter(int iBig, int jBig, int whichPermutation)
        {
            ClearCellsInBigCell(iBig, jBig);

            List<string>[,] arrayOfListOfAvailableElemensts = new List<string>[3, 3];

            #region creating array of lists od available elements in sudoku cells:
            Methods.RandomizeList(nineElementList);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayOfListOfAvailableElemensts[i, j] = new List<string>();

                    decreasedElementList.Clear();
                    decreasedElementList = new List<string>(nineElementList);

                    // checking elements from particular column:
                    for (int ii = 0; ii < 9; ii++)
                    {
                        decreasedElementList.Remove(labelNumbersArrayInRectGrid[ii, j + 3 * jBig].Content.ToString());
                    }
                    // checking elements from particular row:
                    for (int jj = 0; jj < 9; jj++)
                    {
                        decreasedElementList.Remove(labelNumbersArrayInRectGrid[i + 3 * iBig, jj].Content.ToString());
                    }
                    if (decreasedElementList.Count == 0)
                        return false;

                    arrayOfListOfAvailableElemensts[i, j] = new List<string>(decreasedElementList);
                }
            }
            #endregion


            IfSomeCellHasOnlyOneElementThenRemoveItFromTheOtherCellsAndReturnIfOK(arrayOfListOfAvailableElemensts);

            #region WhenTwoOfThreeNumbersCanBeInEachOfThreeCells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (arrayOfListOfAvailableElemensts[i, j].Count == 2)
                    {
                        bool ifBreak = false;
                        for (int ii = 0; ii < 3; ii++)
                        {
                            for (int jj = 0; jj < 3; jj++)
                            {
                                if (ii == i && jj == j) continue;
                                int howManyElementRepeats = 0;
                                if (arrayOfListOfAvailableElemensts[ii, jj].Count == 2)
                                {
                                    for (int n = 0; n < 2; n++)
                                    {
                                        if (arrayOfListOfAvailableElemensts[ii, jj][n] == arrayOfListOfAvailableElemensts[i, j][n])
                                        {
                                            howManyElementRepeats++;
                                        }
                                    }
                                    if (howManyElementRepeats == 1)
                                    {
                                        arrayOfListOfAvailableElemensts[i, j].RemoveAt(0);
                                        IfSomeCellHasOnlyOneElementThenRemoveItFromTheOtherCellsAndReturnIfOK(arrayOfListOfAvailableElemensts);
                                        ifBreak = true;
                                        break;
                                    }
                                }
                            }
                            if (ifBreak) break;
                        }
                    }
                }
            }
            #endregion

            // creating elementsForPermutation:
            ElementsForPermutation[,] elementsForPermutation = new ElementsForPermutation[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    elementsForPermutation[i, j] = new ElementsForPermutation();
                }
            }

            #region checking which cells can have the same elements:
            List<string> listOfElementsFromGroup1 = new List<string>();
            List<string> listOfElementsFromGroup2 = new List<string>();
            List<string> listOfElementsFromGroup3 = new List<string>();
            List<string> listOfElementsFromGroup4 = new List<string>();

            int usedGroupsNr = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bool ifIncreseUsedGroupNr = false;
                    if (elementsForPermutation[i, j].UsedGroupNr != 0) continue;
                    for (int ii = 0; ii < 3; ii++)
                    {
                        for (int jj = 0; jj < 3; jj++)
                        {
                            if (ii == i && jj == j) continue;
                            bool sameElements = false;
                            if (arrayOfListOfAvailableElemensts[ii, jj].Count == arrayOfListOfAvailableElemensts[i, j].Count)
                            {
                                for (int n = 0; n < arrayOfListOfAvailableElemensts[i, j].Count; n++)
                                {
                                    if (arrayOfListOfAvailableElemensts[ii, jj][n] != arrayOfListOfAvailableElemensts[i, j][n])
                                    {
                                        sameElements = false;
                                        break;
                                    }
                                    sameElements = true;
                                }
                            }
                            if (sameElements)
                            {
                                elementsForPermutation[i, j].HowMany = arrayOfListOfAvailableElemensts[i, j].Count;
                                elementsForPermutation[i, j].UsedGroupNr = usedGroupsNr;

                                elementsForPermutation[ii, jj].HowMany = arrayOfListOfAvailableElemensts[i, j].Count;
                                elementsForPermutation[ii, jj].UsedGroupNr = usedGroupsNr;

                                elementsForPermutation[i, j].ElementsList = new List<string>(arrayOfListOfAvailableElemensts[i, j]);
                                elementsForPermutation[ii, jj].ElementsList = new List<string>(arrayOfListOfAvailableElemensts[i, j]);

                                ifIncreseUsedGroupNr = true;

                                if (usedGroupsNr == 1)
                                    listOfElementsFromGroup1 = new List<string>(elementsForPermutation[ii, jj].ElementsList);
                                else if (usedGroupsNr == 2)
                                    listOfElementsFromGroup2 = new List<string>(elementsForPermutation[ii, jj].ElementsList);
                                else if (usedGroupsNr == 3)
                                    listOfElementsFromGroup3 = new List<string>(elementsForPermutation[ii, jj].ElementsList);
                                else if (usedGroupsNr == 4)
                                    listOfElementsFromGroup4 = new List<string>(elementsForPermutation[ii, jj].ElementsList);
                            }
                        }
                    }
                    if (ifIncreseUsedGroupNr) usedGroupsNr++;
                }
            }
            usedGroupsNr--;
            #endregion

            #region counting how many permutations are in whole bigCell and each group:
            int howManyPermutationsInGroup1 = 1;
            int howManyPermutationsInGroup2 = 1;
            int howManyPermutationsInGroup3 = 1;
            int howManyPermutationsInGroup4 = 1;

            howManyPermutations[iBig, jBig] = 1;
            int groupNr = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (elementsForPermutation[i, j].UsedGroupNr == groupNr)
                    {
                        if (elementsForPermutation[i, j].UsedGroupNr == 1)
                            howManyPermutationsInGroup1 = Methods.Factorial(elementsForPermutation[i, j].HowMany);
                        else if (elementsForPermutation[i, j].UsedGroupNr == 2)
                            howManyPermutationsInGroup2 = Methods.Factorial(elementsForPermutation[i, j].HowMany);
                        else if (elementsForPermutation[i, j].UsedGroupNr == 3)
                            howManyPermutationsInGroup3 = Methods.Factorial(elementsForPermutation[i, j].HowMany);
                        else if (elementsForPermutation[i, j].UsedGroupNr == 4)
                            howManyPermutationsInGroup4 = Methods.Factorial(elementsForPermutation[i, j].HowMany);

                        howManyPermutations[iBig, jBig] *= Methods.Factorial(elementsForPermutation[i, j].HowMany);
                        groupNr++;
                    }
                }
            }
            #endregion

            #region FillingLabelArrayWithElementFromParticularPermutation:
            int permutationIndexForGroup1 = 0;
            int permutationIndexForGroup2 = 0;
            int permutationIndexForGroup3 = 0;
            int permutationIndexForGroup4 = 0;

            if (whichPermutation == 1)
            {
                listOfAllPermutationsOfElementsFromGroup1[2 - iBig + jBig] =
                    new List<List<string>>(Methods.AllPermutationsOfTheList(listOfElementsFromGroup1));

                listOfAllPermutationsOfElementsFromGroup2[2 - iBig + jBig] =
                    new List<List<string>>(Methods.AllPermutationsOfTheList(listOfElementsFromGroup2));

                listOfAllPermutationsOfElementsFromGroup3[2 - iBig + jBig] =
                    new List<List<string>>(Methods.AllPermutationsOfTheList(listOfElementsFromGroup3));

                listOfAllPermutationsOfElementsFromGroup4[2 - iBig + jBig] =
                    new List<List<string>>(Methods.AllPermutationsOfTheList(listOfElementsFromGroup4));
            }


            if (usedGroupsNr == 1)
                permutationIndexForGroup1 = (whichPermutation - 1) % howManyPermutationsInGroup1;

            if (usedGroupsNr == 2)
                permutationIndexForGroup2 = ((whichPermutation - 1) / howManyPermutationsInGroup1) % howManyPermutationsInGroup2;

            if (usedGroupsNr == 3)
                permutationIndexForGroup3 = ((whichPermutation - 1) / (howManyPermutationsInGroup1 * howManyPermutationsInGroup2))
                    % howManyPermutationsInGroup3;

            if (usedGroupsNr == 4)
                permutationIndexForGroup4 = ((whichPermutation - 1) /
                    (howManyPermutationsInGroup1 * howManyPermutationsInGroup2 * howManyPermutationsInGroup3))
                    % howManyPermutationsInGroup4;


            int elementInUseFromGroup1 = 0;
            int elementInUseFromGroup2 = 0;
            int elementInUseFromGroup3 = 0;
            int elementInUseFromGroup4 = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (elementsForPermutation[i, j].UsedGroupNr == 1)
                    {
                        labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content =
                            listOfAllPermutationsOfElementsFromGroup1[2 - iBig + jBig][permutationIndexForGroup1][elementInUseFromGroup1];
                        elementInUseFromGroup1++;
                    }
                    else if (elementsForPermutation[i, j].UsedGroupNr == 2)
                    {
                        labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content =
                            listOfAllPermutationsOfElementsFromGroup2[2 - iBig + jBig][permutationIndexForGroup2][elementInUseFromGroup2];
                        elementInUseFromGroup2++;
                    }
                    else if (elementsForPermutation[i, j].UsedGroupNr == 3)
                    {
                        labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content =
                            listOfAllPermutationsOfElementsFromGroup3[2 - iBig + jBig][permutationIndexForGroup3][elementInUseFromGroup3];
                        elementInUseFromGroup3++;
                    }
                    else if (elementsForPermutation[i, j].UsedGroupNr == 4)
                    {
                        labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content =
                            listOfAllPermutationsOfElementsFromGroup4[2 - iBig + jBig][permutationIndexForGroup4][elementInUseFromGroup4];
                        elementInUseFromGroup4++;
                    }
                    else // czyli: (elementsForPermutation[i, j].UsedGroupNr == 0)
                        labelNumbersArrayInRectGrid[i + 3 * iBig, j + 3 * jBig].Content = arrayOfListOfAvailableElemensts[i, j][0];
                }
            }
            #endregion

            return true;
        }

        private bool FillLast3BigCellsAndReturnIfFilledFullyBetter()
        {
            int whichPermutation = 1;
            do
            {
                if (!FillingCustomBigCellAndReturnIfFilledFullyBetter(2, 1, whichPermutation)) return false;
                if (!FillingCustomBigCellAndReturnIfFilledFullyBetter(1, 2, whichPermutation)) return false;
                whichPermutation++;
                if (whichPermutation > 100) return false;
            } while (!FillingCustomBigCellAndReturnIfFilledFullyBetter(2, 2, 1));

            return true;
        }

        private void RandomizeSudokuGridNumbersBetter()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    labelNumbersArrayInRectGrid[i, j].Content = "";
                }
            }

            do
            {
                do
                {
                    FillFirst6BigCells();
                } while (!FillLast3BigCellsAndReturnIfFilledFullyBetter());
            } while (!IfAllElementsArePlacedProperly());
        }

        #endregion

        // ***********************************************************************************************
        // ***********************************************************************************************
        // ***********************************************************************************************



        private void Checking(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //smallRectArray[i, j].Fill = colorOfCellsToFill;
                    smallRectArray[i, j].Stroke = Brushes.Gray;
                    smallRectArray[i, j].StrokeThickness = 1;
                    labelNumbersArrayInRectGrid[i, j].FontWeight = FontWeights.Normal;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == checkNr.ToString())
                    {
                        //smallRectArray[i, j].Fill = colorOfCellOnCheck;
                        smallRectArray[i, j].Stroke = Brushes.DeepSkyBlue;
                        smallRectArray[i, j].StrokeThickness = 4;
                        labelNumbersArrayInRectGrid[i, j].FontWeight = FontWeights.Bold;
                    }
                }
            }
            checkNr++;
            if (checkNr > 9) checkNr = 0;

        }

        bool guideUsed = false;
        private void Guide(object sender, RoutedEventArgs e)
        {
            ShowAvailableElementsAreInEachCell();
            guideUsed = true;
        }

        private void Answer(object sender, RoutedEventArgs e)
        {
            ShowFullFilledSudoku();
        }



        // ***********************************************************************************************
        // ***********************************************************************************************
        // ***********************************************************************************************

        #region Removing elements from sudoku grid

        Random rndRemove = new Random();
        List<int[]> indexesListForPartOne;
        List<int[]> indexesListForPartTwo;
        int iIndex;
        int jIndex;
        string lastRemovedElement;

        private void FillingBothIndexesLists()
        {
            indexesListForPartOne = new List<int[]>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    indexesListForPartOne.Add(new int[2]);
                    indexesListForPartOne[indexesListForPartOne.Count - 1][0] = i;
                    indexesListForPartOne[indexesListForPartOne.Count - 1][1] = j;
                }
            }
            indexesListForPartTwo = new List<int[]>(indexesListForPartOne);
        }

        private void RemoveElementFromIndexesListAndTryToRemoveElementFromSudokuGrid(List<int[]> indexesList)
        {
            int currentIndexOfIndexesList = rndRemove.Next(0, indexesList.Count);
            iIndex = indexesList[currentIndexOfIndexesList][0];
            jIndex = indexesList[currentIndexOfIndexesList][1];
            indexesList.RemoveAt(currentIndexOfIndexesList);

            lastRemovedElement = labelNumbersArrayInRectGrid[iIndex, jIndex].Content.ToString();
            labelNumbersArrayInRectGrid[iIndex, jIndex].Content = "";
        }

        private List<string> ElementsAvailableToFillTheRemovedCell(int iIndex, int jIndex)
        {
            List<string> listOfAvailableElemensts = new List<string>(elementListFrom1To9);

            // checking elements from particular column:
            for (int ii = 0; ii < 9; ii++)
            {
                listOfAvailableElemensts.Remove(labelNumbersArrayInRectGrid[ii, jIndex].Content.ToString());
            }
            // checking elements from particular row:
            for (int jj = 0; jj < 9; jj++)
            {
                listOfAvailableElemensts.Remove(labelNumbersArrayInRectGrid[iIndex, jj].Content.ToString());
            }
            // checking elements from particular big cell:
            int iBig = iIndex / 3;
            int jBig = jIndex / 3;
            for (int ii = 0 + 3 * iBig; ii < 3 + 3 * iBig; ii++)
            {
                for (int jj = 0 + 3 * jBig; jj < 3 + 3 * jBig; jj++)
                {
                    listOfAvailableElemensts.Remove(labelNumbersArrayInRectGrid[ii, jj].Content.ToString());
                }
            }

            return listOfAvailableElemensts;
        }

        private bool TryToSolveSudokuAndReturnIfCanBeSolved()
        {
            string[,] tempLabelNumbersArrayRepository = new string[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempLabelNumbersArrayRepository[i, j] = labelNumbersArrayInRectGrid[i, j].Content.ToString();
                }
            }

            List<string> tempListOfAvailableElements;
            int howManyCellsFilledBeforeLoop = 0;
            int howManyCellsFilledAfterLoop = 0;
            int howManyEmptyCellsFound = 0;
            do
            {
                howManyEmptyCellsFound = 0;
                howManyCellsFilledBeforeLoop = howManyCellsFilledAfterLoop;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == "")
                        {
                            howManyEmptyCellsFound++;
                            tempListOfAvailableElements = new List<string>(ElementsAvailableToFillTheRemovedCell(i, j));
                            if (tempListOfAvailableElements.Count == 1)
                            {
                                labelNumbersArrayInRectGrid[i, j].Content = tempListOfAvailableElements[0];
                                howManyCellsFilledAfterLoop++;
                                howManyEmptyCellsFound--;
                            }
                        }
                    }
                }
            } while (howManyCellsFilledBeforeLoop != howManyCellsFilledAfterLoop);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    labelNumbersArrayInRectGrid[i, j].Content = tempLabelNumbersArrayRepository[i, j];
                }
            }

            if (howManyEmptyCellsFound == 0) return true;
            else return false;
        }

        private void EraseElementsFromSudokuGrid()
        {
            int howManyElementsErased = 0;
            FillingBothIndexesLists();
            do
            {
                RemoveElementFromIndexesListAndTryToRemoveElementFromSudokuGrid(indexesListForPartOne);
                if (ElementsAvailableToFillTheRemovedCell(iIndex, jIndex).Count == 1)
                {
                    indexesListForPartTwo.RemoveAll(p => p[0] == iIndex && p[1] == jIndex);
                    // pytanie: czy szybciej tak, czy po tej pętli zrobić pętlę w której if (... == "") {list.Add(...)}
                    howManyElementsErased++;
                }
                else
                {
                    labelNumbersArrayInRectGrid[iIndex, jIndex].Content = lastRemovedElement;
                }
            } while (indexesListForPartOne.Count > 0);


            do
            {
                RemoveElementFromIndexesListAndTryToRemoveElementFromSudokuGrid(indexesListForPartTwo);
                if (!TryToSolveSudokuAndReturnIfCanBeSolved())
                {
                    labelNumbersArrayInRectGrid[iIndex, jIndex].Content = lastRemovedElement;
                }
            } while (indexesListForPartTwo.Count > 0);
        }

        private void ShowAvailableElementsAreInEachCell()
        {
            //int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == "")
                    {
                        if (ElementsAvailableToFillTheRemovedCell(i, j).Count == 1)
                        {
                            smallRectArray[i, j].Fill = colorOfCellOnGuide;
                        }
                    }
                    //else count++;
                }
            }
            //btn_randPt2_name.Content = count;
        }


        #endregion

        // ***********************************************************************************************
        // ***********************************************************************************************
        // ***********************************************************************************************



        private void MakeColorOfStartCellsDarker()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (labelNumbersArrayInRectGrid[i, j].Content.ToString() != "")
                        smallRectArray[i, j].Fill = colorOfCellsFilledAtStart;
                }
            }
        }

        private void AddEventToClickedRects()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    labelNumbersArrayInRectGrid[i, j].MouseUp -= ActivateCellToFill;
                    // jak zróbić, aby można było kliknąć smallRect mimo że na nim jest label?????
                    if (smallRectArray[i, j].Fill != colorOfCellsFilledAtStart)
                        labelNumbersArrayInRectGrid[i, j].MouseUp += ActivateCellToFill;
                }
            }
            grid1.KeyUp += InsertNumber;
        }

        private void RemoveEventFromClickedRects()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    labelNumbersArrayInRectGrid[i, j].MouseUp -= ActivateCellToFill;
                    // jak zróbić, aby można było kliknąć smallRect mimo że na nim jest label?????
                }
            }
            grid1.KeyUp -= InsertNumber;
        }

        string currentNumber = "";
        private void ActivateCellToFill(object sender, MouseEventArgs e)
        {
            iClicked = int.Parse((sender as Label).Name[4].ToString());
            jClicked = int.Parse((sender as Label).Name[5].ToString());
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i != iClicked || j != jClicked) && (labelNumbersArrayInRectGrid[i, j].Content.ToString() == "?"))
                        labelNumbersArrayInRectGrid[i, j].Content = "";
                }
            }
            if ((sender as Label).Content.ToString() != "?")
            {
                currentNumber = (sender as Label).Content.ToString();
                (sender as Label).Content = "?";
            }

            else
                (sender as Label).Content = currentNumber;
        }

        private void InsertNumber(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();
            string number;
            if (key == "D1" || key == "D2" || key == "D3" || key == "D4" || key == "D5"
                || key == "D6" || key == "D7" || key == "D8" || key == "D9")
                number = key[1].ToString();
            else if (key == "NumPad1" || key == "NumPad2" || key == "NumPad3" || key == "NumPad4"
                || key == "NumPad5" || key == "NumPad6" || key == "NumPad7" || key == "NumPad8" || key == "NumPad9")
                number = key[6].ToString();
            else number = "";
            if (labelNumbersArrayInRectGrid[iClicked, jClicked].Content.ToString() == "?")
            {
                labelNumbersArrayInRectGrid[iClicked, jClicked].Content = number;
                smallRectArray[iClicked, jClicked].Fill = colorOfCellsToFill;
            }

            if (CheckIfEveryCellIsFilled())
            {
                ShowFullFilledSudoku();
            }
        }

        private bool CheckIfEveryCellIsFilled()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (labelNumbersArrayInRectGrid[i, j].Content.ToString() == "") return false;
                }
            }
            return true;
        }

        private void ShowHideTime(object sender, RoutedEventArgs e)
        {
            if (btn_showHideTime_name.Content.ToString() == "Ukryj czas")
            {
                lbl_time.Visibility = System.Windows.Visibility.Hidden;
                btn_showHideTime_name.Content = "Pokaż czas";
            }
            else
            {
                lbl_time.Visibility = System.Windows.Visibility.Visible;
                btn_showHideTime_name.Content = "Ukryj czas";
            }
        }


        bool gamePaused = false;
        string[,] currentNumbersArray = new string[9, 9];
        private void Pause(object sender, RoutedEventArgs e)
        {
            if (!gamePaused)
            {
                timer.Stop();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        currentNumbersArray[i, j] = labelNumbersArrayInRectGrid[i, j].Content.ToString();
                        labelNumbersArrayInRectGrid[i, j].Content = "";
                    }
                }
                gamePaused = true;
                btn_pause_name.Content = "Wznów";
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        labelNumbersArrayInRectGrid[i, j].Content = currentNumbersArray[i, j];
                        currentNumbersArray[i, j] = "";
                    }
                }
                timer.Start();
                gamePaused = false;
                btn_pause_name.Content = "Pauza";
            }

        }


    }
}
