Imports System.Text.RegularExpressions
Public Class Form1
    Dim pot As Integer = 0
    Dim strPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    Dim i As Integer = strPath.Count
    Dim strpath2 As String = strPath.Substring(6, i - 16)
    Dim path As String = strpath2 + "\Resources\"
    Dim blk() As PictureBox
    Dim win As Boolean
    Dim turn As Boolean
    Enum Face
        Ace
        Two
        Three
        Four
        Five
        Six
        Seven
        Eight
        Nine
        Ten
        Jack
        Queen
        King
    End Enum
    Enum Suit
        Spades
        Hearts
        Clubs
        Diamonds
    End Enum
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        blk = New PictureBox() {PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5, PictureBox6, PictureBox7, PictureBox8, PictureBox9}
        Console.WriteLine(blk)
        Dim j As Integer = 1
        For i = 2 To 6
            blk(i).Top = 167
            blk(i).Left = 100 * j
            j += 1
        Next

        For i = 0 To 8
            blk(i).Width = 82
            blk(i).Height = 130
            blk(i).SizeMode = PictureBoxSizeMode.StretchImage
            Me.Controls.Add(blk(i))
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnNewDeal.Click
        Randomize()
        Dim cardpack As New ArrayList
        Dim Suits = [Enum].GetNames(GetType(Suit))
        Dim Faces = [Enum].GetNames(GetType(Face))
        For Each Face In Faces 'Initizalizing cardpack with 52 cards
            For Each Suit In Suits
                cardpack.Add(Face + Suit)
            Next
        Next
        Dim ShuffledDeck As New ArrayList
        Dim rand As New Random
        While cardpack.Count > 0 'Adding cards at random to new arraylist and ensuring no double picks
            Dim index As Integer = rand.Next(0, cardpack.Count)
            ShuffledDeck.Add(cardpack(index))
            cardpack.RemoveAt(index)
        End While

        Dim playerHand As New ArrayList
        For i = 0 To 1 'Taking cards from top of the deck, from index 0 and removing them
            playerHand.Add(ShuffledDeck(0))
            ShuffledDeck.RemoveAt(0)
        Next
        Dim playerPlayedCards As New ArrayList
        For i = 0 To 1
            blk(i).Image = Image.FromFile(path + playerHand(i) + ".png")
            playerPlayedCards.Add(playerHand(i))
        Next
        Label1.Text = playerHand(0)
        Label2.Text = playerHand(1)
        Dim computerPlayedCards As New ArrayList
        Dim computerHand As New ArrayList
        For i = 0 To 1
            computerHand.Add(ShuffledDeck(0))
            ShuffledDeck.RemoveAt(0)
            computerPlayedCards.Add(computerHand)
        Next
        Label3.Text = computerHand(0)
        Label4.Text = computerHand(1)

        Dim communityCards As New ArrayList
        For i = 0 To 4
            communityCards.Add(ShuffledDeck(0))
            ShuffledDeck.RemoveAt(0)
        Next

        For i = 0 To 4
            blk(i + 2).Image = Image.FromFile(path + communityCards(i) + ".png")
            playerPlayedCards.Add(communityCards(i))
            computerPlayedCards.Add(communityCards(i))
        Next

        'Dim playerPlayedCards As String() = {"AceSpades", "AceClubs", "TwoDiamonds", "TwoHearts", "FiveHearts", playerHand(0), playerHand(1)}
    End Sub
    'Function IsFlush2(ByVal card As ArrayList) As String
    '    Dim myDict As New Dictionary(Of String, ArrayList)
    '    Dim operands As New ArrayList
    '    Dim return_str As String = ""
    '    For i = 0 To 6
    '        operands.Add(Regex.Split(card(i), "(?=[A-Z])"))
    '    Next

    '    Dim ArrayItem As String
    '    For i = 0 To 6
    '        ArrayItem = operands(i)(2)
    '        If myDict.ContainsKey(ArrayItem) Then
    '            myDict(ArrayItem) = myDict(ArrayItem + 1)
    '        Else
    '            myDict.Add(ArrayItem, operands(i)(2))
    '        End If
    '    Next
    '    Dim keys As List(Of String) = myDict.Keys.ToList
    '    keys.Sort()
    '    Dim str As String
    '    For Each str In keys
    '        If myDict.Item(str) >= 4 Then
    '            return_str = str
    '        End If
    '    Next
    '    Return return_str
    'End Function

    Function IsStraightFlush(ByVal card As ArrayList) As Boolean
        If IsFlush(card) <> "False" And IsStraight(card) = True Then
            Return True
        End If
        Return False
    End Function
    Function FourOfAKind(ByVal card As ArrayList) As Boolean
        Dim Faces = [Enum].GetNames(GetType(Face))
        Dim QuadDict As New Dictionary(Of String, Integer)
        Dim QuadFaces As New ArrayList
        Dim cardArray As New ArrayList
        For Each Face In Faces
            QuadFaces.Add(Face)
        Next
        For i = 0 To 6
            cardArray.Add(Regex.Split(card(i), "(?=[A-Z])"))
        Next
        For i = 0 To 12
            QuadDict.Add(QuadFaces(i), 0)
        Next
        For i = 0 To 6
            QuadDict(cardArray(i)(1)) += 1
        Next
        For i = 0 To 12
            If QuadDict(QuadFaces(i)) > 0 Then
                Return True
            End If
        Next
        Return False
    End Function
    Function IsFullHouse(ByVal card As ArrayList) As String
        If IsFullHouse(card) = True Then
            Dim Faces = [Enum].GetNames(GetType(Face))
            Dim FullHouseDict As New Dictionary(Of String, Integer)
            Dim FullHouseFaces As New ArrayList
            Dim cardArray As New ArrayList
            For Each Face In Faces
                FullHouseFaces.Add(Face)
            Next
            For i = 0 To 6
                cardArray.Add(Regex.Split(card(i), "(?=[A-Z])"))
            Next
            For i = 0 To 12
                FullHouseDict.Add(FullHouseFaces(i), 0)
            Next
            For i = 0 To 6
                FullHouseDict(cardArray(i)(1)) += 1
            Next
            For i = 0 To 12
                If FullHouseDict(FullHouseFaces(i)) = 2 Then
                    Return "Full House"
                End If
            Next
        End If
        Return "False"
    End Function
    Function IsFlush(ByVal card As ArrayList) As String
        Dim pattern() As String = {"*Diamonds", "*Clubs", "*Hearts", "*Spades"}
        Dim cardStr As String = ""
        For i = 0 To 6
            For j = 0 To 3
                If card(i) Like pattern(j) = True Then
                    cardStr = cardStr + CStr(j + 1)
                End If
            Next
        Next
        Dim suitsArray() As Integer = {0, 0, 0, 0}
        While cardStr.Length > 0
            For i = 1 To 4
                If cardStr.IndexOf(i) <> -1 Then
                    Dim y As Integer = cardStr.IndexOf(i)
                    cardStr = cardStr.Remove(y, 1)
                    suitsArray(i - 1) += 1
                End If
            Next
        End While
        For i = 0 To 3
            If suitsArray(i) >= 5 Then
                If i = 0 Then
                    Return "Diamonds"
                ElseIf i = 1 Then
                    Return "Clubs"
                ElseIf i = 2 Then
                    Return "Hearts"
                ElseIf i = 3 Then
                    Return "Spades"
                End If
            End If
        Next
        Return "False"
    End Function
    Function IsStraight(ByVal card As ArrayList) As String
        Dim Faces = [Enum].GetNames(GetType(Face))
        Dim straightDict As New Dictionary(Of String, Integer)
        Dim straightFaces As New ArrayList
        Dim cardArray As New ArrayList
        Dim return_str As String
        For Each Face In Faces
            straightFaces.Add(Face)
        Next
        For i = 0 To 6
            cardArray.Add(Regex.Split(card(i), "(?=[A-Z])"))
        Next
        For i = 0 To 12
            straightDict.Add(straightFaces(i), 0)
        Next
        For i = 0 To 6
            straightDict(cardArray(i)(1)) += 1
        Next
        For i = 0 To 8
            If straightDict(straightFaces(i)) > 0 Then
                If straightDict(straightFaces(i + 1)) > 0 Then
                    If straightDict(straightFaces(i + 2)) > 0 Then
                        If straightDict(straightFaces(i + 3)) > 0 Then
                            If straightDict(straightFaces(i + 4)) > 0 Then
                                return_str = straightFaces(i + 4)
                                Return return_str
                            End If
                        End If
                    End If
                End If
            End If
        Next

        Return "False"
    End Function

    Function IsTriple(ByVal card As ArrayList) As String
        Dim Faces = [Enum].GetNames(GetType(Face))
        Dim TripleDict As New Dictionary(Of String, Integer)
        Dim TripleFaces As New ArrayList
        Dim cardArray As New ArrayList
        For Each Face In Faces
            TripleFaces.Add(Face)
        Next
        For i = 0 To 6
            cardArray.Add(Regex.Split(card(i), "(?=[A-Z])"))
        Next
        For i = 0 To 12
            TripleDict.Add(TripleFaces(i), 0)
        Next
        For i = 0 To 6
            TripleDict(cardArray(i)(1)) += 1
        Next
        For i = 0 To 11
            If TripleDict(TripleFaces(i)) = 3 Then
                Return TripleFaces(i)
            End If
        Next
        Return "No Triple"
    End Function

    Function TwoPairOrSinglePair(ByVal card As ArrayList) As String
        Dim Faces = [Enum].GetNames(GetType(Face))
        Dim PairDict As New Dictionary(Of String, Object)
        Dim PairFaces As New ArrayList
        Dim cardArray As New ArrayList
        For Each Face In Faces
            PairFaces.Add(Face)
        Next
        For i = 0 To 6
            cardArray.Add(Regex.Split(card(i), "(?=[A-Z])"))
            cardArray(i)(0) = 0
            If cardArray(i)(2) = "Spades" Then
                cardArray(i)(2) = CInt(4)
            ElseIf cardArray(i)(2) = "Hearts" Then
                cardArray(i)(2) = CInt(3)
            ElseIf cardArray(i)(2) = "Clubs" Then
                cardArray(i)(2) = CInt(2)
            ElseIf cardArray(i)(2) = "Diamonds" Then
                cardArray(i)(2) = CInt(1)
            End If
        Next
        For i = 0 To 12
            PairDict.Add(PairFaces(i), {0, 0})
        Next
        For i = 0 To 6
            If PairDict.ContainsKey(cardArray(i)(1)) Then
                PairDict(cardArray(i)(1))(0) += 1
                Dim oldSuit As Integer = PairDict(cardArray(i)(1))(1)
                If cardArray(i)(2) > oldSuit Then
                    PairDict(cardArray(i)(1))(1) = cardArray(i)(2)
                Else
                    PairDict(cardArray(i)(1))(1) = oldSuit
                End If
            End If
        Next
        Dim pairCount As Integer = 0
        Dim pairValue As String = ""
        For i = 0 To 12
            If PairDict(PairFaces(i))(0) = 2 Then
                pairCount += 1
                pairValue = PairFaces(i)
            End If
        Next
        If pairCount = 1 Then
            Return pairValue
        ElseIf pairCount > 1 Then
            Return "Two Pair"
        End If
        Return "No Pair"
    End Function

    'Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
    '    Dim BetAmount As Integer = Val(Me.TextBox1.Text)

    '    If BetAmount = 0 Then
    '        MsgBox("Enter a bet amount!")
    '    Else

    '        pot += BetAmount
    '    End If
    '    lblpotTotal.Text = pot
    '    TextBox1.Text = ""
    'End Sub

End Class
