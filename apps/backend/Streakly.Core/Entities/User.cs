using Streakly.Core.Exceptions;
using Streakly.Core.ValueObjects;

namespace Streakly.Core.Entities;

public class User
{

    public UserId UserId { get; private set; }
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoggedAtUtc { get; private set; }
    public List<Activity> Activities { get; private set; } = new();
    
    
    public User(UserId userId, Email email, Username username, Password password,
        FullName fullName, UserRole role, DateTime createdAt,  List<Activity> activities)
    {
        UserId = userId;
        Email = email;
        Username = username;
        Password = password;
        FullName = fullName;
        Role = role;
        CreatedAt = createdAt;
        Activities = activities;
    }

    public User()
    {
    }

    public void ChangeUsername(string newUsername)
        => Username = new Username(newUsername);
    public void ChangeFullName(string newFullname)
        => FullName = new FullName(newFullname);
    public void ChangePassword(Password newSecuredPassword)
        => Password = newSecuredPassword;
    public void ChangeEmail(Email newEmail)
        => Email = newEmail;
    public void MarkAsLoggedIn(DateTime loggedInAt)
        => LastLoggedAtUtc = loggedInAt;
    public void AddActivity(Activity newActivity)
        => Activities.Add(newActivity);
    public void DeleteActivity(Guid activityToDeleteId)
    {
        var activityToDelete = Activities.SingleOrDefault(a => a.Id == activityToDeleteId) ??
                               throw new ActivityNotFoundException(activityToDeleteId);
        
        Activities.Remove(activityToDelete);
    }

    public void MarkActivityAsCompleted(Guid activityToCompleteId)
    {
        var activityToMarkAsCompleted = Activities.SingleOrDefault(a => a.Id == activityToCompleteId) ??
                                        throw new ActivityNotFoundException(activityToCompleteId);
        
        activityToMarkAsCompleted.MarkActivityAsCompleted();
    }
    
    public void MarkActivityAsIncompleted(Guid activityToCompleteId)
    {
        var activityToMarkAsCompleted = Activities.SingleOrDefault(a => a.Id == activityToCompleteId) ??
                                        throw new ActivityNotFoundException(activityToCompleteId);
        
        activityToMarkAsCompleted.MarkActivityAsIncompleted();
    }
}


