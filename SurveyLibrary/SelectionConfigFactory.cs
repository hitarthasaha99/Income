using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.SurveyLibrary
{
    // --- Configuration containers ---
    public class StratumConfig
    {
        public int StratumId { get; set; }
        public int Ni { get; set; } // required from this stratum
        public Dictionary<int, int> NijBySSS { get; set; } = new();
        public List<int> SubStrataOrder { get; set; } = new();
        public List<int> StratumLevelPriority { get; set; } = new();
        public Dictionary<int, List<int>> SSSLevelPriority { get; set; } = new();
    }

    public class SelectionConfig
    {
        public Dictionary<int, StratumConfig> ByStratum { get; set; } = new();
        public StratumConfig GetOrCreate(int stratumId)
        {
            if (!ByStratum.TryGetValue(stratumId, out var cfg))
            {
                cfg = new StratumConfig { StratumId = stratumId };
                ByStratum[stratumId] = cfg;
            }
            return cfg;
        }
    }

    // Factory: build config from the document sequences (sector-sensitive)
    public static class SelectionConfigFactory
    {
        /// <summary>
        /// Build config using the sequences from Compensation Rule_final.docx
        /// sector: 1 = rural, 2 = urban
        /// </summary>
        public static SelectionConfig Build(int sector)
        {
            var cfg = new SelectionConfig();

            if (sector == 1) // RURAL
            {
                // Stratum 10 (Self-employed agriculture), ni = 6, SSS 11=3,12=3
                var s10 = cfg.GetOrCreate(10);
                s10.Ni = 6;
                s10.NijBySSS = new Dictionary<int, int> { { 11, 3 }, { 12, 3 } };
                s10.SubStrataOrder = new List<int> { 11, 12 };
                s10.StratumLevelPriority = new List<int> { 22, 21, 20, 32, 31, 30, 40, 90 };
                s10.SSSLevelPriority[11] = new List<int> { 12, 21, 22, 20, 31, 32, 30, 40, 90 };
                s10.SSSLevelPriority[12] = new List<int> { 11, 22, 21, 20, 32, 31, 30, 40, 90 };

                // Stratum 20 (Self-employed non-agriculture), ni = 4, SSS 21=2,22=2
                var s20 = cfg.GetOrCreate(20);
                s20.Ni = 4;
                s20.NijBySSS = new Dictionary<int, int> { { 21, 2 }, { 22, 2 } };
                s20.SubStrataOrder = new List<int> { 21, 22 };
                s20.StratumLevelPriority = new List<int> { 12, 11, 10, 32, 31, 30, 40, 90 };
                s20.SSSLevelPriority[21] = new List<int> { 22, 11, 12, 10, 31, 32, 30, 40, 90 };
                s20.SSSLevelPriority[22] = new List<int> { 21, 12, 11, 10, 32, 31, 30, 40, 90 };

                // Stratum 30 (Regular wage/salary), ni = 4, SSS 31=2,32=2
                var s30 = cfg.GetOrCreate(30);
                s30.Ni = 4;
                s30.NijBySSS = new Dictionary<int, int> { { 31, 2 }, { 32, 2 } };
                s30.SubStrataOrder = new List<int> { 31, 32 };
                s30.StratumLevelPriority = new List<int> { 22, 21, 20, 12, 11, 10, 40, 90 };
                s30.SSSLevelPriority[31] = new List<int> { 32, 21, 22, 20, 11, 12, 10, 40, 90 };
                s30.SSSLevelPriority[32] = new List<int> { 31, 22, 21, 20, 12, 11, 10, 40, 90 };

                // Stratum 40 (Casual labour), ni = 4, SSS 40=4 (no sub-strata)
                var s40 = cfg.GetOrCreate(40);
                s40.Ni = 4;
                s40.NijBySSS = new Dictionary<int, int> { { 40, 4 } };
                s40.SubStrataOrder = new List<int> { 40 };
                s40.StratumLevelPriority = new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 90 };
                // SSS-level priorities are less relevant here since only SSS 40 exists, but we include a fallback:
                s40.SSSLevelPriority[40] = new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 90 };

                // Stratum 90 (Others), ni = 2, SSS 90=2
                var s90 = cfg.GetOrCreate(90);
                s90.Ni = 2;
                s90.NijBySSS = new Dictionary<int, int> { { 90, 2 } };
                s90.SubStrataOrder = new List<int> { 90 };
                s90.StratumLevelPriority = new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 40 };
                s90.SSSLevelPriority[90] = new List<int> { 11, 12, 10, 21, 22, 20, 31, 32, 30, 40 };
            }
            else // URBAN (sector == 2)
            {
                // Stratum 10 (Self-employed urban), ni=6, SSS 11=2,121=2,122=2
                var s10 = cfg.GetOrCreate(10);
                s10.Ni = 6;
                s10.NijBySSS = new Dictionary<int, int> { { 11, 2 }, { 121, 2 }, { 122, 2 } };
                s10.SubStrataOrder = new List<int> { 11, 121, 122 };
                s10.StratumLevelPriority = new List<int> { 222, 221, 21, 20, 40, 90 };
                s10.SSSLevelPriority[11] = new List<int> { 121, 122, 21, 221, 222, 20, 40, 90 };
                s10.SSSLevelPriority[121] = new List<int> { 122, 11, 221, 222, 21, 20, 40, 90 };
                s10.SSSLevelPriority[122] = new List<int> { 121, 11, 222, 221, 21, 20, 40, 90 };

                // Stratum 20 (Regular wage/salary urban), ni=9, SSS 21=4,221=2,222=3
                var s20 = cfg.GetOrCreate(20);
                s20.Ni = 9;
                s20.NijBySSS = new Dictionary<int, int> { { 21, 4 }, { 221, 2 }, { 222, 3 } };
                s20.SubStrataOrder = new List<int> { 21, 221, 222 };
                s20.StratumLevelPriority = new List<int> { 122, 121, 11, 10, 40, 90 };
                s20.SSSLevelPriority[21] = new List<int> { 221, 222, 11, 121, 122, 10, 40, 90 };
                s20.SSSLevelPriority[221] = new List<int> { 222, 21, 121, 122, 11, 10, 40, 90 };
                s20.SSSLevelPriority[222] = new List<int> { 221, 21, 122, 121, 11, 10, 40, 90 };

                // Stratum 40 (Casual labour urban), ni = 3, SSS 40=3
                var s40 = cfg.GetOrCreate(40);
                s40.Ni = 3;
                s40.NijBySSS = new Dictionary<int, int> { { 40, 3 } };
                s40.SubStrataOrder = new List<int> { 40 };
                s40.StratumLevelPriority = new List<int> { 11, 121, 122, 10, 21, 221, 222, 20, 90 };
                s40.SSSLevelPriority[40] = new List<int> { 11, 121, 122, 10, 21, 221, 222, 20, 90 };

                // Stratum 90 (Others urban), ni = 2, SSS 90=2
                var s90 = cfg.GetOrCreate(90);
                s90.Ni = 2;
                s90.NijBySSS = new Dictionary<int, int> { { 90, 2 } };
                s90.SubStrataOrder = new List<int> { 90 };
                s90.StratumLevelPriority = new List<int> { 21, 221, 222, 20, 11, 121, 122, 10, 40 };
                s90.SSSLevelPriority[90] = new List<int> { 21, 221, 222, 20, 11, 121, 122, 10, 40 };
            }

            return cfg;
        }
    }

    // --- Result / Logging ---
    public class SelectionLogEntry
    {
        public Guid HouseholdId { get; set; }
        public int OriginalSSS { get; set; }
        public int PostedSSS { get; set; }
        public string Reason { get; set; } = "";
    }

    public class SelectionResult
    {
        public List<Tbl_Sch_0_0_Block_7> Households { get; set; } = new();
        public List<SelectionLogEntry> Logs { get; set; } = new();
    }


    // --- Selector implementation (same algorithm as earlier) ---
    public class HouseholdSelector
    {
        private readonly SelectionConfig _config;
        private readonly Random _rng;

        public HouseholdSelector(SelectionConfig config, int? randomSeed = null)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _rng = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();
        }

        public SelectionResult Select(List<Tbl_Sch_0_0_Block_7> households, int sector)
        {
            if (households == null) throw new ArgumentNullException(nameof(households));
            var result = new SelectionResult { Households = households };

            // Group households by stratum for the FSU
            var byStratum = households.GroupBy(h => h.Stratum).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var kv in _config.ByStratum)
            {
                int stratumId = kv.Key;
                var sCfg = kv.Value;

                byStratum.TryGetValue(stratumId, out var stratumHouseholds);
                stratumHouseholds ??= new List<Tbl_Sch_0_0_Block_7>();

                int totalInStratum = stratumHouseholds.Count;
                int requiredNi = sCfg.Ni;

                if (totalInStratum < requiredNi)
                {
                    // Case A: stratum-level shortfall -> no sub-stratum formation
                    int postedSSS = sCfg.SubStrataOrder.FirstOrDefault();
                    if (postedSSS == 0)
                        postedSSS = stratumHouseholds.Select(h => h.SSS).FirstOrDefault();

                    int remainingNeeded = requiredNi;

                    foreach (int candidateSSS in sCfg.StratumLevelPriority)
                    {
                        if (remainingNeeded <= 0) break;
                        var available = stratumHouseholds.Where(h => !h.isSelected && h.SSS == candidateSSS).ToList();
                        if (!available.Any()) continue;
                        var chosen = TakeRandom(available, Math.Min(available.Count, remainingNeeded));
                        foreach (var hh in chosen)
                        {
                            hh.isSelected = true;
                            hh.SelectedFromSSS = hh.SSS;
                            hh.SelectedPostedSSS = postedSSS;
                            result.Logs.Add(new SelectionLogEntry
                            {
                                HouseholdId = hh.id,
                                OriginalSSS = hh.SSS,
                                PostedSSS = postedSSS,
                                Reason = $"Stratum-level shortfall; stratum {stratumId}"
                            });
                        }
                        remainingNeeded -= chosen.Count;
                    }

                    // last resort: any remaining in stratum
                    if (remainingNeeded > 0)
                    {
                        var available = stratumHouseholds.Where(h => !h.isSelected).ToList();
                        if (available.Any())
                        {
                            var chosen = TakeRandom(available, Math.Min(available.Count, remainingNeeded));
                            foreach (var hh in chosen)
                            {
                                hh.isSelected = true;
                                hh.SelectedFromSSS = hh.SSS;
                                hh.SelectedPostedSSS = postedSSS;
                                result.Logs.Add(new SelectionLogEntry
                                {
                                    HouseholdId = hh.id,
                                    OriginalSSS = hh.SSS,
                                    PostedSSS = postedSSS,
                                    Reason = $"Stratum-level shortfall; fallback in stratum {stratumId}"
                                });
                            }
                            remainingNeeded -= chosen.Count;
                        }
                    }

                    if (remainingNeeded > 0)
                    {
                        result.Logs.Add(new SelectionLogEntry
                        {
                            HouseholdId = Guid.Empty,
                            OriginalSSS = -1,
                            PostedSSS = postedSSS,
                            Reason = $"UNRESOLVED: Could not find enough households for stratum {stratumId}. Still need {remainingNeeded}."
                        });
                    }
                }
                else
                {
                    // Case B: stratum adequate - enforce nij
                    foreach (var nijKvp in sCfg.NijBySSS)
                    {
                        int sss = nijKvp.Key;
                        int requiredNij = nijKvp.Value;
                        int present = stratumHouseholds.Count(h => !h.isSelected && h.SSS == sss);

                        if (present >= requiredNij)
                        {
                            var notYetSelectedInThisSSS = stratumHouseholds.Where(h => !h.isSelected && h.SSS == sss).ToList();
                            if (notYetSelectedInThisSSS.Count > requiredNij)
                            {
                                var chosen = TakeRandom(notYetSelectedInThisSSS, requiredNij);
                                foreach (var hh in chosen)
                                {
                                    hh.isSelected = true;
                                    hh.SelectedFromSSS = hh.SSS;
                                    hh.SelectedPostedSSS = hh.SSS;
                                    result.Logs.Add(new SelectionLogEntry
                                    {
                                        HouseholdId = hh.id,
                                        OriginalSSS = hh.SSS,
                                        PostedSSS = hh.SSS,
                                        Reason = $"Selected within SSS {sss} for stratum {stratumId}"
                                    });
                                }
                            }
                            else
                            {
                                foreach (var hh in notYetSelectedInThisSSS)
                                {
                                    hh.isSelected = true;
                                    hh.SelectedFromSSS = hh.SSS;
                                    hh.SelectedPostedSSS = hh.SSS;
                                    result.Logs.Add(new SelectionLogEntry
                                    {
                                        HouseholdId = hh.id,
                                        OriginalSSS = hh.SSS,
                                        PostedSSS = hh.SSS,
                                        Reason = $"Selected within SSS {sss} for stratum {stratumId}"
                                    });
                                }
                            }
                            continue;
                        }

                        // SSS-level shortfall
                        int remainingNeeded = requiredNij - present;
                        var presentHhs = stratumHouseholds.Where(h => !h.isSelected && h.SSS == sss).ToList();
                        foreach (var hh in presentHhs)
                        {
                            hh.isSelected = true;
                            hh.SelectedFromSSS = hh.SSS;
                            hh.SelectedPostedSSS = hh.SSS;
                            result.Logs.Add(new SelectionLogEntry
                            {
                                HouseholdId = hh.id,
                                OriginalSSS = hh.SSS,
                                PostedSSS = hh.SSS,
                                Reason = $"Selected existing in SSS {sss} for stratum {stratumId}"
                            });
                        }

                        if (!sCfg.SSSLevelPriority.TryGetValue(sss, out var priorityList) || priorityList == null || !priorityList.Any())
                        {
                            priorityList = sCfg.StratumLevelPriority;
                        }

                        foreach (int donorSSS in priorityList)
                        {
                            if (remainingNeeded <= 0) break;
                            var available = stratumHouseholds.Where(h => !h.isSelected && h.SSS == donorSSS).ToList();
                            if (!available.Any()) continue;

                            var chosen = TakeRandom(available, Math.Min(available.Count, remainingNeeded));
                            foreach (var hh in chosen)
                            {
                                hh.isSelected = true;
                                hh.SelectedFromSSS = hh.SSS;
                                hh.SelectedPostedSSS = sss; // posted to target SSS
                                result.Logs.Add(new SelectionLogEntry
                                {
                                    HouseholdId = hh.id,
                                    OriginalSSS = hh.SSS,
                                    PostedSSS = sss,
                                    Reason = $"SSS-level shortfall for {sss} in stratum {stratumId}; taken from {hh.SSS}"
                                });
                            }
                            remainingNeeded -= chosen.Count;
                        }

                        if (remainingNeeded > 0)
                        {
                            result.Logs.Add(new SelectionLogEntry
                            {
                                HouseholdId = Guid.Empty,
                                OriginalSSS = -1,
                                PostedSSS = sss,
                                Reason = $"UNRESOLVED: After priority list, still need {remainingNeeded} for SSS {sss} in stratum {stratumId}"
                            });
                        }
                    } // for each nij
                } // end else (case B)
            } // foreach stratum config

            return result;
        }

        public Guid? Substitute(List<Tbl_Sch_0_0_Block_7> allHouseholds, Guid missingHouseholdId)
        {
            var missing = allHouseholds.FirstOrDefault(h => h.id == missingHouseholdId);
            if (missing == null) return null;

            int sss = missing.SSS;
            var candidates = allHouseholds.Where(h => !h.isSelected && h.SSS == sss).ToList();
            if (!candidates.Any()) return null;

            var chosen = TakeRandom(candidates, 1).First();
            chosen.isSelected = true;
            chosen.SelectedFromSSS = chosen.SSS;
            chosen.SelectedPostedSSS = missing.SelectedPostedSSS ?? chosen.SSS;

            return chosen.id;
        }

        private List<Tbl_Sch_0_0_Block_7> TakeRandom(List<Tbl_Sch_0_0_Block_7> list, int n)
        {
            if (n <= 0) return new List<Tbl_Sch_0_0_Block_7>();
            if (n >= list.Count) return new List<Tbl_Sch_0_0_Block_7>(list);

            var arr = list.ToArray();
            for (int i = 0; i < n; i++)
            {
                int j = _rng.Next(i, arr.Length);
                var tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }
            return arr.Take(n).ToList();
        }
    }
}
