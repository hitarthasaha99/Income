using Income.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            public int GetReservedCount(int? stratum = null, int? sss = null)
            {
                return allHouseholds.Where(h =>
                    reservedIds.Contains(h.id) &&
                    (stratum == null || h.Stratum == stratum) &&
                    (sss == null || h.SSS == sss)
                ).Count();
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

                    CompensateShortfall(pool, compensationOrder, shortfall, firstSSS, result, allocation, isRural, stratum, null);

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

                            // Get all households in the stratum for special case check
                            var allInStratum = pool.GetAvailable(stratum: stratum);

                            // For SSS-level compensation, use the unified method
                            CompensateShortfall(pool, compensationOrder, shortfall, sss, result, allocation, isRural, stratum, allInStratum);
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
        Dictionary<int, Dictionary<int, int>> allocation,
        bool isRural = true,
        int stratum = 0,
        List<Tbl_Sch_0_0_Block_7> allHouseholdsInStratum = null)
        {
            int compensated = 0;
            int targetStratum = GetStratumFromSSS(postedSSS); // Get the stratum we're compensating for

            // Check if urban special case applies for this stratum
            bool isUrbanSpecialCase = false;
            if (!isRural && allHouseholdsInStratum != null && (targetStratum == 10 || targetStratum == 20))
            {
                var allSSSInStratum = allHouseholdsInStratum.Select(h => h.SSS).Distinct().ToList();

                if (targetStratum == 10)
                {
                    // Check if ALL households are 121 or 122 (no SSS 11)
                    isUrbanSpecialCase = allSSSInStratum.All(s => s == 121 || s == 122);
                }
                else if (targetStratum == 20)
                {
                    // Check if ALL households are 221 or 222 (no SSS 21)
                    isUrbanSpecialCase = allSSSInStratum.All(s => s == 221 || s == 222);
                }
            }

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
                    availableHouseholds.Count,
                    targetStratum);

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
                        int hhStratum = GetStratumFromSSS(hh.SSS); // Get household's stratum

                        if (hhStratum == targetStratum)
                        {
                            // Same stratum compensation
                            // Check if this household qualifies for urban special case
                            bool applySpecialCase = false;

                            if (isUrbanSpecialCase)
                            {
                                // Special case applies to entire stratum
                                // Keep household's original SSS if it's 121/122 or 221/222
                                if (targetStratum == 10 && (hh.SSS == 121 || hh.SSS == 122))
                                {
                                    applySpecialCase = true;
                                }
                                else if (targetStratum == 20 && (hh.SSS == 221 || hh.SSS == 222))
                                {
                                    applySpecialCase = true;
                                }
                            }

                            if (applySpecialCase)
                            {
                                // Urban special case: Keep household's original SSS
                                hh.SelectedPostedSSS = hh.SSS;
                            }
                            else
                            {
                                // Normal same stratum: assign to posted SSS
                                hh.SelectedPostedSSS = postedSSS;
                            }
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

        private int GetStratumFromSSS(int sss)
        {
            if (sss >= 100)
            {
                // 3-digit SSS (Urban): 121, 122, 221, 222
                return (sss / 100) * 10; // 121 -> 10, 221 -> 20
            }
            else
            {
                // 2-digit SSS (Rural and some Urban): 11, 12, 21, 22, 31, 32, 40, 90
                return (sss / 10) * 10; // 11 -> 10, 21 -> 20, 40 -> 40
            }
        }

        // Calculate how many households can be spared from a given SSS or Stratum
        private int CalculateSpareableHouseholds(
            SelectionPool pool,
            int sssOrStratum,
            bool isSSS,
            Dictionary<int, Dictionary<int, int>> allocation,
            int available,
            int currentProcessingStratum = 0)
        {
            Debug.WriteLine($"[DEBUG] CalculateSpareableHouseholds: sssOrStratum={sssOrStratum}, isSSS={isSSS}, available={available}, currentProcessingStratum={currentProcessingStratum}");

            if (isSSS)
            {
                // For SSS: Check if this SSS has its own requirement
                int stratum = GetStratumFromSSS(sssOrStratum);
                Debug.WriteLine($"[DEBUG]   SSS {sssOrStratum} belongs to Stratum {stratum}");

                if (!allocation.ContainsKey(stratum))
                {
                    Debug.WriteLine($"[DEBUG]   Stratum {stratum} not in allocation, can spare all {available}");
                    return available;
                }

                var sssAllocations = allocation[stratum];

                if (!sssAllocations.ContainsKey(sssOrStratum))
                {
                    Debug.WriteLine($"[DEBUG]   SSS {sssOrStratum} has no specific requirement, can spare all {available}");
                    return available;
                }

                // CRITICAL: First check if the parent STRATUM has shortfall
                // If stratum is in shortfall, SSS cannot spare any households
                int stratumTotalRequired = sssAllocations.Values.Sum();
                var allInStratum = pool.GetAvailable(stratum: stratum);
                int stratumAlreadyReserved = pool.GetReservedCount(stratum: stratum);
                int stratumTotalOriginal = allInStratum.Count + stratumAlreadyReserved;

                Debug.WriteLine($"[DEBUG]   Stratum {stratum} check: totalOriginal={stratumTotalOriginal}, totalRequired={stratumTotalRequired}");

                if (stratumTotalOriginal < stratumTotalRequired)
                {
                    Debug.WriteLine($"[DEBUG]   Stratum {stratum} is in SHORTFALL, cannot spare any");
                    return 0;
                }

                // NEW CHECK: If we're being asked to compensate for a DIFFERENT stratum,
                // we need to check if our OWN stratum has been fully processed yet
                // An SSS can only spare to other strata AFTER its own stratum is processed
                if (currentProcessingStratum > 0 && stratum != currentProcessingStratum && stratum > currentProcessingStratum)
                {
                    Debug.WriteLine($"[DEBUG]   SSS {sssOrStratum} (Stratum {stratum}) not yet processed, current={currentProcessingStratum}, cannot spare");
                    return 0;
                }

                // Stratum is NOT in shortfall, now check if this SSS can spare
                int required = sssAllocations[sssOrStratum];

                // Count how many from this SSS have already been reserved
                int alreadyReserved = pool.GetReservedCount(sss: sssOrStratum);

                // Total originally available = current available + already reserved
                int totalOriginal = available + alreadyReserved;

                // IMPORTANT FIX: When compensating within same stratum, we need to check
                // if there are enough households LEFT (available now) to meet our own needs
                // The formula should be: can spare = available - (required - already used for self)

                // How many has this SSS already provided for its own requirement?
                // This is trickier - we need to know how many were selected FOR this SSS specifically
                // For now, let's use a simpler approach:
                // Can spare = current available - remaining needed for self

                int remainingNeededForSelf = Math.Max(0, required - alreadyReserved);
                int spareable = available - remainingNeededForSelf;

                Debug.WriteLine($"[DEBUG]   SSS {sssOrStratum}: available={available}, required={required}, alreadyReserved={alreadyReserved}, remainingNeededForSelf={remainingNeededForSelf}, spareable={spareable}");

                return Math.Max(0, spareable); // Can't be negative
            }
            else
            {
                // For Stratum: Check total stratum requirement
                Debug.WriteLine($"[DEBUG]   Checking STRATUM {sssOrStratum}");

                if (!allocation.ContainsKey(sssOrStratum))
                {
                    Debug.WriteLine($"[DEBUG]   Stratum {sssOrStratum} not in allocation, can spare all {available}");
                    return available;
                }

                var sssAllocations = allocation[sssOrStratum];
                int totalRequired = sssAllocations.Values.Sum();

                // Get total available in this stratum (not just the available passed in)
                var allInStratum = pool.GetAvailable(stratum: sssOrStratum);
                int totalAvailable = allInStratum.Count;

                // Count how many from this stratum have already been reserved
                int alreadyReserved = pool.GetReservedCount(stratum: sssOrStratum);

                // Total originally available = current available + already reserved
                int totalOriginal = totalAvailable + alreadyReserved;

                Debug.WriteLine($"[DEBUG]   Stratum {sssOrStratum}: totalOriginal={totalOriginal}, totalRequired={totalRequired}, alreadyReserved={alreadyReserved}");

                // NEW CHECK: Similar to SSS check above
                if (currentProcessingStratum > 0 && sssOrStratum != currentProcessingStratum && sssOrStratum > currentProcessingStratum)
                {
                    Debug.WriteLine($"[DEBUG]   Stratum {sssOrStratum} not yet processed, current={currentProcessingStratum}, cannot spare");
                    return 0;
                }

                // Can spare = total - required - already reserved
                int spareable = totalOriginal - totalRequired - alreadyReserved;

                Debug.WriteLine($"[DEBUG]   Stratum {sssOrStratum} can spare: {spareable}");

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
