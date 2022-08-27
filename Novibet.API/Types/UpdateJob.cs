namespace Novibet.API.Types
{

    public class UpdateJob
    {
        //Task task { get; set; }
        public int itemsTotal { get; }
        public int itemsDone { get; set; }

        public string GetDoneRatio()
        {
            return $"{itemsDone}/{itemsTotal}";
        }
        public UpdateJob(int itemsTotal)
        {
            //this.task = task;
            this.itemsTotal = itemsTotal;
            this.itemsDone = 0;
        }
    }

}
