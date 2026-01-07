using Income.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.SurveyLibrary
{
    public class HouseholdSelectionService
    {
        DBQueries dQ;
        public HouseholdSelectionService()
        {
            dQ = new DBQueries();
        }
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
                            //result.Messages[stratum] = $"Stratum {stratum}: {hh.Block_7_3} was reserved in SSS {hh.SSS}";
                            hh.isSelected = true;
                            hh.SelectedPostedSSS = firstSSS;
                            hh.SelectedFromSSS = hh.SSS;
                            result.SelectedHouseholds.Add(hh);
                        }
                    }

                    // Compensate shortfall
                    int shortfall = totalRequired - totalAvailable;
                    var compensationOrder = isRural ? RuralStratumCompensation[stratum] : UrbanStratumCompensation[stratum];

                    CompensateShortfall(pool, compensationOrder, shortfall, firstSSS, result, allocation);
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

                            CompensateShortfall(pool, compensationOrder, shortfall, sss, result, allocation);
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
        SelectionResult result,
        Dictionary<int, Dictionary<int, int>> allocation)
        {
            int compensated = 0;
            int targetStratum = (postedSSS / 10) * 10; // Get the stratum we're compensating for

            foreach (int compensationSSS in compensationOrder)
            {
                if (compensated >= shortfall) break;

                // Find available households from this SSS or Stratum
                var availableHouseholds = pool.GetAvailable(sss: compensationSSS);
                bool isSSS = availableHouseholds.Count > 0;

                // If no households found for SSS, try as stratum
                if (!isSSS)
                {
                    availableHouseholds = pool.GetAvailable(stratum: compensationSSS);
                }

                if (availableHouseholds.Count == 0)
                    continue;

                // Calculate how many households can actually be spared
                int canSpare = CalculateSpareableHouseholds(
                    pool,
                    compensationSSS,
                    isSSS,
                    allocation,
                    availableHouseholds.Count);

                if (canSpare <= 0)
                    continue; // Can't spare any, move to next in compensation order

                int needed = shortfall - compensated;
                int toSelect = Math.Min(needed, canSpare);

                var selected = SelectRandomly(availableHouseholds, toSelect);

                foreach (var hh in selected)
                {
                    if (pool.Reserve(hh))
                    {
                        hh.isSelected = true;

                        // Determine SelectedPostedSSS based on stratum
                        int hhStratum = (hh.SSS / 10) * 10; // Get household's stratum

                        if (hhStratum == targetStratum)
                        {
                            // Same stratum - assign to first SSS (posted SSS)
                            hh.SelectedPostedSSS = postedSSS;
                        }
                        else
                        {
                            // Different stratum - keep the household's original SSS
                            hh.SelectedPostedSSS = hh.SSS;
                        }

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

        // Calculate how many households can be spared from a given SSS or Stratum
        private int CalculateSpareableHouseholds(
            SelectionPool pool,
            int sssOrStratum,
            bool isSSS,
            Dictionary<int, Dictionary<int, int>> allocation,
            int available)
        {
            if (isSSS)
            {
                // For SSS: Check if this SSS has its own requirement
                int stratum = (sssOrStratum / 10) * 10; // Get parent stratum

                if (!allocation.ContainsKey(stratum))
                    return available; // No requirement for this stratum, can spare all

                var sssAllocations = allocation[stratum];

                if (!sssAllocations.ContainsKey(sssOrStratum))
                    return available; // This SSS has no specific requirement, can spare all

                int required = sssAllocations[sssOrStratum];
                int spareable = available - required;

                return Math.Max(0, spareable); // Can't be negative
            }
            else
            {
                // For Stratum: Check total stratum requirement
                if (!allocation.ContainsKey(sssOrStratum))
                    return available; // No requirement for this stratum, can spare all

                var sssAllocations = allocation[sssOrStratum];
                int totalRequired = sssAllocations.Values.Sum();

                // Get total available in this stratum (not just the available passed in)
                var allInStratum = pool.GetAvailable(stratum: sssOrStratum);
                int totalAvailable = allInStratum.Count;

                int spareable = totalAvailable - totalRequired;

                return Math.Max(0, spareable); // Can't be negative
            }
        }

        private List<Tbl_Sch_0_0_Block_7> SelectRandomly(List<Tbl_Sch_0_0_Block_7> households, int count)
        {
            if (count >= households.Count)
                return households;

            var random = new Random();
            return households.OrderBy(x => random.Next()).Take(count).ToList();
        }

        // Method to find substitute for a selected household
        // Returns: 
        //   - The substitute household if found
        //   - null if no substitute available (method returns 0 in your context)
        public Tbl_Sch_0_0_Block_7? FindSubstitute(
            List<Tbl_Sch_0_0_Block_7> allHouseholds,
            Tbl_Sch_0_0_Block_7 originalHousehold)
        {
            // Validation
            if (originalHousehold == null || !originalHousehold.isSelected)
            {
                Console.WriteLine("❌ Cannot substitute: Household is not selected");
                return null;
            }

            // Get the SSS from which the household was originally selected
            // This is important because we need to find substitute from the ORIGINAL SSS, not the posted SSS
            int sourceSSS = originalHousehold.SSS;

            Console.WriteLine($"Finding substitute for HH {originalHousehold.Block_7_3} from SSS {sourceSSS}...");

            // Find available households from the same SSS that:
            // 1. Are from the same SSS
            // 2. Have not been selected
            // 3. Are not casualties
            // 4. Are valid households
            // 5. Have not been used as substitutes before
            var availableSubstitutes = allHouseholds
                .Where(h => h.SSS == sourceSSS
                    && !h.isSelected
                    && !h.isCasualty
                    && h.is_household == 2
                    && !h.isSubstitute
                    && (h.hhdStatus == 0 || h.hhdStatus == 11)
                    && h.Block_7_3 != originalHousehold.Block_7_3)
                .ToList();

            if (availableSubstitutes.Count == 0)
            {
                Console.WriteLine($"⚠️ No substitute available in SSS {sourceSSS}");
                return null; // Return null, which you can interpret as 0
            }

            // Random selection from available substitutes
            var random = new Random();
            var substitute = availableSubstitutes[random.Next(availableSubstitutes.Count)];

            // Update original household status
            //originalHousehold.hhdStatus = GetStatusCode("SUBSTITUTED"); // You may need to define status codes
            originalHousehold.status = "SUBSTITUTED";
            originalHousehold.SubstitutionCount = originalHousehold.SubstitutionCount + 1;
            // Note: Keep isSelected = true for the original household for tracking

            // Update substitute household
            substitute.isSelected = true;
            substitute.isSubstitute = true;
            //substitute.hhdStatus = GetStatusCode("SUBSTITUTE");
            substitute.status = "SUBSTITUTE";
            substitute.SubstitutedForID = originalHousehold.Block_7_3;
            substitute.OriginalHouseholdID = originalHousehold.OriginalHouseholdID ?? 0;

            // Inherit the posted SSS from the original household
            substitute.SelectedPostedSSS = originalHousehold.SelectedPostedSSS;
            substitute.SelectedFromSSS = sourceSSS;

            // Assign SSS_household_id
            substitute.SSS_household_id = GetNextSSSHouseholdId(allHouseholds, sourceSSS);

            Console.WriteLine($"✅ Substitute found: HH {substitute.Block_7_3} (SSS_household_id: {substitute.SSS_household_id})");

            return substitute;
        }

        // Helper method to get the next SSS_household_id for a given SSS
        // This is a running serial number within each SSS
        private int GetNextSSSHouseholdId(List<Tbl_Sch_0_0_Block_7> allHouseholds, int sss)
        {
            // Find the maximum SSS_household_id currently assigned in this SSS
            var maxId = allHouseholds
                .Where(h => h.SSS == sss && h.SSS_household_id.HasValue)
                .Select(h => h.SSS_household_id.Value)
                .DefaultIfEmpty(0)
                .Max();

            return maxId + 1;
        }

        public async Task<int> ReassignSSSHouseholdIds(int sss)
        {
            Console.WriteLine($"Re-assigning SSS_household_id for SSS {sss}...");

            var allHouseholds = await dQ.Get_SCH0_0_Block_5A_HouseHoldBy_FSUP(SessionStorage.SelectedFSUId);
            // Get all selected households from this SSS, ordered by Block_7_3
            var householdsInSSS = allHouseholds
                .Where(h => h.SSS == sss && h.isSelected && h.is_household == 2 && h.status != "SUBSTITUTED" && h.status != "CASUALTY")
                .OrderBy(h => h.Block_7_3)
                .ToList();

            if (householdsInSSS.Count == 0)
            {
                Console.WriteLine($"⚠️ No selected households found in SSS {sss}");
                return 0;
            }

            // Re-assign SSS_household_id sequentially
            int sequentialId = 1;
            foreach (var household in householdsInSSS)
            {
                household.SSS_household_id = sequentialId;
                Console.WriteLine($"  HH {household.Block_7_3}: SSS_household_id = {sequentialId}");
                sequentialId++;
                await dQ.Update_SCH0_0_Block_7(household);
            }

            Console.WriteLine($"✅ Re-assigned {householdsInSSS.Count} households in SSS {sss}");
            return householdsInSSS.Count;
        }

        private int GetStatusCode(string status)
        {
            return status switch
            {
                "SELECTED" => 1,
                "SUBSTITUTED" => 9,
                "SUBSTITUTE" => 2,
                "CASUALTY" => 3,
                _ => 0
            };
        }
    }
}
