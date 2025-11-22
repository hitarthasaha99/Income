using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.HIS2026
{
    public class Block_7_1_Constants
    {
        public static readonly List<Tbl_Lookup> CropCodes =
        [
            // Cereals
            new() { id = 0101, title = "paddy - 0101" },
            new() { id = 0102, title = "wheat - 0102" },
            new() { id = 0103, title = "oats - 0103" },
            new() { id = 0104, title = "coarse grains (bajra, jowar, maize, ragi, barley, small millets) - 0104" },
            new() { id = 0199, title = "other cereals - 0199" },

            // Pulses
            new() { id = 0201, title = "gram (incl. horse gram) - 0201" },
            new() { id = 0202, title = "tur (arhar) - 0202" },
            new() { id = 0203, title = "urad - 0203" },
            new() { id = 0204, title = "moong - 0204" },
            new() { id = 0205, title = "masur - 0205" },
            new() { id = 0299, title = "other pulses - 0299" },

            // Sugar Crops
            new() { id = 0301, title = "sugarcane - 0301" },
            new() { id = 0399, title = "other sugar crops - 0399" },

            // Oilseeds
            new() { id = 0401, title = "groundnut - 0401" },
            new() { id = 0402, title = "sesamum (til) - 0402" },
            new() { id = 0403, title = "rapeseed & mustard - 0403" },
            new() { id = 0404, title = "coconut - 0404" },
            new() { id = 0405, title = "sunflower - 0405" },
            new() { id = 0406, title = "soyabean - 0406" },
            new() { id = 0499, title = "other oilseeds - 0499" },

            // Fibres
            new() { id = 0501, title = "cotton - 0501" },
            new() { id = 0502, title = "jute - 0502" },
            new() { id = 0503, title = "mesta - 0503" },
            new() { id = 0504, title = "sunhemp - 0504" },
            new() { id = 0599, title = "other fibres - 0599" },

            // Condiments & Spices
            new() { id = 0601, title = "pepper (black) - 0601" },
            new() { id = 0602, title = "ginger - 0602" },
            new() { id = 0603, title = "turmeric - 0603" },
            new() { id = 0604, title = "garlic - 0604" },
            new() { id = 0605, title = "coriander - 0605" },
            new() { id = 0606, title = "tamarind - 0606" },
            new() { id = 0607, title = "cumin seed - 0607" },
            new() { id = 0699, title = "other condiments & spices - 0699" },

            // Fruits
            new() { id = 0701, title = "mango - 0701" },
            new() { id = 0702, title = "orange/mausami/kinu - 0702" },
            new() { id = 0703, title = "banana - 0703" },
            new() { id = 0704, title = "apple - 0704" },
            new() { id = 0705, title = "grapes - 0705" },
            new() { id = 0706, title = "guava - 0706" },
            new() { id = 0707, title = "papaya - 0707" },
            new() { id = 0708, title = "watermelon - 0708" },
            new() { id = 0709, title = "musk melon - 0709" },
            new() { id = 0710, title = "jackfruit - 0710" },
            new() { id = 0711, title = "almond - 0711" },
            new() { id = 0712, title = "walnut - 0712" },
            new() { id = 0713, title = "cashewnuts - 0713" },
            new() { id = 0714, title = "apricot - 0714" },
            new() { id = 0799, title = "other fruits - 0799" },

            // Vegetables
            new() { id = 0801, title = "potato - 0801" },
            new() { id = 0802, title = "onion - 0802" },
            new() { id = 0803, title = "tomato - 0803" },
            new() { id = 0804, title = "leafy vegetables - 0804" },
            new() { id = 0805, title = "cabbage - 0805" },
            new() { id = 0806, title = "brinjal - 0806" },
            new() { id = 0807, title = "lady’s finger (bhindi) - 0807" },
            new() { id = 0808, title = "cauliflower - 0808" },
            new() { id = 0809, title = "gourd - 0809" },
            new() { id = 0810, title = "beans/barbati - 0810" },
            new() { id = 0811, title = "exotic vegetables (mushroom, bell pepper, broccoli, etc.) - 0811" },
            new() { id = 0812, title = "pumpkin - 0812" },
            new() { id = 0813, title = "radish - 0813" },
            new() { id = 0814, title = "carrot - 0814" },
            new() { id = 0815, title = "lemon - 0815" },
            new() { id = 0816, title = "green chillies - 0816" },
            new() { id = 0899, title = "other vegetables - 0899" },

            // Flower Crops
            new() { id = 1001, title = "orchids - 1001" },
            new() { id = 1002, title = "rose - 1002" },
            new() { id = 1003, title = "gladiolus - 1003" },
            new() { id = 1004, title = "carnation - 1004" },
            new() { id = 1005, title = "marigold - 1005" },
            new() { id = 1099, title = "other flowers - 1099" },

            // Fodder Crops
            new() { id = 1101, title = "guar - 1101" },
            new() { id = 1102, title = "green manures - 1102" },
            new() { id = 1199, title = "other fodder crops - 1199" },

            // Drugs & Narcotics
            new() { id = 1201, title = "tobacco - 1201" },
            new() { id = 1299, title = "other drugs & narcotics - 1299" },

            // Plantation Crops
            new() { id = 1301, title = "tea - 1301" },
            new() { id = 1302, title = "coffee - 1302" },
            new() { id = 1303, title = "rubber - 1303" },
            new() { id = 1399, title = "other plantation crops - 1399" },

            // Medicinal Plants
            new() { id = 1401, title = "ashwagandha - 1401" },
            new() { id = 1402, title = "isabgol - 1402" },
            new() { id = 1403, title = "sena - 1403" },
            new() { id = 1404, title = "moosli - 1404" },
            new() { id = 1499, title = "other medicinal plants - 1499" },

            // Aromatic Plants
            new() { id = 1501, title = "lemon grass - 1501" },
            new() { id = 1502, title = "mint - 1502" },
            new() { id = 1503, title = "menthol - 1503" },
            new() { id = 1504, title = "eucalyptus - 1504" },
            new() { id = 1599, title = "other aromatic plants - 1599" },

            // Other non-food crops
            new() { id = 1601, title = "canes - 1601" },
            new() { id = 1602, title = "bamboos - 1602" },
            new() { id = 1699, title = "other non-food crops - 1699" }
        ];

    }
}
