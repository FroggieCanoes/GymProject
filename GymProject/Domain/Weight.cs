namespace GymProject.Domain;

public class Weight
{
    public double Kg { get; set; }
    public DateTime Day { get; set; } = DateTime.Now;
    public int Exercicie { get; set; }
}
