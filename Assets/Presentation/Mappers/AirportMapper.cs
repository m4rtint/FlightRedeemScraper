using CathayDomain;

public static class AirportMapper
{
    public static readonly Dictionary<Airport, string> AirportDisplayNames = new()
    {
        // Africa
        { Airport.JNB, "Johannesburg (JNB)" },

        // Americas
        { Airport.YVR, "Vancouver (YVR)" },
        { Airport.YYZ, "Toronto (YYZ)" },
        { Airport.BOS, "Boston (BOS)" },
        { Airport.ORD, "Chicago (ORD)" },
        { Airport.LAX, "Los Angeles (LAX)" },
        { Airport.JFK, "New York (JFK)" },
        { Airport.SFO, "San Francisco (SFO)" },

        // Asia-Pacific
        { Airport.DAC, "Dhaka (DAC)" },
        { Airport.PNH, "Phnom Penh (PNH)" },
        { Airport.DEL, "Delhi (DEL)" },
        { Airport.BOM, "Mumbai (BOM)" },
        { Airport.BLR, "Bengaluru (BLR)" },
        { Airport.MAA, "Chennai (MAA)" },
        { Airport.HYD, "Hyderabad (HYD)" },
        { Airport.CGK, "Jakarta (CGK)" },
        { Airport.DPS, "Denpasar (DPS)" },
        { Airport.SUB, "Surabaya (SUB)" },
        { Airport.HND, "Tokyo Haneda (HND)" },
        { Airport.NRT, "Tokyo Narita (NRT)" },
        { Airport.FUK, "Fukuoka (FUK)" },
        { Airport.NGO, "Nagoya (NGO)" },
        { Airport.KIX, "Osaka (KIX)" },
        { Airport.CTS, "Sapporo (CTS)" },
        { Airport.ICN, "Seoul (ICN)" },
        { Airport.KUL, "Kuala Lumpur (KUL)" },
        { Airport.PEN, "Penang (PEN)" },
        { Airport.KTM, "Kathmandu (KTM)" },
        { Airport.CEB, "Cebu (CEB)" },
        { Airport.MNL, "Manila (MNL)" },
        { Airport.SIN, "Singapore (SIN)" },
        { Airport.CMB, "Colombo (CMB)" },
        { Airport.BKK, "Bangkok (BKK)" },
        { Airport.HKT, "Phuket (HKT)" },
        { Airport.HAN, "Hanoi (HAN)" },
        { Airport.SGN, "Ho Chi Minh City (SGN)" },

        // China – the Chinese Mainland, Hong Kong SAR, Macao SAR and Taiwan Region
        { Airport.PEK, "Beijing (PEK)" },
        { Airport.CTU, "Chengdu (CTU)" },
        { Airport.CKG, "Chongqing (CKG)" },
        { Airport.FOC, "Fuzhou (FOC)" },
        { Airport.CAN, "Guangzhou (CAN)" },
        { Airport.HAK, "Haikou (HAK)" },
        { Airport.HGH, "Hangzhou (HGH)" },
        { Airport.NKG, "Nanjing (NKG)" },
        { Airport.TAO, "Qingdao (TAO)" },
        { Airport.PVG, "Shanghai Pudong (PVG)" },
        { Airport.SHA, "Shanghai Hongqiao (SHA)" },
        { Airport.WNZ, "Wenzhou (WNZ)" },
        { Airport.WUH, "Wuhan (WUH)" },
        { Airport.XIY, "Xi'an (XIY)" },
        { Airport.XMN, "Xiamen (XMN)" },
        { Airport.CGO, "Zhengzhou (CGO)" },
        { Airport.HKG, "Hong Kong (HKG)" },
        { Airport.KHH, "Kaohsiung (KHH)" },
        { Airport.TPE, "Taipei (TPE)" },

        // Australasia
        { Airport.BNE, "Brisbane (BNE)" },
        { Airport.MEL, "Melbourne (MEL)" },
        { Airport.PER, "Perth (PER)" },
        { Airport.SYD, "Sydney (SYD)" },
        { Airport.AKL, "Auckland (AKL)" },
        { Airport.CHC, "Christchurch (CHC)" },

        // Europe
        { Airport.BRU, "Brussels (BRU)" },
        { Airport.CPH, "Copenhagen (CPH)" },
        { Airport.FRA, "Frankfurt (FRA)" },
        { Airport.DUS, "Dusseldorf (DUS)" },
        { Airport.MUC, "Munich (MUC)" },
        { Airport.CDG, "Paris Charles de Gaulle (CDG)" },
        { Airport.AMS, "Amsterdam (AMS)" },
        { Airport.LHR, "London Heathrow (LHR)" },
        { Airport.MAN, "Manchester (MAN)" },
        { Airport.ZRH, "Zurich (ZRH)" },

        // Middle East
        { Airport.TLV, "Tel Aviv (TLV)" },
        { Airport.DOH, "Doha (DOH)" },
        { Airport.AUH, "Abu Dhabi (AUH)" },
        { Airport.DXB, "Dubai (DXB)" }
    };
    
    public static Airport FromString(string airportCode)
    {
        if (Enum.TryParse(airportCode, true, out Airport airport))
        {
            return airport;
        }

        throw new ArgumentException("Invalid airport code", nameof(airportCode));
    }
}