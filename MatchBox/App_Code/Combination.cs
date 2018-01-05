using System.Collections.Generic;

namespace MatchBox
{
    public class Combination<T>
    {
        // Get all combinations of size n_size_temp in arr_item[] of size n_size_item.

        public static void Bind_Combination<T>(T[] arr_item, int n_size_item, int n_size_temp, ref List<List<T>> lst_combination)
        {
            T[] arr_temp = new T[n_size_temp];    // A temporary array to store all combination one by one

            Get_Combination(arr_item, arr_temp, 0, n_size_item - 1, 0, n_size_temp, ref lst_combination);   // Get all combination using temporary array 'arr_temp[]'
        }

        /*
         * arr_item[] ---> Original array
         * arr_temp[] ---> Temporary array to store current
         * i_start & i_end ---> Staring and Ending indexes in arr_item[]
         * i_temp ---> Current index in arr_temp[]
         * n_size_temp ---> Size of a combination to get
         */

        private static void Get_Combination<T>(T[] arr_item, T[] arr_temp, int i_start, int i_end, int i_temp, int n_size_temp, ref List<List<T>> lst_combination)
        {
            // Current combination

            if (i_temp == n_size_temp)
            {
                lst_combination.Add(Array_To_List(arr_temp));

                return;
            }

            // Replace i_temp with all possible elements.
            // The condition "i_end - i + 1 >= n_size_temp - i_temp" makes sure that including one element at i_temp will make a combination with remaining elements at remaining positions

            for (int i = i_start; i <= i_end && i_end - i + 1 >= n_size_temp - i_temp; i++)
            {
                arr_temp[i_temp] = arr_item[i];

                Get_Combination(arr_item, arr_temp, i + 1, i_end, i_temp + 1, n_size_temp, ref lst_combination);
            }
        }

        private static List<T> Array_To_List<T>(T[] arr)
        {
            List<T> lst = new List<T>();

            foreach (T x_item in arr)
            {
                lst.Add(x_item);
            }

            return lst;
        }
    }
}
