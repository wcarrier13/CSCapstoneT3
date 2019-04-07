using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class CheckInModel
    {
        public String ScoreTitle;
        public int id;
        public List<Musician> Musicians;
        public List<List<Piece>> Pieces;

        public CheckInModel(String title, int id)
        {
            ScoreTitle = title;
            this.id = id;
            Musicians = new List<Musician>();
            Pieces = new List<List<Piece>>();
        }

        public void AddMusician(Musician m)
        {
            if(Musicians.Any(e => e.MusicianId == m.MusicianId))
            {
                return;
            }

            Musicians.Add(m);
            Pieces.Add(new List<Piece>());
        }

        public void AddPiece(Piece p, int musicianId)
        {
            int index = Musicians.FindIndex(e => e.MusicianId == musicianId);
            Pieces[index].Add(p);
        }

    }
}
