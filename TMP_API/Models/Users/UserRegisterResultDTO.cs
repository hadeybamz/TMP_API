namespace TMP_API.Models.Users
{
    public class UserRegisterResultDTO
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static implicit operator UserRegisterResultDTO(UserLoginResultDTO v)
        {
            throw new NotImplementedException();
        }
    }
}
