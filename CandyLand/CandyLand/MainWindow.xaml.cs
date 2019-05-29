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

namespace CandyLand { 

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        int tileSize = 40, CandyLandBoardSize = 20, nextCard = 0, nextSquare, currentPlayerIndex = 0;
        Random random;
        int[] cardDeck = new int[60]; //card deck containing 60 cards
        int[] player = { 0, 0 };
        Brush[] playerColors = { Brushes.Black, Brushes.Blue };
        Boolean draw = true;
        Button[] spaces = new Button[135]; //135 clickable button spaces on the board
        Rectangle rect1, rect2;
        Rectangle[] playerRect = new Rectangle[2];
        //layout direction of the spaces/path
        int[] path = { 380, 381, 382, 383, 384, 385, 365, 366, 367, 368, 388, 389, 390, 391, 392, 393, 373, 353, 352, 351, 331, 311, 291, 292, 293, 313,
                       314, 315, 316, 336, 356, 355, 375, 395, 396, 397, 398, 399, 379, 359, 358, 338, 318, 298, 278, 277, 257, 256, 236, 216, 215, 214,
                       234, 233, 232, 231, 211, 210, 190, 170, 171, 151, 152, 132, 112, 113, 114, 115, 135, 155, 156, 157, 158, 138, 139, 119, 99, 98,
                       78, 77, 76, 75, 74, 73, 72, 71, 70, 90, 110, 109, 129, 128, 148,  168, 167, 166, 165, 185, 184, 204, 224, 223, 222, 202, 182, 181,
                       161, 141, 121, 101, 102, 103, 83, 84, 64, 65, 66, 46, 47, 48, 28, 8, 7, 6, 5, 4, 3, 23, 22, 42, 41, 40, 20, 0 };
        //colors of the spaces
        Brush[] brush = { Brushes.DeepPink, Brushes.Violet, Brushes.Yellow, Brushes.LightBlue, Brushes.Orange, Brushes.LimeGreen };

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            this.Height = tileSize * CandyLandBoardSize + 50;
            this.Width = tileSize * CandyLandBoardSize + 250;
            //overll board size
            for (int i = 0; i < CandyLandBoardSize; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(tileSize);
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(tileSize);
                CandyLandBoard.RowDefinitions.Add(rd);
                CandyLandBoard.ColumnDefinitions.Add(cd);
            }
            //generates the board's spaces and colors
            for (int i = 0; i < path.Length; i++)
            {
                spaces[i] = new Button();
                spaces[i].Click += new RoutedEventHandler(this.Button_Click);
                spaces[i].Background = brush[i % 6];
                spaces[i].Name = "a" + path[i].ToString();
                Grid.SetRow(spaces[i], path[i] / CandyLandBoardSize);
                Grid.SetColumn(spaces[i], path[i] % CandyLandBoardSize);
                CandyLandBoard.Children.Add(spaces[i]);
            }
            //generates the card deck and it's location
            spaces[134] = new Button();
            spaces[134].Click += new RoutedEventHandler(this.cardDeck_Click);
            Grid.SetRow(spaces[134], 11);
            Grid.SetColumn(spaces[134], 6);
            Grid.SetColumnSpan(spaces[134], 3);
            Grid.SetRowSpan(spaces[134], 5);
            spaces[134].Background = new ImageBrush(new BitmapImage(new Uri("C:/Users/Meagan/Documents/Visual Studio 2015/Projects/CandyLand/CandyLand/Properties/deck.jpg", UriKind.Relative)));
            CandyLandBoard.Children.Add(spaces[134]);
            generateDeck();
            playerRect[0] = new Rectangle();
            playerRect[1] = new Rectangle();
            playerRect[0].Fill = playerColors[0];
            playerRect[1].Fill = playerColors[1];
            playerRect[0].Height = 35;
            playerRect[0].Width = 35;
            playerRect[1].Height = 30;
            playerRect[1].Width = 30;
            Grid.SetRow(playerRect[0], path[0] / CandyLandBoardSize);
            Grid.SetColumn(playerRect[0], path[0] % CandyLandBoardSize);
            Grid.SetRow(playerRect[1], path[0] / CandyLandBoardSize);
            Grid.SetColumn(playerRect[1], path[0] % CandyLandBoardSize);
            CandyLandBoard.Children.Add(playerRect[0]);
            CandyLandBoard.Children.Add(playerRect[1]);
        }

        //movement on the board
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            if (!draw && int.Parse(b.Name.Substring(1)) == path[nextSquare])
            {
                player[currentPlayerIndex] = nextSquare;
                Grid.SetRow(playerRect[currentPlayerIndex], path[nextSquare] / CandyLandBoardSize);
                Grid.SetColumn(playerRect[currentPlayerIndex], path[nextSquare] % CandyLandBoardSize);
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                if (nextSquare == 133)
                    System.Environment.Exit(0);   // The game exits after a player wins
                draw = true;
            }
        }

        //to draw a card
        private void cardDeck_Click(object sender, RoutedEventArgs e)
        {
            if (draw)
            {
                spaces[134].Background = Brushes.White;
                if (rect1 != null)
                    rect1.Fill = Brushes.White;
                if (rect2 != null)
                    rect2.Fill = Brushes.White;
                if (cardDeck[nextCard] < 6)
                {
                    rect1 = new Rectangle();
                    rect1.Height = 50;
                    rect1.Width = 50;
                    rect1.Fill = brush[cardDeck[nextCard] % 6];
                    Grid.SetRow(rect1, 13);
                    Grid.SetColumn(rect1, 7);
                    CandyLandBoard.Children.Add(rect1);
                }
                else {
                    rect1 = new Rectangle();
                    rect1.Height = 80;
                    rect1.Width = 80;
                    rect1.Fill = brush[cardDeck[nextCard] % 6];
                    Grid.SetRow(rect1, 12);
                    Grid.SetColumn(rect1, 7);
                    CandyLandBoard.Children.Add(rect1);
                    rect2 = new Rectangle();
                    rect2.Height = 50;
                    rect2.Width = 50;
                    rect2.Fill = brush[cardDeck[nextCard] % 6];
                    Grid.SetRow(rect2, 14);
                    Grid.SetColumn(rect2, 7);
                    CandyLandBoard.Children.Add(rect2);
                }
            }
            nextSquare = (cardDeck[nextCard] % 6) - (player[currentPlayerIndex] % 6);
            if (nextSquare <= 0)
                nextSquare += 6;
            if (cardDeck[nextCard] > 5)
                nextSquare += 6;
            nextSquare += player[currentPlayerIndex];
            if (nextSquare > 133)
                nextSquare = 133;
            nextCard++;
            if (nextCard == 60)
                generateDeck();
            draw = false;
        }
        //creates the deck of cards and shuffles them
        public void generateDeck()
        {
            int temp;
            for (int i = 0; i < 60; i++)
                cardDeck[i] = i % 12;
            nextCard = 0;
            for (int i = 0; i < 60; i++)
            {
                temp = cardDeck[i];
                int rnd = random.Next(60);
                cardDeck[i] = cardDeck[rnd];
                temp = cardDeck[i];
                cardDeck[rnd] = temp;
            }
        }
    }
}