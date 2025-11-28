using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.SurveyLibrary
{
    public class HouseholdSelectionService
    {
        // Allocation rules from the document
        private static readonly Dictionary<int, Dictionary<int, int>> RuralAllocation = new()
    {
        { 10, new Dictionary<int, int> { { 11, 3 }, { 12, 3 } } },
        { 20, new Dictionary<int, int> { { 21, 2 }, { 22, 2 } } },
        { 30, new Dictionary<int, int> { { 31, 2 }, { 32, 2 } } },
        { 40, new Dictionary<int, int> { { 40, 4 } } },
        { 90, new Dictionary<int, int> { { 90, 2 } } }
    };

        private static readonly Dictionary<int, Dictionary<int, int>> UrbanAllocation = new()
    {
        { 10, new Dictionary<int, int> { { 11, 2 }, { 121, 2 }, { 122, 2 } } },
        { 20, new Dictionary<int, int> { { 21, 4 }, { 221, 2 }, { 222, 3 } } },
        { 40, new Dictionary<int, int> { { 40, 3 } } },
        { 90, new Dictionary<int, int> { { 90, 2 } } }
    };

        // Compensation priority orders - Rural (Scenario 1: Stratum shortfall)
        private static readonly Dictionary<int, List<int>> RuralStratumCompensation = new()
    {
        { 10, new List<int> { 22, 21, 20, 32, 31, 30, 40, 90 } },
        { 20, new List<int> { 12, 11, 10, 32, 31, 30, 40, 90 } },
        { 30, new List<int> { 22, 21, 20, 12, 11, 10, 40, 90 } },
        { 40, new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 90 } },
        { 90, new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 40 } }
    };

        // Compensation priority orders - Rural (Scenario 2: Sub-stratum shortfall)
        private static readonly Dictionary<int, List<int>> RuralSSSCompensation = new()
    {
        { 11, new List<int> { 12, 21, 22, 20, 31, 32, 30, 40, 90 } },
        { 12, new List<int> { 11, 22, 21, 20, 32, 31, 30, 40, 90 } },
        { 21, new List<int> { 22, 11, 12, 10, 31, 32, 30, 40, 90 } },
        { 22, new List<int> { 21, 12, 11, 10, 32, 31, 30, 40, 90 } },
        { 31, new List<int> { 32, 21, 22, 20, 11, 12, 10, 40, 90 } },
        { 32, new List<int> { 31, 22, 21, 20, 12, 11, 10, 40, 90 } },
        { 40, new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 90 } },
        { 90, new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 40 } }
    };

        // Compensation priority orders - Urban (Scenario 1: Stratum shortfall)
        private static readonly Dictionary<int, List<int>> UrbanStratumCompensation = new()
    {
        { 10, new List<int> { 222, 221, 21, 20, 40, 90 } },
        { 20, new List<int> { 122, 121, 11, 10, 40, 90 } },
        { 40, new List<int> { 11, 121, 122, 10, 21, 221, 222, 20, 90 } },
        { 90, new List<int> { 21, 221, 222, 20, 11, 121, 122, 10, 40 } }
    };

        // Compensation priority orders - Urban (Scenario 2: Sub-stratum shortfall)
        private static readonly Dictionary<int, List<int>> UrbanSSSCompensation = new()
    {
        { 11, new List<int> { 121, 122, 21, 221, 222, 20, 40, 90 } },
        { 121, new List<int> { 122, 11, 221, 222, 21, 20, 40, 90 } },
        { 122, new List<int> { 121, 11, 222, 221, 21, 20, 40, 90 } },
        { 21, new List<int> { 221, 222, 11, 121, 122, 10, 40, 90 } },
        { 221, new List<int> { 222, 21, 121, 122, 11, 10, 40, 90 } },
        { 222, new List<int> { 221, 21, 122, 121, 11, 10, 40, 90 } },
        { 40, new List<int> { 11, 121, 122, 10, 21, 221, 222, 20, 90 } },
        { 90, new List<int> { 21, 221, 222, 20, 11, 121, 122, 10, 40 } }
    };

        public class SelectionResult
        {
            public List<Tbl_Sch_0_0_Block_7> SelectedHouseholds { get; set; } = new();
            public Dictionary<int, string> Messages { get; set; } = new();
            public bool Success { get; set; }
        }

        // Track which households have been reserved for selection
        private class SelectionPool
        {
            private HashSet<Guid> reservedIds = new HashSet<Guid>();
            private List<Tbl_Sch_0_0_Block_7> allHouseholds;

            public SelectionPool(List<Tbl_Sch_0_0_Block_7> households)
            {
                allHouseholds = households;
            }

            public List<Tbl_Sch_0_0_Block_7> GetAvailable(int? stratum = null, int? sss = null)
            {
                return allHouseholds.Where(h =>
                    !reservedIds.Contains(h.id) &&
                    (stratum == null || h.Stratum == stratum) &&
                    (sss == null || h.SSS == sss)
                ).ToList();
            }

            public bool Reserve(Tbl_Sch_0_0_Block_7 household)
            {
                if (reservedIds.Contains(household.id))
                    return false;

                reservedIds.Add(household.id);
                return true;
            }

            public bool IsReserved(Tbl_Sch_0_0_Block_7 household)
            {
                return reservedIds.Contains(household.id);
            }
        }

        public SelectionResult SelectHouseholds(List<Tbl_Sch_0_0_Block_7> households, int sectorType)
        {
            var result = new SelectionResult { Success = true };

            // Validate sector type
            if (sectorType != 1 && sectorType != 2)
            {
                result.Success = false;
                result.Messages[0] = "Invalid sector type. Use 1 for Rural, 2 for Urban.";
                return result;
            }

            bool isRural = sectorType == 1;
            var allocation = isRural ? RuralAllocation : UrbanAllocation;

            // Filter only households that are eligible
            var eligibleHouseholds = households
                .Where(h => h.is_household == 2 && !h.isSelected && !h.isCasualty)
                .ToList();

            // Create a selection pool to track reservations
            var pool = new SelectionPool(eligibleHouseholds);

            // Process each stratum in order
            var stratumOrder = allocation.Keys.OrderBy(k => k).ToList();

            foreach (int stratum in stratumOrder)
            {
                var sssAllocations = allocation[stratum];
                int totalRequired = sssAllocations.Values.Sum();

                var availableInStratum = pool.GetAvailable(stratum: stratum);
                int totalAvailable = availableInStratum.Count;

                // Scenario 1: Shortfall in total stratum households
                if (totalAvailable < totalRequired)
                {
                    result.Messages[stratum] = $"Stratum {stratum}: Total shortfall. Available: {totalAvailable}, Required: {totalRequired}. No sub-strata formed.";

                    // Get the first SSS for this stratum
                    int firstSSS = sssAllocations.Keys.Min();

                    // Select all available households from this stratum
                    foreach (var hh in availableInStratum)
                    {
                        if (pool.Reserve(hh))
                        {
                            hh.isSelected = true;
                            hh.SelectedPostedSSS = firstSSS;
                            hh.SelectedFromSSS = hh.SSS;
                            result.SelectedHouseholds.Add(hh);
                        }
                    }

                    // Compensate shortfall
                    int shortfall = totalRequired - totalAvailable;
                    var compensationOrder = isRural ? RuralStratumCompensation[stratum] : UrbanStratumCompensation[stratum];

                    CompensateShortfall(pool, compensationOrder, shortfall, firstSSS, result);
                }
                else
                {
                    // Scenario 2: Sufficient households in stratum, check each SSS
                    foreach (var sssKvp in sssAllocations)
                    {
                        int sss = sssKvp.Key;
                        int required = sssKvp.Value;

                        var availableInSSS = pool.GetAvailable(sss: sss);
                        int available = availableInSSS.Count;

                        if (available >= required)
                        {
                            // Sufficient households - select randomly
                            var selected = SelectRandomly(availableInSSS, required);
                            foreach (var hh in selected)
                            {
                                if (pool.Reserve(hh))
                                {
                                    hh.isSelected = true;
                                    hh.SelectedPostedSSS = sss;
                                    hh.SelectedFromSSS = sss;
                                    result.SelectedHouseholds.Add(hh);
                                }
                            }
                        }
                        else
                        {
                            // Sub-stratum shortfall
                            result.Messages[sss] = $"SSS {sss}: Shortfall. Available: {available}, Required: {required}";

                            // Select all available
                            foreach (var hh in availableInSSS)
                            {
                                if (pool.Reserve(hh))
                                {
                                    hh.isSelected = true;
                                    hh.SelectedPostedSSS = sss;
                                    hh.SelectedFromSSS = sss;
                                    result.SelectedHouseholds.Add(hh);
                                }
                            }

                            // Compensate shortfall
                            int shortfall = required - available;
                            var compensationOrder = isRural ? RuralSSSCompensation[sss] : UrbanSSSCompensation[sss];

                            CompensateShortfall(pool, compensationOrder, shortfall, sss, result);
                        }
                    }
                }
            }

            return result;
        }

        private void CompensateShortfall(
            SelectionPool pool,
            List<int> compensationOrder,
            int shortfall,
            int postedSSS,
            SelectionResult result)
        {
            int compensated = 0;

            foreach (int compensationSSS in compensationOrder)
            {
                if (compensated >= shortfall) break;

                // Find available households from this SSS or Stratum
                var availableHouseholds = pool.GetAvailable(sss: compensationSSS);

                // If no households found for SSS, try as stratum
                if (availableHouseholds.Count == 0)
                {
                    availableHouseholds = pool.GetAvailable(stratum: compensationSSS);
                }

                int needed = shortfall - compensated;
                int toSelect = Math.Min(needed, availableHouseholds.Count);

                var selected = SelectRandomly(availableHouseholds, toSelect);

                foreach (var hh in selected)
                {
                    if (pool.Reserve(hh))
                    {
                        hh.isSelected = true;
                        hh.SelectedPostedSSS = postedSSS;
                        hh.SelectedFromSSS = hh.SSS;
                        result.SelectedHouseholds.Add(hh);
                        compensated++;
                    }
                }
            }

            if (compensated < shortfall)
            {
                result.Messages[postedSSS * 1000] = $"WARNING: Could not fully compensate shortfall for SSS {postedSSS}. Shortfall: {shortfall}, Compensated: {compensated}";
            }
        }

        private List<Tbl_Sch_0_0_Block_7> SelectRandomly(List<Tbl_Sch_0_0_Block_7> households, int count)
        {
            if (count >= households.Count)
                return households;

            var random = new Random();
            return households.OrderBy(x => random.Next()).Take(count).ToList();
        }

        // Method to find substitutes when a household becomes a casualty
        public Tbl_Sch_0_0_Block_7 FindSubstitute(
            List<Tbl_Sch_0_0_Block_7> allHouseholds,
            Tbl_Sch_0_0_Block_7 casualtyHousehold)
        {
            // Find unselected households from the same SSS
            var availableSubstitutes = allHouseholds
                .Where(h => h.SSS == casualtyHousehold.SSS
                    && !h.isSelected
                    && !h.isCasualty
                    && h.is_household == 2)
                .ToList();

            if (availableSubstitutes.Count == 0)
                return null;

            // Random selection
            var random = new Random();
            var substitute = availableSubstitutes[random.Next(availableSubstitutes.Count)];

            substitute.isSelected = true;
            substitute.isSubstitute = true;
            substitute.SubstitutedForID = casualtyHousehold.Block_7_3;
            substitute.OriginalHouseholdID = casualtyHousehold.OriginalHouseholdID ?? casualtyHousehold.Block_7_3;
            substitute.SubstitutionCount = casualtyHousehold.SubstitutionCount + 1;
            substitute.SelectedPostedSSS = casualtyHousehold.SelectedPostedSSS;
            substitute.SelectedFromSSS = substitute.SSS;

            return substitute;
        }
    }

    // Usage example:
    /*
    var service = new HouseholdSelectionService();
    var households = GetHouseholdsFromDatabase(); // Your list of Tbl_Sch_0_0_Block_7
    int sectorType = 1; // 1 = Rural, 2 = Urban

    var result = service.SelectHouseholds(households, sectorType);

    if (result.Success)
    {
        Console.WriteLine($"Total households selected: {result.SelectedHouseholds.Count}");

        foreach (var msg in result.Messages)
        {
            Console.WriteLine(msg.Value);
        }

        // Process selected households
        foreach (var hh in result.SelectedHouseholds)
        {
            Console.WriteLine($"HH {hh.Block_7_3}: SSS {hh.SSS} -> Posted as SSS {hh.SelectedPostedSSS}");
        }
    }

    // Handle casualty and find substitute
    var casualty = result.SelectedHouseholds.First();
    casualty.isCasualty = true;
    var substitute = service.FindSubstitute(households, casualty);
    if (substitute != null)
    {
        Console.WriteLine($"Substitute found: HH {substitute.Block_7_3} for HH {casualty.Block_7_3}");
    }
    */
}
