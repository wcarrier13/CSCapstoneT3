using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lizst.Models
{
    public class SearchModel
    {
        public String Search;
        public String Genre;
        public Score Score;
        public IEnumerable<Piece> sps;
        public IEnumerable<Score> Results;
    }
}
