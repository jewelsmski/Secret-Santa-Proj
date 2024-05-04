using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

public class Database
{
    public Family? Family { get; set; }

    public Database(string familyName)
    {
        Family = new Family { Name = familyName, Members = new List<Member>() };
    }

    public void Save()
    {
        // Save family to the database
        throw new NotImplementedException();
    }

    public static IEnumerable<(string Name, string FileName)> ListExistingFamilies()
    {
        // List existing families
        throw new NotImplementedException();
    }

    public static void ClearAllFamilies()
    {
        // Clear all families from the database
        throw new NotImplementedException();
    }
}

public class Family
{
    public string? Name { get; set; }
    public List<Member> Members { get; set; }
}

public class Member
{
    public string? Name { get; set; }
    public DateOnly Birthday { get; set; }
    public string? GiftIdea { get; set; }
    public List<string>? AvoidMembers { get; set; }
    public string? GiveToName { get; set; }
    public string? GiveToGiftIdea { get; set; }
}

public class FamilyUI
{
    public enum SelectOption
    {
        ShowReport,
        AssignSecretSanta,
        InsertSampleFamilies,
        CreateNewFamily,
        RunSecretSanta // New option
    }

    public enum MenuOption
    {
        Rename,
        OpenMember,
        AddMember,
        Delete
    }

    public static (SelectOption, Database?) SelectOne()
    {
        // Family selection UI
        throw new NotImplementedException();
    }

    public static Database Create()
    {
        // Create new family UI
        throw new NotImplementedException();
    }

    public static void Show(Database family)
    {
        // Show family UI
        throw new NotImplementedException();
    }

    public static MenuOption Menu(Database family)
    {
        // Family menu UI
        throw new NotImplementedException();
    }

    public static void Delete(Database? family)
    {
        // Delete family UI
        throw new NotImplementedException();
    }
}

public class MemberUI
{
    public enum MenuOption
    {
        Edit,
        Delete
    }

    public static Member SelectOne(Database? family)
    {
        // Select member UI
        throw new NotImplementedException();
    }

    public static Member Create(Database? family)
    {
        // Create member UI
        throw new NotImplementedException();
    }

    public static MenuOption Menu(Member member)
    {
        // Member menu UI
        throw new NotImplementedException();
    }

    public static void Edit(Database? family, Member member)
    {
        // Edit member UI
        throw new NotImplementedException();
    }

    public static void Delete(Database? family, Member member)
    {
        // Delete member UI
        throw new NotImplementedException();
    }
}

public class Breadcrumb
{
    public static void Draw(bool forward = false)
    {
        // Breadcrumb draw UI
        throw new NotImplementedException();
    }

    public static void Forward(string? name)
    {
        // Breadcrumb forward UI
        throw new NotImplementedException();
    }

    public static void Back()
    {
        // Breadcrumb back UI
        throw new NotImplementedException();
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Breadcrumb.Draw();

            var (familyAction, family) = FamilyUI.SelectOne();

            if (familyAction == FamilyUI.SelectOption.ShowReport)
            {
                ShowAllFamilyReport(family);
                continue;
            }
            else if (familyAction == FamilyUI.SelectOption.AssignSecretSanta)
            {
                AssignSecretSanta(family);
            }
            else if (familyAction == FamilyUI.SelectOption.InsertSampleFamilies)
            {
                InsertSampleFamilies(true);
                continue;
            }
            else if (familyAction == FamilyUI.SelectOption.CreateNewFamily)
            {
                family = FamilyUI.Create();
            }
            else if (familyAction == FamilyUI.SelectOption.RunSecretSanta)
            {
                RunSecretSanta();
                continue;
            }

            PresentFamily(family);
        }
    }

    static void AssignSecretSanta(Database? family)
    {
        if (family == null || family.Family == null)
        {
            AnsiConsole.MarkupLine("[red]No family selected. Unable to assign Secret Santa.[/]");
            return;
        }

        var members = family.Family.Members;
        var random = new Random();

        foreach (var member in members)
        {
            // Select a random member to give a gift to
            Member recipient;
            do
            {
                recipient = members[random.Next(members.Count)];
            } while (recipient == member || (member.AvoidMembers != null && member.AvoidMembers.Contains(recipient.Name)));

            // Assign recipient and gift idea
            member.GiveToName = recipient.Name;
            member.GiveToGiftIdea = recipient.GiftIdea;
        }

        AnsiConsole.MarkupLine("[green]Secret Santa assignment completed![/]");
    }

    static void ShowAllFamilyReport(Database? family)
    {
        if (family == null || family.Family == null)
        {
            AnsiConsole.MarkupLine("[red]No family selected. Unable to show report.[/]");
            return;
        }

        var tree = new Tree("Family Report");
        var members = family.Family.Members;

        foreach (var member in members)
        {
            var node = tree.AddNode($"{member.Name}: {member.GiveToName} ({member.GiveToGiftIdea})");
            if (member.AvoidMembers != null && member.AvoidMembers.Any())
            {
                node.AddNode("Avoids: " + string.Join(", ", member.AvoidMembers));
            }
        }

        AnsiConsole.Render(tree);
    }

    static void InsertSampleFamilies(bool clearFirst = false)
    {
        if (clearFirst)
        {
            // Clear existing families
            Database.ClearAllFamilies();
        }

        // Sample family 1
        var smithFamily = new Database("Smith");
        smithFamily.Family.Members.Add(new Member { Name = "John", GiftIdea = "Books" });
        smithFamily.Family.Members.Add(new Member { Name = "Jane", GiftIdea = "Kitchenware" });
        smithFamily.Save();

        // Sample family 2
        var doeFamily = new Database("Doe");
        doeFamily.Family.Members.Add(new Member { Name = "Alice", GiftIdea = "Tech gadgets" });
        doeFamily.Family.Members.Add(new Member { Name = "Bob", GiftIdea = "Sports equipment" });
        doeFamily.Save();

        // Add more families if needed
    }

    static void RunSecretSanta()
    {
        var (_, family) = FamilyUI.SelectOne();
        if (family == null)
        {
            AnsiConsole.MarkupLine("[red]No family selected. Unable to run Secret Santa.[/]");
            return;
        }

        AssignSecretSanta(family);
        ShowAllFamilyReport(family);
    }

    static void PresentFamily(Database? family)
    {
        if (family == null || family.Family == null)
        {
            AnsiConsole.MarkupLine("[red]No family selected. Unable to present family.[/]");
            return;
        }

        Breadcrumb.Forward(family.Family.Name);

        while (true)
        {
            Breadcrumb.Draw(true);
            FamilyUI.Show(family);
            var familyOption = FamilyUI.Menu(family);

            if (familyOption == FamilyUI.MenuOption.Rename)
            {
                FamilyUI.Edit(family);
                family.Save();
                continue;
            }
            else if (familyOption == FamilyUI.MenuOption.OpenMember)
            {
                var member = MemberUI.SelectOne(family);
                PresentMember(family, member);
                continue;
            }
            else if (familyOption == FamilyUI.MenuOption.AddMember)
            {
                var member = MemberUI.Create(family);
                family.Family.Members.Add(member);
                family.Save();
                continue;
            }
            else if (familyOption == FamilyUI.MenuOption.Delete)
            {
                FamilyUI.Delete(family);
            }

            Breadcrumb.Back();
            return;
        }
    }

    static void PresentMember(Database? family, Member member)
    {
        if (family == null || family.Family == null)
        {
            AnsiConsole.MarkupLine("[red]No family selected. Unable to present member.[/]");
            return;
        }

        Breadcrumb.Forward(member.Name);

        while (true)
        {
            Breadcrumb.Draw(true);
            MemberUI.Show(member);

            var selection = MemberUI.Menu(member);
            if (selection == MemberUI.MenuOption.Edit)
            {
                MemberUI.Edit(family, member);
                family.Save();
                continue;
            }
            else if (selection == MemberUI.MenuOption.Delete)
            {
                MemberUI.Delete(family, member);
                family.Save();
            }

            Breadcrumb.Back();
            return;
        }
    }
}

