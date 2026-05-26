namespace CVMigrator.Models
{
   
    /// <summary>
    /// Class that represent a migration object
    /// </summary>
    public class Migration
    {
        public int Number { get; set; }
        public int Version { get; set; } 
        public String Script { get; set;}
        public String ExecutedAt { get; set; }
        public String ExecutedBy { get; set; }
    }
}
