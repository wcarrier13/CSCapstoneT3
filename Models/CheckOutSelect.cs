using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class CheckOutSelect
    {
        public List<Musician> Musicians;
        public List<Score> Scores;
        public List<List<Piece>> Pieces;
        public int[,] Defaults;



        public CheckOutSelect()
        {
            Musicians = new List<Musician>();
            Scores = new List<Score>();
            Pieces = new List<List<Piece>>();
        }

        public void AddMusician(Musician m)
        {
            Musicians.Add(m);
        }

        public void AddAllMusicians(IEnumerable<Musician> musicians)
        {
            foreach(Musician m in musicians)
            {
                AddMusician(m);
            }
        }

        public void AddScore(Score s)
        {
            Scores.Add(s);
            Pieces.Add(new List<Piece>());
        }

        public void AddAllScores(IEnumerable<Score> scores)
        {
            foreach(Score s in scores)
            {
                AddScore(s);
            }

        }

        public void AddPiece(Piece p)
        {
            int s = Scores.FindIndex(e=>e.ScoreId == p.ScoreId);

            List<Piece> list = Pieces[s];
            list.Add(p);
        }

        public void AddAllPieces(IEnumerable<Piece> pieces)
        {
            foreach(Piece p in pieces)
            {
                AddPiece(p);
            }
        }

        public void MatchMusicians()
        {
            Defaults = new int[Musicians.Count(),Scores.Count()];
            //For every musician and every score, find the best part to assign the musician.
            for(int k = 0; k < Musicians.Count(); k++)
            {
                Musician m = Musicians[k];
                for (int i = 0; i < Scores.Count(); i++)
                {
                    Score s = Scores[i];
                    List<Piece> piecesInScore = Pieces[i];
                    Boolean added = false;
                    for(int j = 0; j < piecesInScore.Count(); j++)
                    {
                        //Musician matched a piece, assign them.
                        if(piecesInScore[j].Instrument == m.Part)
                        {
                            Defaults[k, i] = j;
                            added = true;
                            break;
                        }
                    }
                    //No pieces matched, do not assign anything.
                    if (!added)
                    {
                        Defaults[k, i] = -1;
                    }
                }
            }
        }
    }
}
