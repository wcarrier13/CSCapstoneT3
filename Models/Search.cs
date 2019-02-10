using System;

namespace Lizst.Models
{
    public static class Search
    {
        //Implements the Levenshtein Distance between two strings
        //Takes two strings and returns an integer that is their
        //distance. This function ignores case.
        public static int compare(string a, string b)
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


        //Takes a score and a search string, determines relevancy.
        //Currently only looks for title contains text.
        public static bool Relevant(Score score, string search)
        {
            string title = score.Title.ToLower();
            if (title.Contains(search))
            {
                return true;
            }

            return false;
        }
    }
}