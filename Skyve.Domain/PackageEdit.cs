using System;

namespace Skyve.Domain;
public class PackageEdit(string username, DateTime editDate, string note)
{
	public string Username { get; set; } = username;
	public DateTime EditDate { get; set; } = editDate;
	public string Note { get; set; } = note;
}

