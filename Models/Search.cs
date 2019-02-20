using System;
using System.Collections.Generic;
using System.Linq;

namespace Lizst.Models
{
    public static class Search
    {
        public const int StringDistance = 4;

        public static IEnumerable<Score> FindRelevant(String search, LizstContext context)
        {
            search = search.ToLower();

            //Search by title
            IEnumerable<Score> scores =
                from score in context.Score
                where score.Title.Contains(search)
                select score;

            //Search by composer
            IEnumerable<Score> byComposer =
                from score in context.Score
                where score.Composer.Contains(search)
                select score;
            scores = scores.Concat(byComposer);

            //Search by genre
            IEnumerable<Score> byGenre =
                from score in context.Score
                where score.Genre.Contains(search)
                select score;
            scores = scores.Concat(byGenre);

            //Search by secondary classification
            IEnumerable<Score> byClass =
                from score in context.Score
                where score.SecondaryClassification.Contains(search)
                select score;
            scores = scores.Concat(byClass);

            //No scores found, lower strictness to fuzzy string matching.
            if (!scores.Any())
            {
                String[] pieces = search.Split(" ");
                scores =
                    from score in context.Score
                    where FuzzyTitles(pieces, score)
                    select score;
            }
            return scores;
        }

        // Goes through the array of search terms and the array of title terms.
        // If any pair have a Levenshtein distance of less than StringDistance,
        // this will return true
        public static Boolean FuzzyTitles(String[] search, Score score)
        {
            String[] title = score.Title.Split(" ");
            foreach (String str1 in title)
            {
                foreach (String str2 in search)
                {
                    if(Compare(str1, str2) < StringDistance)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //Implements the Levenshtein Distance between two strings
        //Takes two strings and returns an integer that is their
        //distance. This function ignores case.
        public static int Compare(string a, string b)
        {
            a.ToLower();
            b.ToLower();
            int[,] distanceMatrix = new int[a.Length + 1,b.Length + 1];

            //Initialize the array.
            for(int i = 0; i <= a.Length; i++)
            {
                for(int j = 0; j <= b.Length; j++)
                {
                    distanceMatrix[i, j] = 0;
                }
            }

            //Distance between a substring of a and empty string.
            for(int i = 0; i <= a.Length; i++)
            {
                distanceMatrix[i, 0] = i;
            }

            //Distance between a substring of b and empty string
            for(int j = 0; j <= b.Length; j++)
            {
                distanceMatrix[0, j] = j;
            }

            for(int j = 1; j <= b.Length; j++)
            {
                for(int i = 1; i <= a.Length; i++)
                {
                    int subCost;
                    if(a[i-1] == b[j-1])
                    {
                        subCost = 0;
                    } else
                    {
                        subCost = 1;
                    }

                    distanceMatrix[i, j] = Math.Min(
                        distanceMatrix[i - 1, j] + 1, Math.Min( //Delete to match
                        distanceMatrix[i, j - 1] + 1, //Insert to match
                        distanceMatrix[i - 1, j - 1] + subCost)); //Substitute to match
                }
            }

            return distanceMatrix[a.Length, b.Length];
        }
    }
}