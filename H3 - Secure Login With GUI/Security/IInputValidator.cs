namespace H3___Secure_Login_With_GUI.Security
{
    public interface IInputValidator<T>
    {
        bool Validate(T input);
    }
}
