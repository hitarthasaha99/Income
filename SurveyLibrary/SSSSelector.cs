using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.SurveyLibrary
{
    public class SSSRandomSelector
    {

        /// <summary>
        /// This return the selected list of sss items
        /// </summary>
        /// <param name="ssslist"> all sss item list </param>
        /// <param name="sssRequirements"> Define the required number of entries for each SSS </param>
        /// <param name="shortfallFallbacks"> Define the shortfall fallback order for each SSS </param>
        /// <returns> selected list </returns>
        /// <exception cref="Exception"></exception>
        public List<SSSItem> DoSSSSelection(
            List<SSSItem> ssslist,
            Dictionary<int, int> sssRequirements,
            Dictionary<int, List<int>> shortfallFallbacks)
        {

            if (ssslist?.Count <= 0)
            {
                throw new Exception("SSS list should be not null.");
            }

            // Group entries by SSS
            var groupedEntries = ssslist.GroupBy(x => x.SSSValue).ToDictionary(g => g.Key, g => g.ToList());

            // Selected result collection
            var selectedEntries = new List<SSSItem>();

            // Select random entries for each SSS
            var shortfallEntries = new Dictionary<int, int>();

            foreach (var sssReq in sssRequirements)
            {
                int sssKey = sssReq.Key;
                int requiredCount = sssReq.Value;

                if (groupedEntries.ContainsKey(sssKey))
                {
                    var selectedSSS = GetRandomItems(groupedEntries[sssKey], Math.Min(groupedEntries[sssKey].Count, requiredCount));
                    selectedEntries.AddRange(selectedSSS);

                    int shortfall = requiredCount - selectedSSS.Count;
                    if (shortfall > 0)
                    {
                        shortfallEntries[sssKey] = shortfall;
                    }
                }
                else
                {
                    shortfallEntries[sssKey] = requiredCount; // If the SSS group is missing altogether
                }
            }

            // Handle shortfalls based on fallback order
            foreach (var shortfall in shortfallEntries)
            {
                int sssKey = shortfall.Key;
                int shortfallCount = shortfall.Value;

                // Get fallback order for current SSS
                if (shortfallFallbacks.ContainsKey(sssKey))
                {
                    var fallbackSSSList = shortfallFallbacks[sssKey];

                    foreach (var fallbackSSS in fallbackSSSList)
                    {
                        if (shortfallCount <= 0) break;

                        if (groupedEntries.ContainsKey(fallbackSSS))
                        {
                            var availableEntries = groupedEntries[fallbackSSS].Except(selectedEntries).ToList();
                            var additionalEntries = GetRandomItems(availableEntries, Math.Min(availableEntries.Count, shortfallCount));
                            selectedEntries.AddRange(additionalEntries);
                            shortfallCount -= additionalEntries.Count;
                        }
                    }
                }
            }


            return selectedEntries;
        }

        private List<SSSItem> GetRandomItems(List<SSSItem> source, int count)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            List<SSSItem> selectedList = new List<SSSItem>();

            if (count < source.Count)
            {
                do
                {
                    int index = random.Next(source.Count);
                    SSSItem selected = source[index];

                    if (!selectedList.Any(x => x.SerialNumber == selected.SerialNumber))
                    {
                        selected.IsSelected = true;
                        selectedList.Add(selected);
                    }
                } while (selectedList.Count < count);
            }
            else
            {
                foreach (var entry in source)
                {
                    entry.IsSelected = true;  // Set isSelected flag to true
                }
                selectedList = source;
            }

            return selectedList;
        }

        /// <summary> 
        /// Get a list of items with SItem value, using a selection algorithm it returns a selected list 
        /// It contains items which are already selected in other schedule, It will only consider them when
        /// there is not enough to select for new schedule
        /// The selection login is based on random selection
        /// </summary> 
        /// <param name="sList">list of items with existing schedule selected</param> 
        /// <returns>is selected</returns> 
        public bool DoExistingSelectedListSelection(List<SItem> sList, int selectionCount)
        {
            bool result = false;

            if (sList != null)
            {
                //if the selection count is greater than actual SSS list then return the original list
                //as we will select all
                if (sList.Count() <= selectionCount)
                {
                    foreach (var sssItem in sList)
                    {
                        sssItem.IsSelected = true;
                    }
                    return true;
                }


                var mainSchdeduleList = sList.Where(s => s.OtherScheduleSelected != true);
                int mainCount = mainSchdeduleList?.Count() ?? 0;
                if (mainCount >= selectionCount)
                {
                    //Try first to select only the ones which were not selected in other schedule
                    DoSpecificSSelection(sList, selectionCount);
                }
                else
                {
                    //Try first to select only the ones which were not selected in other schedule
                    DoSpecificSSelection(sList, mainCount);

                    int remainingCount = selectionCount - mainCount;

                    //Next select from other schedule to make up the number
                    DoSameSSelection(sList, remainingCount);
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        ///  Select from only new schedule
        ///  Use random selection logic
        /// </summary>
        /// <param name="sCount">Total number of item we need to select</param>
        /// <param name="sList">List of Selection item with other schedule selected value</param>
        private async void DoSpecificSSelection(List<SItem> sList, int sCount)
        {
            var replacedCount = 0;
            //if there is more than the selection count, select randomly 
            if (sCount < sList.Count())
            {
                int selectedCount = 0;
                //create a list of random items order list, used to select
                List<int> randomIdsList = new List<int>();
                int seed = DateTime.Now.Microsecond;
                Random random = new Random(seed);
                int failCount = 0;
                do
                {
                    if (sList.Count() == 0)
                    { break; }

                    int randomNumber;

                    randomNumber = random.Next(0, sList.Count());


                    //int
                    //randomNumber = GetRandomNumber(sList.Count());

                    if (!randomIdsList.Contains(randomNumber))
                    {
                        randomIdsList.Add(randomNumber);
                        selectedCount++;
                    }
                    else
                    {
                        failCount++;
                    }

                    if (failCount > 100000)
                    {//case when after many tries also cannot make the list
                        break;
                    }

                } while (selectedCount < sList.Count());

                selectedCount = 0;
                foreach (int id in randomIdsList)
                {
                    //check if already selected 
                    var sItem = sList.ToList()[id];

                    if (sItem.OtherScheduleSelected)
                    {
                        replacedCount++;
                    }

                    if (!sItem.IsSelected && !sItem.OtherScheduleSelected)
                    {
                        sItem.IsSelected = true;
                        selectedCount++;
                        if (selectedCount == sCount)
                            break;
                    }
                }
            }
            else
            {
                //adding all as selected
                foreach (var sItem in sList)
                {
                    sItem.IsSelected = true;
                }
            }
            //await App.Database.UpdateReplacedCountAsync(SessionDetail.fsu_id, replacedCount);
        }

        /// <summary>
        ///  Select from other schedule
        ///  Use random selection logic
        /// </summary>
        /// <param name="sCount">Total number of item we need to select</param>
        /// <param name="sList">List of Selection item with other schedule selected value</param>
        private void DoSameSSelection(List<SItem> sList, int sCount)
        {
            var notSelected = sList.Where(s => s.IsSelected == false);
            int notSelectedCount = notSelected?.Count() ?? 0;
            //if there is more than the selection count, select randomly 

            if (sCount < notSelectedCount)
            {
                int selectedCount = 0;
                //create a list of random items order list, used to select
                List<int> randomIdsList = new List<int>();
                int seed = DateTime.Now.Microsecond;
                Random random = new Random(seed);
                int failCount = 0;
                do
                {
                    if (sList.Count() == 0)
                    { break; }

                    int randomNumber;

                    randomNumber = random.Next(0, sList.Count());
                    //randomNumber = GetRandomNumber(sList.Count());

                    if (!randomIdsList.Contains(randomNumber))
                    {
                        randomIdsList.Add(randomNumber);
                        selectedCount++;
                    }
                    else
                    {
                        failCount++;
                    }

                    if (failCount > 100000)
                    {//case when after many tries also cannot make the list
                        break;
                    }

                } while (selectedCount < sList.Count());

                selectedCount = 0;
                foreach (int id in randomIdsList)
                {
                    //check if already selected 
                    var sItem = sList.ToList()[id];

                    if (!sItem.IsSelected)
                    {
                        sItem.IsSelected = true;
                        selectedCount++;
                        if (selectedCount == sCount)
                            break;
                    }
                }
            }
            else
            {
                if (notSelected != null)
                {
                    //adding all as selected
                    foreach (var sItem in notSelected)
                    {
                        sItem.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Generate a random number to do selection
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private static int GetRandomNumber(int maxValue)
        {
            if (maxValue == 0)
            { return 0; }

            int randomNumber;

            Random random = new Random(DateTime.Now.Microsecond);

            randomNumber = random.Next(0, maxValue);

            return randomNumber;

        }
    }
}
