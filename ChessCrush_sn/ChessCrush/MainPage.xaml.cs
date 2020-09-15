using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.Storage;
using System.Windows;
using Windows.Data.Xml.Dom;
using System.Windows.Input;
using System.Diagnostics;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ChessCrush
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            initChessBoard();
        }

        bool isCheck = false;
        bool castlingBlack1 = true, castlingBlack2 = true;
        bool castlingWhite1 = true, castlingWhite2 = true;
        bool PieceInMoveing = false;
        bool CanMoveWhitePice = true;
        bool isPawnMove = false, isKingMove = false;
        int whitePiecesOrientation, blackPiecesOrienation;
        int childIndex = 0;
        int enPassantRow = 0, enPassantColumn = 0;
        int movingPieceIndex = 0, moveingRowIndex = 0, moveingColumnIndex = 0;
        int WhiteKingRow, WhiteKingColumn, BlackKingRow, BlackKingColumn;

        struct Filed
        {
            public int valuePiece, valueColorPiece, indexPiece;
            public string namePiece, codNamePiece;
        }
        Filed[,] fileds = new Filed[10, 10];
        int[,] indexBorder = new int[10, 10];
        int[,] validFiled = new int[10, 10];

        public void initChessBoard()
        {
            //init variabels
            isCheck = false;
            castlingBlack1 = true;  castlingBlack2 = true;
            castlingWhite1 = true; castlingWhite2 = true;
            PieceInMoveing = false;
            CanMoveWhitePice = true;
            isPawnMove = false; isKingMove = false;
            childIndex = 0;
            enPassantRow = 0; enPassantColumn = 0;
            movingPieceIndex = 0; moveingRowIndex = 0; moveingColumnIndex = 0;
            fileds = new Filed[10, 10];
            indexBorder = new int[10, 10];
            validFiled = new int[10, 10];
            //--------------------------------------------
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    addFieldColor(i, j);
                    addFiledBorder(i, j);
                    indexBorder[i, j] = childIndex - 1;
                    validFiled[i, j] = 1;
                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Collapsed;
                }
            }

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    fileds[i, j].valueColorPiece = 0;
                }
            }

            bool piecewiew = true;
            if (piecewiew)
            {
                whitePiecesOrientation = -1;
                blackPiecesOrienation = 1;
                //col and row values
                for (int i = 8; i >= 1; i--)
                {
                    addFiledChessValue(i, false);
                }

                addPiece("BlackKing", "K", 2, 10, 1, 5);
                BlackKingRow = 1;
                BlackKingColumn = 5;
                addPiece("BlackQueen", "Q", 2, 9, 1, 4);
                addPiece("BlackRook", "R", 2, 5, 1, 1);
                addPiece("BlackRook", "R", 2, 5, 1, 8);
                addPiece("BlackKnight", "N", 2, 3, 1, 2);
                addPiece("BlackKnight", "N", 2, 3, 1, 7);
                addPiece("BlackBishop", "B", 2, 3, 1, 3);
                addPiece("BlackBishop", "B", 2, 3, 1, 6);
                for (int i = 1; i <= 8; i++)
                {
                    addPiece("BlackPawn", "", 2, 1, 2, i);
                }

                addPiece("WhiteKing", "K", 1, 10, 8, 5);
                WhiteKingRow = 8;
                WhiteKingColumn = 5;
                addPiece("WhiteQueen", "Q", 1, 9, 8, 4);
                addPiece("WhiteRook", "R", 1, 5, 8, 1);
                addPiece("WhiteRook", "R", 1, 5, 8, 8);
                addPiece("WhiteKnight", "N", 1, 3, 8, 2);
                addPiece("WhiteKnight", "N", 1, 3, 8, 7);
                addPiece("WhiteBishop", "B", 1, 3, 8, 3);
                addPiece("WhiteBishop", "B", 1, 3, 8, 6);
                for (int i = 1; i <= 8; i++)
                {
                    addPiece("WhitePawn", "", 1, 1, 7, i);
                }
            }
            else
            {
                whitePiecesOrientation = 1;
                blackPiecesOrienation = -1;
                //col and row values
                for (int i = 1; i <= 8; i++)
                {
                    addFiledChessValue(i, true);
                }

                addPiece("BlackKing", "K", 2, 10, 8, 4);
                BlackKingRow = 8;
                BlackKingColumn = 4;
                addPiece("BlackQueen", "Q", 2, 9, 8, 5);
                addPiece("BlackRook", "R", 2, 5, 8, 1);
                addPiece("BlackRook", "R", 2, 5, 8, 8);
                addPiece("BlackKnight", "N", 2, 3, 8, 2);
                addPiece("BlackKnight", "N", 2, 3, 8, 7);
                addPiece("BlackBishop", "B", 2, 3, 8, 3);
                addPiece("BlackBishop", "B", 2, 3, 8, 6);
                for (int i = 1; i <= 8; i++)
                {
                    addPiece("BlackPawn", "", 2, 1, 7, i);
                }

                addPiece("WhiteKing", "K", 1, 10, 1, 4);
                WhiteKingRow = 1;
                WhiteKingColumn = 4;
                addPiece("WhiteQueen", "Q", 1, 9, 1, 5);
                addPiece("WhiteRook", "R", 1, 5, 1, 1);
                addPiece("WhiteRook", "R", 1, 5, 1, 8);
                addPiece("WhiteKnight", "N", 1, 3, 1, 2);
                addPiece("WhiteKnight", "N", 1, 3, 1, 7);
                addPiece("WhiteBishop", "B", 1, 3, 1, 3);
                addPiece("WhiteBishop", "B", 1, 3, 1, 6);
                for (int i = 1; i <= 8; i++)
                {
                    addPiece("WhitePawn", "", 1, 1, 2, i);
                }
            }
        }

        public void addPiece(string nm, string cnm, int valcol, int valpiece, int i, int j)
        {
            //set img;
            Image img = new Image();
            childIndex++;
            GridBoard.Children.Add(img);
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j);
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri(img.BaseUri, "Resource/Pieces/" + nm + ".png");
            bitmapImage.UriSource = uri;
            img.Source = bitmapImage;

            //set data
            fileds[i, j].indexPiece = childIndex - 1;
            fileds[i, j].valueColorPiece = valcol;
            fileds[i, j].valuePiece = valpiece;
            fileds[i, j].namePiece = nm;
            fileds[i, j].codNamePiece = cnm;
        }

        public void addFieldColor(int i, int j)
        {
            Rectangle rect = new Rectangle();
            rect = new Rectangle();
            if ((i + j) % 2 == 0)
            {
                rect.Fill = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            }
            else
            {
                rect.Fill = new SolidColorBrush(Windows.UI.Colors.Brown);
            }
            childIndex++;
            GridBoard.Children.Add(rect);
            Grid.SetRow(rect, i);
            Grid.SetColumn(rect, j);
        }

        public void addFiledBorder(int i, int j)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(5);
            border.BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Green);
            childIndex++;
            GridBoard.Children.Add(border);
            Grid.SetRow(border, i);
            Grid.SetColumn(border, j);
        }

        public void addFiledChessValue(int i, bool isBlack)
        {
            // first column numbers
            TextBlock text = new TextBlock();
            text.Text = i.ToString();
            text.FontSize = 18;
            text.Foreground = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            text.TextAlignment = TextAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            childIndex++;
            GridBoard.Children.Add(text);
            if (isBlack)
            {
                Grid.SetRow(text, i);
            }
            else
            {
                Grid.SetRow(text, 9 - i);
            }
            Grid.SetColumn(text, 0);

            // last row strings
            text = new TextBlock();
            char val = (char)(105 - i);
            text.Text = val.ToString();
            text.FontSize = 12;
            text.Foreground = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            text.TextAlignment = TextAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            childIndex++;
            GridBoard.Children.Add(text);
            if (isBlack)
            {
                Grid.SetColumn(text, i);
            }
            else
            {
                Grid.SetColumn(text, 9 - i);
            }
            Grid.SetRow(text, 9);
        }

        private void ChesBoard_PreviewMouseClick(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Input.Pointer pointer = e.Pointer;
            if (pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Windows.UI.Input.PointerPoint point = e.GetCurrentPoint(GridBoard);
                if (point.Properties.IsLeftButtonPressed)
                {
                    //get row and column index
                    double mouseX = double.Parse(point.Position.X.ToString());
                    double mouseY = double.Parse(point.Position.Y.ToString());
                    double boardWidth = GridBoard.ActualWidth;
                    double boardHeight = GridBoard.ActualHeight;
                    double rowWidth = boardWidth / (8 + (double)1 / 3);
                    double rowHeight = boardHeight / (8 + (double)1 / 3);
                    double ri = 9 - (boardHeight - mouseY - rowHeight / (double)1 / 3) / rowHeight; ;
                    int rowIndex = (int)ri;
                    double ci = 9 - (boardWidth - mouseX) / rowWidth;
                    int columnIndex = (int)ci;

                    //drag white piece or change white piece
                    if (CanMoveWhitePice && fileds[rowIndex, columnIndex].valueColorPiece == 1)
                    {
                        //init possible fields
                        initValidFields(rowIndex, columnIndex, true);
                        PieceInMoveing = true;
                        movingPieceIndex = fileds[rowIndex, columnIndex].indexPiece;
                        moveingRowIndex = rowIndex;
                        moveingColumnIndex = columnIndex;
                        GridBoard.Children.ElementAt(indexBorder[rowIndex, columnIndex]).Visibility = Visibility.Visible;
                    }
                    //drop white piece
                    else if (CanMoveWhitePice && PieceInMoveing && validFiled[rowIndex, columnIndex] == 1)
                    {
                        //castling drop
                        if (isKingMove && Math.Abs(moveingColumnIndex - columnIndex) == 2)
                        {
                            verifyEnPassant(rowIndex, columnIndex);
                            dropCastling(rowIndex, columnIndex, true);
                            CanMoveWhitePice = false;
                        }
                        //en passant drop
                        else if (isPawnMove && fileds[rowIndex, columnIndex].valueColorPiece == 0 && Math.Abs(enPassantRow - rowIndex) == 1 && (moveingRowIndex == 4 || moveingRowIndex == 5) && enPassantColumn == columnIndex)
                        {
                            GridBoard.Children.ElementAt(fileds[enPassantRow, enPassantColumn].indexPiece).Visibility = Visibility.Collapsed;
                            resetFieldProperties(enPassantRow, enPassantColumn);
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, true);
                            CanMoveWhitePice = false;
                        }
                        //normal drop
                        else if (fileds[rowIndex, columnIndex].valueColorPiece == 0)
                        {
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, true);
                            CanMoveWhitePice = false;
                        }
                        //capture move
                        else if (fileds[rowIndex, columnIndex].valueColorPiece == 2)
                        {
                            GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                            resetFieldProperties(rowIndex, columnIndex);
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, true);
                            CanMoveWhitePice = false;
                        }
                    }
                    //drag black piece or change black piece
                    else if (!CanMoveWhitePice && fileds[rowIndex, columnIndex].valueColorPiece == 2)
                    {
                        //init possible fields
                        initValidFields(rowIndex, columnIndex, false);
                        PieceInMoveing = true;
                        movingPieceIndex = fileds[rowIndex, columnIndex].indexPiece;
                        moveingRowIndex = rowIndex;
                        moveingColumnIndex = columnIndex;
                        GridBoard.Children.ElementAt(indexBorder[rowIndex, columnIndex]).Visibility = Visibility.Visible;
                    }
                    //drop black piece
                    else if (!CanMoveWhitePice && PieceInMoveing && validFiled[rowIndex, columnIndex] == 1)
                    {
                        //castling drop
                        if (isKingMove && Math.Abs(moveingColumnIndex - columnIndex) == 2)
                        {
                            verifyEnPassant(rowIndex, columnIndex);
                            dropCastling(rowIndex, columnIndex, false);
                            CanMoveWhitePice = true;
                        }
                        //en passant drop
                        else if (isPawnMove && fileds[rowIndex, columnIndex].valueColorPiece == 0 && Math.Abs(enPassantRow - rowIndex) == 1 && (moveingRowIndex == 4 || moveingRowIndex == 5) && enPassantColumn == columnIndex)
                        {
                            GridBoard.Children.ElementAt(fileds[enPassantRow, enPassantColumn].indexPiece).Visibility = Visibility.Collapsed;
                            resetFieldProperties(enPassantRow, enPassantColumn);
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, false);
                            CanMoveWhitePice = true;
                        }
                        //normal drop
                        else if (fileds[rowIndex, columnIndex].valueColorPiece == 0)
                        {
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, false);
                            CanMoveWhitePice = true;
                        }
                        //capture drop
                        else if (fileds[rowIndex, columnIndex].valueColorPiece == 1)
                        {
                            GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                            resetFieldProperties(rowIndex, columnIndex);
                            verifyEnPassant(rowIndex, columnIndex);
                            dropPiece(rowIndex, columnIndex, false);
                            CanMoveWhitePice = true;
                        }
                    }
                }
            }
        }

        public void dropPiece(int rowIndex, int columnIndex, bool isWhite)
        {
            var dialog=new MessageDialog("");
            isCheck = false;
            Grid.SetRow((FrameworkElement)GridBoard.Children.ElementAt(movingPieceIndex), rowIndex);
            Grid.SetColumn((FrameworkElement)GridBoard.Children.ElementAt(movingPieceIndex), columnIndex);
            fileds[rowIndex, columnIndex] = fileds[moveingRowIndex, moveingColumnIndex];
            resetFieldProperties(moveingRowIndex, moveingColumnIndex);
            PieceInMoveing = false;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Collapsed;
                }
            }
            //-------------
            if (isPawnMove)
            {
                if (rowIndex == 1 && whitePiecesOrientation == -1)
                {
                    GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                    resetFieldProperties(rowIndex, columnIndex);
                    addPiece("WhiteQueen", "Q", 1, 9, rowIndex, columnIndex);
                }
                else if (rowIndex == 8 && whitePiecesOrientation == 1)
                {
                    GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                    resetFieldProperties(rowIndex, columnIndex);
                    addPiece("WhiteQueen", "Q", 1, 9, rowIndex, columnIndex);
                }
                else if (rowIndex == 1 && whitePiecesOrientation == 1)
                {
                    GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                    resetFieldProperties(rowIndex, columnIndex);
                    addPiece("BlackQueen", "Q", 2, 9, rowIndex, columnIndex);
                }
                else if (rowIndex == 8 && whitePiecesOrientation == -1)
                {
                    GridBoard.Children.ElementAt(fileds[rowIndex, columnIndex].indexPiece).Visibility = Visibility.Collapsed;
                    resetFieldProperties(rowIndex, columnIndex);
                    addPiece("BlackQueen", "Q", 2, 9, rowIndex, columnIndex);
                }
            }
            //-----------

            if (isKingMove && isWhite)
            {
                WhiteKingRow = rowIndex;
                WhiteKingColumn = columnIndex;
            }
            else if (isKingMove)
            {
                BlackKingRow = rowIndex;
                BlackKingColumn = columnIndex;
            }
            if (isWhite)
            {
                if (CheckCheck(BlackKingRow, BlackKingColumn, isWhite) > 0)
                {
                    dialog = new MessageDialog("Check");
                    isCheck = true;
                    dialog.ShowAsync();
                    if (CheckCheckMate(BlackKingRow, BlackKingColumn, isWhite))
                    {
                        dialog = new MessageDialog("It is CheckMate");
                        dialog.ShowAsync();
                    }
                    else
                    {
                        dialog = new MessageDialog("It is NOT CheckMate");
                        dialog.ShowAsync();
                    }
                }
            }
            else
            {
                if (CheckCheck(WhiteKingRow, WhiteKingColumn, isWhite) > 0)
                {
                    dialog = new MessageDialog("Check");
                    isCheck = true;
                    dialog.ShowAsync();
                    if (CheckCheckMate(WhiteKingRow, WhiteKingColumn, isWhite))
                    {
                        dialog = new MessageDialog("It is CheckMate");
                        dialog.ShowAsync();
                    }
                    else
                    {
                        dialog = new MessageDialog("It is NOT CheckMate");
                        dialog.ShowAsync();
                    }
                }
            }

            //invalidate castling
            if (whitePiecesOrientation == -1)
            {
                if (moveingRowIndex == 8 && moveingColumnIndex == 5)
                {
                    castlingWhite1 = false;
                    castlingWhite2 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 5)
                {
                    castlingBlack1 = false;
                    castlingBlack2 = false;
                }
                else if (moveingRowIndex == 8 && moveingColumnIndex == 8)
                {
                    castlingWhite1 = false;
                }
                else if (moveingRowIndex == 8 && moveingColumnIndex == 1)
                {
                    castlingWhite2 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 8)
                {
                    castlingBlack1 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 1)
                {
                    castlingBlack2 = false;
                }
            }
            else
            {
                if (moveingRowIndex == 8 && moveingColumnIndex == 5)
                {
                    castlingBlack1 = false;
                    castlingBlack2 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 5)
                {
                    castlingWhite1 = false;
                    castlingWhite2 = false;
                }
                else if (moveingRowIndex == 8 && moveingColumnIndex == 8)
                {
                    castlingBlack2 = false;
                }
                else if (moveingRowIndex == 8 && moveingColumnIndex == 1)
                {
                    castlingBlack1 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 8)
                {
                    castlingWhite2 = false;
                }
                else if (moveingRowIndex == 1 && moveingColumnIndex == 1)
                {
                    castlingWhite1 = false;
                }
            }
        }

        public void dropCastling(int rowIndex, int columnIndex, bool isWhite)
        {
            dropPiece(rowIndex, columnIndex, isWhite);
            resetFieldProperties(moveingRowIndex, moveingColumnIndex);
            if (columnIndex > moveingColumnIndex)
            {
                moveingColumnIndex = 8;
                movingPieceIndex = fileds[rowIndex, moveingColumnIndex].indexPiece;
                dropPiece(rowIndex, columnIndex - 1, isWhite);
                resetFieldProperties(moveingRowIndex, moveingColumnIndex);
            }
            else
            {
                moveingColumnIndex = 1;
                movingPieceIndex = fileds[rowIndex, moveingColumnIndex].indexPiece;
                dropPiece(rowIndex, columnIndex + 1, isWhite);
                resetFieldProperties(moveingRowIndex, moveingColumnIndex);
            }
            if (isWhite)
            {
                WhiteKingRow = rowIndex;
                WhiteKingColumn = columnIndex;
            }
            else
            {
                BlackKingRow = rowIndex;
                BlackKingColumn = columnIndex;
            }
        }
        public void resetFieldProperties(int row, int col)
        {
            fileds[row, col].indexPiece = 0;
            fileds[row, col].namePiece = "";
            fileds[row, col].valueColorPiece = 0;
            fileds[row, col].valuePiece = 0;
            fileds[row, col].codNamePiece = "";
            GridBoard.Children.ElementAt(indexBorder[row, col]).Visibility = Visibility.Collapsed;
        }

        bool simulateMove(int rowForm, int colFrom, int rowTo, int colTo, bool isWhite, bool isKK = false, bool isEnPass = false)
        {
            var filedEnPass = fileds[enPassantRow, enPassantColumn];
            var filed = fileds[rowForm, colFrom];
            var filedTo = fileds[rowTo, colTo];
            fileds[rowTo, colTo] = filed;
            resetFieldProperties(rowForm, colFrom);
            if (isEnPass)
            {
                resetFieldProperties(enPassantRow, enPassantColumn);
            }
            if (isKK)
            {
                if (CheckCheck(rowTo, colTo, !isWhite) == 0)
                {
                    fileds[rowForm, colFrom] = filed;
                    fileds[rowTo, colTo] = filedTo;
                    return true;
                }
            }
            else if (isWhite)
            {
                if (CheckCheck(WhiteKingRow, WhiteKingColumn, !isWhite) == 0)
                {
                    fileds[rowForm, colFrom] = filed;
                    fileds[rowTo, colTo] = filedTo;
                    fileds[enPassantRow, enPassantColumn] = filedEnPass;
                    return true;
                }
            }
            else
            {
                if (CheckCheck(BlackKingRow, BlackKingColumn, !isWhite) == 0)
                {
                    fileds[rowForm, colFrom] = filed;
                    fileds[rowTo, colTo] = filedTo;
                    fileds[enPassantRow, enPassantColumn] = filedEnPass;
                    return true;
                }
            }
            fileds[rowForm, colFrom] = filed;
            fileds[rowTo, colTo] = filedTo;
            fileds[enPassantRow, enPassantColumn] = filedEnPass;
            return false;
        }

        public void initValidFields(int row, int col, bool isWhite)
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Collapsed;
                    validFiled[i, j] = 0;
                }
            }

            isPawnMove = false;
            isKingMove = false;
            string pieceName = fileds[row, col].codNamePiece;
            var dialog = new MessageDialog("");
            switch (pieceName)
            {
                case "":
                    pawnMove(row, col, isWhite);
                    isPawnMove = true;
                    break;
                case "K":
                    kingMove(row, col, isWhite);
                    isKingMove = true;
                    break;
                case "B":
                    bishopMove(row, col, isWhite);
                    break;
                case "N":
                    knightMove(row, col, isWhite);
                    break;
                case "R":
                    rookMove(row, col, isWhite);
                    break;
                case "Q":
                    bishopMove(row, col, isWhite);
                    rookMove(row, col, isWhite);
                    break;
            }
        }
        public bool pawnMove(int row, int col, bool isWhite, bool VerifyCheck = false, bool VerifyCheckMate = false)
        {
            int orintent;
            int valColOp;
            if (isWhite)
            {
                valColOp = 2;
                orintent = whitePiecesOrientation;
            }
            else
            {
                valColOp = 1;
                orintent = blackPiecesOrienation;
            }
            if (VerifyCheck == false)
            {
                //verify first filed in front
                if (row + 1 * orintent <= 8 && row + 1 * orintent >= 1 && fileds[row + 1 * orintent, col].valueColorPiece == 0)
                {
                    if (VerifyCheckMate == false)
                    {
                        if (simulateMove(row, col, row + 1 * orintent, col, isWhite) == true)
                        {
                            validFiled[row + 1 * orintent, col] = 1;
                            GridBoard.Children.ElementAt(indexBorder[row + 1 * orintent, col]).Visibility = Visibility.Visible;
                        }
                    }
                    if (VerifyCheckMate == true)
                    {
                        if (simulateMove(row, col, row + 1 * orintent, col, isWhite) == true)
                        {
                            return true;
                        }
                    }
                    //verify second filed in front
                    if ((row == 2 || row == 7) && row + 2 * orintent <= 8 && row + 2 * orintent >= 1 && fileds[row + 2 * orintent, col].valueColorPiece == 0)
                    {
                        if (VerifyCheckMate == false)
                        {
                            if (simulateMove(row, col, row + 2 * orintent, col, isWhite) == true)
                            {
                                validFiled[row + 2 * orintent, col] = 1;
                                GridBoard.Children.ElementAt(indexBorder[row + 2 * orintent, col]).Visibility = Visibility.Visible;
                            }
                        }
                    }
                    if (VerifyCheckMate == true)
                    {
                        if (simulateMove(row, col, row + 2 * orintent, col, isWhite) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            //verify side fields
            if (row + 1 * orintent <= 8 && row + 1 * orintent >= 1)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    //verify capture
                    if (col + j <= 8 && col + j >= 1 && fileds[row + 1 * orintent, col + j].valueColorPiece == valColOp)
                    {
                        if (VerifyCheck == false && VerifyCheckMate == false)
                        {
                            if (simulateMove(row, col, row + 1 * orintent, col + j, isWhite) == true)
                            {
                                validFiled[row + 1 * orintent, col + j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[row + 1 * orintent, col + j]).Visibility = Visibility.Visible;
                            }
                        }
                        else if (VerifyCheck == true)
                        {
                            if (fileds[row + 1 * orintent, col + j].codNamePiece == "")
                            {
                                return true;
                            }
                        }
                        if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, row + 1 * orintent, col + j, isWhite) == true)
                            {
                                return true;
                            }
                        }

                    }
                    //verify enPassant
                    if (col + j <= 8 && col + j >= 1 && row == enPassantRow && (col + j) == enPassantColumn)
                    {
                        if (VerifyCheck == false && VerifyCheckMate == false)
                        {
                            if (simulateMove(row, col, row + 1 * orintent, col + j, isWhite,false,true) == true)
                            {
                                validFiled[row + 1 * orintent, col + j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[row + 1 * orintent, col + j]).Visibility = Visibility.Visible;
                            }
                        }
                        if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, row + 1 * orintent, col + j, isWhite, false, true) == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void verifyEnPassant(int row, int col)
        {
            if (Math.Abs(moveingRowIndex - row) == 2 && isPawnMove)
            {
                enPassantRow = row;
                enPassantColumn = col;
            }
            else
            {
                enPassantRow = 0;
                enPassantColumn = 0;
            }
        }

        public bool kingMove(int row, int col, bool isWhite, int checkNr = 1, bool VerifyCheckMate = false)
        {
            int valColOp;
            if (isWhite)
            {
                valColOp = 2;
            }
            else
            {
                valColOp = 1;
            }
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i == row && j == col)
                    {
                        continue;
                    }
                    if (i <= 8 && i >= 1 && j <= 8 && j >= 1)
                    {
                        if ((fileds[i, j].valueColorPiece == 0 || fileds[i, j].valueColorPiece == valColOp) && fileds[i, j].valuePiece != 10 && checkNr == 1)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, i, j, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, i, j, isWhite, true) == true)
                                {
                                    validFiled[i, j] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                        else if (fileds[i, j].valueColorPiece == valColOp && fileds[i, j].valuePiece == 10 && checkNr == 2)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, i, j, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, i, j, isWhite, true) == true)
                                {
                                    validFiled[row, col] = 0;
                                    GridBoard.Children.ElementAt(indexBorder[row, col]).Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                        if (checkNr == 1 && VerifyCheckMate == false)
                        {
                            kingMove(i, j, isWhite, 2);
                        }
                    }
                }
            }
            if (isCheck)
            {
                return false;
            }
            //verify castling 
            if ((col == 5 || col == 4) && (row == 8 || row == 1))
            {
                if (whitePiecesOrientation == -1)
                {
                    if (isWhite)
                    {
                        if (castlingWhite1 && fileds[row, col + 1].valueColorPiece == 0 && fileds[row, col + 2].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true && simulateMove(row, col, row, col + 1, isWhite, true) == true)
                                {
                                    validFiled[row, col + 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col + 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                        if (castlingWhite2 && fileds[row, col - 1].valueColorPiece == 0 && fileds[row, col - 2].valueColorPiece == 0 && fileds[row, col - 3].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true && simulateMove(row, col, row, col - 1, isWhite, true) == true)
                                {
                                    validFiled[row, col - 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col - 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (castlingBlack1 && fileds[row, col + 1].valueColorPiece == 0 && fileds[row, col + 2].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true && simulateMove(row, col, row, col + 1, isWhite, true) == true)
                                {
                                    validFiled[row, col + 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col + 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                        if (castlingBlack2 && fileds[row, col - 1].valueColorPiece == 0 && fileds[row, col - 2].valueColorPiece == 0 && fileds[row, col - 3].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true && simulateMove(row, col, row, col - 1, isWhite, true) == true)
                                {
                                    validFiled[row, col - 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col - 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (isWhite)
                    {
                        if (castlingWhite1 && fileds[row, col - 1].valueColorPiece == 0 && fileds[row, col - 2].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true && simulateMove(row, col, row, col - 1, isWhite, true) == true)
                                {
                                    validFiled[row, col - 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col - 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                        if (castlingWhite2 && fileds[row, col + 1].valueColorPiece == 0 && fileds[row, col + 2].valueColorPiece == 0 && fileds[row, col + 3].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true && simulateMove(row, col, row, col + 1, isWhite, true) == true)
                                {
                                    validFiled[row, col + 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col + 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (castlingBlack1 && fileds[row, col - 1].valueColorPiece == 0 && fileds[row, col - 2].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col - 2, isWhite, true) == true && simulateMove(row, col, row, col - 1, isWhite, true) == true)
                                {
                                    validFiled[row, col - 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col - 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                        if (castlingBlack2 && fileds[row, col + 1].valueColorPiece == 0 && fileds[row, col + 2].valueColorPiece == 0 && fileds[row, col + 3].valueColorPiece == 0)
                        {
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (simulateMove(row, col, row, col + 2, isWhite, true) == true && simulateMove(row, col, row, col + 1, isWhite, true) == true)
                                {
                                    validFiled[row, col + 2] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[row, col + 2]).Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool knightMove(int row, int col, bool isWhite, bool VerifyCheck = false, bool VerifyCheckMate = false)
        {
            int valColOp;
            if (isWhite)
            {
                valColOp = 2;
            }
            else
            {
                valColOp = 1;
            }
            for (int i = row - 2; i <= row + 2; i += 4)
            {
                for (int j = col - 1; j <= col + 1; j += 2)
                {
                    if (i <= 8 && i >= 1 && j <= 8 && j >= 1)
                    {
                        if ((fileds[i, j].valueColorPiece == 0 || fileds[i, j].valueColorPiece == valColOp))
                        {
                            if (VerifyCheck == false)
                            {
                                if (simulateMove(row, col, i, j, isWhite) == true)
                                {
                                    validFiled[i, j] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                if (fileds[i, j].codNamePiece == "N")
                                {
                                    return true;
                                }
                            }
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, i, j, isWhite) == true)
                                {
                                    return true;
                                }
                            }
                        }

                    }
                }
            }
            for (int i = row - 1; i <= row + 1; i += 2)
            {
                for (int j = col - 2; j <= col + 2; j += 4)
                {
                    if (i <= 8 && i >= 1 && j <= 8 && j >= 1)
                    {
                        if ((fileds[i, j].valueColorPiece == 0 || fileds[i, j].valueColorPiece == valColOp))
                        {
                            if (VerifyCheck == false)
                            {
                                if (simulateMove(row, col, i, j, isWhite) == true)
                                {
                                    validFiled[i, j] = 1;
                                    GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                if (fileds[i, j].codNamePiece == "N")
                                {
                                    return true;
                                }
                            }
                            if (VerifyCheckMate == true)
                            {
                                if (simulateMove(row, col, i, j, isWhite) == true)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool bishopMove(int row, int col, bool isWhite, bool VerifyCheck = false, bool VerifyCheckMate = false)
        {
            int valColOp;
            if (isWhite)
            {
                valColOp = 2;
            }
            else
            {
                valColOp = 1;
            }
            for (int ki = -1; ki <= 1; ki += 2)
            {
                for (int kj = -1; kj <= 1; kj += 2)
                {
                    int i = row;
                    int j = col;
                    while (i + ki <= 8 && i + ki >= 1 && j + kj <= 8 && j + kj >= 1 && fileds[i + ki, j + kj].valueColorPiece == 0)
                    {
                        i += ki;
                        j += kj;
                        if (VerifyCheck == false && VerifyCheckMate == false)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                validFiled[i, j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                            }
                        }
                        if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                return true;
                            }
                        }
                    }
                    if (i + ki <= 8 && i + ki >= 1 && j + kj <= 8 && j + kj >= 1 && fileds[i + ki, j + kj].valueColorPiece == 0 || fileds[i + ki, j + kj].valueColorPiece == valColOp)
                    {
                        i += ki;
                        j += kj;
                        if (VerifyCheck == false && VerifyCheckMate == false)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                validFiled[i, j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                            }
                        }
                        else if (VerifyCheck == true)
                        {
                            if (fileds[i, j].codNamePiece == "Q" || fileds[i, j].codNamePiece == "B")
                            {
                                return true;
                            }
                        }
                        else if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool rookMove(int row, int col, bool isWhite, bool VerifyCheck = false, bool VerifyCheckMate = false)
        {
            int valColOp;
            if (isWhite)
            {
                valColOp = 2;
            }
            else
            {
                valColOp = 1;
            }
            for (int ki = -1; ki <= 1; ki++)
            {
                for (int kj = -1; kj <= 1; kj++)
                {
                    if (Math.Abs(ki + kj) != 1)
                    {
                        continue;
                    }
                    int i = row;
                    int j = col;
                    while (i + ki <= 8 && i + ki >= 1 && j + kj <= 8 && j + kj >= 1 && fileds[i + ki, j + kj].valueColorPiece == 0)
                    {
                        i += ki;
                        j += kj;
                        if (VerifyCheck == false)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                validFiled[i, j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                            }
                        }
                        if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                return true;
                            }
                        }
                    }
                    if (i + ki <= 8 && i + ki >= 1 && j + kj <= 8 && j + kj >= 1 && fileds[i + ki, j + kj].valueColorPiece == 0 || fileds[i + ki, j + kj].valueColorPiece == valColOp)
                    {
                        i += ki;
                        j += kj;
                        if (VerifyCheck == false)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                validFiled[i, j] = 1;
                                GridBoard.Children.ElementAt(indexBorder[i, j]).Visibility = Visibility.Visible;
                            }
                        }
                        else if (VerifyCheck == true)
                        {
                            if (fileds[i, j].codNamePiece == "Q" || fileds[i, j].codNamePiece == "R")
                            {
                                return true;
                            }
                        }
                        else if (VerifyCheckMate == true)
                        {
                            if (simulateMove(row, col, i, j, isWhite) == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        int CheckCheck(int kingRow, int kingCloumn, bool isWhite)
        {
            if (knightMove(kingRow, kingCloumn, !isWhite, true) ||
               bishopMove(kingRow, kingCloumn, !isWhite, true) ||
               rookMove(kingRow, kingCloumn, !isWhite, true) ||
               pawnMove(kingRow, kingCloumn, !isWhite, true))
            {
                return 1;
            }
            return 0;
        }

        bool CheckCheckMate(int kingRoW, int kingColumn, bool isWhite)
        {
            int valOP;
            if (isWhite)
            {
                valOP = 2;
            }
            else
            {
                valOP = 1;
            }
            isWhite = !isWhite;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (fileds[i, j].valueColorPiece == valOP)
                    {
                        string pieceName = fileds[i, j].codNamePiece;
                        switch (pieceName)
                        {
                            case "":
                                if (pawnMove(i, j, isWhite, false, true))
                                {
                                    return false;
                                }
                                break;
                            case "K":
                                if (kingMove(i, j, isWhite, 1, true))
                                {
                                    return false;
                                }
                                break;
                            case "B":
                                if (bishopMove(i, j, isWhite, false, true) == true)
                                {
                                    return false;
                                }
                                break;
                            case "N":
                                if (knightMove(i, j, isWhite, false, true) == true)
                                {
                                    return false;
                                }
                                break;
                            case "R":
                                if (rookMove(i, j, isWhite, false, true) == true)
                                {
                                    return false;
                                }
                                break;
                            case "Q":
                                if (bishopMove(i, j, isWhite, false, true) == true)
                                {
                                    return false;
                                }
                                if (rookMove(i, j, isWhite, false, true) == true)
                                {
                                    return false;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }
    }
}