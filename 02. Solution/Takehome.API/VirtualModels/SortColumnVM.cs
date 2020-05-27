namespace Takehome.API.VirtualModels
{
    public class SortColumnVM
    {
        public string ColumnName { get; set; }        
        public bool DirectionReverse { get; set; }
        public bool IsValidColumn { get; set; }

        public SortColumnVM(string token) 
        {
            string tkn = token.Trim().ToLower();

            if (tkn.EndsWith(" desc"))
            {
                ColumnName = tkn.Substring(0, tkn.Length - 4).Trim();
                DirectionReverse = true;
            }
            else if (tkn.EndsWith(" asc"))
            {
                ColumnName = tkn.Substring(0, tkn.Length - 3).Trim();
                DirectionReverse = false;
            }
            else
            {
                ColumnName = tkn;
                DirectionReverse = false;
            }
            IsValidColumn = (typeof(MunroVM).GetProperty(ColumnName) != null);
        }
    }
}