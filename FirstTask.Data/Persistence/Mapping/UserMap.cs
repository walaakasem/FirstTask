using FirstTask.Data.Domain;
namespace FirstTask.Data.Persistence.Mapping
{
    public class UserMap : BaseMap<User>
    {
        public UserMap()
        {
            ToTable("User");
        }
    }
}
