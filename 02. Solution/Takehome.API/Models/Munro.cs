namespace Takehome.API.Models
{
    public enum Hill_Categories
    {
        None = 0,
        Munro = 1,
        MunroTop = 2,
        Both = 3
    }

    public class Munro
    {
        public int pRunning_No { get; set; }
        public string pDoBIH_Number { get; set; }
        public string pStreetmap { get; set; }
        public string pGeograph { get; set; }
        public string pHill_bagging { get; set; }
        public string pName { get; set; }
        public string pSMC_Section { get; set; }
        public string pRHB_Section { get; set; }
        public string p_Section { get; set; }
        public decimal pHeight__m_ { get; set; }
        public string pHeight__ft_ { get; set; }
        public string pMap_1_50 { get; set; }
        public string pMap_1_25 { get; set; }
        public string pGrid_Ref { get; set; }
        public string pGridRefXY { get; set; }
        public string pxcoord { get; set; }
        public string pycoord { get; set; }
        public string p1891 {get; set;}
        public string p1921 { get; set; }
        public string p1933 { get; set; }
        public string p1953 { get; set; }
        public string p1969 { get; set; }
        public string p1974 { get; set; }
        public string p1981 { get; set; }
        public string p1984 { get; set; }
        public string p1990 { get; set; }
        public string p1997 { get; set; }
        public string pPost_1997 { get; set; }
        public string pComments { get; set; }

        public Hill_Categories Hill_Category
        {
            get {
                Hill_Categories retVal = Hill_Categories.None;

                switch (pPost_1997)
                {
                    case "MUN":
                        retVal = Hill_Categories.Munro;
                        break;
                    case "TOP":
                        retVal = Hill_Categories.MunroTop;
                        break;
                }

                return retVal; 
            }
        }
    }
}