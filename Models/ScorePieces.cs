using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Lizst.Models
{
    public class ScorePieces
    {
        //Keep a static version of all types of instruments
        public static String[] Instruments = new string[] { "Flute","Piccolo","Clarinet", "Oboe", "Horn",
            "Trumpet", "Trombone", "Tuba", "Other_Brass", "Timpani", "Percussion", "Keyboards", "Violin", "Strings"};

        //Keep a static version of all sub-categories of instrument
        public static String[][] names = new string[][] { new String[]{ "Flute 1", "Flute 2", "Flute 3", "Flute 4"},
            new String[]{ "Piccolo 1", "Piccolo 2" }, new String[]{ "Clarinet 1", "Clarinet 2", "Clarinet 3" },
            new String[]{ "Oboe 1", "Oboe 2", "Oboe 3" }, new String[]{ "Horn 1", "Horn 2", "Horn 3", "Horn 4" },
            new String[]{ "Trumpet 1", "Trumpet 2", "Trumpet 3" }, new String[]{ "Trombone 1", "Trombone 2", "Trombone 3" },
            new String[]{ "Tuba 1", "Tuba 2"}, new String[]{ "Other Brass"}, new String[]{ "Timpani 1", "Timpani 2", "Timpani 3", "Timpani 4" , "Timpani 5"},
            new String[]{ "Snare Drum", "Tenor Drum", "Bass Drum", "Cymbals", "Triangle", "Tam-Tam", "Tambourine", "Wood Block", "Glockenspiel",
            "Xylophone", "Vibraphone", "Marimba", "Crotales", "Tubular Bells", "Mark Tree", "Drum Kit", "Other Percussion"},
            new String[]{ "Piano", "Celesta", "Pipe Organ", "Harpsichord", "Accordion", "Claviharp", "Other Keyboard"}, new String[]{ "Violin 1", "Violin 2", "Violin 3" },new String[]{ "Harp", "Viola", "Cello", "Double Bass", "Other String" }  };

        public int ScoreId { get; set; }
        public int PieceId { get; set; }
        public static IDictionary<string, Piece> IndexedSPieces;
        //public IDictionary<string, Piece> IndexedPieces = new Dictionary<string, Piece>();
    }

    public class ScorePiecesDBContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<ScorePieces> scorepieces { get; set; }
    }
}
