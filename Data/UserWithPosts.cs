namespace AlticeApi.Data;
public class UserWithPosts 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string UserAddress { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public string Company { get; set; }
    public List<Post>? Posts { get; set; }
}