using Data.Contexts;

namespace Business.Helpers
{
    //Tog hjälp utav ChatGPT att skapa denna generator.
    public class ProjectNumberGenerator(DataContext context)
    {
        private readonly DataContext _context = context;

        public string GenerateProjectNumber()
        {
            
            var lastProject = _context.Projects
                                      .OrderByDescending(p => p.Id) //Hämtar det senaste projektet, baserat på ID.
                                      .FirstOrDefault();

            int nextProjectNumber = 100; //Gör så att första projektet får siffran 100.

            if (lastProject != null)
            {
                //Hämtar värdet från det senaste projektnumret. 
                var lastNumber = lastProject.ProjectNumber;
                if (lastNumber.StartsWith("P-"))
                {
                    //Hämtar värdet från det senaste projektnumret.
                    int lastInt = int.Parse(lastNumber.Substring(2));  //Tar bort P- och konverterar över till INT.
                    nextProjectNumber = lastInt + 1; //Ökar med 1
                }
            }

            //Skapar nästa projektnummer i formatet "P-101"
            return $"P-{nextProjectNumber}";
        }
    }
}
