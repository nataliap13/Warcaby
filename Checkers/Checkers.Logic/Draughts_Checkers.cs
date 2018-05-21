﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
    public class Draughts_checkers
    {
        private Checkers_piece[,] board_black;//row,column
        private int _number_of_fields_in_row;
        private int _player_black_secret_key = 0;
        private int _player_white_secret_key = 0;
        private int _player_turn_key;
        private Random _rand = new Random();

        public Draughts_checkers(int number_of_fields_in_row_, int number_of_pieces_per_player)
        {
            if (number_of_pieces_per_player >= (number_of_fields_in_row_ * number_of_fields_in_row_ / 4))
            {
                throw new Exception("To many pieces per player.");
            }
            _number_of_fields_in_row = number_of_fields_in_row_;
            int number_of_whites = number_of_pieces_per_player;
            int number_of_blacks = number_of_pieces_per_player;
            board_black = new Checkers_piece[_number_of_fields_in_row, _number_of_fields_in_row];

            for (int i = 0; i < _number_of_fields_in_row; i++)//i is row
            {
                for (int j = 0; j < _number_of_fields_in_row; j++)//j is column
                {
                    if ((i % 2 + j % 2) == 1 && number_of_whites > 0)
                    {
                        board_black[i, j] = new Checkers_piece(Color.White, Type.Man);
                        number_of_whites--;
                    }
                    else if (number_of_whites == 0)
                    { break; }
                }
            }

            for (int i = _number_of_fields_in_row - 1; i >= 0; i--)//i is row
            {
                for (int j = 0; j < _number_of_fields_in_row; j++)//j is column
                {
                    if ((i % 2 + j % 2) == 1 && number_of_blacks > 0)
                    {
                        board_black[i, j] = new Checkers_piece(Color.Black, Type.Man);
                        number_of_blacks--;
                    }
                    else if (number_of_blacks == 0)
                    { break; }
                }
            }
        }
        public void Load_board(Checkers_piece[,] board, Color color)
        {
            if (color == Color.Black)
            { board_black = board; }
            else { board_black = Rotate_board(board); }
        }
        public int Number_of_fields_in_row { get => _number_of_fields_in_row; }
        public int Number_of_pieces(Color color, Type type)
        {
            int number_of_pieces = 0;
            for (int i = 0; i < Number_of_fields_in_row; i++)
            {
                for (int j = 0; j < Number_of_fields_in_row; j++)
                {
                    if (board_black[i, j].Color == color && board_black[i, j].Type == type)
                    { number_of_pieces++; }
                }
            }
            return number_of_pieces;
        }

        public Checkers_piece[,] Get_board(Color color)//row,column
        {
            if (color == Color.Black)
            { return board_black; }
            else
            { return Rotate_board(board_black); }
        }
        private void Set_board(Color color, Checkers_piece[,] new_board)//row,column
        {
            if (color == Color.Black)
            { board_black = new_board; }
            else
            { board_black = Rotate_board(new_board); }
        }
        private Checkers_piece[,] Rotate_board(Checkers_piece[,] board_to_rotate)
        {
            Checkers_piece[,] board_rotated = new Checkers_piece[_number_of_fields_in_row, _number_of_fields_in_row];//row,column

            for (int i = 0; i < _number_of_fields_in_row; i++)//i is row
            {
                for (int j = 0; j < _number_of_fields_in_row; j++)//j is column
                {
                    board_rotated[_number_of_fields_in_row - i - 1, _number_of_fields_in_row - j - 1] = board_to_rotate[i, j];
                }
            }
            return board_rotated;
        }

        public int Generate_player_key(bool is_player_white)
        {
            if (_player_white_secret_key == 0 && is_player_white == true)
            {
                _player_turn_key = _player_white_secret_key = _rand.Next(10000, 32000);
                return _player_white_secret_key;
            }
            else if (_player_black_secret_key == 0 && is_player_white == false)
            {
                return _player_black_secret_key = _rand.Next(10000, 32000);
            }
            else
            {
                throw new Exception("Player already exists!");
            }

        }
        private void Switch_player_turn()
        {
            if (_player_turn_key == _player_black_secret_key)
            { _player_turn_key = _player_white_secret_key; }

            else if (_player_turn_key == _player_white_secret_key)
            { _player_turn_key = _player_black_secret_key; }
            else
            { throw new Exception("Unexpected Switch_player_turn_key exception!"); };
        }
        public Color Check_active_player()
        {
            if (_player_turn_key == _player_black_secret_key)
            { return Color.Black; }

            else if (_player_turn_key == _player_white_secret_key)
            { return Color.White; }
            else
            { throw new Exception("The player did not join yet!"); }
        }
        public bool Check_active_player(int player_secret_key)
        {
            if (_player_turn_key == player_secret_key)
            { return true; }
            else
            { return false; }
        }
        public Color Check_player_color(int player_secret_key)
        {
            if (player_secret_key == _player_black_secret_key)
            { return Color.Black; }

            else if (player_secret_key == _player_white_secret_key)
            { return Color.White; }
            else
            { throw new Exception("The player did not join yet!"); }
        }
        public Color Check_oponent_player()
        {
            if (_player_turn_key != _player_black_secret_key)
            { return Color.Black; }

            else if (_player_turn_key != _player_white_secret_key)
            { return Color.White; }
            else
            { throw new Exception("The player did not join yet!"); }
        }

        public void Make_move(int player_secret_key, Coordinates origin, Coordinates destination)//int is error code, to be implemented
        {
            if (Check_active_player(player_secret_key) == false)
            { throw new Exception("Wrong player is trying to make a move. It's Not your turn, " + Check_oponent_player() + "!"); }

            if (origin == destination)
            { throw new Exception("Origin and destination is the same field!"); }

            var work_board = Get_board(Check_active_player());
            var current_piece = work_board[origin.Y, origin.X];
            if (current_piece == null)
            { throw new Exception("Origin field is empty!"); }

            var checkers_piece_dest = work_board[destination.Y, destination.X];
            if (checkers_piece_dest != null)
            { throw new Exception("Destination field " + destination.ToString() + " is not empty!"); }

            if (Check_active_player() != current_piece.Color)
            { throw new Exception("Your are trying to move not your piece!"); }

            if ((destination.Y + destination.X) % 2 == 0)
            { throw new Exception("Your are trying to move a piece to the white field!"); }
            //wlasciwa rozgrywka
            {
                //szukanie najdluzszych bic dla pionkow ktore to bicie oferuja
                List<List<Coordinates>> all_the_longest_possible_ways = new List<List<Coordinates>>();
                int length_of_capturing = 0;
                for (int i = 0; i < _number_of_fields_in_row; i++)//row
                {
                    for (int j = 0; j < _number_of_fields_in_row; j++)//column
                    {
                        var possible_ways_for_this_piece = Find_next_capture_for_this_piece(work_board, new Coordinates(j, i));
                        if (possible_ways_for_this_piece.Count() > 0)
                        {
                            foreach (var way in possible_ways_for_this_piece)
                            {//dlugosc bicia jest o 1 krotsza od dlugosci sciezki, dlatego najpierw sprawdzamy a potem dodajemy poczatkowe wspolrzedne
                                if (way.Count == length_of_capturing && length_of_capturing > 0)
                                {
                                    way.Reverse();
                                    way.Add(new Coordinates(j, i));
                                    way.Reverse();
                                    all_the_longest_possible_ways.Add(way);
                                }
                                else if (way.Count() > length_of_capturing)
                                {
                                    length_of_capturing = way.Count();
                                    all_the_longest_possible_ways = new List<List<Coordinates>>();
                                    way.Reverse();
                                    way.Add(new Coordinates(j, i));
                                    way.Reverse();
                                    all_the_longest_possible_ways.Add(way);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("\nZnaleziono " + length_of_capturing + " bic z rzedu.");
                Console.WriteLine("Najdluzsze bicia mozna wykonac sciezkami: ");
                foreach (var way in all_the_longest_possible_ways)
                {
                    foreach (var step in way)
                    {
                        Console.Write(" -> " + step.ToString());
                    }
                    Console.Write("\n");
                }

                //odleglosc wraz ze znakiem zwrotu/kierunku
                var x_distance = destination.X - origin.X;
                var y_distance = destination.Y - origin.Y;
                if (length_of_capturing > 0)//bicie jest obowiazkowe
                {
                    List<Coordinates> chosen_way = new List<Coordinates>();//szukanie sciezki wybranej przez gracza
                    foreach (var way in all_the_longest_possible_ways)
                    {
                        if ((way[0] == origin) && (way[1] == destination))
                        {
                            chosen_way = way;
                        }
                    }
                    if (chosen_way.Count() == 0)//exception
                    {
                        string s = string.Empty;
                        foreach (var way in all_the_longest_possible_ways)
                        {
                            foreach (var step in way)
                            {
                                s += " -> " + step.ToString();
                            }
                            s += "\n";
                        }
                        throw new Exception("\nYou have to choose one of presented ways:\n" + s);
                    }
                    else//wlasciwe bicie
                    {
                        Single_capturing_by_piece(ref work_board, origin, destination);
                        if (length_of_capturing > 1)
                        {
                            Switch_player_turn();//zmiana aktywnego gracza. Na koncu sprawdzania zasad zawsze jest zamiana,
                            //wiec poprzez podwojna zamiane, tura wroci na bijacego gracza i bedzie on miec dodatkowy ruch na bicie
                        }
                        else if (length_of_capturing == 1 && current_piece.Type == Type.Man && destination.Y == 0)//jesli ruszymy pionek o 1 i dociera on do bandy to na pewno zostanie zamieniony na dame
                        {
                            work_board[destination.Y, destination.X] = new Checkers_piece(Check_active_player(), Type.King);
                            Set_board(Check_active_player(), work_board);
                        }
                    }
                }
                else if ((x_distance == 1 || x_distance == -1) && y_distance == -1)//przesuniecie pionka lub damy do przodu
                {
                    work_board[destination.Y, destination.X] = work_board[origin.Y, origin.X];
                    work_board[origin.Y, origin.X] = null;
                    Set_board(Check_active_player(), work_board);
                    if (current_piece.Type == Type.Man && destination.Y == 0)//jesli ruszymy pionek o 1 i dociera on do bandy to na pewno zostanie zamieniony na dame
                    {
                        work_board[destination.Y, destination.X] = new Checkers_piece(Check_active_player(), Type.King);
                        Set_board(Check_active_player(), work_board);
                    }
                }
                else if ((x_distance == y_distance) && current_piece.Type == Type.King)//przesuniecie damy w dowolnym kierunku ukosnym
                {
                    //trzeba sprawdzic czy po drodze nie ma pionka przeciwnika ktory mozna zbic
                    //jesli na drodze jest nasz pionek to ruch nie moze zostac wykonany
                    //todo to do
                    work_board[destination.Y, destination.X] = work_board[origin.Y, origin.X];
                    work_board[origin.Y, origin.X] = null;
                    Set_board(Check_active_player(), work_board);
                }
                //czy jest bicie
                else
                { throw new Exception("Move " + origin.ToString() + "->" + destination.ToString() + " not allowed!"); }
            }
            Switch_player_turn();//zmien aktywnego gracza na drugiego gracza jesli nie bylo bicia albo bylo to juz ostatnie mozliwe bicie
        }
        //todo to do finish this
        //!!!!!!!!!!!!!!!!!!!!!
        private List<List<Coordinates>> Find_next_capture_for_this_piece(Checkers_piece[,] work_board, Coordinates origin)
        {//jesli wykonano juz jedno bicie, to kolejne musi byc wykonane tym samym pionkiem
            try
            {
                if (work_board[origin.Y, origin.X].Color == Check_active_player())
                {
                    var possible_ways = new List<List<Coordinates>>();

                    if (work_board[origin.Y, origin.X].Type == Type.Man)
                    {
                        Coordinates oponent1 = new Coordinates(origin.X - 1, origin.Y - 1);
                        Coordinates oponent2 = new Coordinates(origin.X + 1, origin.Y - 1);
                        Coordinates oponent3 = new Coordinates(origin.X + 1, origin.Y + 1);
                        Coordinates oponent4 = new Coordinates(origin.X - 1, origin.Y + 1);
                        List<Coordinates> oponents = new List<Coordinates>();
                        oponents.Add(oponent1);
                        oponents.Add(oponent2);
                        oponents.Add(oponent3);
                        oponents.Add(oponent4);

                        Coordinates dest1 = new Coordinates(origin.X - 2, origin.Y - 2);
                        Coordinates dest2 = new Coordinates(origin.X + 2, origin.Y - 2);
                        Coordinates dest3 = new Coordinates(origin.X + 2, origin.Y + 2);
                        Coordinates dest4 = new Coordinates(origin.X - 2, origin.Y + 2);
                        List<Coordinates> dests = new List<Coordinates>();
                        dests.Add(dest1);
                        dests.Add(dest2);
                        dests.Add(dest3);
                        dests.Add(dest4);

                        for (int i = 0; i < dests.Count(); i++)
                        {
                            try
                            {
                                if ((work_board[oponents[i].Y, oponents[i].X].Color == Check_oponent_player()) && (work_board[dests[i].Y, dests[i].X] == null))
                                {
                                    //Console.WriteLine("\noponent: " + oponents[i].ToString());
                                    //Console.WriteLine("dest: " + dests[i].ToString());
                                    var copy_of_board = new Checkers_piece[_number_of_fields_in_row, _number_of_fields_in_row];
                                    copy_of_board = work_board.Clone() as Checkers_piece[,];
                                    Single_capturing_by_piece(ref copy_of_board, origin, dests[i]);//trzeba wykonac to bicie na kopii planszy
                                    var local_possible_ways = Find_next_capture_for_this_piece(copy_of_board, dests[i]);
                                    //Console.WriteLine("Found " + local_possible_ways.Count());
                                    if (local_possible_ways.Count() == 0)
                                    {
                                        var new_list = new List<Coordinates>();
                                        new_list.Add(new Coordinates(dests[i].X, dests[i].Y));
                                        local_possible_ways.Add(new_list);
                                    }
                                    else
                                    {
                                        foreach (var way in local_possible_ways)
                                        {
                                            way.Reverse();
                                            way.Add(new Coordinates(dests[i].X, dests[i].Y));
                                            way.Reverse();
                                        }
                                    }
                                    possible_ways = possible_ways.Concat(local_possible_ways).ToList();
                                }
                            }
                            catch (Exception e)
                            { }
                        }
                    }
                    //dama todo, tylo przekopiowane mniej wiecej, ale nie moze tak sobie nadpisywac tej liczby magicznej
                    else if (work_board[origin.Y, origin.X].Type == Type.King)
                    {//dama moze bic po skosie i zatrzymac sie na dowlonym polu za pionkiem. Nie musi to byc pole bezposrednio za pionkiem.
                        for (int i = 1; i < Number_of_fields_in_row; i++)//i to polozenie pionka/damy przeciwnika
                        {
                            for (int j = i + 1; j < Number_of_fields_in_row; j++)//j to odleglosc pomiedzy polem zbijanego pionka a wolnym polem za zbijanym pionkiem/dama
                            {
                                try
                                {
                                    if ((work_board[origin.Y - i, origin.X - i].Color == Check_oponent_player()) && (work_board[origin.Y - j, origin.X - j] == null))
                                    {
                                    }
                                }
                                catch (Exception e)
                                { }

                                try
                                {
                                    if ((work_board[origin.Y - i, origin.X + i].Color == Check_oponent_player()) && (work_board[origin.Y - j, origin.X + j] == null))
                                    {
                                    }
                                }
                                catch (Exception e)
                                { }

                                try
                                {
                                    if ((work_board[origin.Y + i, origin.X + i].Color == Check_oponent_player()) && (work_board[origin.Y + j, origin.X + j] == null))
                                    {
                                    }
                                }
                                catch (Exception e)
                                { }

                                try
                                {
                                    if ((work_board[origin.Y + i, origin.X - i].Color == Check_oponent_player()) && (work_board[origin.Y + j, origin.X - j] == null))
                                    {
                                    }
                                }
                                catch (Exception e)
                                { }
                            }
                        }
                    }
                    else
                    { throw new Exception("Something went wrong with piece type!"); }
                    var final_possible_ways = new List<List<Coordinates>>();
                    int final_length = 0;
                    foreach (var way in possible_ways)
                    {
                        if (way.Count() == final_length)
                        {
                            final_possible_ways.Add(way);
                        }
                        else if (way.Count() > final_length)
                        {
                            final_possible_ways = new List<List<Coordinates>>();
                            final_possible_ways.Add(way);
                        }
                    }
                    return final_possible_ways;
                }
                else
                { throw new Exception("Something went wrong!"); }
            }
            catch (Exception e)
            { }
            return new List<List<Coordinates>>();
        }
        private void Single_capturing_by_piece(ref Checkers_piece[,] work_board, Coordinates origin, Coordinates destination)//changes a work_board!
        {
            var current_piece = work_board[origin.Y, origin.X];
            //odleglosc wraz ze znakiem zwrotu/kierunku
            var x_distance = destination.X - origin.X;
            var y_distance = destination.Y - origin.Y;

            if ((x_distance == 2 || x_distance == -2) && (y_distance == 2 || y_distance == -2))//bicie pionkiem w dowolnym kierunku
            {
                work_board[destination.Y, destination.X] = work_board[origin.Y, origin.X];
                work_board[origin.Y, origin.X] = null;
                var oponent_piece_coords = new Coordinates((destination.X - x_distance / 2), (destination.Y - y_distance / 2));
                var oponent_piece = work_board[oponent_piece_coords.Y, oponent_piece_coords.X];
                if (Check_oponent_player() == oponent_piece.Color)
                { }
                else
                { throw new Exception("Your are trying to jump over your own piece!"); }

                work_board[oponent_piece_coords.Y, oponent_piece_coords.X] = null;
                Set_board(Check_active_player(), work_board);
                var possible_ways = Find_next_capture_for_this_piece(work_board, origin);

                if (current_piece.Type == Type.Man && destination.Y == 0 && (possible_ways.Count() == 0))//jesli nie ma dalszego bicia to pionek sie zmienia na dame
                { work_board[destination.Y, destination.X] = new Checkers_piece(Check_active_player(), Type.King); }
            }
            else
            { throw new Exception("Capturing is not allowed right now!"); }
        }

        //todo to do ta funkcja ponizej
        private void Single_capturing_by_king() { }
        /*private void Display_board_helper(Checkers_piece[,] board, Color color)
        {
            Console.Write("\n---");
            for (int i = 0; i < _number_of_fields_in_row; i++)
            { Console.Write(i + " "); }

            for (int i = 0; i < _number_of_fields_in_row; i++)//i is row
            {
                Console.Write("\n" + i + ". ");
                for (int j = 0; j < _number_of_fields_in_row; j++)//j is column
                {
                    if (board[i, j] == null)
                    { Console.Write("= "); }
                    //{ Console.Write("# "); }
                    else
                    { Console.Write(board[i, j].ToString() + " "); }
                }
            }
            Console.Write("\n---");
            for (int i = 0; i < _number_of_fields_in_row; i++)
            { Console.Write(i + " "); }
            Console.Write(color + "\n");
        }*/
    }
}
