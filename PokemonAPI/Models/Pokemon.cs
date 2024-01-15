namespace PokemonAPI.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PokemonType[] Types { get; set; }

        public int Base_experience { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }


    }
}
