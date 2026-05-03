using Delwings.Models.Basic;
using System.Diagnostics;


namespace Delwings.Services.Dashboards
{
    public class DeveloperPageService
    {
        private List<Order> BubbleSort(List<Order> list)
        {
            for (int i=0; i<list.Count; i++)
            {
                bool swapped = false;

                for (int j=0; j<list.Count - i - 1; j++)
                {
                    if (list[j + 1].CurrentLocationId > list[j].CurrentLocationId)
                    {
                        Order temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        swapped = true;
                    }
                }

                if (!swapped) break;
            }

            return list;
        }

        public async Task<double> GetBubbleSortTime(List<Order> orders, int numberOfSorts = 1)
        {
            Stopwatch sw = Stopwatch.StartNew();

            for (int i=0; i < numberOfSorts;i++)
            {
                List<Order> copy = new List<Order>(orders);
                BubbleSort(copy);
            }

            sw.Stop();

            return sw.Elapsed.TotalMilliseconds / numberOfSorts;
        }

        public async Task<double> GetLINQSortTime(List<Order> orders, int numberOfSorts = 1)
        {
            Stopwatch sw = Stopwatch.StartNew();

            for (int i=0; i < numberOfSorts;i++)
            {
                List<Order> copy = new List<Order>(orders);
                copy.OrderBy(ord => ord.CurrentLocationId).ToList();
            }

            sw.Stop();

            return sw.Elapsed.TotalMilliseconds / numberOfSorts;
        }
    }
}
