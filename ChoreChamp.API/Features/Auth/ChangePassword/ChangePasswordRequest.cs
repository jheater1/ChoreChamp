namespace ChoreChamp.API.Features.Auth.ChangePassword;

public record ChangePasswordRequest(string Password, string NewPassword, string ConfirmNewPassword);
